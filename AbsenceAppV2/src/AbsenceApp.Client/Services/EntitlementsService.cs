/*
===============================================================================
 File        : EntitlementsService.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Client-side entitlement service responsible for retrieving,
               storing, and evaluating user entitlements. Provides a simple API
               for UI components and layouts to check whether the authenticated
               user has specific permissions. Loads entitlements from the
               backend endpoint /api/entitlements/effective and exposes helper
               methods Has, HasAny, HasAll, and Reset for deterministic UI
               control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation for Phase 2 — Step 2.1.
   - 1.1.0  2026-04-05  GAP CLOSURES: (G2) Fixed LoadAsync deserialization to
                         use a private DTO matching the API response envelope
                         {allowedKeys:[...], generatedAtUtc:...} instead of
                         List<string> (which caused JsonException on every
                         successful response). (G3) Added Reset() to clear the
                         entitlements dict and unset _isLoaded so Login.razor
                         can reload entitlements for each authenticated session,
                         preventing stale entitlement carry-over across logouts.
-------------------------------------------------------------------------------
 Notes       :
   - This service is registered as Scoped in MauiProgram.cs.
   - Entitlements are stored in-memory for the duration of the session.
   - Call Reset() then LoadAsync() on every successful login.
===============================================================================
*/

using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AbsenceApp.Client.Services
{
    // ===========================================================================
    // EntitlementsService
    // ===========================================================================
    public sealed class EntitlementsService
    {
        // ---------------------------------------------------------------------------
        // Private response DTO — matches the API envelope returned by
        // GET /api/entitlements/effective: { allowedKeys:[...], generatedAtUtc:... }
        // ---------------------------------------------------------------------------

        private sealed class EntitlementsApiResponse
        {
            [JsonPropertyName("allowedKeys")]
            public string[] AllowedKeys { get; set; } = [];

            [JsonPropertyName("generatedAtUtc")]
            public DateTime GeneratedAtUtc { get; set; }
        }

        // ---------------------------------------------------------------------------
        // Fields
        // ---------------------------------------------------------------------------

        private readonly HttpClient _httpClient;

        // Thread-safe entitlement store
        private readonly ConcurrentDictionary<string, bool> _entitlements =
            new(StringComparer.OrdinalIgnoreCase);

        private bool _isLoaded = false;
        private readonly object _loadLock = new();

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------

        public EntitlementsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ---------------------------------------------------------------------------
        // LoadAsync
        // Loads entitlements from the backend once per session.
        // ---------------------------------------------------------------------------

        public async Task LoadAsync()
        {
            if (_isLoaded)
                return;

            lock (_loadLock)
            {
                if (_isLoaded)
                    return;

                _isLoaded = true;
            }

            var response = await _httpClient.GetFromJsonAsync<EntitlementsApiResponse>(
                "/api/entitlements/effective");

            if (response?.AllowedKeys is null)
                return;

            foreach (var key in response.AllowedKeys)
            {
                _entitlements[key] = true;
            }
        }

        // ---------------------------------------------------------------------------
        // Has
        // Returns true if the user has the specified entitlement key.
        // ---------------------------------------------------------------------------

        public bool Has(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            return _entitlements.ContainsKey(key);
        }

        // ---------------------------------------------------------------------------
        // HasAny
        // Returns true if the user has at least one of the specified keys.
        // ---------------------------------------------------------------------------

        public bool HasAny(params string[] keys)
        {
            if (keys is null || keys.Length == 0)
                return false;

            foreach (var key in keys)
            {
                if (Has(key))
                    return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------
        // HasAll
        // Returns true if the user has all of the specified keys.
        // ---------------------------------------------------------------------------

        public bool HasAll(params string[] keys)
        {
            if (keys is null || keys.Length == 0)
                return false;

            foreach (var key in keys)
            {
                if (!Has(key))
                    return false;
            }

            return true;
        }

        // ---------------------------------------------------------------------------
        // Reset
        // Clears all loaded entitlements and resets the load flag so that
        // LoadAsync can be called again for the next authenticated session.
        // Must be called before LoadAsync on every login to prevent stale
        // entitlements from a previous session being carried forward.
        // ---------------------------------------------------------------------------

        public void Reset()
        {
            _entitlements.Clear();
            _isLoaded = false;
        }
    }
}

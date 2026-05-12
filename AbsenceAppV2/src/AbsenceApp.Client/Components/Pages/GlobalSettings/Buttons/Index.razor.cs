// ===========================================================================
// File        : Index.razor.cs
// Namespace   : AbsenceApp.Client.Components.Pages.GlobalSettings.Buttons
// Author      : Michael
// Version     : 1.0.0
// Created     : 2026-05-12
// ---------------------------------------------------------------------------
// Purpose     : Code-behind partial class for the Buttons demo page.
//               Raw string literals (C# 11+) are NOT supported by the Razor
//               parser inside .razor files, so all multi-line code-snippet
//               constants live here where the C# compiler handles them.
// ===========================================================================

namespace AbsenceApp.Client.Components.Pages.GlobalSettings.Buttons;

public partial class Index
{
    protected override void OnInitialized()
    {
        Console.WriteLine("[DBG] Buttons/Index.OnInitialized — page component initialised");
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"[DBG] Buttons/Index.OnAfterRender — firstRender={firstRender}");
    }
    private const string _basicCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary">Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--secondary">Secondary</button>
        <button type="button" class="dsv2-btn dsv2-btn--success">Success</button>
        <button type="button" class="dsv2-btn dsv2-btn--danger">Danger</button>
        <button type="button" class="dsv2-btn dsv2-btn--warning">Warning</button>
        <button type="button" class="dsv2-btn dsv2-btn--info">Info</button>
        <button type="button" class="dsv2-btn dsv2-btn--light">Light</button>
        <button type="button" class="dsv2-btn dsv2-btn--dark">Dark</button>
        <button type="button" class="dsv2-btn dsv2-btn--link">Link</button>
        """;

    private const string _outlineCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--outline-primary">Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary">Secondary</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-success">Success</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-danger">Danger</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-warning">Warning</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-info">Info</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-light">Light</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-dark">Dark</button>
        <button type="button" class="dsv2-btn dsv2-btn--link">Link</button>
        """;

    private const string _lightCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--light kbp-light-primary">Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--light kbp-light-secondary">Secondary</button>
        <button type="button" class="dsv2-btn dsv2-btn--light kbp-light-success">Success</button>
        <button type="button" class="dsv2-btn dsv2-btn--light kbp-light-danger">Danger</button>
        <button type="button" class="dsv2-btn dsv2-btn--light kbp-light-warning">Warning</button>
        <button type="button" class="dsv2-btn dsv2-btn--light kbp-light-info">Info</button>
        """;

    private const string _iconCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary">
          <i class="bi bi-download"></i> Primary
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--secondary">
          <i class="bi bi-triangle"></i> Secondary
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-primary">
          <i class="bi bi-download"></i> Primary
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary">
          <i class="bi bi-triangle"></i> Secondary
        </button>
        """;

    private const string _radiusCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary kbp-pill">
          <i class="bi bi-download"></i> Primary
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--secondary kbp-pill">
          <i class="bi bi-triangle"></i> Secondary
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-primary kbp-pill">
          <i class="bi bi-download"></i> Primary
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary kbp-pill">
          <i class="bi bi-triangle"></i> Secondary
        </button>
        """;

    private const string _socialCode =
        """
        <button type="button" class="dsv2-btn kbp-social kbp-social--facebook">
          <i class="bi bi-facebook"></i> Facebook
        </button>
        <button type="button" class="dsv2-btn kbp-social kbp-social--twitter">
          <i class="bi bi-twitter-x"></i> Twitter
        </button>
        <button type="button" class="dsv2-btn kbp-social kbp-social--pinterest">
          <i class="bi bi-pinterest"></i> Pinterest
        </button>
        <button type="button" class="dsv2-btn kbp-social kbp-social--linkedin">
          <i class="bi bi-linkedin"></i> Linkedin
        </button>
        <button type="button" class="dsv2-btn kbp-social kbp-social--reddit">
          <i class="bi bi-reddit"></i> Reddit
        </button>
        """;

    private const string _disableCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary" disabled>Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--secondary" disabled>Secondary</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-primary" disabled>Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary" disabled>Secondary</button>
        """;

    private const string _activeCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary active">Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--secondary active">Secondary</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-primary active">Primary</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary active">Secondary</button>
        """;

    private const string _loadingCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary" disabled>
          <span class="kbp-spinner"></span> Loading...
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--secondary" disabled>
          <span class="kbp-spinner"></span> Wait...
        </button>
        <button type="button" class="dsv2-btn dsv2-btn--success" disabled>
          <span class="kbp-spinner"></span>
        </button>
        """;

    private const string _blockCode =
        """
        <div class="d-grid gap-2">
          <button type="button" class="dsv2-btn dsv2-btn--primary">Primary</button>
          <button type="button" class="dsv2-btn dsv2-btn--secondary">Secondary</button>
          <button type="button" class="dsv2-btn dsv2-btn--success">Success</button>
        </div>
        """;

    private const string _sizesCode =
        """
        <button type="button" class="dsv2-btn dsv2-btn--primary dsv2-btn--lg">Large</button>
        <button type="button" class="dsv2-btn dsv2-btn--primary">Default</button>
        <button type="button" class="dsv2-btn dsv2-btn--primary dsv2-btn--sm">Small</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary dsv2-btn--lg">Large</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary">Default</button>
        <button type="button" class="dsv2-btn dsv2-btn--outline-secondary dsv2-btn--sm">Small</button>
        """;

    private const string _groupCode =
        """
        <div class="kbp-btn-group" role="group">
          <button type="button" class="dsv2-btn dsv2-btn--primary kbp-group-btn">Left</button>
          <button type="button" class="dsv2-btn dsv2-btn--primary kbp-group-btn">Middle</button>
          <button type="button" class="dsv2-btn dsv2-btn--primary kbp-group-btn">Right</button>
        </div>
        <div class="kbp-btn-group" role="group">
          <button type="button" class="dsv2-btn dsv2-btn--outline-secondary kbp-group-btn">Left</button>
          <button type="button" class="dsv2-btn dsv2-btn--outline-secondary kbp-group-btn">Middle</button>
          <button type="button" class="dsv2-btn dsv2-btn--outline-secondary kbp-group-btn">Right</button>
        </div>
        """;
}

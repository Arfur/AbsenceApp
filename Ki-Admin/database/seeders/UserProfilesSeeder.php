<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Carbon\Carbon;
use Exception;

/**
 * =========================================================
 * Seeder    : UserProfilesSeeder
 * Project   : ki-admin - v1.0.0
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 *
 * Description:
 *   Imports user profiles from orchard_users.csv into the `user_profiles` table,
 *   linking user identity data with organizational affiliations.
 * =========================================================
 */
class UserProfilesSeeder extends Seeder
{
    public function run(): void
    {
        // Prefer the 'ki_' CSV in the Data Export folder when available, else fall back to Data/
        $preferred = base_path('database/seeders/Data Export/ki_user_profiles.csv');
        $fallback = base_path('database/seeders/Data/user_profiles.csv');
        if (file_exists($preferred)) {
            $csvPath = $preferred;
        } elseif (file_exists($fallback)) {
            $csvPath = $fallback;
        } else {
            $this->command->error("CSV file not found. Checked: {$preferred} and {$fallback}");
            return;
        }

        $handle = fopen($csvPath, 'r');
        if (! $handle) {
            $this->command->error("Failed to open CSV file at {$csvPath}");
            return;
        }

        $header = null;
        $records = [];
        while (($row = fgetcsv($handle)) !== false) {
            if ($header === null) {
                // Clean BOM from first header cell if present and trim all header names
                $row[0] = preg_replace('/^\xEF\xBB\xBF/', '', $row[0]);
                $header = array_map('trim', $row);
                continue;
            }

            // Ensure we get an associative record even when columns mismatch
            if (count($header) === count($row)) {
                $record = array_combine($header, $row);
            } else {
                $record = [];
                for ($i = 0; $i < count($header); $i++) {
                    $record[$header[$i]] = $row[$i] ?? null;
                }
            }
            $records[] = $record;
        }
        fclose($handle);

        // Debug: Show existing users in database
        $existingUsers = DB::table('users')->select('id', 'user_id', 'email')->get();
        $this->command->info("Existing users in database:");
        foreach ($existingUsers as $user) {
            $this->command->info("  ID: {$user->id}, user_id: {$user->user_id}, Email: {$user->email}");
        }

        $count = 0;
        foreach ($records as $record) {
            if (empty($record['user_id'])) {
                continue;
            }
            
            // Debug: Log what user_id we're trying to insert
            $user_id = trim($record['user_id']); // Remove any whitespace
            $this->command->info("Processing user_id: '{$user_id}'");
            
            // Check if the user_id exists in the users table and get the actual id
            $user = DB::table('users')->where('user_id', $user_id)->first();
            if (!$user) {
                $this->command->warn("Skipping user profile for user_id {$user_id} - user does not exist");
                continue;
            }
            
            // Use the actual id from the users table for the foreign key
            $actual_user_id = $user->id;
            
            // Normalize and validate department_id
            $department_id = $record['department_id'] ?? null;
            if (is_string($department_id)) {
                $department_id = trim($department_id);
                if (strtolower($department_id) === 'null' || $department_id === '') {
                    $department_id = null;
                }
            }
            if ($department_id !== null) {
                $department_id = is_numeric($department_id) ? (int)$department_id : null;
                if ($department_id !== null && !DB::table('departments')->where('id', $department_id)->exists()) {
                    $this->command->warn("Invalid department_id {$department_id} for user_id {$user_id} - setting to null");
                    $department_id = null;
                }
            }

            // Normalize and validate job_title_id
            $job_title_id = $record['job_title_id'] ?? null;
            if (is_string($job_title_id)) {
                $job_title_id = trim($job_title_id);
                if (strtolower($job_title_id) === 'null' || $job_title_id === '') {
                    $job_title_id = null;
                }
            }
            if ($job_title_id !== null) {
                $job_title_id = is_numeric($job_title_id) ? (int)$job_title_id : null;
                if ($job_title_id !== null && !DB::table('job_titles')->where('id', $job_title_id)->exists()) {
                    $this->command->warn("Invalid job_title_id {$job_title_id} for user_id {$user_id} - setting to null");
                    $job_title_id = null;
                }
            }
            // Convert date_of_birth to MySQL format if needed
            $date_of_birth = null;
            if (!empty($record['date_of_birth']) && strtolower($record['date_of_birth']) != 'null') {
                try {
                    // Try to parse DD/MM/YYYY format
                    $date_of_birth = Carbon::createFromFormat('d/m/Y', $record['date_of_birth'])->format('Y-m-d');
                } catch (Exception $e) {
                    // If that fails, try other common formats
                    try {
                        $date_of_birth = Carbon::parse($record['date_of_birth'])->format('Y-m-d');
                    } catch (Exception $e2) {
                        // If all fails, set to null
                        $date_of_birth = null;
                    }
                }
            }

            // Convert created_at and updated_at to MySQL format if needed
            $created_at = Carbon::now();
            if (!empty($record['created_at']) && strtolower($record['created_at']) != 'null') {
                try {
                    // Try to parse DD/MM/YYYY HH:MM format
                    $created_at = Carbon::createFromFormat('d/m/Y H:i', $record['created_at']);
                } catch (Exception $e) {
                    $created_at = Carbon::now();
                }
            }

            $updated_at = Carbon::now();
            if (!empty($record['updated_at']) && strtolower($record['updated_at']) != 'null') {
                try {
                    // Try to parse DD/MM/YYYY HH:MM format
                    $updated_at = Carbon::createFromFormat('d/m/Y H:i', $record['updated_at']);
                } catch (Exception $e) {
                    $updated_at = Carbon::now();
                }
            }

            // Normalize school_id
            $school_id = $record['school_id'] ?? null;
            if (is_string($school_id)) {
                $school_id = trim($school_id);
                if (strtolower($school_id) === 'null' || $school_id === '') {
                    $school_id = null;
                }
            }
            if ($school_id !== null) {
                $school_id = is_numeric($school_id) ? (int)$school_id : null;
                if ($school_id !== null && !DB::table('schools')->where('id', $school_id)->exists()) {
                    $this->command->warn("Invalid school_id {$school_id} for user_id {$user_id} - setting to null");
                    $school_id = null;
                }
            }

            DB::table('user_profiles')->updateOrInsert([
                'user_id' => $actual_user_id,
            ], [
                'first_name' => $record['first_name'] ?? null,
                'last_name' => $record['last_name'] ?? null,
                'preferred_name' => $record['preferred_name'] ?? null,
                'title' => $record['title'] ?? null,
                'date_of_birth' => $date_of_birth,
                'profile_picture_url' => $record['profile_picture_url'] ?? null,
                'bio' => $record['bio'] ?? null,
                'gender' => $record['gender'] ?? null,
                'timezone' => $record['timezone'] ?? null,
                'language' => $record['language'] ?? null,
                'department_id' => $department_id,
                'job_title_id' => $job_title_id,
                'school_id' => $school_id,
                'created_at' => $created_at,
                'updated_at' => $updated_at,
            ]);
            $count++;
        }
        $this->command->info("✅ Seeded {$count} user profiles.");
    }
}

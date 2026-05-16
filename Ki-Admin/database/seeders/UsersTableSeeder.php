<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

/**
 * =========================================================
 * Seeder    : A05_UsersTableSeeder
 * Project   : ki-admin - v1.0.0
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 *
 * Description:
 *   Imports users from CSV into the `users` table,
 *   mapping only columns that exist in our schema
 *   and skipping extraneous CSV fields.
 * =========================================================
 */
class UsersTableSeeder extends Seeder
{
    public function run(): void
    {
        $kiPath = base_path('database/seeders/Data Export/ki_users.csv');
        $csvPath = file_exists($kiPath) ? $kiPath : base_path('database/seeders/Data/users.csv');
        if (!file_exists($csvPath)) {
            $this->command->error("CSV file not found at {$csvPath}");
            return;
        }

        $file = fopen($csvPath, 'r');
        if ($file === false) {
            $this->command->error("Unable to open CSV: {$csvPath}");
            return;
        }

        $header = fgetcsv($file);
        $count = 0;
        while (($row = fgetcsv($file)) !== false) {
            $data = count($header) === count($row) ? array_combine($header, $row) : null;
            if ($data === null) {
                continue; // skip malformed rows
            }
            if (empty($data['email'])) {
                continue;
            }
            $email_verified_at = (empty($data['email_verified_at']) || strtolower($data['email_verified_at']) == 'null') ? null : date('Y-m-d H:i:s', strtotime($data['email_verified_at']));
            $created_at = (empty($data['created_at']) || strtolower($data['created_at']) == 'null') ? now() : date('Y-m-d H:i:s', strtotime($data['created_at']));
            $updated_at = (empty($data['updated_at']) || strtolower($data['updated_at']) == 'null') ? now() : date('Y-m-d H:i:s', strtotime($data['updated_at']));
            $otp_expires_at = (empty($data['otp_expires_at']) || strtolower($data['otp_expires_at']) == 'null') ? null : date('Y-m-d H:i:s', strtotime($data['otp_expires_at']));

            DB::table('users')->updateOrInsert([
                'email' => $data['email'],
            ], [
                'user_id' => $data['user_id'] ?? null,
                'username' => $data['username'] ?? null,
                'email_verified_at' => $email_verified_at,
                'password' => $data['password'] ?? null,
                'status' => $data['status'] ?? 'active',
                'remember_token' => $data['remember_token'] ?? null,
                'role_type_id' => $data['role_type_id'] ?? null,
                'created_at' => $created_at,
                'updated_at' => $updated_at,
                'otp_code' => $data['otp_code'] ?? null,
                'otp_expires_at' => $otp_expires_at,
                'is_verified' => isset($data['is_verified']) ? (bool)$data['is_verified'] : 0,
            ]);
            $count++;
        }
        fclose($file);
        $this->command->info("✅ Seeded {$count} users from {$csvPath}.");
    }
}

<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : DatabaseSeeder.php
 * 
 * Author    : Michael Battle
 * Created On: 27/Aug/2025
 * Updated On: [Last Updated Date]
 * 
 * Description:
 * Main database seeder runner. Add seeders in dependency order.
 * Test each seeder individually before including in the run.
 * 
 * Origin:
 * Used for initial database population and test data setup.
 * 
 * Changes:
 * - Initial template created
 * - Section comments added for each seeder
 * =========================================================
 */

namespace Database\Seeders;
use Illuminate\Database\Seeder;

class DatabaseSeeder extends Seeder
{
    public function run()
    {
    /**
     * =========================================================
     * Section: Role Types Seeder
     * Description:
     *   Populates the `role_types` table with system roles used
     *   for access control and FK references from `users`.
     * Linked Tables: role_types
     * =========================================================
     */
          $this->call(RoleTypesTableSeeder::class);

    /**
     * =========================================================
     * Section: Job Group Seeder
     * Description:
     *   Seeds `job_group` with role clusters used by job titles
     *   and user profiles.
     * Linked Tables: job_group
     * =========================================================
     */
          $this->call(JobGroupTableSeeder::class);

    /**
     * =========================================================
     * Section: Job Titles Seeder
     * Description:
     *   Populates `job_titles` used by user profiles and assignments.
     * Linked Tables: job_titles
     * =========================================================
     */
          $this->call(JobTitlesTableSeeder::class);

    /**
     * =========================================================
     * Section: Schools Seeder
     * Description:
     *   Upserts `schools` used by departments and user profiles.
     * Linked Tables: schools
     * =========================================================
     */
          $this->call(SchoolsTableSeeder::class);

    /**
     * =========================================================
     * Section: Departments Seeder
     * Description:
     *   Populates `departments` used by user profiles and org mapping.
     * Linked Tables: departments
     * =========================================================
     */
          $this->call(DepartmentSeeder::class);

    /**
     * =========================================================
     * Section: Users Seeder
     * Description:
     *   Imports `users` and links to role_types, job_titles and departments.
     * Linked Tables: users, role_types, job_titles, departments
     * =========================================================
     */
          $this->call(UsersTableSeeder::class);

      // UserProfilesTableSeeder removed; using UserProfilesSeeder (legacy/mapping) instead

    /**
      * =========================================================
      * Section: User Profiles (legacy/orchard)
      * Description:
      *   Additional legacy profile imports; kept separate to allow
      *   incremental testing and troubleshooting.
      * Linked Tables: user_profiles
      * =========================================================
      */
            $this->call(UserProfilesSeeder::class);

    /**
      * =========================================================
      * Section: Menu Items
      * Description:
      *   Upserts `menu_items` used across role mappings and navigation.
      * Linked Tables: menu_items
      * =========================================================
      */
            $this->call(MenuItemsSeeder::class);

    /**
      * =========================================================
      * Section: User Menu Items
      * Description:
      *   Imports per-user menu item preferences and favorites.
      * Linked Tables: user_menu_items, users, menu_items
      * =========================================================
      */
            $this->call(UserMenuItemsSeeder::class);

    /**
      * =========================================================
      * Section: Role Menu Items
      * Description:
      *   Grants/denies menu access per role type. Uses upserts to be idempotent.
      * Linked Tables: role_menu_items, role_types, menu_items
      * =========================================================
      */
            $this->call(RoleMenuItemsSeeder::class);

      // AdminUserSeeder removed by request to avoid creating test users
    }
}

<?php
/**
 * =========================================================
 * File: UserProfile.php
 * Project: Ki-Admin - v1.0.0
 * 
 * Description:
 *   Eloquent model for user_profiles table.
 *   Handles extended user data including:
 *     - Personal and contact details
 *     - Profile picture and biography
 *     - Job grouping and organizational mappings
 * 
 * Updated: 2025-08-11
 * Author : Michael Battle
 * =========================================================
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

use App\Models\User;
use App\Models\JobGroup;
use App\Models\Department;
use App\Models\JobTitle;
use App\Models\School;

class UserProfile extends Model
{
    /* =====================================================
     * Section: Table & Timestamps
     * Description: Table name and timestamp flags
     * ===================================================== */
    protected $table = 'user_profiles';
    public $timestamps = true;

    /* =====================================================
     * Section: Mass Assignment
     * Description: Fields that can be bulk-assigned
     * ===================================================== */
    protected $fillable = [
        'user_id',
        'first_name',
        'last_name',
        'preferred_name',
        'title',
        'date_of_birth',
        'profile_picture_url',
        'biography',
        'phone_number',
        'job_group_id',
        'gender',
        'timezone',
        'language',
        'department_id',
        'job_title_id',
        'school_id',
    ];

    /* =====================================================
     * Section: User Relation
     * Description: Link to primary users table
     * ===================================================== */
    public function user(): BelongsTo
    {
        return $this->belongsTo(User::class, 'user_id');
    }

    /* =====================================================
     * Section: Job Group Relation
     * Description: Classification from job_group table
     * ===================================================== */
    public function jobGroup(): BelongsTo
    {
        return $this->belongsTo(JobGroup::class, 'job_group_id');
    }

    /* =====================================================
     * Section: Department Relation
     * Description: Organizational department link
     * ===================================================== */
    public function department(): BelongsTo
    {
        return $this->belongsTo(Department::class, 'department_id');
    }

    /* =====================================================
     * Section: Job Title Relation
     * Description: Standard job titles reference
     * ===================================================== */
    public function jobTitle(): BelongsTo
    {
        return $this->belongsTo(JobTitle::class, 'job_title_id');
    }

    /* =====================================================
     * Section: School Relation
     * Description: Link to associated school record
     * ===================================================== */
    public function school(): BelongsTo
    {
        return $this->belongsTo(School::class, 'school_id');
    }
}

<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : SidebarDebugController.php
 * Author    : Michael Battle
 * Created On: 19/Aug/2025
 * Description:
 * Handles POST requests for sidebar JS debug output.
 * =========================================================
 */

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Http\Response;

class SidebarDebugController extends Controller
{
    /**
     * Store sidebar JS debug output to storage/sidebar_js_debug.txt
     * =========================================================
     * @param Request $request
     * @return Response
     */
    /**
     * Store sidebar JS debug output to storage/sidebar_js_debug.txt
     * =========================================================
     * @param Request $request
     * @return \Illuminate\Http\JsonResponse
     */
    public function store(Request $request)
    {
        $debug = $request->input('debug');
        file_put_contents(storage_path('sidebar_js_debug.txt'), $debug, LOCK_EX);
        return response()->json(['status' => 'ok']);
    }
}

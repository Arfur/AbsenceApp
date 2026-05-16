

{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : footer.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 06/Aug/2025
 * Updated On: 07/Aug/2025
 * 
 * Description:
 * Main footer layout component providing copyright information
 * and help links. Styled as fixed banner at bottom of viewport
 * with proper sidebar offset to match Projects.png reference.
 * 
 * Origin:
 * Footer template included in layout.master for all main pages
 * 
 * Changes:
 * - Added fixed positioning to match header banner style
 * - Applied proper sidebar offset using CSS variables
 * - Styled as banner with background and border
 * - Added z-index for proper layering
 * =========================================================
 */
--}}

{{-- =========================================================
 * Section: Fixed Footer Banner
 * Description: Main footer with copyright and help links
 * Positioned as fixed banner at bottom with sidebar offset
 * ========================================================= --}}
<!-- Footer Section starts-->
<footer style="position: fixed; bottom: 0; left: var(--sidebar-width, 18rem); right: 0; z-index: 1000; background: white; border-top: 1px solid #e5e7eb; padding: 12px 0; transition: left 0.3s ease;">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-9 col-12">
                <p class="footer-text f-w-600 mb-0">Copyright © 2025 ki-admin. All rights reserved 💖 V1.0.0</p>
            </div>
            <div class="col-md-3">
                <div class="footer-text text-end">
                    <a class="f-w-500 text-primary" href="mailto:teqlathemes@gmail.com"> Need Help <i
                            class="ti ti-help"></i></a>
                </div>
            </div>
        </div>
    </div>
</footer>
<!-- Footer Section ends-->

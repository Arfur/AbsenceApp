<style>
/**
 * =========================================================
 * Section: Sidebar Chevron Styles (chg0243x)
 * Description: Chevron indicator rotation for expanded/collapsed states
 * =========================================================
 */
.sidebar-accordion .main-nav .another-level > a .indicator-icon {
    transform: rotate(0deg);
}
.sidebar-accordion .main-nav .another-level > a[aria-expanded="true"] .indicator-icon {
    transform: rotate(90deg);
}
/**
 * =========================================================
 * Section: Sidebar Nested List Padding (chg0244x)
 * Description: Ensure nested lists are visually nested with left padding
 * =========================================================
 */
.sidebar-accordion .main-nav ul { padding-left: 0.75rem; }
</style>

<script>
/**
 * =========================================================
 * Section: Sidebar Collapse Init (chg0245x)
 * Description: Initialize collapse state and keep aria-expanded attributes
 *              in sync with Bootstrap collapse show/hide events.
 * =========================================================
 */
document.addEventListener('DOMContentLoaded', function() {
    document.querySelectorAll('.sidebar-accordion .collapse').forEach(function(collapseEl) {
        collapseEl.addEventListener('shown.bs.collapse', function (e) {
            var parentA = document.querySelector('[data-bs-target="#' + this.id + '"]');
            if (parentA) parentA.setAttribute('aria-expanded', 'true');
        });
        collapseEl.addEventListener('hidden.bs.collapse', function (e) {
            var parentA = document.querySelector('[data-bs-target="#' + this.id + '"]');
            if (parentA) parentA.setAttribute('aria-expanded', 'false');
        });
    });

    /**
     * =========================================================
     * Section: Sidebar Collapse Default State (chg0246x)
     * Description: Ensure top-level groups use Bootstrap Collapse API to
     *              show/hide according to server-rendered aria-expanded state.
     * =========================================================
     */
    document.querySelectorAll('.sidebar-accordion .main-nav .another-level').forEach(function(li){
        var a = li.querySelector('a[data-bs-toggle="collapse"]');
        var collapseId = a && a.getAttribute('data-bs-target') ? a.getAttribute('data-bs-target').replace('#','') : null;
        var collapseEl = collapseId ? document.getElementById(collapseId) : null;
        if (collapseEl) {
            var shouldShow = a.getAttribute('aria-expanded') === 'true';
            var bsCollapse = bootstrap.Collapse.getOrCreateInstance(collapseEl, {toggle: false});
            if (shouldShow) {
                bsCollapse.show();
            } else {
                bsCollapse.hide();
            }
        }
    });
});
</script>

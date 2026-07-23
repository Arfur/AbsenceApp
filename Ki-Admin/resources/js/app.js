import './bootstrap';

// Handle parent menu navigation + collapse
document.addEventListener('DOMContentLoaded', function() {
    document.querySelectorAll('a[data-bs-toggle="collapse"]').forEach(link => {
        link.addEventListener('click', function(e) {
            // Only handle parent items (those with collapse targets)
            const href = this.getAttribute('href');
            const target = this.getAttribute('data-bs-target');
            
            if (href && href !== '#' && target) {
                e.preventDefault(); // Prevent immediate navigation
                
                // Toggle the collapse first
                const targetElement = document.querySelector(target);
                if (targetElement) {
                    const bsCollapse = new bootstrap.Collapse(targetElement, { toggle: true });
                }
                
                // Navigate after a small delay to allow collapse animation
                setTimeout(() => {
                    window.location.href = href;
                }, 100);
            }
        });
    });
});

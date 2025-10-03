// Animated counter for statistics
function animateCounter(element, target, duration = 2000) {
    let start = 0;
    const increment = target / (duration / 16);
    const timer = setInterval(() => {
        start += increment;
        if (start >= target) {
            element.textContent = target;
            clearInterval(timer);
        } else {
            element.textContent = Math.floor(start);
        }
    }, 16);
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', function () {
    // Animate statistics counters
    const incidentsElement = document.getElementById('incidentsCount');
    const volunteersElement = document.getElementById('volunteersCount');
    const donationsElement = document.getElementById('donationsCount');
    const communitiesElement = document.getElementById('communitiesCount');

    if (incidentsElement) {
        animateCounter(incidentsElement, 47);
        animateCounter(volunteersElement, 123);
        animateCounter(donationsElement, 892);
        animateCounter(communitiesElement, 56);
    }

    // Auto-dismiss alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});
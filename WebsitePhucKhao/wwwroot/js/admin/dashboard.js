// Dashboard JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Khởi tạo tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Khởi tạo popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Cập nhật thời gian thực
    function updateRealTime() {
        var now = new Date();
        var timeString = now.toLocaleTimeString('vi-VN');
        var dateString = now.toLocaleDateString('vi-VN');
        
        // Cập nhật thời gian nếu có phần tử hiển thị
        var timeElement = document.querySelector('.d-real-time');
        if (timeElement) {
            timeElement.textContent = timeString + ' ' + dateString;
        }
    }

    // Cập nhật thời gian mỗi giây
    setInterval(updateRealTime, 1000);
    updateRealTime(); // Cập nhật ngay lập tức

    // Xử lý sự kiện cho các nút hành động
    var actionButtons = document.querySelectorAll('.d-action-btn');
    actionButtons.forEach(function(button) {
        button.addEventListener('click', function(e) {
            // Thêm hiệu ứng ripple khi click
            var ripple = document.createElement('span');
            ripple.classList.add('d-ripple');
            this.appendChild(ripple);

            var rect = button.getBoundingClientRect();
            var size = Math.max(rect.width, rect.height);
            var x = e.clientX - rect.left - size / 2;
            var y = e.clientY - rect.top - size / 2;

            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';

            setTimeout(function() {
                ripple.remove();
            }, 600);
        });
    });

    // Thêm hiệu ứng hover cho các card
    var statCards = document.querySelectorAll('.d-stat-card');
    statCards.forEach(function(card) {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.transition = 'transform 0.3s ease';
        });

        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });

    // Thêm hiệu ứng loading cho bảng
    var tables = document.querySelectorAll('.d-table');
    tables.forEach(function(table) {
        var rows = table.querySelectorAll('tbody tr');
        rows.forEach(function(row, index) {
            row.style.opacity = '0';
            row.style.transform = 'translateY(20px)';
            row.style.transition = 'all 0.3s ease';
            
            setTimeout(function() {
                row.style.opacity = '1';
                row.style.transform = 'translateY(0)';
            }, index * 100);
        });
    });
}); 
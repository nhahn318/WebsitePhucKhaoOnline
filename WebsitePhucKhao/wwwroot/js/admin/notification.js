// Khởi tạo các biến toàn cục
let notificationInterval;

// Hàm cập nhật thông báo
function updateNotifications() {
    $.get(notificationUrls.getNotifications, function(data) {
        // Cập nhật số lượng thông báo
        $('#notificationCount').text(data.count);
        
        // Cập nhật danh sách thông báo
        var notificationList = $('#notificationList');
        notificationList.empty();
        
        if (data.notifications.length > 0) {
            data.notifications.forEach(function(notification) {
                var notificationItem = $('<div>')
                    .addClass('notification-item')
                    .attr('data-ma-don', notification.maDon)
                    .attr('data-ma-mon-hoc', notification.maMonHoc)
                    .html(`
                        <div class="notification-content">
                            <p class="mb-0">${notification.message}</p>
                            <small class="text-muted">${notification.time}</small>
                        </div>
                    `);
                notificationList.append(notificationItem);
            });
        } else {
            notificationList.html('<div class="text-center p-3">Không có thông báo mới</div>');
        }
    });
}

// Hàm xử lý sự kiện click vào thông báo
function handleNotificationClick(e) {
    e.preventDefault();
    var maDon = $(this).data('ma-don');
    if (maDon) {
        var maMonHoc = $(this).data('ma-mon-hoc');
        if (maMonHoc) {
            window.location.href = `${notificationUrls.chiTietPhucKhao}?id=${maDon}&maMonHoc=${maMonHoc}`;
        }
    }
}

// Hàm xử lý sự kiện click vào chuông thông báo
function handleBellClick(e) {
    e.preventDefault();
    const dropdownMenu = this.nextElementSibling;
    if (dropdownMenu) {
        dropdownMenu.classList.toggle('show');
    }
}

// Hàm xử lý click ra ngoài dropdown
function handleOutsideClick(e) {
    const notificationDropdown = document.getElementById('notificationDropdown');
    const dropdownMenu = notificationDropdown?.nextElementSibling;
    
    if (dropdownMenu && !notificationDropdown.contains(e.target) && !dropdownMenu.contains(e.target)) {
        dropdownMenu.classList.remove('show');
    }
}

// Hàm khởi tạo thông báo
function initializeNotifications() {
    // Xử lý click vào thông báo
    $(document).on('click', '.notification-item', handleNotificationClick);

    // Xử lý click vào chuông thông báo
    const notificationDropdown = document.getElementById('notificationDropdown');
    if (notificationDropdown) {
        notificationDropdown.addEventListener('click', handleBellClick);
    }

    // Xử lý click ra ngoài dropdown
    document.addEventListener('click', handleOutsideClick);

    // Cập nhật thông báo ngay lập tức
    updateNotifications();

    // Cập nhật thông báo mỗi 30 giây
    notificationInterval = setInterval(updateNotifications, 30000);
}

// Hàm dọn dẹp khi component bị unmount
function cleanupNotifications() {
    // Xóa event listeners
    $(document).off('click', '.notification-item');
    document.removeEventListener('click', handleOutsideClick);
    
    // Dừng interval
    if (notificationInterval) {
        clearInterval(notificationInterval);
    }
}

// Khởi tạo khi DOM đã tải xong
document.addEventListener('DOMContentLoaded', function() {
    initializeNotifications();
});

// Dọn dẹp khi trang bị unload
window.addEventListener('beforeunload', function() {
    cleanupNotifications();
}); 
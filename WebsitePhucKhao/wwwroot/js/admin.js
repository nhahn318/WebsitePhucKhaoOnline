// Đảm bảo các biến toàn cục chỉ được khai báo một lần
let allDropdown, sidebar, chartInitialized;

if (typeof allDropdown === 'undefined') {
    allDropdown = document.querySelectorAll('#sidebar .side-dropdown');
    sidebar = document.getElementById('sidebar');

    allDropdown.forEach(item => {
        const a = item.parentElement.querySelector('a:first-child');
        a.addEventListener('click', function (e) {
            e.preventDefault();

            if (!this.classList.contains('active')) {
                allDropdown.forEach(i => {
                    const aLink = i.parentElement.querySelector('a:first-child');
                    aLink.classList.remove('active');
                    i.classList.remove('show');
                });
            }

            this.classList.toggle('active');
            item.classList.toggle('show');
        });
    });

    // SIDEBAR COLLAPSE
    const toggleSidebar = document.querySelector('nav .toggle-sidebar');
    const allSideDivider = document.querySelectorAll('#sidebar .divider');

    if (sidebar.classList.contains('hide')) {
        allSideDivider.forEach(item => {
            item.textContent = '-';
        });
        allDropdown.forEach(item => {
            const a = item.parentElement.querySelector('a:first-child');
            a.classList.remove('active');
            item.classList.remove('show');
        });
    } else {
        allSideDivider.forEach(item => {
            item.textContent = item.dataset.text;
        });
    }

    toggleSidebar.addEventListener('click', function () {
        sidebar.classList.toggle('hide');

        if (sidebar.classList.contains('hide')) {
            allSideDivider.forEach(item => {
                item.textContent = '-';
            });
            allDropdown.forEach(item => {
                const a = item.parentElement.querySelector('a:first-child');
                a.classList.remove('active');
                item.classList.remove('show');
            });
        } else {
            allSideDivider.forEach(item => {
                item.textContent = item.dataset.text;
            });
        }
    });

    sidebar.addEventListener('mouseleave', function () {
        if (this.classList.contains('hide')) {
            allDropdown.forEach(item => {
                const a = item.parentElement.querySelector('a:first-child');
                a.classList.remove('active');
                item.classList.remove('show');
            });
            allSideDivider.forEach(item => {
                item.textContent = '-';
            });
        }
    });

    sidebar.addEventListener('mouseenter', function () {
        if (this.classList.contains('hide')) {
            allDropdown.forEach(item => {
                const a = item.parentElement.querySelector('a:first-child');
                a.classList.remove('active');
                item.classList.remove('show');
            });
            allSideDivider.forEach(item => {
                item.textContent = item.dataset.text;
            });
        }
    });

    // PROFILE DROPDOWN
    const profile = document.querySelector('nav .profile');
    const imgProfile = profile.querySelector('img');
    const dropdownProfile = profile.querySelector('.profile-link');

    imgProfile.addEventListener('click', function () {
        dropdownProfile.classList.toggle('show');
    });

    // MENU
    const allMenu = document.querySelectorAll('main .content-data .head .menu');

    allMenu.forEach(item => {
        const icon = item.querySelector('.icon');
        const menuLink = item.querySelector('.menu-link');

        icon.addEventListener('click', function () {
            menuLink.classList.toggle('show');
        });
    });

    window.addEventListener('click', function (e) {
        if (e.target !== imgProfile && !dropdownProfile.contains(e.target)) {
            dropdownProfile.classList.remove('show');
        }

        allMenu.forEach(item => {
            const icon = item.querySelector('.icon');
            const menuLink = item.querySelector('.menu-link');

            if (e.target !== icon && !menuLink.contains(e.target)) {
                menuLink.classList.remove('show');
            }
        });
    });

    // PROGRESSBAR
    const allProgress = document.querySelectorAll('main .card .progress');

    allProgress.forEach(item => {
        item.style.setProperty('--value', item.dataset.value);
    });
}

// APEXCHART
if (typeof chartInitialized === 'undefined') {
    chartInitialized = false;

    const chartElement = document.querySelector("#chart");
    if (chartElement) {
        var options = {
            series: [{
                name: 'series1',
                data: [31, 40, 28, 51, 42, 109, 100]
            }, {
                name: 'series2',
                data: [11, 32, 45, 32, 34, 52, 41]
            }],
            chart: {
                height: 350,
                type: 'area'
            },
            dataLabels: {
                enabled: false
            },
            stroke: {
                curve: 'smooth'
            },
            xaxis: {
                type: 'datetime',
                categories: ["2018-09-19T00:00:00.000Z", "2018-09-19T01:30:00.000Z", "2018-09-19T02:30:00.000Z", "2018-09-19T03:30:00.000Z", "2018-09-19T04:30:00.000Z", "2018-09-19T05:30:00.000Z", "2018-09-19T06:30:00.000Z"]
            },
            tooltip: {
                x: {
                    format: 'dd/MM/yy HH:mm'
                },
            },
        };

        var chart = new ApexCharts(chartElement, options);
        chart.render();
        chartInitialized = true;
    }
}

// Xử lý thao tác xóa
function initializeDeleteActions() {
    // Kiểm tra và xóa toast cũ nếu có
    const existingToast = document.querySelector('.alert-toast');
    if (existingToast) {
        existingToast.remove();
    }

    // Tạo toast thông báo mới
    const toastHtml = `
        <div class="alert-toast">
            <i class="bi"></i>
            <span class="toast-message"></span>
        </div>
    `;
    document.body.insertAdjacentHTML('beforeend', toastHtml);

    const toast = document.querySelector('.alert-toast');

    // Xử lý click nút xóa
    document.querySelectorAll('.btn-delete').forEach(btn => {
        // Loại bỏ tất cả event listeners cũ
        const newBtn = btn.cloneNode(true);
        btn.parentNode.replaceChild(newBtn, btn);
        
        newBtn.addEventListener('click', function(e) {
            // Ngăn chặn hành vi mặc định
            e.preventDefault();
            e.stopPropagation();
            
            const deleteUrl = this.getAttribute('data-delete-url');
            const itemName = this.getAttribute('data-item-name') || 'mục này';

            // Hiển thị thông báo xác nhận
            showToast('confirm', `Bạn có chắc chắn muốn xóa ${itemName} không?`, async () => {
                try {
                    // Thêm class loading
                    this.classList.add('loading');

                    // Gọi API xóa
                    const response = await fetch(deleteUrl, {
                        method: 'POST',
                        headers: {
                            'X-Requested-With': 'XMLHttpRequest'
                        }
                    });

                    if (response.ok) {
                        // Hiển thị thông báo thành công
                        showToast('success', 'Xóa thành công!');
                        
                        // Reload trang sau 1.5s
                        setTimeout(() => {
                            window.location.reload();
                        }, 1500);
                    } else {
                        throw new Error('Xóa thất bại');
                    }
                } catch (error) {
                    // Hiển thị thông báo lỗi
                    showToast('error', 'Xóa thất bại. Vui lòng thử lại.');
                    this.classList.remove('loading');
                }
            });

            // Ngăn chặn sự kiện nổi bọt
            return false;
        });
    });

    // Hàm hiển thị toast
    function showToast(type, message, onConfirm = null) {
        // Xóa toast cũ nếu có
        const oldConfirmBtn = toast.querySelector('.toast-confirm-btn');
        if (oldConfirmBtn) {
            oldConfirmBtn.remove();
        }
        
        const toastIcon = toast.querySelector('i');
        const toastMessage = toast.querySelector('.toast-message');
        
        // Ẩn toast trước khi thay đổi nội dung
        toast.classList.remove('show');
        
        // Đợi animation kết thúc
        setTimeout(() => {
            toast.className = `alert-toast ${type}`;
            toastIcon.className = `bi ${type === 'success' ? 'bi-check-circle' : type === 'error' ? 'bi-x-circle' : 'bi-question-circle'}`;
            toastMessage.textContent = message;
            
            // Thêm nút xác nhận nếu là thông báo confirm
            if (type === 'confirm') {
                const confirmBtn = document.createElement('button');
                confirmBtn.className = 'toast-confirm-btn';
                confirmBtn.textContent = 'Xác nhận';
                confirmBtn.onclick = () => {
                    toast.classList.remove('show');
                    if (onConfirm) onConfirm();
                };
                toast.appendChild(confirmBtn);
            }
            
            toast.classList.add('show');
            
            // Tự động ẩn sau 3s nếu không phải là thông báo confirm
            if (type !== 'confirm') {
                setTimeout(() => {
                    toast.classList.remove('show');
                }, 3000);
            }
        }, 300); // Đợi 300ms cho animation ẩn hoàn tất
    }
}

// Khởi tạo khi DOM đã tải xong
document.addEventListener('DOMContentLoaded', function() {
    initializeDeleteActions();
});




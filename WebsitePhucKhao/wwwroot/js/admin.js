// Admin Layout JavaScript

$(document).ready(function() {
    // Xử lý AJAX cho tất cả các form
    $('form').on('submit', function(e) {
        e.preventDefault();
        const form = $(this);
        const submitBtn = form.find('button[type="submit"]');
        const originalText = submitBtn.html();
        
        // Thêm loading state
        submitBtn.html('<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...').prop('disabled', true);
        
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function(response) {
                if (response.success) {
                    showNotification('success', response.message || 'Thao tác thành công!');
                    if (response.redirect) {
                        setTimeout(() => {
                            window.location.href = response.redirect;
                        }, 1000);
                    }
                } else {
                    showNotification('error', response.message || 'Có lỗi xảy ra!');
                }
            },
            error: function() {
                showNotification('error', 'Có lỗi xảy ra! Vui lòng thử lại.');
            },
            complete: function() {
                submitBtn.html(originalText).prop('disabled', false);
            }
        });
    });

    // Xử lý AJAX cho các link
    $('a[data-ajax="true"]').on('click', function(e) {
        e.preventDefault();
        const link = $(this);
        const url = link.attr('href');
        
        $.ajax({
            url: url,
            type: 'GET',
            success: function(response) {
                $('#mainContent').html(response);
            },
            error: function() {
                showNotification('error', 'Có lỗi xảy ra! Vui lòng thử lại.');
            }
        });
    });

    // Xử lý AJAX cho các nút action
    $('.btn-action').on('click', function(e) {
        e.preventDefault();
        const btn = $(this);
        const url = btn.data('url');
        const action = btn.data('action');
        
        if (confirm('Bạn có chắc chắn muốn thực hiện thao tác này?')) {
            $.ajax({
                url: url,
                type: 'POST',
                data: { action: action },
                success: function(response) {
                    if (response.success) {
                        showNotification('success', response.message || 'Thao tác thành công!');
                        if (response.reload) {
                            setTimeout(() => {
                                location.reload();
                            }, 1000);
                        }
                    } else {
                        showNotification('error', response.message || 'Có lỗi xảy ra!');
                    }
                },
                error: function() {
                    showNotification('error', 'Có lỗi xảy ra! Vui lòng thử lại.');
                }
            });
        }
    });

    // Hàm hiển thị thông báo
    function showNotification(type, message) {
        const notification = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        
        $('#notificationContainer').html(notification);
        
        setTimeout(() => {
            $('.alert').alert('close');
        }, 5000);
    }

    // Sidebar Toggle
    $('#sidebarToggle').on('click', function() {
        const sidebar = $('#sidebar');
        const mainContent = $('#mainContent');
        
        // Thêm class để bắt đầu animation
        sidebar.addClass('transitioning');
        mainContent.addClass('transitioning');
        
        // Toggle classes
        sidebar.toggleClass('collapsed');
        mainContent.toggleClass('expanded');
        
        // Xóa class transitioning sau khi animation hoàn tất
        setTimeout(() => {
            sidebar.removeClass('transitioning');
            mainContent.removeClass('transitioning');
        }, 300);
    });

    // Xử lý hover effect cho menu items
    $('#sidebar ul li a').hover(
        function() {
            $(this).addClass('hover');
        },
        function() {
            $(this).removeClass('hover');
        }
    );

    // Xử lý active state cho menu items
    $('#sidebar ul li a').on('click', function() {
        $('#sidebar ul li a').removeClass('active');
        $(this).addClass('active');
    });

    // Xử lý responsive sidebar
    $(window).on('resize', function() {
        if ($(window).width() > 768) {
            $('#sidebar').removeClass('active');
        }
    });

    // Initialize Tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize Popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Table Search
    $('.table-search').on('keyup', function() {
        const searchText = $(this).val().toLowerCase();
        $('table tbody tr').each(function() {
            const rowText = $(this).text().toLowerCase();
            $(this).toggle(rowText.includes(searchText));
        });
    });

    // Table Sort
    $('th[data-sortable]').on('click', function() {
        const table = $(this).closest('table');
        const tbody = table.find('tbody');
        const rows = tbody.find('tr').toArray();
        const index = $(this).index();
        const isAsc = $(this).hasClass('asc');

        rows.sort((a, b) => {
            const aValue = $(a).children('td').eq(index).text().trim();
            const bValue = $(b).children('td').eq(index).text().trim();
            return isAsc ? 
                bValue.localeCompare(aValue) : 
                aValue.localeCompare(bValue);
        });

        $(this).toggleClass('asc');
        tbody.empty().append(rows);
    });

    // Form Validation
    $('form[data-validate]').on('submit', function(e) {
        if (!this.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
        }
        $(this).addClass('was-validated');
    });

    // Loading Button
    $('.btn-loading').on('click', function() {
        const btn = $(this);
        const originalText = btn.html();
        btn.html('<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...').prop('disabled', true);
        setTimeout(() => {
            btn.html(originalText).prop('disabled', false);
        }, 2000);
    });

    // Card Hover Effect
    $('.card').hover(
        function() {
            $(this).addClass('shadow-sm');
        },
        function() {
            $(this).removeClass('shadow-sm');
        }
    );

    // List Item Selection
    $('.list-group-item').on('click', function() {
        $('.list-group-item').removeClass('active');
        $(this).addClass('active');
    });

    // Export Button
    $('.btn-export').on('click', function() {
        const btn = $(this);
        const originalText = btn.html();
        btn.html('<i class="fas fa-spinner fa-spin me-2"></i>Đang xuất...').prop('disabled', true);
        setTimeout(() => {
            btn.html(originalText).prop('disabled', false);
            showNotification('success', 'Xuất báo cáo thành công!');
        }, 2000);
    });

    // Modal Form Reset
    $('.modal').on('hidden.bs.modal', function() {
        $(this).find('form').trigger('reset');
        $(this).find('form').removeClass('was-validated');
    });

    // Responsive Table
    $('.table-responsive').on('scroll', function() {
        $(this).find('thead th').css('transform', 'translateY(' + $(this).scrollTop() + 'px)');
    });

    // Filter Section Toggle
    $('.filter-toggle').on('click', function() {
        $('.filter-section').slideToggle();
    });

    // Status Badge Update
    $('.status-select').on('change', function() {
        const status = $(this).val();
        const badge = $(this).closest('tr').find('.status-badge');
        badge.removeClass('status-pending status-approved status-rejected');
        badge.addClass('status-' + status.toLowerCase());
    });

    // Avatar Upload Preview
    $('.avatar-upload').on('change', function() {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                $('.avatar-preview').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });

    // Search Box Clear
    $('.search-clear').on('click', function() {
        $(this).prev('input').val('').trigger('keyup');
    });

    // Date Range Picker
    if ($.fn.daterangepicker) {
        $('.date-range').daterangepicker({
            locale: {
                format: 'DD/MM/YYYY'
            }
        });
    }

    // Select2 Initialization
    if ($.fn.select2) {
        $('.select2').select2({
            theme: 'bootstrap-5'
        });
    }

    // DataTable Initialization
    if ($.fn.DataTable) {
        $('.datatable').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/vi.json'
            }
        });
    }

    // Notification Handling
    function updateNotificationBadge(count) {
        const badge = $('.notification-badge');
        if (count > 0) {
            badge.text(count).show();
        } else {
            badge.hide();
        }
    }

    function addNotification(notification) {
        const notificationList = $('.notification-list');
        const notificationItem = `
            <div class="notification-item p-3">
                <div class="d-flex align-items-center">
                    <div class="notification-icon me-3">
                        <i class="fas fa-file-alt fa-lg text-primary"></i>
                    </div>
                    <div class="notification-content">
                        <p class="mb-1">${notification.message}</p>
                        <small class="text-muted">${notification.time}</small>
                    </div>
                </div>
            </div>
        `;
        notificationList.prepend(notificationItem);
    }

    // Check for new notifications every 30 seconds
    function checkNewNotifications() {
        $.get('/Admin/DonPhucKhao/GetNewNotifications', function(data) {
            if (data.count > 0) {
                updateNotificationBadge(data.count);
                data.notifications.forEach(notification => {
                    addNotification(notification);
                });
            }
        });
    }

    // Initialize notifications
    checkNewNotifications();
    setInterval(checkNewNotifications, 30000);

    // Dark Mode Toggle
    function initDarkMode() {
        const darkModeToggle = $('#darkModeToggle');
        const isDarkMode = localStorage.getItem('darkMode') === 'true';
        
        // Thêm CSS cho dark mode
        const darkModeStyle = document.createElement('style');
        darkModeStyle.textContent = `
            /* Nền tối */
            body.dark-mode {
                background: #0f172a;
                color: #fff;
            }

            /* Page Header trong nền tối */
            body.dark-mode .page-header {
                background: linear-gradient(135deg, #0a0f1a 0%, #0f172a 50%, #1a1f2a 100%);
                border: 1px solid rgba(255, 255, 255, 0.05);
                border-radius: 1rem;
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
                backdrop-filter: blur(10px);
                margin: 1rem;
                padding: 1.5rem;
                transition: all 0.3s ease;
            }

            body.dark-mode .page-header:hover {
                box-shadow: 0 6px 25px rgba(0, 0, 0, 0.4);
                transform: translateY(-2px);
            }

            body.dark-mode .page-title {
                color: #fff;
                font-weight: 600;
                text-shadow: 0 0 10px rgba(255, 255, 255, 0.2);
                margin-bottom: 0.5rem;
                font-size: 1.75rem;
                letter-spacing: 0.5px;
            }

            body.dark-mode .page-subtitle {
                color: rgba(255, 255, 255, 0.7);
                font-size: 0.875rem;
                margin-bottom: 0;
                letter-spacing: 0.3px;
            }

            body.dark-mode .container-fluid {
                background: transparent;
            }

            /* Navbar trong nền tối */
            body.dark-mode .top-navbar {
                background: linear-gradient(135deg, #0a0f1a 0%, #0f172a 50%, #1a1f2a 100%);
                border-bottom: 1px solid rgba(255, 255, 255, 0.05);
                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
            }

            body.dark-mode .btn-link {
                color: rgba(255, 255, 255, 0.8);
                transition: all 0.3s ease;
            }

            body.dark-mode .btn-link:hover {
                color: #fff;
                transform: scale(1.1);
            }

            body.dark-mode .navbar-right {
                background: transparent;
            }

            body.dark-mode .notification-dropdown,
            body.dark-mode .user-dropdown-menu {
                background: linear-gradient(135deg, #0a0f1a 0%, #0f172a 50%, #1a1f2a 100%);
                border: 1px solid rgba(255, 255, 255, 0.05);
                box-shadow: 0 0 20px rgba(0, 0, 0, 0.3);
            }

            body.dark-mode .dropdown-header {
                background: rgba(0, 0, 0, 0.2);
                border-bottom: 1px solid rgba(255, 255, 255, 0.05);
            }

            body.dark-mode .dropdown-divider {
                border-color: rgba(255, 255, 255, 0.05);
            }

            body.dark-mode .dropdown-item {
                color: rgba(255, 255, 255, 0.8);
                transition: all 0.3s ease;
            }

            body.dark-mode .dropdown-item:hover {
                background: rgba(255, 255, 255, 0.05);
                color: #fff;
                transform: translateX(5px);
            }

            body.dark-mode .notification-header {
                background: rgba(0, 0, 0, 0.2);
                border-bottom: 1px solid rgba(255, 255, 255, 0.05);
            }

            body.dark-mode .notification-footer {
                background: rgba(0, 0, 0, 0.2);
                border-top: 1px solid rgba(255, 255, 255, 0.05);
            }

            body.dark-mode .notification-footer a {
                color: rgba(255, 255, 255, 0.6);
                transition: all 0.3s ease;
            }

            body.dark-mode .notification-footer a:hover {
                color: #fff;
            }

            body.dark-mode .user-info .user-name {
                color: #fff;
                font-weight: 500;
            }

            body.dark-mode .user-info .user-role {
                color: rgba(255, 255, 255, 0.6);
                font-size: 0.875rem;
            }

            body.dark-mode .avatar-circle {
                background: rgba(255, 255, 255, 0.1);
                color: rgba(255, 255, 255, 0.8);
                transition: all 0.3s ease;
            }

            body.dark-mode .avatar-circle:hover {
                background: rgba(255, 255, 255, 0.2);
                transform: scale(1.1);
            }

            body.dark-mode .form-check-input {
                background-color: rgba(255, 255, 255, 0.1);
                border-color: rgba(255, 255, 255, 0.2);
                transition: all 0.3s ease;
            }

            body.dark-mode .form-check-input:checked {
                background-color: #6366f1;
                border-color: #6366f1;
            }

            body.dark-mode .badge {
                background: #ef4444;
                color: #fff;
                box-shadow: 0 0 10px rgba(239, 68, 68, 0.5);
            }

            body.dark-mode .fas {
                color: rgba(255, 255, 255, 0.8);
                transition: all 0.3s ease;
            }

            body.dark-mode .btn-link:hover .fas {
                color: #fff;
                transform: scale(1.1);
            }

            body.dark-mode .dropdown-item:hover .fas {
                color: #fff;
                transform: scale(1.1);
            }

            /* Sidebar trong nền tối */
            body.dark-mode .sidebar {
                background: linear-gradient(135deg, #0a0f1a 0%, #0f172a 50%, #1a1f2a 100%);
                box-shadow: 0 0 30px rgba(0, 0, 0, 0.5);
                border-right: 1px solid rgba(255, 255, 255, 0.05);
            }

            .dark-mode {
                background-color: #0f172a;
                color: #ffffff;
            }

            .dark-mode .sidebar {
                background: linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #334155 100%);
                box-shadow: 0 0 30px rgba(0, 0, 0, 0.3);
                border-right: 1px solid rgba(255, 255, 255, 0.05);
                position: fixed;
                top: 0;
                left: 0;
                height: 100vh;
                width: 250px;
                z-index: 1000;
                transition: all 0.3s ease;
                overflow-y: auto;
                overflow-x: hidden;
            }

            .dark-mode .sidebar::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: 
                    radial-gradient(2px 2px at 20px 30px, rgba(255, 255, 255, 0.8), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 40px 70px, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 50px 160px, rgba(255, 255, 255, 0.6), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 90px 40px, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 130px 80px, rgba(255, 255, 255, 0.8), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 160px 120px, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 200px 160px, rgba(255, 255, 255, 0.6), rgba(255, 255, 255, 0)),
                    radial-gradient(2px 2px at 240px 200px, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0));
                opacity: 0.3;
                animation: twinkle 4s infinite;
            }

            .dark-mode .sidebar::after {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: 
                    radial-gradient(circle at 50% 50%, rgba(99, 102, 241, 0.1) 0%, transparent 70%),
                    radial-gradient(circle at 30% 30%, rgba(99, 102, 241, 0.05) 0%, transparent 50%),
                    radial-gradient(circle at 70% 70%, rgba(99, 102, 241, 0.05) 0%, transparent 50%);
                opacity: 0.5;
                pointer-events: none;
            }

            @keyframes twinkle {
                0%, 100% { opacity: 0.3; }
                50% { opacity: 0.5; }
            }

            .dark-mode .sidebar .menu-item {
                position: relative;
                z-index: 1;
                background: rgba(255, 255, 255, 0.05);
                border-radius: 8px;
                margin: 4px 8px;
                padding: 12px 16px;
                transition: all 0.3s ease;
                backdrop-filter: blur(5px);
            }

            .dark-mode .sidebar .menu-item:hover {
                background: rgba(255, 255, 255, 0.1);
                transform: translateX(5px);
                box-shadow: 0 0 15px rgba(99, 102, 241, 0.2);
            }

            .dark-mode .sidebar .menu-item.active {
                background: rgba(99, 102, 241, 0.2);
                box-shadow: 0 0 20px rgba(99, 102, 241, 0.3);
            }

            .dark-mode .sidebar .menu-item i {
                color: #818cf8;
                transition: all 0.3s ease;
            }

            .dark-mode .sidebar .menu-item:hover i {
                transform: scale(1.2);
                color: #6366f1;
            }

            .dark-mode .sidebar .menu-item span {
                color: rgba(255, 255, 255, 0.8);
                transition: all 0.3s ease;
            }

            .dark-mode .sidebar .menu-item:hover span {
                color: #ffffff;
            }

            .dark-mode .sidebar-header {
                background: rgba(15, 23, 42, 0.8);
                backdrop-filter: blur(10px);
                border-bottom: 1px solid rgba(255, 255, 255, 0.05);
                padding: 20px;
                margin-bottom: 10px;
                position: sticky;
                top: 0;
                z-index: 2;
            }

            .dark-mode .sidebar-footer {
                background: rgba(15, 23, 42, 0.8);
                backdrop-filter: blur(10px);
                border-top: 1px solid rgba(255, 255, 255, 0.05);
                padding: 15px;
                position: sticky;
                bottom: 0;
                z-index: 2;
            }

            .dark-mode .sidebar-brand {
                color: #ffffff;
                text-shadow: 0 0 10px rgba(99, 102, 241, 0.5);
                font-weight: 600;
            }

            .dark-mode .sidebar-brand i {
                color: #818cf8;
                margin-right: 10px;
            }

            .dark-mode .sidebar-divider {
                border-color: rgba(255, 255, 255, 0.05);
                margin: 10px 0;
            }

            .dark-mode .sidebar-section-title {
                color: rgba(255, 255, 255, 0.5);
                font-size: 0.75rem;
                text-transform: uppercase;
                letter-spacing: 0.05em;
                padding: 0 16px;
                margin: 16px 0 8px;
            }

            .dark-mode .sidebar.collapsed {
                transform: translateX(-100%);
                width: 0;
                opacity: 0;
                visibility: hidden;
            }

            .dark-mode #mainContent {
                margin-left: 250px;
                transition: all 0.3s ease;
            }

            .dark-mode #mainContent.expanded {
                margin-left: 0;
            }

            @media (max-width: 768px) {
                .dark-mode .sidebar {
                    transform: translateX(-100%);
                }
                
                .dark-mode .sidebar.active {
                    transform: translateX(0);
                }
                
                .dark-mode #mainContent {
                    margin-left: 0;
                }
            }
        `;
        document.head.appendChild(darkModeStyle);
        
        // Set initial state
        if (isDarkMode) {
            document.documentElement.setAttribute('data-theme', 'dark');
            darkModeToggle.prop('checked', true);
            $('body').addClass('dark-mode');
        }

        // Handle toggle
        darkModeToggle.on('change', function() {
            const isChecked = $(this).prop('checked');
            if (isChecked) {
                document.documentElement.setAttribute('data-theme', 'dark');
                localStorage.setItem('darkMode', 'true');
                $('body').addClass('dark-mode');
            } else {
                document.documentElement.removeAttribute('data-theme');
                localStorage.setItem('darkMode', 'false');
                $('body').removeClass('dark-mode');
            }
        });
    }

    // Initialize dark mode
    initDarkMode();

    // Tối ưu hiệu ứng chuyển đổi
    document.addEventListener('DOMContentLoaded', function() {
        // Thêm class transition-ready khi trang đã load xong
        document.body.classList.add('transition-ready');

        // Tối ưu hiệu ứng chuyển đổi cho các phần tử
        const elements = document.querySelectorAll('.card, .table, .form-control, .btn, .nav-link');
        elements.forEach(element => {
            element.classList.add('hover-effect', 'active-effect', 'focus-effect');
        });

        // Tối ưu hiệu ứng chuyển trang
        const links = document.querySelectorAll('a[href]');
        links.forEach(link => {
            link.addEventListener('click', function(e) {
                if (!this.href.includes('#')) {
                    e.preventDefault();
                    document.body.classList.add('page-transition');
                    setTimeout(() => {
                        window.location.href = this.href;
                    }, 100);
                }
            });
        });

        // Tối ưu hiệu ứng loading
        const forms = document.querySelectorAll('form');
        forms.forEach(form => {
            form.addEventListener('submit', function() {
                const submitBtn = this.querySelector('button[type="submit"]');
                if (submitBtn) {
                    submitBtn.classList.add('loading-effect', 'loading');
                    submitBtn.disabled = true;
                }
            });
        });

        // Tối ưu hiệu ứng scroll
        let scrollTimeout;
        window.addEventListener('scroll', function() {
            if (scrollTimeout) {
                window.cancelAnimationFrame(scrollTimeout);
            }
            scrollTimeout = window.requestAnimationFrame(function() {
                const scrolled = window.scrollY;
                document.body.style.setProperty('--scroll-position', scrolled + 'px');
            });
        });

        // Tối ưu hiệu ứng hover cho menu
        const menuItems = document.querySelectorAll('.menu-item');
        menuItems.forEach(item => {
            item.classList.add('hover-effect');
        });

        // Tối ưu hiệu ứng cho dropdown
        const dropdowns = document.querySelectorAll('.dropdown-toggle');
        dropdowns.forEach(dropdown => {
            dropdown.addEventListener('click', function(e) {
                e.preventDefault();
                const menu = this.nextElementSibling;
                menu.classList.toggle('show');
                
                // Thêm animation cho dropdown
                if (menu.classList.contains('show')) {
                    menu.style.animation = 'dropdownIn 0.1s ease forwards';
                } else {
                    menu.style.animation = 'dropdownOut 0.1s ease forwards';
                }
            });
        });
    });

    // Thêm CSS cho hiệu ứng ripple
    const style = document.createElement('style');
    style.textContent = `
        .ripple {
            position: absolute;
            border-radius: 50%;
            background: rgba(255,255,255,0.3);
            transform: scale(0);
            animation: ripple 0.6s linear;
            pointer-events: none;
        }

        @keyframes ripple {
            to {
                transform: scale(4);
                opacity: 0;
            }
        }

        .page-transition {
            opacity: 0;
            transition: opacity 0.2s ease;
        }

        .loading {
            position: relative;
            pointer-events: none;
        }

        .loading::after {
            content: '';
            position: absolute;
            width: 20px;
            height: 20px;
            top: 50%;
            left: 50%;
            margin: -10px 0 0 -10px;
            border: 2px solid rgba(255,255,255,0.3);
            border-top-color: #fff;
            border-radius: 50%;
            animation: spin 0.8s linear infinite;
        }

        @keyframes spin {
            to {
                transform: rotate(360deg);
            }
        }
    `;
    document.head.appendChild(style);

    // Thêm CSS cho animation dropdown
    const dropdownStyle = document.createElement('style');
    dropdownStyle.textContent = `
        @keyframes dropdownIn {
            from {
                opacity: 0;
                transform: translateY(-10px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        @keyframes dropdownOut {
            from {
                opacity: 1;
                transform: translateY(0);
            }
            to {
                opacity: 0;
                transform: translateY(-10px);
            }
        }
    `;
    document.head.appendChild(dropdownStyle);
}); 
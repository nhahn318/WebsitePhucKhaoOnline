// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Admin Area JavaScript
$(document).ready(function () {
    // Toggle Sidebar
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    // Initialize DataTables
    $('.datatable').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Vietnamese.json"
        }
    });

    // Initialize Tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    // Auto-hide alerts after 5 seconds
    $('.alert').delay(5000).fadeOut(350);

    // Handle form validation
    $('form').on('submit', function () {
        $(this).find(':input').filter(function () {
            return !this.value;
        }).closest('.form-group').addClass('has-error');
    });

    // Remove error class on input
    $('input').on('input', function () {
        $(this).closest('.form-group').removeClass('has-error');
    });

    // Handle file input change
    $('.custom-file-input').on('change', function () {
        var fileName = $(this).val().split('\\').pop();
        $(this).next('.custom-file-label').html(fileName);
    });

    // Handle dropdown toggle
    $('.dropdown-toggle').on('click', function (e) {
        e.preventDefault();
        $(this).next('.dropdown-menu').slideToggle(300);
    });

    // Close dropdown when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.dropdown').length) {
            $('.dropdown-menu').slideUp(300);
        }
    });

    // Handle modal close
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
    });

    // Handle tab navigation
    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        $(e.target).addClass('active').siblings().removeClass('active');
    });

    // Handle form submission with AJAX
    $('form[data-ajax="true"]').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var url = form.attr('action');
        var method = form.attr('method');
        var data = form.serialize();

        $.ajax({
            url: url,
            method: method,
            data: data,
            success: function (response) {
                if (response.success) {
                    showAlert('success', response.message);
                    if (response.redirectUrl) {
                        setTimeout(function () {
                            window.location.href = response.redirectUrl;
                        }, 1500);
                    }
                } else {
                    showAlert('danger', response.message);
                }
            },
            error: function () {
                showAlert('danger', 'Có lỗi xảy ra. Vui lòng thử lại sau.');
            }
        });
    });

    // Helper function to show alerts
    function showAlert(type, message) {
        var alertHtml = '<div class="alert alert-' + type + ' alert-dismissible fade show" role="alert">' +
            message +
            '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>' +
            '</div>';
        $('.container-fluid').prepend(alertHtml);
        $('.alert').delay(5000).fadeOut(350);
    }

    // Confirm delete
    $('.delete-confirm').on('click', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        if (confirm('Bạn có chắc chắn muốn xóa mục này?')) {
            window.location.href = url;
        }
    });

    // Tooltip initialization
    $('[data-toggle="tooltip"]').tooltip();
});

// Xử lý thông báo chào mừng
document.addEventListener('DOMContentLoaded', function() {
    const welcomeAlert = document.querySelector('.alert-info');
    if (welcomeAlert) {
        // Hiển thị thông báo
        welcomeAlert.classList.add('show');
        
        // Tự động ẩn sau 5 giây
        setTimeout(() => {
            welcomeAlert.classList.remove('show');
            welcomeAlert.classList.add('fade');
            setTimeout(() => {
                welcomeAlert.style.display = 'none';
            }, 500); // Thời gian fade out
        }, 5000); // Hiển thị trong 5 giây
    }
});

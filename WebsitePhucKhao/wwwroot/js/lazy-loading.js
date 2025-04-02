// Hàm xử lý lazy loading cho các trang
function initLazyLoading() {
    // Lấy tất cả các link trong sidebar
    const sidebarLinks = document.querySelectorAll('.sidebar a[href]');
    
    // Thêm sự kiện click cho mỗi link
    sidebarLinks.forEach(link => {
        link.addEventListener('click', async (e) => {
            e.preventDefault();
            
            // Lấy URL từ href của link
            const url = link.getAttribute('href');
            
            // Chỉ xử lý các link trong cùng domain
            if (!url.startsWith('/')) return;
            
            // Thêm class active cho link được chọn
            sidebarLinks.forEach(l => l.parentElement.classList.remove('active'));
            link.parentElement.classList.add('active');
            
            // Hiển thị loading spinner
            showLoadingSpinner();
            
            try {
                // Thêm độ trễ 1 giây
                await new Promise(resolve => setTimeout(resolve, 1000));
                
                // Gọi API để lấy nội dung trang
                const response = await fetch(url);
                const html = await response.text();
                
                // Tạo một DOM parser để parse HTML
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, 'text/html');
                
                // Lấy nội dung chính từ trang
                const mainContent = doc.querySelector('.page-content');
                
                if (mainContent) {
                    // Cập nhật nội dung chính
                    const currentPageContent = document.querySelector('.page-content');
                    currentPageContent.innerHTML = mainContent.innerHTML;
                    
                    // Cập nhật title của trang
                    document.title = doc.title;
                    
                    // Cập nhật URL mà không reload trang
                    window.history.pushState({}, '', url);
                    
                    // Khởi tạo lại các script cần thiết
                    initializePageScripts();
                }
            } catch (error) {
                console.error('Lỗi khi tải trang:', error);
                showError('Có lỗi xảy ra khi tải trang. Vui lòng thử lại.');
            } finally {
                // Ẩn loading spinner
                hideLoadingSpinner();
            }
        });
    });
}

// Hàm hiển thị loading spinner
function showLoadingSpinner() {
    let spinner = document.getElementById('page-loading-spinner');
    if (!spinner) {
        spinner = document.createElement('div');
        spinner.id = 'page-loading-spinner';
        spinner.innerHTML = `
            <div class="spinner-overlay">
                <div class="spinner-container">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Đang tải...</span>
                    </div>
                    <div class="mt-3 text-primary fw-semibold">Đang tải...</div>
                </div>
            </div>
        `;
        document.querySelector('.page-content').appendChild(spinner);
    }
    spinner.style.display = 'block';
}

// Hàm ẩn loading spinner
function hideLoadingSpinner() {
    const spinner = document.getElementById('page-loading-spinner');
    if (spinner) {
        spinner.style.display = 'none';
    }
}

// Hàm hiển thị thông báo lỗi
function showError(message) {
    const alertDiv = document.createElement('div');
    alertDiv.className = 'alert alert-danger alert-dismissible fade show';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    const mainContent = document.querySelector('.page-content');
    mainContent.insertBefore(alertDiv, mainContent.firstChild);
    
    // Tự động ẩn sau 5 giây
    setTimeout(() => {
        alertDiv.remove();
    }, 5000);
}

// Hàm khởi tạo lại các script cần thiết
function initializePageScripts() {
    // Khởi tạo lại các tooltip
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    // Khởi tạo lại các popover
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
    
    // Khởi tạo lại các modal
    const modalTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="modal"]'));
    modalTriggerList.map(function (modalTriggerEl) {
        modalTriggerEl.addEventListener('click', function() {
            const targetModal = document.querySelector(this.getAttribute('data-bs-target'));
            if (targetModal) {
                const modal = new bootstrap.Modal(targetModal);
                modal.show();
            }
        });
    });
    
    // Khởi tạo lại các form validation
    const forms = document.querySelectorAll('.needs-validation');
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
}

// Khởi tạo lazy loading khi trang được tải
document.addEventListener('DOMContentLoaded', initLazyLoading);
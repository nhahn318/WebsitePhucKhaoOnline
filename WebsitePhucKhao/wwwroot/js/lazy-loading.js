// Hàm xử lý lazy loading cho các trang
function initLazyLoading() {
    console.log("Khởi tạo lazy loading...");
    
    // Lấy tất cả các link trong sidebar 
    const sidebarLinks = document.querySelectorAll('.side-menu a');
    console.log("Tìm thấy", sidebarLinks.length, "link trong menu");
    
    // Thêm sự kiện click cho mỗi link
    sidebarLinks.forEach(link => {
        link.addEventListener('click', async (e) => {
            const url = link.getAttribute('href');
            
            // Bỏ qua nếu là link đến trang khác hoặc # anchor
            if (!url || url === '#' || url.startsWith('http') || url.startsWith('mailto:')) {
                return; // Cho phép hành vi mặc định
            }
            
            // Ngăn chặn hành vi mặc định của link
            e.preventDefault();
            console.log("Click vào link:", url);
            
            // Bỏ qua nếu đây là nút dropdown trong menu
            if (link.classList.contains('icon-right') || link.parentElement.querySelector('.icon-right')) {
                return;
            }
            
            // Cập nhật trạng thái active cho link được chọn
            document.querySelectorAll('.side-menu a.active').forEach(l => l.classList.remove('active'));
            link.classList.add('active');
            
            // Hiển thị loading spinner
            showLoadingSpinner();
            
            try {
                // Gọi AJAX để lấy nội dung trang
                const response = await fetch(url, {
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
                
                // Kiểm tra trạng thái response
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                
                // Lấy nội dung HTML
                const html = await response.text();
                
                // Kiểm tra nếu HTML quá ngắn, có thể là lỗi
                if (html.length < 100) {
                    console.warn("Nội dung tải về quá ngắn, kiểm tra:", html);
                }
                
                // Tạo một container tạm để phân tích nội dung HTML
                const tempContainer = document.createElement('div');
                tempContainer.innerHTML = html;
                
                // Tìm phần nội dung chính từ HTML nhận được
                let mainContent = tempContainer.querySelector('#main-content');
                if (!mainContent) {
                    mainContent = tempContainer.querySelector('.content-wrapper');
                }
                if (!mainContent) {
                    // Nếu không tìm thấy phần tử có ID cụ thể, lấy nội dung toàn bộ body
                    mainContent = tempContainer;
                }
                
                // Cập nhật nội dung
                const contentContainer = document.querySelector('#main-content');
                if (contentContainer) {
                    // Lưu lại các script tags để thêm lại sau
                    const scripts = Array.from(mainContent.querySelectorAll('script'));
                    
                    // Cập nhật nội dung
                    contentContainer.innerHTML = mainContent.innerHTML;
                    
                    // Cập nhật URL mà không reload trang
                    window.history.pushState({}, '', url);
                    
                    // Cập nhật tiêu đề trang nếu có
                    const titleElement = tempContainer.querySelector('title');
                    if (titleElement) {
                        document.title = titleElement.textContent;
                    }
                    
                    // Thực thi các script
                    scripts.forEach(script => {
                        const newScript = document.createElement('script');
                        Array.from(script.attributes).forEach(attr => {
                            newScript.setAttribute(attr.name, attr.value);
                        });
                        newScript.textContent = script.textContent;
                        document.body.appendChild(newScript);
                    });
                    
                    // Khởi tạo lại các script cần thiết
                    initializePageScripts();
                } else {
                    console.error("Không tìm thấy phần tử #main-content");
                    showError("Không tìm thấy phần tử #main-content");
                }
            } catch (error) {
                console.error('Lỗi khi tải trang:', error);
                showError(`Có lỗi xảy ra khi tải trang: ${error.message}`);
            } finally {
                // Ẩn loading spinner
                hideLoadingSpinner();
            }
        });
    });
    
    // Xử lý lịch sử trình duyệt (back/forward)
    window.addEventListener('popstate', async (e) => {
        const currentUrl = window.location.href;
        await loadContent(currentUrl, false);
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
                    <div class="loading-dots">
                        <div class="dot"></div>
                        <div class="dot"></div>
                        <div class="dot"></div>
                    </div>
                    <div class="mt-3 text-primary fw-semibold">Đang tải...</div>
                </div>
            </div>
        `;
        document.body.appendChild(spinner);
        
        // Thêm CSS cho hiệu ứng dấu chấm
        const style = document.createElement('style');
        style.textContent = `
            .spinner-overlay {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(255, 255, 255, 0.8);
                display: flex;
                justify-content: center;
                align-items: center;
                z-index: 9999;
            }
            
            .spinner-container {
                text-align: center;
            }
            
            .loading-dots {
                display: flex;
                justify-content: center;
                align-items: center;
                gap: 8px;
            }
            
            .dot {
                width: 12px;
                height: 12px;
                background-color: #4e73df;
                border-radius: 50%;
                animation: bounce 1.4s infinite ease-in-out both;
            }
            
            .dot:nth-child(1) {
                animation-delay: -0.32s;
            }
            
            .dot:nth-child(2) {
                animation-delay: -0.16s;
            }
            
            @keyframes bounce {
                0%, 80%, 100% { 
                    transform: scale(0);
                } 
                40% { 
                    transform: scale(1.0);
                }
            }
        `;
        document.head.appendChild(style);
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
    
    // Thêm alert vào đầu main content
    const mainContent = document.querySelector('#main-content');
    if (mainContent) {
        mainContent.insertBefore(alertDiv, mainContent.firstChild);
        
        // Tự động ẩn sau 5 giây
        setTimeout(() => {
            alertDiv.classList.remove('show');
            setTimeout(() => alertDiv.remove(), 150);
        }, 5000);
    } else {
        console.error("Không tìm thấy phần tử #main-content để hiển thị lỗi");
    }
}

// Hàm khởi tạo lại các script cần thiết
function initializePageScripts() {
    console.log("Khởi tạo lại các script...");
    
    // Khởi tạo lại tooltips nếu có
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(function (tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
    
    // Khởi tạo lại DataTables nếu có
    if (typeof $ !== 'undefined' && $.fn.DataTable) {
        $('.datatable').DataTable();
    }
    
    console.log("Đã khởi tạo lại các script");
}

// Hàm loadContent để có thể gọi từ bên ngoài
async function loadContent(url, updateHistory = true) {
    console.log("Gọi loadContent với URL:", url);
    
    // Hiển thị loading spinner
    showLoadingSpinner();
    
    try {
        // Gọi AJAX để lấy nội dung trang
        const response = await fetch(url, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        
        // Kiểm tra trạng thái response
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        // Lấy nội dung HTML
        const html = await response.text();
        
        // Tạo một container tạm để phân tích nội dung HTML
        const tempContainer = document.createElement('div');
        tempContainer.innerHTML = html;
        
        // Tìm phần nội dung chính từ HTML nhận được
        let mainContent = tempContainer.querySelector('#main-content');
        if (!mainContent) {
            mainContent = tempContainer.querySelector('.content-wrapper');
        }
        if (!mainContent) {
            // Nếu không tìm thấy phần tử có ID cụ thể, lấy nội dung toàn bộ body
            mainContent = tempContainer;
        }
        
        // Cập nhật nội dung
        const contentContainer = document.querySelector('#main-content');
        if (contentContainer) {
            // Lưu lại các script tags để thêm lại sau
            const scripts = Array.from(mainContent.querySelectorAll('script'));
            
            // Cập nhật nội dung
            contentContainer.innerHTML = mainContent.innerHTML;
            
            // Cập nhật URL mà không reload trang nếu cần
            if (updateHistory) {
                window.history.pushState({}, '', url);
            }
            
            // Cập nhật tiêu đề trang nếu có
            const titleElement = tempContainer.querySelector('title');
            if (titleElement) {
                document.title = titleElement.textContent;
            }
            
            // Thực thi các script
            scripts.forEach(script => {
                const newScript = document.createElement('script');
                Array.from(script.attributes).forEach(attr => {
                    newScript.setAttribute(attr.name, attr.value);
                });
                newScript.textContent = script.textContent;
                document.body.appendChild(newScript);
            });
            
            // Khởi tạo lại các script cần thiết
            initializePageScripts();
        } else {
            console.error("Không tìm thấy phần tử #main-content");
            showError("Không tìm thấy phần tử #main-content");
        }
    } catch (error) {
        console.error('Lỗi khi tải trang:', error);
        showError(`Có lỗi xảy ra khi tải trang: ${error.message}`);
    } finally {
        // Ẩn loading spinner
        hideLoadingSpinner();
    }
}

// Xử lý form submit bằng AJAX
function initAjaxFormSubmit() {
    document.addEventListener('submit', function(e) {
        // Chỉ xử lý các form có data-ajax="true"
        const form = e.target;
        if (form.getAttribute('data-ajax') === 'true') {
            e.preventDefault();
            
            const formData = new FormData(form);
            const url = form.getAttribute('action') || window.location.href;
            const method = form.getAttribute('method') || 'POST';
            
            showLoadingSpinner();
            
            fetch(url, {
                method: method,
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.text();
            })
            .then(html => {
                const tempContainer = document.createElement('div');
                tempContainer.innerHTML = html;
                
                // Nếu response có redirect (được trả về dưới dạng JSON)
                if (html.includes('"redirectUrl"')) {
                    try {
                        const jsonResponse = JSON.parse(html);
                        if (jsonResponse.redirectUrl) {
                            window.location.href = jsonResponse.redirectUrl;
                            return;
                        }
                    } catch (e) {
                        // Không phải JSON, tiếp tục xử lý như HTML
                    }
                }
                
                // Tìm nội dung chính
                let mainContent = tempContainer.querySelector('#main-content');
                if (!mainContent) {
                    mainContent = tempContainer.querySelector('.content-wrapper');
                }
                if (!mainContent) {
                    mainContent = tempContainer;
                }
                
                // Cập nhật nội dung
                const contentContainer = document.querySelector('#main-content');
                if (contentContainer) {
                    contentContainer.innerHTML = mainContent.innerHTML;
                    initializePageScripts();
                }
            })
            .catch(error => {
                console.error('Lỗi khi submit form:', error);
                showError(`Có lỗi xảy ra khi submit form: ${error.message}`);
            })
            .finally(() => {
                hideLoadingSpinner();
            });
        }
    });
}

// Khởi tạo lazy loading khi trang đã tải xong
document.addEventListener('DOMContentLoaded', function() {
    console.log("DOM đã tải xong, khởi tạo lazy loading");
    initLazyLoading();
    initAjaxFormSubmit();
});
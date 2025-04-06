// Quản lý việc tải trang
function initLazyLoading() {
    // Lưu trữ các CSS đã tải
    const loadedStyles = new Set();

    // Hàm tải CSS
    async function loadCSS(href) {
        return new Promise((resolve, reject) => {
            if (loadedStyles.has(href)) {
                resolve();
                return;
            }

            const link = document.createElement('link');
            link.rel = 'stylesheet';
            link.href = href;

            link.onload = () => {
                loadedStyles.add(href);
                resolve();
            };

            link.onerror = () => reject(new Error(`Không thể tải CSS: ${href}`));
            document.head.appendChild(link);
        });
    }

    // Hàm tải trang
    async function loadPage(url) {
        try {
            const mainContent = document.querySelector('#main-content');
            // Ẩn nội dung cũ bằng hiệu ứng mờ dần
            mainContent.style.opacity = '0';

            // Hiển thị loading spinner
            document.body.style.cursor = 'wait';
            const loadingOverlay = document.createElement('div');
            loadingOverlay.className = 'loading-overlay';
            loadingOverlay.innerHTML = '<div class="spinner"></div>';
            document.body.appendChild(loadingOverlay);

            const response = await fetch(url);
            const html = await response.text();

            // Tạo DOM tạm thời để phân tích HTML
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');

            // Tìm tất cả các link CSS trong trang mới
            const cssLinks = Array.from(doc.querySelectorAll('link[rel="stylesheet"]'));

            // Tải tất cả CSS trước
            await Promise.all(cssLinks.map(link => loadCSS(link.href)));

            // Sau khi tất cả CSS đã tải xong, cập nhật nội dung
            const content = doc.querySelector('#main-content').innerHTML;

            // Thêm độ trễ để tạo cảm giác mượt mà
            await new Promise(resolve => setTimeout(resolve, 400));

            mainContent.innerHTML = content;
            mainContent.style.opacity = '1'; // Hiện lại nội dung mới

            // Cập nhật title
            document.title = doc.title;

            // Cập nhật URL
            history.pushState({}, '', url);

            // Xóa loading spinner
            document.body.style.cursor = 'default';
            loadingOverlay.remove();

            // Khởi tạo lại các event listeners
            initializeEventListeners();

        } catch (error) {
            console.error('Lỗi khi tải trang:', error);
            // Trong trường hợp lỗi, chuyển hướng thông thường
            window.location.href = url;
        }
    }

    // Xử lý click cho các link lazy-load
    document.addEventListener('click', (e) => {
        const link = e.target.closest('a.lazy-load');
        if (link) {
            e.preventDefault();
            loadPage(link.href);
        }
    });

    // Xử lý nút back/forward của trình duyệt
    window.addEventListener('popstate', () => {
        loadPage(window.location.href);
    });
}

// Thêm CSS cho loading spinner + hiệu ứng mờ dần nội dung
const style = document.createElement('style');
style.textContent = `
.loading-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(255, 255, 255, 0.8);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
}

.spinner {
    width: 50px;
    height: 50px;
    border: 5px solid #f3f3f3;
    border-top: 5px solid #3498db;
    border-radius: 50%;
    animation: spin 1s linear infinite;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

/* Thêm hiệu ứng chuyển mượt cho phần nội dung */
#main-content {
    transition: opacity 0.3s ease;
}
`;
document.head.appendChild(style);

// Khởi tạo khi trang đã tải xong
document.addEventListener('DOMContentLoaded', initLazyLoading);

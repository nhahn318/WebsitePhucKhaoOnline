$(document).ready(function() {
    // Khởi tạo DataTable với các tùy chọn nâng cao
    $('#dataTable').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Vietnamese.json",
            "paginate": {
                "previous": "<i class='bi bi-chevron-left'></i>",
                "next": "<i class='bi bi-chevron-right'></i>"
            }
        },
        "pageLength": 10,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Tất cả"]],
        "order": [[1, "asc"]],
        "columnDefs": [
            { "orderable": false, "targets": 2 },
            { "className": "text-center", "targets": [0, 2] }
        ],
        "autoWidth": false,
        "responsive": true,
        "dom": '<"top"lf>rt<"bottom"ip>',
        "pagingType": "simple_numbers",
        "initComplete": function() {
            $('.dataTables_length select').addClass('form-select');
            $('.dataTables_filter input').addClass('form-control');
        }
    });

    // Khởi tạo tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    // Xác nhận xóa và gửi form
    $('.btn-delete').click(function(e) {
        e.preventDefault();
        var form = $(this).closest('form');
        var khoaName = $(this).closest('tr').find('td:nth-child(2)').text().trim();

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: `Bạn có chắc chắn muốn xóa khoa "${khoaName}"?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit();
            }
        });
    });
}); 
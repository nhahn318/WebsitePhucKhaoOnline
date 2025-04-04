$(document).ready(function() {
    // Initialize DataTable with improved options
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
            { "orderable": false, "targets": 4 },
            { "className": "text-center", "targets": [0, 4] }
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

    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    // Confirm delete and submit
    $('.btn-delete').click(function() {
        var id = $(this).data('id');
        var nvName = $(this).closest('tr').find('td:nth-child(2)').text().trim();

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: `Bạn có chắc chắn muốn xóa nhân viên "${nvName}"?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post(deleteUrl, { id: id })
                    .done(function() {
                        Swal.fire(
                            'Đã xóa!',
                            'Nhân viên đã được xóa thành công.',
                            'success'
                        ).then(() => {
                            location.reload();
                        });
                    })
                    .fail(function() {
                        Swal.fire(
                            'Lỗi!',
                            'Có lỗi xảy ra khi xóa nhân viên.',
                            'error'
                        );
                    });
            }
        });
    });
}); 
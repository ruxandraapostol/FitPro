$(document).ready(function () {
    $('#search-button').click(function () {

        var searchString = $('#search-input').val();
        var currentPage = $('#currentPage').val();
        var rowNumber = $('#rowNumber').val();
        var sortColumn = $('#sortColumn').val();
        var sortColumnIndex = $('#sortColumnIndex').val();

        var url = '/Admin/AdminUsersList?currentPage=' + currentPage + '&rowNumber=' + rowNumber
            + '&sortColumn=' + sortColumn + '&sortColumnInndex=' + sortColumnIndex + '&searchString=' + searchString;


        $.ajax({
            type: "GET",
            url: url,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            success: (data) => {
                window.location.href = url;
            },
            error: (err) => { alert(err); }
        });

    });
});
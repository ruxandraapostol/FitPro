$(document).ready(function () {
    var rowNumber = document.getElementById("rowNumber").getAttribute("name");
    document.getElementById('select-rowNumber').value = rowNumber;

    $('#select-rowNumber').change(function () {
        var select = document.getElementById('select-rowNumber');
        var row = select.value;
        var searchString = $('#searchString').val();
        var currentPage = $('#currentPage').val();
        var sortColumnIndex = $('#sortColumnIndex').val();


        var url = '/Admin/AdminUsersList?currentPage=' + currentPage + '&rowNumber=' + row
            + '&sortColumnInndex=' + sortColumnIndex + '&searchString=' + searchString;

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
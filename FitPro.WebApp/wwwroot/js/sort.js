function sortByColumn(column, sortIndex) {
    var searchString = $('#searchString').val();
    var currentPage = $('#currentPage').val();
    var rowNumber = $('#rowNumber').val();

    var url = '/Admin/AdminUsersList?currentPage=' + currentPage + '&rowNumber=' + rowNumber
        + '&sortColumn=' + column + '&sortColumnInndex=' + sortIndex + '&searchString=' + searchString;

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
}


$(document).ready(function () {
    $('#email-cresc').on('click', function () { sortByColumn("Email", -1); });
    $('#email-descresc').on('click', function () { sortByColumn("Email", 0); });
    $('#email-simple').on('click', function () { sortByColumn("Email", 1); });

    $('#username-cresc').on('click', function () { sortByColumn("UserName", -1); });
    $('#username-descresc').on('click', function () { sortByColumn("UserName", 0); });
    $('#username-simple').on('click', function () { sortByColumn("UserName", 1); });

    $('#name-cresc').on('click', function () { sortByColumn("Name", -1); });
    $('#name-descresc').on('click', function () { sortByColumn("Name", 0); });
    $('#name-simple').on('click', function () { sortByColumn("Name", 1); });

    $('#role-cresc').on('click', function () { sortByColumn("Role", -1); });
    $('#role-descresc').on('click', function () { sortByColumn("Role", 0); });
    $('#role-simple').on('click', function () { sortByColumn("Role", 1); });
});
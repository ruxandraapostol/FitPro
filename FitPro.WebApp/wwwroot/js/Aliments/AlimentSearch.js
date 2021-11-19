$(document).ready(function () {
    $('#search-button-A').click(function () {

        var searchString = $('#search-input-A').val();
        var currentPage = $('#currentPage-A').val();


        var url = '/Nutritionist/AlimentsList?currentPage=' + currentPage + '&searchString=' + searchString;

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
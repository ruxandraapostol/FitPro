var activDayChange = function (event) {
    var date = $(event.currentTarget).data("date");
    var idCurrentUser = $('#currentUserId').val();

    $.ajax({
        url: '/NutritionTrack/ChangeActiveDay',
        data: {
            idRegularUser: idCurrentUser,
            date: date,
        }
    }).done(function () {
        window.location.href = '/NutritionTrack/DailyTrack?idRegularUser=' + idCurrentUser
            + '&date=' + date;
    });
}
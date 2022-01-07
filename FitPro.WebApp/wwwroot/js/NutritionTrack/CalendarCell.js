var redirect = function (event) {
    var date = $(event.currentTarget).data("date");
    var currentUser = $('#currentUserId').val();

    window.location.href = '/NutritionTrack/DailyTrack?idRegularUser=' + currentUser
        + '&date='  + date;
}
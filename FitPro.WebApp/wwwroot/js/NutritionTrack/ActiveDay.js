var activDayChange = function (event) {
    var date = $(event.currentTarget).data("date");

    $.ajax({
        url: '/NutritionTrack/ChangeActiveDay',
        data: {
            date: date,
        }
    }).done(function () {
        window.location.href = '/NutritionTrack/DailyTrack?date=' + date;
    });
}
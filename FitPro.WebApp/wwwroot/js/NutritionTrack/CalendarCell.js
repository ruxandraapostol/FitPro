var redirect = function (event) {
    var date = $(event.currentTarget).data("date");

    window.location.href = '/NutritionTrack/DailyTrack?date='  + date;
}
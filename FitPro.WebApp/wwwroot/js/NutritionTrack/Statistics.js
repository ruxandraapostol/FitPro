var year = $('#year').val();
var month = $('#month').val();

var selecetMacroChange = function (event) {
    monthStatistics();
}

var monthStatistics = function () {

    $.ajax({
        url: '/NutritionTrack/MonthStatistic',
        data: {
            year: year,
            month: month,
            macronutrient: $('#macronutrientSelect').val()
        }
    }).done(function (data) {

        new Chart("monthlyChart", {
            type: "line",
            data: {
                labels: data.xValues,
                datasets: [{
                    data: data.yValues,
                    borderColor: "#cb997e",
                    fill: false
                }]
            },
            options: {
                legend: { display: false }
            }
        })
    });
}

var monthStatistics = function () {
    $.ajax({
        url: '/NutritionTrack/YearStatistic',
        data: {
            year: year,
            macronutrient: $('#macronutrientSelect').val()
        }
    }).done(function (data) {

        new Chart("yearlyChart", {
            type: "line",
            data: {
                labels: data.xValues,
                datasets: [{
                    data: data.yValues,
                    borderColor: "#cb997e",
                    fill: false
                }]
            },
            options: {
                legend: { display: false }
            }
        })
    });
}
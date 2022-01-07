var year = $('#year').val();
var month = $('#month').val();
var macronutrientVal = document.getElementById('macronutrientSelect').value
var macronutrientName = "Calories"


$(document).ready(function () {
    selecetMacroChange();
});

var selecetMacroChange = function () {
    macronutrientVal = document.getElementById('macronutrientSelect').value

    switch (macronutrientVal) {
        case "1":
            macronutrientName = "Calories";
            break;
        case "2":
            macronutrientName = "Proteins";
            break;
        case "3":
            macronutrientName = "Carbohidrates";
            break;
        case "4":
            macronutrientName = "Fats";
            break;
    }

    monthStatistics();
    yearStatistics();
}

var monthStatistics = function () {

    $.ajax({
        url: '/NutritionTrack/MonthStatistic',
        data: {
            year: year,
            month: month,
            macronutrient: macronutrientVal
        }
    }).done(function (data) {
        $("#monthContainer").empty();
        document.getElementById("monthContainer").innerHTML = '<canvas id="monthlyChart" style="width: 100 %; max-width: 700px; margin: auto"></canvas>';

        var ctxPC = document.getElementById("monthlyChart").getContext('2d');

        new Chart(ctxPC, {
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
                legend: { display: false },
                scales: {
                    yAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Quantity of ' + macronutrientName + ' (g)'
                        }
                    }],
                    xAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Day of the month'
                        }
                    }]
                }
            }
        })
    });
}

var yearStatistics = function () {
    $.ajax({
        url: '/NutritionTrack/YearStatistic',
        data: {
            year: year,
            macronutrient: macronutrientVal
        }
    }).done(function (data) {
        $("#yearContainer").empty();
        document.getElementById("yearContainer").innerHTML = '<canvas id="yearlyChart" style="width: 100 %; max-width: 700px; margin: auto"></canvas>';

        var ctxPC = document.getElementById("yearlyChart").getContext('2d');

        new Chart(ctxPC, {
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
                legend: { display: false },
                scales: {
                    yAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Average quantity of ' + macronutrientName + ' (g)'
                        }
                    }],
                    xAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Month of the year'
                        }
                    }]
                }
            }
        })
    });
}
var ctxP = document.getElementById("pieChartCalo").getContext('2d');
var recommendedCalories = parseFloat($('#recommendedCalories').val());
var totalCalories = parseFloat($('#totalCalories').val());
var remainingCalories = recommendedCalories - totalCalories;

var myPieChart = new Chart(ctxP, {
    type: 'pie',
    data: {
        labels: [
            "Remaining Calories (g)",
            "Cosumed Calories (g)"
        ],
        datasets: [{
            data: [remainingCalories, totalCalories],
            backgroundColor: ["#f4a261", "#e76f51"]
        }]
    },
});


var ctxPC = document.getElementById("pieChartCarbs").getContext('2d');
var recommendedCarbs = parseFloat($('#recommendedCarbs').val());
var totalCarbs = parseFloat($('#totalCarbs').val());
var remainingCarbs = recommendedCarbs - totalCarbs;

var myPieChartC = new Chart(ctxPC, {
    type: 'pie',
    data: {
        labels: [
            "Remaining Carbs (g)",
            "Cosumed Carbs (g)"
        ],
        datasets: [{
            data: [remainingCarbs, totalCarbs],
            backgroundColor: ["#f4a261", "#e76f51"]
        }]
    },
});

var ctxPF = document.getElementById("pieChartFats").getContext('2d');
var recommendedFats = parseFloat($('#recommendedFats').val());
var totalFats = parseFloat($('#totalFats').val());
var remainingFats = recommendedFats - totalFats;

var myPieChartF = new Chart(ctxPF, {
    type: 'pie',
    data: {
        labels: [
            "Remaining Fats (g)",
            "Cosumed Fats (g)"
        ],
        datasets: [{
            data: [remainingFats, totalFats],
            backgroundColor: ["#f4a261", "#e76f51"]
        }]
    },
});


var ctxPP = document.getElementById("pieChartProts").getContext('2d');
var recommendedProt = parseFloat($('#recommendedProt').val());
var totalProt = parseFloat($('#totalProt').val());
var remainingProt = recommendedProt - totalProt;

var myPieChartP = new Chart(ctxPP, {
    type: 'pie',
    data: {
        labels: [
            "Remaining Prot (g)",
            "Cosumed Prot (g)"
        ],
        datasets: [{
            data: [remainingProt, totalProt],
            backgroundColor: ["#f4a261", "#e76f51"]
        }]
    },
});

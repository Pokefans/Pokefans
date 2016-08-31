var lastMonthDays = ["1 Aug", "2 Aug"];
var lastMonthHits = ["91", "78"];

var lastYearMonths = ["Aug 2016"];
var lastYearHits = ["169"];

$(document).ready(function () {
    var lastMonthChartCtx = document.getElementById("lastMonthChart").getContext('2d');
    var last12MonthsChartCtx = document.getElementById("last12MonthsChart").getContext('2d');

    var lastMonthData = {
        labels: lastMonthDays,
        datasets: [
            {
                label: "My First dataset",
                fillColor: "rgba(220,220,220,0.2)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: lastMonthHits
            }
        ]
    };

    var lastYearData = {
        labels: lastYearMonths,
        datasets: [
            {
                label: "My First dataset",
                fillColor: "rgba(220,220,220,0.2)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: lastYearHits
            }
        ]
    };

    var lastMonthChart = new Chart(lastMonthChartCtx).Bar(lastMonthData, { bezierCurve: false });
    var last12MonthsChart = new Chart(last12MonthsChartCtx).Bar(lastYearData, { bezierCurve: false });
});
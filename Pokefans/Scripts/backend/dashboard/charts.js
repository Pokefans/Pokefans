// copyright 2019 the pokefans authors. see copying.md for details

var charts = {};

function loadChart(target) {
    $.ajax({
        url: target.data("load"),
        xhrFields: {
                    withCredentials: true
                },
        success: function(data) {
            $("#"+target.data("spinner")).hide();

            if(target.attr("id") in charts) {
                var myChart =  charts[target.attr("id")];
                myChart.data = data.data;
                myChart.update();
            }
            else {
                var myChart = new Chart(target[0], data);
                charts[target.attr("id")] = myChart;
            }
        },
        error: function()  {
            $("#"+target.data("spinner")).hide();
            $("#"+target.data("error")).removeClass("hidden");
        }
    });
}

$("canvas[role='dashboard-chart']").each(function() {
    var self = $(this);
    loadChart(self);
});

$("a[role='reload-chart']").click(function(ev) {
    ev.preventDefault();
    var target = $("#"+$(this).data("reload"));
    loadChart(target);
});

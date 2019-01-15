// copyright 2019 the pokefans authors. see copying.md for legal info

$("div[role='dashboard-chart-container']").each(function() {
        var self = $(this);
        ko.applyBindings(new ChartViewModel(self), self[0]);
});

$("table[role='ko-table']").each(function() {
    var self = $(this);
    ko.applyBindings(new DashboardTableViewModel(self), document.getElementById(self.data("bind")));
});

$("div[role='ko-data']").each(function () {
    var self = $(this);
    ko.applyBindings(new DashboardDataViewModel(self), self[0]);
});


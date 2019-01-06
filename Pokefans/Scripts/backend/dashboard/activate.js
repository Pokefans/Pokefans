// copyright 2019 the pokefans authors. see copying.md for legal info

$("table[role='ko-table']").each(function() {
    var self = $(this);
    ko.applyBindings(new DashboardTableViewModel(self, document.getElementById(self.data("bind"))));
});

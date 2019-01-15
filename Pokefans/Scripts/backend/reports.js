// copyright 2019 the pokefans authors. see copying.md for details

function ReportsViewModel() {
    this.selected = ko.observable(null);

    this.baseurl = $("table[role='report-table']").first().data("load");

    this.investigate = function (id) {
        var self = this;

        alert(self.baseurl + id.toString());
    }
}

ko.applyBindings(new ReportsViewModel())
// copyright 2019 the pokefans authors. see copying.md for legal info

function DashboardTableViewModel(selector) {
    this.tableData = ko.observableArray();
    this.loaded = ko.observable(false);
    this.error = ko.observable(false);
    this.dateRange = ko.observable([moment(selector.data("min")), moment(selector.data("max"))]);

    var self = this;
    this.target = selector;

    this.spinner = $("#"+this.target.data("spinner"));
    this.hasTimeRangePicker = this.target.data("timepicker") == true;
    this.loading = false;
    this.url = this.target.data("load");

    this.loadData = function() {
        if(this.loading) return;

        this.loading = true;
        this.spinner.show();
        this.spinner.addClass("bokeh");
        var url = this.url;
        if(this.hasTimeRangePicker) {
            url += "?from="+this.dateRange()[0].format("YYYY-MM-DD")+"&until="+this.dateRange()[1].format("YYYY-MM-DD");
        }

        var self = this;
        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                self.error(false);
                self.spinner.hide();
                self.spinner.removeClass("bokeh");

                self.tableData.removeAll();
                ko.utils.arrayPushAll(self.tableData, data);

                self.loading = false;
                self.loaded(true);
            },
            error: function() {
                self.spinner.hide();
                self.spinner.removeClass("bokeh");
                self.error(true);
                self.loading = false;
            }
        });
    }

    this.loadData();
}


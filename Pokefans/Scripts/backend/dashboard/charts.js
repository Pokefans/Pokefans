// copyright 2019 the pokefans authors. see copying.md for details
function ChartViewModel(target) {
    this.error = ko.observable(false);
    this.dateRange = ko.observable([moment(target.data("min")), moment(target.data("max"))]);

    this.target = target;
    this.spinner = $("#"+target.data("spinner"));
    this.canvas = document.getElementById(target.data("canvas"));
    this.url = target.data("load");
    this.hasTimeRangePicker = target.data("timepicker") == true;
    this.loading = false;
    this.chart = null;

    this.loadData = function() {
        if(this.loading == true) return;

        // yadda yadda locking yadda yadda
        // stfu, js is single threaded anyways. this is with the intention of
        // preventing angry users of refreshing with bad network connection
        // not to end race condition once and for all.
        this.loading = true;

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
            success: function(data) {
                self.error(false);
                self.spinner.hide();
                self.spinner.removeClass("bokeh");

                if(self.chart != null) {
                    self.chart.data = data.data;
                    self.chart.update();
                }
                else {
                    self.chart = new Chart(self.canvas, data);
                }
                self.loading = false;
            },
            error: function()  {
                self.loading = false;
                self.spinner.hide();
                self.spinner.removeClass("bokeh");
                self.error(true);
            }
        });
    }

    this.loadData();

}
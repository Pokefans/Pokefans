// copyright 2019 the pokefans authors. see copying.md for legal info

function DashboardDataViewModel(selector) {
    this.model = ko.observable();
    this.loaded = ko.observable(false);
    this.error = ko.observable(false);

    this.innermodel = null;

    var self = this;
    this.target = selector;
    this.spinner = $("#"+this.target.data("spinner"));
    this.loading = false;

    this.loadData = function() {
        // only one at a time.
        if(this.loading) return;

        this.loading = true;
        this.spinner.addClass("bokeh");
        this.spinner.show();
        var self = this;
        $.ajax({
            url: self.target.data("load"),
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                self.error(false)
                self.spinner.hide();
                self.spinner.removeClass("bokeh");
                if (self.innermodel == null) {
                    self.innermodel = ko.mapping.fromJS(data);
                } else {
                    ko.mapping.fromJS(data, self.innermodel);
                }
                self.model(self.innermodel);
                self.loading = false;
                self.loaded(true);
            },
            error: function() {
                self.spinner.hide();
                self.spinner.removeClass("bokeh");
                self.error(true)
                self.loading = false;
            }
        });
    }

    this.loadData();
}


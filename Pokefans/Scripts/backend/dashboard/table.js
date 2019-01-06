// copyright 2019 the pokefans authors. see copying.md for legal info

function DashboardTableViewModel(selector) {
    this.pmreports = ko.observableArray();
    this.loaded = ko.observable(false);

    var self = this;
    this.target = selector;
    this.error = $("#"+this.target.data("error"));
    this.spinner = $("#"+this.target.data("spinner"));
    this.loading = false;

    $("a[role='"+this.target.data("reload")+"']").click(function(ev) {
        ev.preventDefault();
        if(!self.loading)
            self.loadData();
    });

    this.loadData = function() {
        this.loading = true;
        this.spinner.show();
        var self = this;
        $.ajax({
            url: self.target.data("load"),
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                if(!self.error.hasClass("hidden")) this.error.addClass("hidden");
                self.spinner.hide();
                self.pmreports.removeAll();
                ko.utils.arrayPushAll(self.pmreports, data);
                self.loading = false;
                self.loaded(true);
            },
            error: function() {
                self.spinner.hide();
                self.error.removeClass("hidden");
                self.loading = false;
            }
        });
    }

    this.loadData();
}


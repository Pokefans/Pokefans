// Copyright 2019 the pokefans authors. See copying.md for legal info

function GlobalBansViewModel(endpoint) {
    this.error = ko.observable();
    this.loading = ko.observable(false);
    this.id = ko.observable();
    this.banMessage = ko.observable();
    this.reason = ko.observable();
    this.bvs = ko.observable();
    this.banButton = ko.observable();

    this.endpoint = endpoint;

    var expirepicker = flatpickr($("#globalbanexpirepicker"), {
        wrap: true,
        enableTime: true,
        time_24hr: true,
        min_date: "today",
        altInput: true,
        altFormat: "d.m.Y H:i",
        dateFormat: "Y-m-d H:i"    ,
    });
        
    var self = this;
    this.update = function() {
        var mydata = {
                id = self.id,
                reason = self.reason,
                bvs = self.bvs,
            };

        if(expirepicker.selectedDates.length > 0) {
            mydata.expireTime = expirepicker.selectedDates[0].toString("Y-m-d H:i");
        }

        self.loading(true);
        $.ajax({
            type: "POST",
            url: self.endpoint,
            data: mydata,
            success: function(data) {
                self.banMessage(data.Message);
                self.bvs("");
                self.banButton(data.Button);
                self.loading(false);
            },
            error: function(data) {
                self.error(true);
                self.loading(false);
            }
        });
    }
}
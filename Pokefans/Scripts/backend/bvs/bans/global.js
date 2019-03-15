// Copyright 2019 the pokefans authors. See copying.md for legal info

function GlobalBanViewModel(endpoint) {
    this.error = ko.observable();
    this.loading = ko.observable(false);
    this.id = ko.observable();
    this.banMessage = ko.observable();
    this.reason = ko.observable();
    this.bvs = ko.observable();
    this.banButton = ko.observable();

    this.endpoint = endpoint;
    this.csrftoken = getCSRFToken();

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
                id: self.id,
                reason: self.reason,
                bvs: self.bvs,
                __RequestVerificationToken: self.csrftoken
            };

        if(expirepicker.selectedDates.length > 0) {
            // javascript is a piece of utter crap
            var date = expirepicker.selectedDates[0];
            mydata.expireTime = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes();
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
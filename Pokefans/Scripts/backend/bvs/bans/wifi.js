// Copyright 2019 the pokefans authors. See copying.md for legal info.

function WifiBanViewModel(endpoint) {

    this.loading = ko.observable(false);
    this.error = ko.observable(false);

    this.canAdd = ko.observable();
    this.canInterest = ko.observable();
    this.id = ko.observable();

    this.endpoint = endpoint;

    var canAddPicker = flatpickr($("#wificanaddexpirepicker"), {
        wrap: true,
        enableTime: true,
        time_24hr: true,
        min_date: "today",
        altInput: true,
        altFormat: "d.m.Y H:i",
        dateFormat: "Y-m-d H:i"    ,
    });

    var canInterestPicker = flatpickr($("#wificaninterestexpirepicker"), {
        wrap: true,
        enableTime: true,
        time_24hr: true,
        min_date: "today",
        altInput: true,
        altFormat: "d.m.Y H:i",
        dateFormat: "Y-m-d H:i"    ,
    });

    this.update = function() {
        this.error(false);
        this.loading(true);

        var postdata = {
                CanAddOffers: self.canAdd,
                CanInterest: self.canInterest,
                id: self.id,
                __RequestVerificationToken: getCSRFToken()
            };

        if(canAddPicker.selectedDates.length > 0) {
            // javascript is a piece of utter crap
            var date = canAddPicker.selectedDates[0];
            postdata.ExpireAddOffers = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes();
        }
      if(canInterestPicker.selectedDates.length > 0) {
            // javascript is a piece of utter crap
            var date = canInterestPicker.selectedDates[0];
            postdata.ExpireInterest = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes();
        }

        $.ajax({
            type: "POST",
            url: self.endpoint,
            data: postdata,
            success: function (data) {
                self.loading(false);
            },
            error: function() {
                self.error(true);
                self.loading(false);
            }
        });
    }

    this.id = ko.observable();
}
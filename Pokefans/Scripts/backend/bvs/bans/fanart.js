// copyright 2019 the pokefans authors. see copying.md for legal info.

function FanartBansViewModel(endpoint) {

    this.CanEdit = ko.observable();
    this.CanDelete = ko.observable();
    this.CanUpload = ko.observable();
    this.CanRate = ko.observable();
    this.id = ko.observable();

    this.loading = ko.observable(false);
    this.error = ko.observable();
    this.__RequestVerificationToken = getCSRFToken();

    this.endpoint = endpoint;


    var self = this;
    this.update = function() {
        this.error(false);
        this.loading(true);
        $.ajax({
            type: "POST",
            url: self.endpoint,
            data: ko.toJSON(self), // serializes the form's elements.
            success: function (data) {
                ko.mapping.fromJSON(data, self);
                this.loading(false);
            },
            error: function() {
                this.error(true);
                this.loading(false);
            }
        });
    }

}
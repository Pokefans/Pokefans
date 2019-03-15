// copyright 2019 the pokefans authors. see copying.md for legal info.

function FanartBansViewModel(endpoint) {

    this.CanEdit = ko.observable();
    this.CanDelete = ko.observable();
    this.CanUpload = ko.observable();
    this.CanRate = ko.observable();
    this.id = ko.observable();

    this.loading = ko.observable(false);
    this.error = ko.observable();

    this.endpoint = endpoint;


    var self = this;
    this.update = function() {
        this.error(false);
        this.loading(true);
        $.ajax({
            type: "POST",
            url: self.endpoint,
            data:  {
                CanEdit: self.CanEdit,
                CanDelete: self.CanDelete,
                CanUpload: self.CanUpload,
                CanRate: self.CanRate,
                id: self.id,
                __RequestVerificationToken: getCSRFToken()
            },
            success: function (data) {
                self.CanEdit = data.CanEdit;
                self.CanDelete = data.CanDelete;
                self.CanUpload = data.CanUpload;
                self.CanRate = data.CanRate;
                self.loading(false);
            },
            error: function() {
                self.error(true);
                self.loading(false);
            }
        });
    }

}
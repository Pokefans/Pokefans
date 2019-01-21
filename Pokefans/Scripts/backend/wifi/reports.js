// copyright 2019 the pokefans authors. see copying.md for details



function WifiReportsViewModel() {
    this.selected = ko.observable(null);
    this.error = ko.observable(false);
    this.innermodel = null;
    this.loading = ko.observable(false);
    this.currentid = null;

    var target = $("#wifi-reports");
    this.baseurl = target.data("load");
    this.cheaturl = target.data("cheat");
    this.resolveurl = target.data("resolve");
    this.deleteurl = target.data("delete");
    this.csrftoken = getCSRFToken();

    var self = this;
    this._setData = function(data) {
        self.error(false)
        if (self.innermodel == null) {
            self.innermodel = ko.mapping.fromJS(data);
        } else {
            ko.mapping.fromJS(data, self.innermodel);
        }
        self.selected(self.innermodel);
    }
    
    this.investigate = function (id) {
        if(self.loading()) return;

        self.loading(true);
        self.currentid = id;
        var url = self.baseurl + id.toString();
        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                self._setData(data);
                self.loading(false);
            },
            error: function() {
                self.error(true)
                self.loading(false);
            }
        });
    }
    this.reload = function() {
        self.investigate(self.currentid);
    }

    this.markCheat = function() {
        if(self.loading()) return;

        self.loading(true);
        $.ajax({
            type: "POST",
            url: self.cheaturl,
            data: {
                id: self.currentid,
                __RequestVerificationToken: self.csrftoken
            },
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                self._setData(data);
                self.loading(false);
            },
            error: function() {
                self.error(true);
                self.loading(false);
            }
        });
    }

    this.delete = function() {
        if(self.loading()) return;

        self.loading(true);
        $.ajax({
            type: "POST",
            url: self.deleteurl,
            data: {
                id: self.currentid,
                __RequestVerificationToken: self.csrftoken
            },
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                self._setData(data);
                self.loading(false);
            },
            error: function() {
                self.error(true);
                self.loading(false);
            }
        });
    }

    this.resolve = function() {
        if(self.loading()) return;

        self.loading(true);
        $.ajax({
            type: "POST",
            url: self.resolveurl,
            data: {
                id: self.currentid,
                __RequestVerificationToken: self.csrftoken
            },
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                self._setData(data);
                self.loading(false);
            },
            error: function() {
                self.error(true);
                self.loading(false);
            }
        });
    }

    this.close = function () {
        self.selected(null);
    }
}

if(document.getElementById("wifi-reports") != undefined) 
    ko.applyBindings(new WifiReportsViewModel())
// Copyright 2016 the pokefans authors. See copying.md for legal info.
if (typeof (Waypoint) != undefined) {
    var sticky = new Waypoint.Sticky({
        element: $("#fanartbar")
    });
}

function FanartImage(data) {
    var self = this;
    self.title = data.Title;
    self.rating = ko.observable(data.Rating); // rating can change - the rest can not, so there's no point in wrapping it.
    self.author = data.Author;
    self.description = data.Description;
    self.imageUri = function () {
        return "//files." + getDomainName(window.location.hostname) + "/fanart/uploads/" + data.Url;
    };
    self.comments = ko.observableArray([]); // oh, comments can change too of course.

    $.ajax({
        url: '/api/v1/comments/' + data.Id
    }).done(function (cdata) {
        $.map(cdata, function (comment) {
            self.comments.push(new Comment(comment));
        });
    });
}

function Comment(data) {
    // TODO
}

function FanartViewModel() {
    this.Fanarts = ko.observableArray([]);
    this.selectedFanart = ko.observable(null);

    // this basically groups the fanarts in rows of eight.
    // it actually transforms the array, so we can use a binding.
    this.groupedFanarts = ko.computed(function () {
        var rows = [], current = [];
        rows.push(current);
        for (var i = 0; i < this.Fanarts().length; i += 1) {
            current.push(this.Fanarts()[i]);
            if (((i + 1) % 8) === 0) {
                current = [];
                rows.push(current);
            }
        }
        return rows;
    }, this)
}

ko.applyBindings(new FanartViewModel());

function getDomainName(hostName) {
    return hostName.substring(hostName.lastIndexOf(".", hostName.lastIndexOf(".") - 1) + 1);
}
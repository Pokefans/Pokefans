// Copyright 2019 the pokefans authors. See copying.md for legal info.

function WifiBanViewModel(endpoint) {

    this.loading = ko.observable(false);
    this.canAdd = ko.observable();
    this.id = ko.observable();
}
// Copyright 2016 the pokefans authors. See copying.md for legal info.

function BvsBansViewModel() {
    this.fanartBans = new FanartBansViewModel($("#fanartbans").data("endpoint"));
    this.globalBan = new GlobalBanViewModel($("#globalban").data("endpoint"));
}

if(document.getElementById("#bvs-bans") != undefined)
    ko.applyBindings(new BvsBansViewModel());
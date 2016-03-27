// Copyright 2016 the pokefans authors. See copying.md for legal info.
if (typeof (Waypoint) != undefined) {
    var sticky = new Waypoint.Sticky({
        element: $("#fanartbar")
    });
}

$(document).ready(function () {
    // setup some button hooks, because html5 stuff

    var state = {
        index: "popular",
        start: 0,
        search: false
    };

    $('#new').click(function (e) {
        e.preventDefault();

        state.index = "new";

        window.history.pushState(state, "Neu", "/neu");

        load(state);
    });

    $('#popular').click(function (e) {
        e.preventDefault();

        state.index = "popular";

        window.history.pushState(state, "Beliebt", "/");
        load(state);
    });

    load(state);
});

function load(state) {

}
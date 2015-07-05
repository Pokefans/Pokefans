// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
$(document).ready(function () {
    $('#advertisingform').on('change', function () {
        var sel = $("option:selected", this);
        var target = $("#targetuser");
        if (sel.data("is-targeted").toLowerCase() == "true") {
            target.show("fast");
        }
        else {
            if (!target.hidden) {
                target.hide("fast");
            }
        }
    });
});
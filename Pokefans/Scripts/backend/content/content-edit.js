// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

$(function () {
    $("#html-label").hide();
    $("#css-label").hide();
    $("#switch-stacked").click(function () {
        $("#editors ul").hide();
        $("#html-label").show();
        $("#css-label").show();
        $("#editors .editorhost").removeClass("col-lg-6");
        $("#editors .editorhost").removeClass("tab-pane");
        $("#editors .editorhost").addClass("panel");
        $("#editors .editorhost").addClass("panel-default");
        $("#editors .editorhost .form-group").addClass("panel-body");
        $("#editors .editorhost .content-editor").height(200);
        resetEditorWrapping();
    });

    $("#switch-sidebyside").click(function () {
        $("#editors ul").hide();
        $("#html-label").show();
        $("#css-label").show();
        $("#editors .editorhost").removeClass("tab-pane");
        $("#editors .editorhost").addClass("col-lg-6");
        $("#editors .editorhost").addClass("panel");
        $("#editors .editorhost").addClass("panel-default");
        $("#editors .editorhost .form-group").addClass("panel-body");
        $("#editors .editorhost .content-editor").height(500);
        resetEditorWrapping();
    });

    $("#switch-tabbed").click(function () {
        $("#html-label").hide();
        $("#css-label").hide();
        $("#editors ul").show();
        $("#editors .editorhost").removeClass("col-lg-6");
        $("#editors .editorhost").removeClass("panel");
        $("#editors .editorhost").removeClass("panel-default");
        $("#editors .editorhost .form-group").removeClass("panel-body");
        $("#editors .editorhost").addClass("tab-pane");
        $("#editors .editorhost .content-editor").height(500);
        resetEditorWrapping();
    });

    $("[id^=\"ic\"]").click(function (e) {
        e.preventDefault();
        editors[0].insert($(this).attr("data-code"));
    });

    $(document).on("submit", "#content-form", function (e) {
        window.onbeforeunload = null;
    });

    $(window).bind("keydown", function (e) {
        var saveCommandPressed = ((e.which == 83 || e.which == 115) && e.ctrlKey) || (e.which == 19);

        if (saveCommandPressed) {
            window.onbeforeunload = null;
            $("#content-form").submit();
            e.preventDefault();
            return false;
        }

        return true;
    });
});

function resetEditorWrapping() {
    for (var i = 0; i < editors.length; i++) {
        editors[i].getSession().setUseWrapMode(false);
        editors[i].getSession().setUseWrapMode(true);
    }
}

function previewContent() {
    // Workaround for Ace Editor, it doesn't propagate a changed value.
    var formData = {};
    $.each($('#content-form').serializeArray(), function (_, kv) {
        formData[kv.name] = kv.value;
    });
    formData["UnparsedContent"] = editors[0].getValue();
    formData["StylesheetCode"] = editors[1].getValue();

    $.post(previewUrl, $.param(formData))
        .done(function (data) {
            $("#preview > #content").html(data);
        });

    $('.nav-tabs a[href="#preview"]').tab('show');

    return false;
}

window.onbeforeunload =  function(e) {
    var message = "Du hast deine Änderungen nicht gespeichert. Willst du die Seite wirklich verlassen?";

    if (editors[0].session.getUndoManager().isClean() && editors[1].session.getUndoManager().isClean()) {
        return null;
    }

    if (e) {
        e.returnValue = message;
    }

    return message;
}; 
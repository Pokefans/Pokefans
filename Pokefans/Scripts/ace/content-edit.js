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

    $('[id^="ic"]').click(function (e) {
        e.preventDefault();
        editors[0].insert($(this).attr("data-code"));
    });

});
function resetEditorWrapping() {
    for (i = 0; i < editors.length; i++) {
        editors[i].getSession().setUseWrapMode(false);
        editors[i].getSession().setUseWrapMode(true);
    }
}
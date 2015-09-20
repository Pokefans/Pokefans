//Copyright 2015 the pokefans authors. See copying.md for legal info.
var arc = new arcrypt();

$("#encrypt").click(function () {
    $("#output").val(arc.encrypt($("#input").val(), $("#arv3").is(':checked')));
});
$("#decrypt").click(function () {
    $("#output").val(arc.decrypt($("#input").val(), $("#arv3").is(':checked')));
});
$("#copy-to-input").click(function () {
    $("#input").val($("#output").val())
});
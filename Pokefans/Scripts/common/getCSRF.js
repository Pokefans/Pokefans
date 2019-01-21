// copyright 2019 the pokefans authors. see copying.md for legal info

function getCSRFToken() {
    var t = document.getElementsByName("__RequestVerificationToken");
    if(t.length > 0) {
        return t[0].value;
    }
    return undefined;
}

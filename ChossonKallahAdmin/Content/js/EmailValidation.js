function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }
};
function isUrlValid(url) {
    return /^(?:(?:https?|ftp):\/\/)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/\S*)?$/i.test(url);
};
function onlyAlphabetsandNumber(e) {
    return (e.charCode > 64 && e.charCode < 91) || (e.charCode > 47 && e.charCode < 58) || (e.charCode > 96 && e.charCode < 123) || e.charCode == 45;
}
function onlyNumber(e) {
    return (e.charCode > 47 && e.charCode < 58);
}
function PhoneNumber(e) {
    return (e.charCode > 47 && e.charCode < 58) || (e.charCode == 40) || (e.charCode == 41) || (e.charCode == 32) || (e.charCode == 43) || (e.charCode == 45);
}
function onlyOnetoFiveNumber(e) {
    return (e.charCode > 48 && e.charCode < 54);
}

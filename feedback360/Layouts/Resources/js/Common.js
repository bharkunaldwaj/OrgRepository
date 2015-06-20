
//function to allow only numeric value in text box
function AlphaOnly(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode <= 13) {
        return true;
    }
    else {
        var keyChar = String.fromCharCode(charCode);
        var re = /[a-zA-Z]/
        return re.test(keyChar);
    }


}
function AlphaNumOnly(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode <= 13) {
        return true;
    }
    else if (charCode == 32) {
        var keyChar = String.fromCharCode(charCode);
        return keyChar;
    }
    else {
        var keyChar = String.fromCharCode(charCode);
        var re = /[a-zA-Z0-9_-]/
        return re.test(keyChar);
    }

}

//function to allow only numeric value in text box
function NumberOnly(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode >= 48 && charCode <= 57) || (charCode == 8 || charCode == 127)) {
        return true;
    }
    else {
        return false;
    }
}

//function to allow only numeric value in text box
function DecimalOnly(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode >= 48 && charCode <= 57) || (charCode == 8 || charCode == 127 || charCode == 46)) {
        return true;
    }
    else {
        return false;
    }
}

//reset Date
function SetDTPickerDate(htmlDate, aspDate)
{
    
    var aspD = document.getElementById("ctl00_cphMaster_" + aspDate);
    if (document.getElementById("ctl00_cphMaster_" + htmlDate).value != "")
    {
        document.getElementById("ctl00_cphMaster_" + aspDate).value = document.getElementById("ctl00_cphMaster_" + htmlDate).value;
    }
}

function ResetDTPickerDate(htmlDate, aspDate)
{
    
    var aspD = document.getElementById("ctl00_cphMaster_" + aspDate);
    
    if (document.getElementById("ctl00_cphMaster_" + aspDate))
    {
        if (aspD.value != "")
        {
            document.getElementById("ctl00_cphMaster_" + htmlDate).value = aspD.value;
        }
    }
}

function TextAreaMaxLengthChecker(id, length)
{

    length = length - 1;
    var val = document.getElementById(id).value;
    if (val.length <= length)
        return true;
    else
    {

        event.keyCode = 0;


    }
}
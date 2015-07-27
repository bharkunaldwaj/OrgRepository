﻿
function ValidateRepeaterCandidateList() {
    debugger;
    var reperaterCandidate = document.getElementById("RepeatorCandidateList");
    var inputType = reperaterCandidate.getElementsByTagName("input");
    var selectType = reperaterCandidate.getElementsByTagName("select");

    var counter;
    var invalidCounter = 0;
    for (var i = 0; i < selectType.length; i++) {

        var ddlRelationship = (selectType[i].id).split("_ddlRelationship")[0];

        var index = ddlRelationship.split("ctl00_cphMaster_rptrCandidateList_ctl")[1];

        var textBoxControlID = document.getElementById(inputType[i].id);

        // if (selectType[i] != undefined) {
        var relationShipID = document.getElementById(selectType[i].id);

        if (relationShipID.type == "select-one") {
            var relationShipValue = relationShipID.value;

            var TextBoxName = document.getElementById("ctl00_cphMaster_rptrCandidateList_ctl" + index + "_txtName");
            var textBoxEmail = document.getElementById("ctl00_cphMaster_rptrCandidateList_ctl" + index + "_txtEmailID");

            if (relationShipValue == "0") {

                if (TextBoxName.value != "" && textBoxEmail.value != "") {
                    relationShipID.style.borderColor = "red";
                    invalidCounter = invalidCounter + 1;
                }
                else if (TextBoxName.value == "" && textBoxEmail.value != "") {
                    relationShipID.style.borderColor = "red";
                    TextBoxName.style.borderColor = "red";
                    invalidCounter = invalidCounter + 1;
                }
                else if (TextBoxName.value != "" && textBoxEmail.value == "") {
                    relationShipID.style.borderColor = "red";
                    textBoxEmail.style.borderColor = "red";
                }
                else {
                    relationShipID.style.borderColor = "";
                    TextBoxName.style.borderColor = "";
                    textBoxEmail.style.borderColor = "";

                    if (invalidCounter > 1)
                        invalidCounter = invalidCounter - 1;
                }
            }
            else {

                if (TextBoxName.value == "" && textBoxEmail.value == "") {
                    TextBoxName.style.borderColor = "red";
                    textBoxEmail.style.borderColor = "red";
                    invalidCounter = invalidCounter + 1;
                }
                else if (TextBoxName.value != "" && textBoxEmail.value == "") {

                    textBoxEmail.style.borderColor = "red";
                    invalidCounter = invalidCounter + 1;
                }
                else if (TextBoxName.value == "" && textBoxEmail.value != "") {
                    TextBoxName.style.borderColor = "red";
                    invalidCounter = invalidCounter + 1;
                }
                else {
                    relationShipID.style.borderColor = "";
                    TextBoxName.style.borderColor = "";
                    textBoxEmail.style.borderColor = "";

                 if (invalidCounter>1)
                     invalidCounter = invalidCounter - 1;
                }
            }
        }
    }


    if (invalidCounter > 0) {
        return false;
    }
    else {
        return true;
    }
}


function checkEmail(email) {

    //var email = document.getElementById('txtEmail');
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

    return filter.test(email);
    
    
 }


function ValidateRepeaterSurveyCandidateList() {

    var reperaterCandidate = document.getElementById("grdvAssignQuestionnaire");
    var inputType = reperaterCandidate.getElementsByTagName("input");
    //var selectType = reperaterCandidate.getElementsByTagName("select");

    var counter;
    var invalidCounter = 0;
    for (var i = 0; i < inputType.length; i++) {
        var textBoxControlID = document.getElementById(inputType[i].id);

        if (textBoxControlID.type == "text") {
            if (inputType[i].id.indexOf("txtCandidateEmail") != -1) {

                var resultValue = textBoxControlID.value;

               if(!checkEmail(resultValue))
               {
                   textBoxControlID.style.borderColor = "red";
                   invalidCounter = invalidCounter + 1;
               }
               else
               {
                   textBoxControlID.style.borderColor = "";
               }
               
            }


            var resultValue = textBoxControlID.value;

            if (resultValue == "") {
                textBoxControlID.style.borderColor = "red";
                invalidCounter = invalidCounter + 1;
            }
        }
    }

    if (invalidCounter > 0) {
        alert("Please enter valid details.");
        return false;
    }
    else {
        return true;
    }
}
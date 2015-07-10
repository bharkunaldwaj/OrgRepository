
function ValidateRepeaterCandidateList() {

    var reperaterCandidate = document.getElementById("RepeatorCandidateList");
    var inputType = reperaterCandidate.getElementsByTagName("input");
    var selectType = reperaterCandidate.getElementsByTagName("select");

    var counter;
    var invalidCounter = 0;
    for (var i = 0; i < inputType.length; i++) {
        var textBoxControlID = document.getElementById(inputType[i].id);

        if (selectType[i] != undefined) {
            var relationShipID = document.getElementById(selectType[i].id);

            if (relationShipID.type == "select-one") {
                var relationShipValue = relationShipID.value;

                if (relationShipValue == "0") {
                    relationShipID.style.borderColor = "red";
                    invalidCounter = invalidCounter + 1;
                }
            }
        }

        if (textBoxControlID.type == "text") {
            var resultValue = textBoxControlID.value;

            if (resultValue == "") {
                textBoxControlID.style.borderColor = "red";
                invalidCounter = invalidCounter + 1;
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
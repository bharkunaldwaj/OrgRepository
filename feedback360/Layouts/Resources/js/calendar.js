function positionInfo(object) {

  var p_elm = object;

  this.getElementLeft = getElementLeft;
  function getElementLeft() {
    var x = 0;
    var elm;
    if(typeof(p_elm) == "object"){
      elm = p_elm;
    } else {
      elm = document.getElementById(p_elm);
    }
    while (elm != null) {
      x+= elm.offsetLeft;
      elm = elm.offsetParent;
    }
    return parseInt(x);
  }

  this.getElementWidth = getElementWidth;
  function getElementWidth(){
    var elm;
    if(typeof(p_elm) == "object"){
      elm = p_elm;
    } else {
      elm = document.getElementById(p_elm);
    }
    return parseInt(elm.offsetWidth);
  }

  this.getElementRight = getElementRight;
  function getElementRight(){
    return getElementLeft(p_elm) + getElementWidth(p_elm);
  }

  this.getElementTop = getElementTop;
  function getElementTop() {
    var y = 0;
    var elm;
    if(typeof(p_elm) == "object"){
      elm = p_elm;
    } else {
      elm = document.getElementById(p_elm);
    }
    while (elm != null) {
      y+= elm.offsetTop;
      elm = elm.offsetParent;
    }
    return parseInt(y);
  }

  this.getElementHeight = getElementHeight;
  function getElementHeight(){
    var elm;
    if(typeof(p_elm) == "object"){
      elm = p_elm;
    } else {
      elm = document.getElementById(p_elm);
    }
    return parseInt(elm.offsetHeight);
  }

  this.getElementBottom = getElementBottom;
  function getElementBottom(){
    return getElementTop(p_elm) + getElementHeight(p_elm);
  }
}

function CalendarControl() {

  var calendarId = 'CalendarControl';
  var currentYear = 0;
  var currentMonth = 0;
  var currentDay = 0;

  var selectedYear = 0;
  var selectedMonth = 0;
  var selectedDay = 0;

  var months = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
  var dateField = null;

  function getProperty(p_property){
   
    var p_elm = calendarId;
    var elm = null;

    if(typeof(p_elm) == "object"){
      elm = p_elm;
    } else {
      elm = document.getElementById(p_elm);
    }
    if (elm != null){
      if(elm.style){
        elm = elm.style;
        if(elm[p_property]){
          return elm[p_property];
        } else {
          return null;
         //return elm[p_property];
        }
      } else {
        return null;
        //return elm[p_property];
      }
    }
  }

  function setElementProperty(p_property, p_value, p_elmId){
    var p_elm = p_elmId;
    var elm = null;

    if(typeof(p_elm) == "object"){
      elm = p_elm;
    } else {
      elm = document.getElementById(p_elm);
    }
    if((elm != null) && (elm.style != null)){
      elm = elm.style;
      elm[ p_property ] = p_value;
    }
  }

  function setProperty(p_property, p_value) {
    setElementProperty(p_property, p_value, calendarId);
  }

  function getDaysInMonth(year, month) {
    return [31,((!(year % 4 ) && ( (year % 100 ) || !( year % 400 ) ))?29:28),31,30,31,30,31,31,30,31,30,31][month-1];
  }

  function getDayOfWeek(year, month, day) {
    var date = new Date(year,month-1,day)
    return date.getDay();
  }

  this.setDate = setDate;
  function setDate(year, month, day) {
    if (dateField) {
      if (month < 10) {month = "0" + month;}
      if (day < 10) {day = "0" + day;}

      //Changed the Date in Format mm-dd-yyyy
      //var dateString = day+"/"+month+"/"+year;
      var dateString = month+"/"+day+"/"+year;
      //var dateString = month+"/"+day+"/"+year;
      //alert(dateString);
      dateField.value = dateString;
      //CalculateAge('dtDOBMand', 'txtAge');

      hide();
    }
    return;
}

  function CalculateAge(dt, ag) {
      //alert(document.getElementById(dt));
      if (document.getElementById(ctrl+ag)) {
          var d = document.getElementById(ctrl + dt).value;
          if (d != "") {
              if (isDate(d) == false) {
                  //document.getElementById(ctrl + dt).focus();
                  return false;
              }
              myDOB = d.split('/');
              myDate = myDOB[0];
              myMonth = myDOB[1];
              myYear = myDOB[2];

              var age;
              var now = new Date();
              var todayDate = now.getDate();
              var todayMonth = now.getMonth() + 1;
              var todayYear = now.getYear();
              if (myDate <= todayDate) {
                  if (myMonth <= todayMonth)
                      age = todayYear - myYear;
                  else if (myMonth > todayMonth)
                      age = todayYear - myYear - 1;
              }
              else if (myDate > todayDate) {
                  if (myMonth < todayMonth)
                      age = todayYear - myYear - 1;
                  else
                      age = todayYear - myYear - 1;
              }
              if (age > 15) {
                  document.getElementById(ctrl + ag).value = age;
                  //document.getElementById(ctrl + 'txtBnfFisrtNameMand').value = document.getElementById(ctrl + ag).value;
              }
              else {
                  alert("Age is less than 15.");
              }
         }
      }
  }

  function chkIssueDate() {
      alert();
      var d = document.getElementById(ctrl + dt).value;
      if (d != "") {
          if (isDate(d) == false) {
              //document.getElementById(ctrl + dt).focus();
              return false;
          }
          var now = new Date();
          if (d < now) {
              alert("Issue date ");
          }
      }
  }
  
  this.changeMonth = changeMonth;
  function changeMonth(change) {
    currentMonth += change;
    currentDay = 0;
    if(currentMonth > 12) {
      currentMonth = 1;
      currentYear++;
    } else if(currentMonth < 1) {
      currentMonth = 12;
      currentYear--;
    }
//alert(calendarId);
    calendar = document.getElementById(calendarId);
    calendar.innerHTML = calendarDrawTable();
  }

  this.changeYear = changeYear;
  
  function changeYear(change) 
  {
    currentYear += change;
    currentDay = 0;
    calendar = document.getElementById(calendarId);
    calendar.innerHTML = calendarDrawTable();
  }

  function getCurrentYear() {
    var year = new Date().getYear();
    if(year < 1900) year += 1900;
    return year;
  }

  function getCurrentMonth() {
    return new Date().getMonth() + 1;
  } 

  function getCurrentDay() {
    return new Date().getDate();
  }

  function calendarDrawTable() {
     
    var dayOfMonth = 1;
    var validDay = 0;
    var startDayOfWeek = getDayOfWeek(currentYear, currentMonth, dayOfMonth);
    var daysInMonth = getDaysInMonth(currentYear, currentMonth);
    var css_class = null; //CSS class for each day

    var table = "<table cellspacing='0' cellpadding='0' border='0'>";
    table = table + "<tr class='header'>";
    table = table + "  <td colspan='2' class='previous'><a href='javascript:changeCalendarControlMonth(-1);'>&lt;</a> <a href='javascript:changeCalendarControlYear(-1);'>&laquo;</a></td>";
    table = table + "  <td colspan='3' class='title'>" + months[currentMonth-1] + " " + currentYear + "</td>";
    table = table + "  <td colspan='2' class='next'><a href='javascript:changeCalendarControlYear(1);'>&raquo;</a> <a href='javascript:changeCalendarControlMonth(1);'>&gt;</a></td>";
    table = table + "</tr>";
    table = table + "<tr><th>S</th><th>M</th><th>T</th><th>W</th><th>T</th><th>F</th><th>S</th></tr>";

    for(var week=0; week < 6; week++) {
      table = table + "<tr>";
      for(var dayOfWeek=0; dayOfWeek < 7; dayOfWeek++) {
        if(week == 0 && startDayOfWeek == dayOfWeek) {
          validDay = 1;
        } else if (validDay == 1 && dayOfMonth > daysInMonth) {
          validDay = 0;
        }

        if(validDay) {
          if (dayOfMonth == selectedDay && currentYear == selectedYear && currentMonth == selectedMonth) {
            css_class = 'current';
          } else if (dayOfWeek == 0 || dayOfWeek == 6) {
            css_class = 'weekend';
          } else {
            css_class = 'weekday';
          }

          table = table + "<td><a class='"+css_class+"' href=\"javascript:setCalendarControlDate("+currentYear+","+currentMonth+","+dayOfMonth+")\">"+dayOfMonth+"</a></td>";
          dayOfMonth++;
        } else {
          table = table + "<td class='empty'>&nbsp;</td>";
        }
      }
      table = table + "</tr>";
    }

    table = table + "<tr class='header'><th colspan='7' align='center' style='padding: 3px;'><a href='javascript:hideCalendarControl();'>Close</a></td></tr>";
    table = table + "</table>";

    return table;
  }

  this.show = show;
  function show(field) {
  
    // If the calendar is visible and associated with
    // this field do not do anything.
    //alert(field);
    if (dateField == field) {
      return;
    } else {
      dateField = field;
    }

    if(dateField) {
      try {
        var dateString = new String(dateField.value);
        var dateParts = dateString.split("/");
        
        selectedMonth = parseInt(dateParts[0],10);
        selectedDay = parseInt(dateParts[1],10);
        selectedYear = parseInt(dateParts[2],10);
      } catch(e) {}
    }

    if (!(selectedYear && selectedMonth && selectedDay)) {
      selectedMonth = getCurrentMonth();
      selectedDay = getCurrentDay();
      selectedYear = getCurrentYear();
    }

    currentMonth = selectedMonth;
    currentDay = selectedDay;
    currentYear = selectedYear;

    if(document.getElementById){

      // var calendarId = 'CalendarControl';
      //alert('hi.');
      calendar = document.getElementById(calendarId);
      calendar.innerHTML = calendarDrawTable(currentYear, currentMonth);

      setElementProperty('display', 'block', 'CalendarControlIFrame');
      setProperty('display', 'block');

      var fieldPos = new positionInfo(dateField);
      var calendarPos = new positionInfo(calendarId);

      var x = fieldPos.getElementLeft();
      var y = fieldPos.getElementBottom();

      setProperty('left', x + "px");
      setProperty('top', y + "px");
      setElementProperty('left', x + "px", 'CalendarControlIFrame');
      setElementProperty('top', y + "px", 'CalendarControlIFrame');
      setElementProperty('width', calendarPos.getElementWidth() + "px", 'CalendarControlIFrame');
      setElementProperty('height', calendarPos.getElementHeight() + "px", 'CalendarControlIFrame');
    }
  }

  this.hide = hide;
  function hide() {
    if(dateField) {
      setProperty('display', 'none');
      setElementProperty('display', 'none', 'CalendarControlIFrame');
      dateField = null;
    }
  }
}

var calendarControl = new CalendarControl();

function showCalendarControl(textField) 
{
    //  document.getElementById("compDate").style.display="none";

  calendarControl.show(document.getElementById(textField));

  if (document.getElementById(textField).value != "") {
      document.getElementById(textField).focus();
  }
  return false;
}

function hideCalendarControl() 
{
  calendarControl.hide();
}


function setCalendarControlDate(year, month, day) 
{
  calendarControl.setDate(year, month, day);
}

function changeCalendarControlYear(change) 
{
  calendarControl.changeYear(change);
}

function changeCalendarControlMonth(change) 
{
  calendarControl.changeMonth(change);
}

document.write("<iframe id='CalendarControlIFrame' src='javascript:false;' frameBorder='0' scrolling='no'></iframe>");
document.write("<div id='CalendarControl'></div>");

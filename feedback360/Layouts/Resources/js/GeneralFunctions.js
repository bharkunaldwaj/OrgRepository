/*--==========================================================================================
 Project              : Damco Web Template
 File Name            : GeneralFunctions.js
 Program Description  : Generalized-functions js file.
 Programmed By        : Robins
 Programmed On        : 18-Feb-2009. 
 Modification History :
==========================================================================================--*/

var _varGlbMouseX = 0;
var _varGlbMouseY = 0;


//Function Added By SubrataM to Get the CurrentPage Reference
/*
domain --> (uri.dom) : "javascript.about.com"
path --> (uri.path) : "library"
page -->(uri.page) : "blurl1"
extension -->(uri.ext) : "htm"
filename -->(uri.file) : "blurl1.htm"
query string -->(uri.arg) : ""
*/
function getURL(uri) 
{
    uri.dir = location.href.substring(0, location.href.lastIndexOf('\/'));
    uri.dom = uri.dir; 
    if (uri.dom.substr(0,7) == 'http:\/\/') 
        uri.dom = uri.dom.substr(7);
    
    uri.path = ''; 
    
    var pos = uri.dom.indexOf('\/'); 
    
    if (pos > -1) 
    {
        uri.path = uri.dom.substr(pos+1); 
        uri.dom = uri.dom.substr(0,pos);
    }
    
    uri.page = location.href.substring(uri.dir.length+1, location.href.length+1);
    pos = uri.page.indexOf('?');
    
    if (pos > -1) 
    {
        uri.page = uri.page.substring(0, pos);
    }
    pos = uri.page.indexOf('#');
    
    if (pos > -1) 
    {
        uri.page = uri.page.substring(0, pos);
    }
    uri.ext = '';
    pos = uri.page.indexOf('.');
    
    if (pos > -1) 
    {
        uri.ext =uri.page.substring(pos+1); 
        uri.page = uri.page.substr(0,pos);
    }
    uri.file = uri.page;
    if (uri.ext != '') uri.file += '.' + uri.ext;
    
    if (uri.file == '') 
        uri.page = 'index';
    
    uri.args = location.search.substr(1).split("?");
    
    return uri;
}


//Added By: SubrataM for Enable/Disable the Search Controls
var HandleSerchControl=function(ddlId, otherIds)
{
    var ids = otherIds.split(',');
    
    if(document.getElementById(ddlId).options[document.getElementById(ddlId).options.selectedIndex].value == '0')
    {
        var uri = new Object();
        getURL(uri);
        if(uri.file == "ViewMasterData.aspx")
        {
            window.location.href = uri.file+'?Code='+(window.location).toString().substring((window.location).toString().length - 4) ;
        }
        else
        {
            window.location.href = uri.file;
        }
    }
    else
    {
        for(i=0; i< ids.length; i++)
        {
            document.getElementById('ctl00_ContentPlaceHolder1_'+ids[i]).disabled = false;
        }    
    }
}


/*Kumar Rakesh March17 2009*/
// Start :// Function for compatibility in mozilla:equivalent to innerText in IE// 
 	var isIEBrowser = (window.navigator.userAgent.indexOf("MSIE") > 0);
	if (! isIEBrowser) {
  		HTMLElement.prototype.__defineGetter__("innerText", 
              function () { return(this.textContent); });
  		HTMLElement.prototype.__defineSetter__("innerText", 
              function (txt) { this.textContent = txt; });
	}
 // End :// Function for compatibility in mozilla:equivalent to innerText in IE// 
 
 

// Start : Function for detecting the browser 
        var browser   = '';
        var idxOfMSIE = '';
        var isIEBrowser = '';
        var IEversion = '';
        // BROWSER?
        if (browser == ''){
        if (navigator.appName.indexOf('Microsoft') != -1)
        browser = 'IE'
        else if (navigator.appName.indexOf('Netscape') != -1)
        browser = 'Netscape'
        else browser = 'IE';
        }
                        
        isIEBrowser = window.navigator.userAgent.indexOf("MSIE");
        if(isIEBrowser){ 
                        if (IEversion == ''){
                        var idxOfMSIE = window.navigator.userAgent.indexOf("MSIE"); 
                        IEversion = navigator.appVersion.substring(idxOfMSIE,idxOfMSIE-3);
                        //alert('IEversion:'+IEversion);        
                        }
        }
// End : Function for detecting the browser 


/*Kumar Rakesh March17 2009 END*/





function GetXmlHttpObject()
{
    var xmlHttp=null;
    try
    {
        // Firefox, Opera 8.0+, Safari
        xmlHttp=new XMLHttpRequest();
    }
    catch (e)
    {
        // Internet Explorer
        try
        {
            xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e)
        {
            xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
        }
    }
    return xmlHttp;
}	


/*
*   Created By  :   SubrataM
*   Created On  :   26/02/2009
*   Purpose     :   Used while opening a Modal/Popup Window
*/

function openMyWindow( pageToLoad, winName, width, height, center)
{
     xposition=0; yposition=0;
     if ((parseInt(navigator.appVersion) >= 4 ) && (center)){
         xposition = (screen.width - width) / 2;
         yposition = (screen.height - height) / 2;
     }
 	
    var args = "";
    	args += "width=" + width + "," + "height=" + height + ","
		+ "location=0,"
		+ "menubar=0,"
		+ "resizable=0,"
		+ "scrollbars=0,"
		+ "statusbar=false,dependent,alwaysraised,"
		+ "status=false,"
		+ "titlebar=no,"
		+ "toolbar=0,"
		+ "hotkeys=0,"
		+ "screenx=" + xposition + ","  //NN Only
		+ "screeny=" + yposition + ","  //NN Only
		+ "left=" + xposition + ","     //IE Only
		+ "top=" + yposition;           //IE Only
		//fullscreen=yes, add for full screen
    	var dmcaWin = window.open(pageToLoad,winName,args );
    	dmcaWin.focus();
    //window.showModalDialog(pageToLoad,"","dialogWidth:650px;dialogHeight:500px");
}




//Kumar Rakesh :: 27/02/2009 
//Description:: These function gives you the tpo-left corner of the element whose id is passed
var ProductX = 0;
var ProductY = 0;
//Function:Find Absolute Positioning of Div
function findPosX(obj)
  {
	var curleft = 0;
	if(obj.offsetParent)
		while(1) 
		{
		  curleft += obj.offsetLeft;
		  if(!obj.offsetParent)
			break;
		  obj = obj.offsetParent;
		}
	else if(obj.x)
		curleft += obj.x;
	return curleft; 
  }

function findPosY(obj)
  {
	 var curtop = 0;
	if(obj.offsetParent)
		while(1)
		{
		  curtop += obj.offsetTop;
		  if(!obj.offsetParent)
			break;
		  obj = obj.offsetParent;
		}
	else if(obj.y)
		curtop += obj.y;
		return curtop; 
  }
//End Function

//Start Trim Function

function trimAll(sString) 
{
    while (sString.substring(0,1) == ' ')
    {
    sString = sString.substring(1, sString.length);
    }
    while (sString.substring(sString.length-1, sString.length) == ' ')
    {
    sString = sString.substring(0,sString.length-1);
    }
    return sString;
}

//End Function

//Start IsNumeric Function

function IsNumeric(sText)
{
   
    var ValidChars = "0123456789";
    var IsNumber=true;
    var Char;


    for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
    return IsNumber;
}

//End IsNumeric Function

//Start Valid Mail function

function isValidEmail(str) {
    return (str.indexOf(".") > 2) && (str.indexOf("@") > 0);
}

//End Valid Mail Function



function FindMousePos(e)
{ 
    
    //get the mouse coordinates 
    // Detect if the browser is IE or not.
    // If it is not IE, we assume that the browser is NS.
    if(!e)e = window.event;
    if (e.clientX) { // grab the x-y pos.s if browser is IE
        tempX = e.clientX + (document.documentElement.scrollLeft ?
        document.documentElement.scrollLeft :
        document.body.scrollLeft);
        
        tempY = e.clientY + (document.documentElement.scrollTop ?
        document.documentElement.scrollTop :
        document.body.scrollTop);
    } else  if (e.pageX) {  // grab the x-y pos.s if browser is NS
        tempX = e.pageX;
        tempY = e.pageY;
    }  
            
    // catch possible negative values in NS4
    if (tempX < 0){tempX = 0}
    if (tempY < 0){tempY = 0}  
    
    _varGlbMouseX = tempX;
    _varGlbMouseY = tempY;
            
} 

/* Added By :: SubrataM for Restricting the user from entering More then the specified length
in the TextArea*/
var TextAreaMaxLengthCheck=function(id,length)
{  
    //alert('Hi');
    length = length-1;
    var val = document.getElementById(id).value;   
    if(val.length <= length)
        return true;
    else
    {
        document.getElementById(id).value = val.substring(0, length);
        //alert('Max Length Reached');
    }
} 


/* Added By : SubrataM for Restricting the user from entering illigal characters
*********************************************************************************/



/*
For All Email Fields
*********************
Allowed = Alphanumeric, @,  ., -,  _
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >, "
*/
var CheckEmail = function(source, arguments)
{
    var IsValidFlag = true;
    var ValidateString = "!#$%^&*()=+[]{}:;’”?/<>";
    var Email = arguments.Value;
    var counter = 0
    
    for(; counter< Email.length; counter++)
    {
        if(ValidateString.indexOf(Email.charAt(counter)) != -1)
            break;
    }
    
    if(counter < Email.length)
    {
        alert('Illegal characters found in the field.');
        IsValidFlag = false;
    }
    arguments.IsValid = IsValidFlag;
}



/*
For All Name Fields
*********************
Allowed = Alphanumeric
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >, @,  ., -,  _"
*/
var CheckName = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?/<>@,.-_";
    var Name = arguments.Value;
    var counter = 0
    
    for(; counter< Name.length; counter++)
    {
        if(ValidateString.indexOf(Name.charAt(counter)) != -1)
            break;
    }
    
    if(counter < Name.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}



/*
For All Phone Number Fields
****************************
Allowed = Integer , Space 
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >, @,  ., -,  _"
*/
var CheckPhoneNumber = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?/<>@,.-_ ";
    var PhoneNumber = arguments.Value;
    var counter = 0
    
    for(; counter< PhoneNumber.length; counter++)
    {
        if(ValidateString.indexOf(PhoneNumber.charAt(counter)) != -1)
            break;
    }
    
    if(counter < PhoneNumber.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}



/*
For All Date Fields
*********************
Allowed = Integer, /
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >, @,  .,  _"
*/
var CheckDate = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?<>@,.-_ ";
    var Date = arguments.Value;
    var counter = 0
    
    for(; counter< Date.length; counter++)
    {
        if(ValidateString.indexOf(Date.charAt(counter)) != -1)
            break;
    }
    
    if(counter < Date.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}



/*
For All Company Code Fields
****************************
Allowed = Alphanumeric
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >, @,  ., -,  _"
*/
var CheckCompanyCode = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?<>@,.-_/";
    var CompanyCode = arguments.Value;
    var counter = 0
    
    for(; counter< CompanyCode.length; counter++)
    {
        if(ValidateString.indexOf(CompanyCode.charAt(counter)) != -1)
            break;
    }
    
    if(counter < CompanyCode.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}



/*
For All Sort Order Fields
*************************
Allowed = Integer
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >, @,  ., -,  _"
*/
var CheckSortOrder = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?<>@,.-_/";
    var SortOrder = arguments.Value;
    var counter = 0
    
    for(; counter< SortOrder.length; counter++)
    {
        if(ValidateString.indexOf(SortOrder.charAt(counter)) != -1)
            break;
    }
    
    if(counter < SortOrder.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}



/*
For All UserName Fields
************************
Allowed = Alphanumeric,@,_
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <, >,  ., -,  _"
*/
var CheckUserName = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?,/<>.-";
    var UserName = arguments.Value;
    var counter = 0
    
    for(; counter< UserName.length; counter++)
    {
        if(ValidateString.indexOf(UserName.charAt(counter)) != -1)
            break;
    }
    
    if(counter < UserName.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}



/*
For All Country Fields
**********************
Allowed = Alphanumeric
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <>, @,  ., -,  _, "
*/
var CheckCountry = function(source, arguments)
{
    var ValidateString = "!#$%^&*()=+[]{}:;’”?,/<>.-";
    var Country = arguments.Value;
    var counter = 0
    
    for(; counter< Country.length; counter++)
    {
        if(ValidateString.indexOf(Country.charAt(counter)) != -1)
            break;
    }
    
    if(counter < Country.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}

/*
For All Country Fields
**********************
Allowed = Alphanumeric
Restricted = "!, #, $,%,^,&,*, ( , ), = ,+, [, ], {, }, :, ;, ’, ”, ?, /, <>, @,  ., -,  _, "
*/
var CheckAddress = function(source, arguments)
{
    var ValidateString = "!#$%@^&*=+[]{};’”?<>~";
    var Address = arguments.Value;
    var counter = 0
    
    for(; counter< Address.length; counter++)
    {
        if(ValidateString.indexOf(Address.charAt(counter)) != -1)
            break;
    }
    
    if(counter < Address.length)
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}
/*************************************************************************************************************************/
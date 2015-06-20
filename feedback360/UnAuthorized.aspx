<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnAuthorized.aspx.cs" Inherits="UnAuthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UnAuthorized Access</title>
    <link href="Layouts/Resources/css/style.css" rel="stylesheet" type="text/css" />
    <link href="Layouts/Resources/css/ddmenu.css" rel="stylesheet" type="text/css" />
    <link href="Layouts/Resources/css/reset.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="custom-doc" class="yui-t7">
        <div id="hd">
            <!--Start Header Section-->
            <div id="header">
                <div class="logo">
                    <img src="Layouts/Resources/images/logo.png" alt="Feedback 360" /></div>
                <div class="welcome">
                    <div class="logout">
                        <a href="#chat" title="Chat">Chat</a> | <a href="Help.html" title="help">Help</a>
                        |
                        <asp:LinkButton ID="lnlLogout" runat="server" Text="Logout" OnClick="lnlBtnLogout_Click"
                            CausesValidation="false"></asp:LinkButton>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
                <div class="nav">
                    <strong>Welcome</strong> User,&nbsp;&nbsp;&nbsp; <strong>Last Login:</strong> Monday,
                    July 12, 2010 10:25:29 AM
                </div>
            </div>
            <div class="bg-main" id="divDashBoardData" style="height:400px">
                <div class="yui-g">
                    <!--start container-->
                    <div class="dashboard" >
                        <p>
                    You are not authorized to view this page. Please click&nbsp;
                    <asp:LinkButton runat="server" ID="lnkBtnRedirect" Text="here" PostBackUrl="~/Login.aspx"></asp:LinkButton>
                    &nbsp;to go to your home page.
                </p>        
                        <!--End container-->
                    </div>
                </div>
            </div>            
            <!--End Header Section-->
        </div>
        <div id="footer">
            
                Copyright © 2010 Organisational Reflections. All rights reserved.
            
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <link rel="stylesheet" type="text/css" href="Layouts/Resources/css/style.css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="custom-doc" >
        <div id="hd">
            <!--Start Header Section-->
            <div id="header">
                <div class="logo"><a href="#" ><img src='<%= ResolveClientUrl("Layouts/Resources/images/logo.png")%>' alt="logo" border="0" /></a></div>
                <div class="headerright"></div>
                <div class="welcome">
                    <div class="logout">
                        <a href="Help.html" title="help">Help</a>
                        |
                        <asp:LinkButton ID="lnlLogout" runat="server" Text="Logout" OnClick="lnlBtnLogout_Click"
                            CausesValidation="false"></asp:LinkButton>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
                
            </div>
            <div  class="bg-main" id="divDashBoardData">
                <div class="yui-g">
                    <!--start container-->
                    <div class="dashboard" style="height:400px">
                        <p>
                            We are sorry but server did not responded properly. Please click&nbsp;
                            <asp:LinkButton runat="server" ID="lnkBtnRedirect" Text="here" PostBackUrl="~/Default.aspx"></asp:LinkButton>
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

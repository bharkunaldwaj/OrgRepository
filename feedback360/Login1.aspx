<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login1.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Shamrock: Login</title>
    <link href="Layouts/Resources/CSS/login.css" rel="Stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
    <!--
        //show hide layer
        function view() {
            if (document.getElementById("login").value == "v") {
                location.href = "dashboard-vendor.html";
            }
            else {
                location.href = "dashboard.html";
            }

        }

        function openpopup() {
            document.getElementById("popup").style.display = "block"
        }
        function closediv() {
            document.getElementById("popup").style.display = "none"
        }

        function setFocusUserName() {
            document.getElementById('txtUserName').focus();
        }
    //-->
    </script>

</head>
<body onload="javascript:setFocusUserName();">
    <form id="form" runat="server">
    <div id="wrapper">
        <div class="header">
            <img src="Layouts/Resources/images/logo-new.png" />
        </div>
        <div class="container">
            <h1>
                User Login</h1>
               
            <div class="loginbox">            
                <ul>
                    <li><asp:ValidationSummary ID="valSummaryLogin" runat="server" DisplayMode="BulletList"
                            ShowMessageBox="true" ShowSummary="false" ValidationGroup="valLogin" />
                     <asp:Label ID="lblMessage" runat="server" Visible="false" Text="Label" CssClass="error">
                        </asp:Label><br />                       
                       
                        <label>
                            User Name:</label><input id="txtUserName" name="" type="text" class="textbox" maxlength="20" runat="server" />
                            <asp:RequiredFieldValidator ID="reqUserName" runat="server" Display="None" SetFocusOnError="true" ValidationGroup="valLogin" ControlToValidate="txtUserName" ErrorMessage="Please enter User Name"></asp:RequiredFieldValidator>
                            </li>
                    <li>
                        <label>
                            Password:</label><input id="txtPassword" name="" type="password" maxlength="50" class="textbox"
                                runat="server" /></li>
                    <li>
                        <label>
                            &nbsp;</label><asp:Button ID="btnLogin" runat="server" align="absmiddle"  Text="Login" CssClass="login" OnClick="btnLogin_Click" ValidationGroup="valLogin"/></li>
                    <li>
                        <label>
                            &nbsp;</label>Forgot Password <asp:LinkButton ID="lnkForgotPassword" 
                            runat="server" ValidationGroup="valLogin" Text="CLICK HERE" 
                            onclick="lnkForgotPassword_Click"></asp:LinkButton></li>
                    <li>
                        <label>
                            &nbsp;</label><input name="" type="button" value="Register" class="register" onclick="javascript:openpopup();"  style="display:none" /></li>
                </ul>
            </div>
        </div>
        <div class="footer">
            SHAMROCK INTERNATIONAL FASTENERS 1475 Industrial Dr. Itasca, IL 60143 Ph: 877.636.2658
            Fax: 630.595.6395
        </div>
    </div>
    </form>
</body>
</html>

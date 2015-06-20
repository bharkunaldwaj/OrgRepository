<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Layouts/Resources/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="Layouts/Resources/css/style.css" rel="stylesheet" type="text/css" />
    <title>Feedback 360 : Login</title>
</head>
<body>
    <form id="frmLogin" runat="server">
    <div id="maincontainer">
        <!-- start header -->
        <div id="login-header">
            <div class="headerright">
                <img src="Layouts/Resources/images/logo.png"  /></div>
            <div class="headerright">
            </div>
        </div>
        <div class="clear">
        </div>
        <!-- end header -->
        <!-- start menu -->
        <div id="menurow">
        </div>
        <!-- end menu -->
        <!-- start bodytext container -->
        <div id="bodytextcontainer">
            <div class="logincontainer">
                <div class="loginbox">
                    <div class="logintop">
                        <h2>
                            Login</h2>
                        <p>
                            Enter your account credentials in the login panel.</p>
                        <div>
                            <img src="Layouts/Resources/images/login-heading-line.png" alt="login horizontal heading line" /></div>
                    </div>
                    <div class="loginbottom">
                        <ul>
                            <li>
                                <label>
                                    Account Code</label><div class="accode">
                                        <asp:TextBox ID="txtAccountCode" runat="server" MaxLength="5" TabIndex="1" ></asp:TextBox></div>
                            </li>
                            <li>
                                <label>
                                    Username</label><div class="inputbox">
                                        <asp:TextBox ID="txtUserName" Text="" MaxLength="30" runat="server"></asp:TextBox></div>
                            </li>
                            <li>
                                <label>
                                    Password</label><div class="inputbox">
                                        <asp:TextBox ID="txtPassword" Text=""  runat="server" MaxLength="30" TextMode="Password"></asp:TextBox></div>
                            </li>
                            <li>
                                <label>
                                    &nbsp;</label>
                                <asp:ImageButton ID="ibtnLogin" runat="server" ImageUrl="~/Layouts/Resources/images/login.png"
                                    OnClick="ibtnLogin_Click" ToolTip="Login" />
                                &nbsp;<%--<a href="#">Forgot your password?</a>--%></li>
                            <li>
                                <center>
                                    <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label></center>
                            </li>
                        </ul>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="loginshadow">
                    </div>
                </div>
            </div>
        </div>
        <!-- start bodytext container -->
         <script type="text/javascript">
             document.getElementById('txtAccountCode').focus();
    </script>
        <!-- start footer -->
        <div class="clear">
        </div>
        <div id="footer">
            Copyright © i-comment360. All rights reserved.
            <%--Copyright © 2011 Organisational Reflections. All rights reserved.--%>
        </div>
        <!-- end footer -->
    </div>
   
    </form>
</body>
</html>

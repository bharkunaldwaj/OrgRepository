<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs"
    Inherits="Survey_Module_Register"  ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Layouts/Resources/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="../Layouts/Resources/css/style.css" rel="stylesheet" type="text/css" />
    <title>Register</title>
    <link rel="stylesheet" type="text/css" href="../Layouts/Resources/css/reset.css" />
    <link rel="stylesheet" type="text/css" href="../Layouts/Resources/css/style.css" />
    <link rel="stylesheet" type="text/css" href="../Layouts/Resources/css/ddmenu.css" />
    <link rel="stylesheet" type="text/css" href="../Layouts/Resources/css/Calendar_360.css" />
    <link rel="stylesheet" type="text/css" href="../Layouts/Resources/css/Calendar.css" />
    <link rel="stylesheet" type="text/css" href="../Layouts/Resources/css/jquery-ui-1.7.2.custom.css" />
    <script src='<%= ResolveClientUrl("../Layouts/Resources/js/common.js")%>' type="text/javascript"></script>
    <script type="text/javascript" src='<%= ResolveClientUrl("../Layouts/Resources/js/GeneralFunctions.js") %>'></script>

      <style type="text/css">
        .graphtext
        {
            color: white; /*#025273;*/
            text-align: center;
            font: bold 10px verdana;
            height: 20px;
        }
        .radiobuttonlist input
        {
            width: 35px;
            
        }
        .radiobuttonlist label
        {
           
            padding-left: 0px;
            padding-right: 0px;
            padding-top: 0px;
            padding-bottom: 0px;
            white-space: nowrap;
            font-weight:normal;
            clear: left;
        }
        .radiobuttonlist td
        {
           width:200px;
           vertical-align:top;
        }

    </style>
</head>
<body>
    <form id="frmFeedback" runat="server">
   <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <table id="maincontainer-feedback_table" width="74%" border="0" cellpadding="0"
        cellspacing="0" style="margin-left:13%;">
        <tr>
            <td width="10%" align="left">
                <asp:Image ID="imgHeader" Height="70" runat="server" />
            </td>
            <td id="tdHeader" runat="server" width="90%" align="right">
                <asp:Image ID="imgProjectLogo" Height="70" runat="server" ImageUrl="" />
            </td>
        </tr>
        <tr>
            <td id="tdMenuBar" colspan="2" runat="server" style="background:#bdbdbd;text-align:right;padding-right:15px; padding-top:3px; color:#ffffff;">
                <b>Welcome
                    <asp:Label ID="lblUserName" runat="server" Text="Guest"></asp:Label>
                </b>
            </td>
        </tr>
    </table>
    <div id="bodytextcontainer" style="  width: 1002px; margin: 1px auto; padding: 10px 0;">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails12">
                <h3>
                    <img id="Img1" src="../Layouts/Resources/images/user.png" runat="server"
                        title="<% $Resources:lblTopHeading %>" align="absmiddle" />
                    <asp:Label ID="lblTopHeading" runat="server" Text="<% $Resources:lblTopHeading %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start user form -->
            <table border="0" width="100%">
                <tr>
                    <td>
                        <div id="summary" runat="server" class="validation-align">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                ShowSummary="true" ValidationGroup="group1" />
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                ShowSummary="true" ValidationGroup="group2" />
                            <asp:Label ID="lblvalidation" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
             <div id="div1" runat="server" visible="True">
               <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="Instructions"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="100%">
                                    <asp:Label ID="lblInst" runat="server" Text=""></asp:Label>
                                </td>
                               
                            </tr>
                            
                        </table>
                    </fieldset>
             </div>
                <div id="divAccount" runat="server" visible="True">
                    <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="lblAccountDetails" runat="server" Text="Account Details"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                           <tr>
                                <td width="15%">
                                    <asp:Label ID="Label2" runat="server" Text="Account Name :"></asp:Label>
                                </td>
                                <td width="35%">
                                    <asp:Label ID="lblAccount" runat="server" Text=""></asp:Label>
                                </td>
                                <td width="15%">
                                    <asp:Label ID="Label4" runat="server" Text="Project :"></asp:Label>
                                </td>
                                <td width="35%">
                                    <asp:Label ID="lblProject" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                           
                           
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblCompany" runat="server" Text="Company Name :"></asp:Label>
                                </td>
                                <td width="35%">
                                    <asp:Label ID="lblCompanyName" runat="server" Text=""></asp:Label>
                                </td>
                                <td width="15%">
                                    <asp:Label ID="lblQuestion" runat="server" Text="Questionnaire :"></asp:Label>
                                </td>
                                <td width="35%">
                                    <asp:Label ID="lblQuestionName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="1">
                    <tr>
                        <td>
                            <fieldset class="fieldsetform assign-question">
                                <legend>
                                    <asp:Label ID="lblGeneralDetails" runat="server" Text="Participant Details"></asp:Label></legend>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                   
                                    
                                    <tr>
                                        
                                        <td width="15%" valign="top">
                                            <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
                                        </td>
                                        <td width="35%" valign="top">
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1"
                                                ErrorMessage="Please Enter Name" SetFocusOnError="True" ControlToValidate="txtName">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="15%" valign="top">
                                            <asp:Label ID="lblEmailAddress" runat="server" Text="Email Address:"></asp:Label>
                                        </td>
                                        <td width="35%" valign="top">
                                            <asp:TextBox ID="txtEmailAddress" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="Please Enter Email Address"
                                                SetFocusOnError="True" ControlToValidate="txtEmailAddress">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                     <tr >
                                        <td width="15%" valign="top">
                                            <asp:Label ID="lblAnalysis1" runat="server" Text="Please Select:"></asp:Label>
                                        </td>
                                        <td width="35%" valign="top">
                                            <asp:DropDownList ID="ddlAnalysis1" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Please Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="15%" valign="top" >
                                            <asp:Label ID="lblAnalysis2" runat="server" Text="Please Select:"></asp:Label>
                                        </td>
                                        <td width="35%" valign="top">
                                            <asp:DropDownList ID="ddlAnalysis2" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Please Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat=server id="trAna3">
                                        <td width="15%" valign="top">
                                            <asp:Label ID="lblAnalysis3" runat="server" Text="Please Select:"></asp:Label>
                                        </td>
                                        <td width="35%" valign="top">
                                            <asp:DropDownList ID="ddlAnalysis3" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Please Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                       <td>
                                       </td>
                                       <td></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
               <%-- <span class="style3">
                    <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>--%>
                <br />
                <div align="center">
                    <asp:ImageButton ID="imbAssign" ImageUrl="~/Layouts/Resources/images/submit.png" ToolTip="Assign"
                        runat="server" OnClick="imbAssign_Click" ValidationGroup="group1" />&nbsp;
                    <asp:ImageButton ID="imbReset" ImageUrl="~/Layouts/Resources/images/reset.png" ToolTip="Reset"
                        runat="server" OnClick="imbReset_Click" Visible="false"/>&nbsp;
                </div>
                <br />
                <div align="center">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
                <br />
            </div>
            <!-- start user form -->
        </div>
    </div>
    </form>
</body>
</html>

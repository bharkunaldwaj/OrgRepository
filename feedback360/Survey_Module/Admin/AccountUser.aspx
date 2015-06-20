<%@ Page Title="Create New User" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="AccountUser.aspx.cs" Inherits="Survey_Module_Admin_AccountUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4
        {
            height: 37px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/user_create.png"  runat="server" alt="<% $Resources:lblToolTip %>"  align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label></h3>
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
                        </div>
                    </td>
                </tr>
            </table>
            <table border="0" width="100%">
                <tr>
                    <td>
                        <div id="Div1" runat="server" class="validation-align">
                            <span class="style3">
                                <asp:Label ID="lblusermsg" runat="server" Text=""></asp:Label></span>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnPassword" runat="server" />
            <div class="userform">
                <div id="divAccount" runat="server" visible="false">
                    <fieldset class="fieldsetform">
                        <legend><asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label> </legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="13%">
                                    <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                        class="style3">*</span>
                                </td>
                                <td width="36%">
                                    <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="13%">
                                    <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                </td>
                                <td width="38%">
                                    <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <fieldset class="fieldsetform">
                    <legend>
                        <asp:Label ID="lblAccountDetails" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblAccountCode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label>
                            </td>
                            <td width="36%">
                                <asp:Label ID="lblAccountCodeValue" runat="server" Text=""></asp:Label>
                            </td>
                            <td width="13%">
                                <asp:Label ID="lblAccountName" runat="server" Text="<% $Resources:lblAccountName %>"></asp:Label>
                            </td>
                            <td width="38%">
                                <asp:Label ID="lblAccountNameValue" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUserProfile" runat="server" Text="<% $Resources:lblUserProfile %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="True" 
                                    onselectedindexchanged="ddlType_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="rq2" runat="server" ControlToValidate="ddlType"
                                    ErrorMessage="<% $Resources:rq2 %> " SetFocusOnError="True" InitialValue="0"
                                    ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblUserStatus" runat="server" Text="<% $Resources:lblUserStatus %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="2">Suspended</asp:ListItem>
                                    <asp:ListItem Value="3">Inactive</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq3" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="<% $Resources:Rq3 %>" SetFocusOnError="True" InitialValue="0"
                                    ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNotificationEmail" runat="server" Text="<% $Resources:lblNotificationEmail %>"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkNotification" runat="server" />
                                <asp:Label ID="lblNotificationDesc" Text="<% $Resources:lblNotificationDesc %>" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="fieldsetform">
                    <legend>
                        <asp:Label ID="lblPersonalDetails" runat="server" Text="<% $Resources:lblPersonalDetails %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblSalutation" runat="server" Text="<% $Resources:lblSalutation %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td width="36%">
                                <asp:DropDownList ID="ddlSalutation" runat="server">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Mr.</asp:ListItem>
                                    <asp:ListItem Value="2">Mrs.</asp:ListItem>
                                    <asp:ListItem Value="3">Ms.</asp:ListItem>
                                    <asp:ListItem Value="4">Miss</asp:ListItem>
                                    <asp:ListItem Value="5">Dr.</asp:ListItem>
                                    <asp:ListItem Value="6">Prof.</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq9" runat="server" ControlToValidate="ddlSalutation"
                                    ErrorMessage="<% $Resources:Rq9 %>" SetFocusOnError="True" InitialValue="0"
                                    ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td width="13%">
                                &nbsp;
                            </td>
                            <td width="38%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFirstName" runat="server" Text="<% $Resources:lblFirstName %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" MaxLength="25" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq10" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="<% $Resources:Rq10 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblLastName" runat="server" Text="<% $Resources:lblLastName %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" MaxLength="25" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq11" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="<% $Resources:Rq11 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblLoginID" runat="server" Text="<% $Resources:lblLoginID %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserId" MaxLength="25" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                &nbsp;<asp:RequiredFieldValidator ID="Rq13" runat="server" ControlToValidate="txtUserId"
                                    ErrorMessage="<% $Resources:Rq13 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="Rq88" runat="server" ValidationGroup="group1"
                                    SetFocusOnError="True" Text="*" ForeColor="White" ControlToValidate="txtUserId"
                                    ErrorMessage="<% $Resources:Rq88 %>" ValidationExpression=".{5}.*" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPassword" runat="server" Text="<% $Resources:lblPassword %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" TextMode="Password" MaxLength="25" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq4" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="<% $Resources:Rq4 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="Rq89" runat="server" ValidationGroup="group1"
                                    ControlToValidate="txtPassword" SetFocusOnError="True" Text="*" ForeColor="White"
                                    ErrorMessage="<% $Resources:Rq89 %>" ValidationExpression=".{6}.*" />
                            </td>
                            <td>
                                <asp:Label ID="lblReEnterPassword" runat="server" Text="<% $Resources:lblReEnterPassword %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfirmPassword" TextMode="Password" MaxLength="25" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq5" runat="server" ControlToValidate="txtConfirmPassword"
                                    ErrorMessage="<% $Resources:Rq5 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtConfirmPassword"
                                    ControlToCompare="txtPassword" ErrorMessage="<% $Resources:CompareValidator1 %>" ValidationGroup="group1"
                                    Text="*" ForeColor="White" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmail" runat="server" Text="<% $Resources:lblEmail %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" MaxLength="80" runat="server" SkinID="email"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq8" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="<% $Resources:Rq8 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                        ID="regEmail" ControlToValidate="txtEmail" ErrorMessage="<% $Resources:regEmail %>"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server"
                                        ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label>
                
                <br />
                <div align="center">
                    <asp:ImageButton ID="imbSave" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server"
                        OnClick="imbSave_Click" ValidationGroup="group1" ToolTip="Save" />&nbsp;
                    <asp:ImageButton ID="imbCancel" ImageUrl="~/Layouts/Resources/images/Cancel.png"
                        runat="server" OnClick="imbCancel_Click" ToolTip="Back to list" />
                    <asp:ImageButton ID="imbBack" Visible="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                        CausesValidation="false" runat="server" PostBackUrl="~/Survey_Module/Admin/AccountUserList.aspx" ToolTip="Back to list" />
                </div>
                <br />
            </div>
            <!-- start user form -->
        </div>
    </div>

    <script type="text/javascript">

        document.getElementById('ctl00_cphMaster_txtPassword').value = document.getElementById('ctl00_cphMaster_hdnPassword').value;
        document.getElementById('ctl00_cphMaster_txtConfirmPassword').value = document.getElementById('ctl00_cphMaster_hdnPassword').value;
        
    </script>

</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegenerateProgrammeDocument.aspx.cs" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
 Inherits="Module_Reports_RegenerateProgrammeDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <div class="topheadingdetails">
                <h3>
                    <img id="Img1" src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server"
                        title="ReGenerate Programme Document" align="absmiddle" alt="" />
                    <asp:Label ID="LabelTopHeading" runat="server" Text="Regenerate Programme Document"></asp:Label>
                    <asp:ImageButton ID="ImageButtonHelp" runat="server" title="Help" CssClass="HeadingInfoHelp"
                        OnClientClick="ShowPopup();" ImageUrl="~/Layouts/Resources/images/help.png" />
                </h3>
                <div class="clear">
                </div>
            </div>
            <div class="userform">
                <div id="divAccount" runat="server">
                    <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="LabelAccountDetails" runat="server" Text="Account Details"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="LabelAccountcode" runat="server" Text="Account code"></asp:Label><span
                                        class="style3">*</span>
                                </td>
                                <td width="35%">
                                    <asp:DropDownList ID="DropDownListAccountCode" runat="server" Style="width: 155px"
                                         OnSelectedIndexChanged="DropDownListAccountCode_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldAccountcode" runat="server" ForeColor="Red"
                                        ErrorMessage="*" SetFocusOnError="True" ControlToValidate="DropDownListAccountCode" Display="Dynamic"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td width="15%">
                                    <asp:Label ID="LabelCompany" runat="server" Text="Company Name"></asp:Label>
                                </td>
                                <td width="35%">
                                    <asp:Label ID="Labelcompanyname" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <fieldset class="fieldsetform assign-question">
                                <legend>
                                    <asp:Label ID="LabelGeneralDetails" runat="server" Text="General Details"></asp:Label></legend>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td width="15%" valign="top">
                                            <asp:Label ID="LabelProject" runat="server" Text="Project"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="35%" valign="top">
                                            <asp:Label ID="LabelProjectText" runat="server" Text=""></asp:Label>
                                            <asp:DropDownList ID="DropDownListProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                AutoPostBack="true" OnSelectedIndexChanged="DropDownListProject_SelectedIndexChanged" >
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldProject" runat="server" Text="*" ForeColor="Red"
                                                SetFocusOnError="True" ControlToValidate="DropDownListProject" Display="Dynamic"
                                                InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        <td width="15%" valign="top">
                                        </td>
                                        <td width="35%" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LabelProgramme" runat="server" Text="Programme"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="LabelProgrammeText" runat="server" Text=""></asp:Label>
                                            <asp:DropDownList ID="DropDownListProgramme" runat="server" Style="width: 155px"
                                                AppendDataBoundItems="True" >
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorProgramme" runat="server" Text="*" Display="Dynamic"
                                                SetFocusOnError="True" ControlToValidate="DropDownListProgramme"  ForeColor="Red"
                                                InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                         <asp:ImageButton ID="ButtonGenerateReport" ImageUrl="~/Layouts/Resources/images/submit.png"
                                                   OnClick="ButtonGenerateReport_Click" runat="server" ToolTip="ReGenerate Report" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="4">
                                    <asp:Label ID="LabelMessge" runat="server" style="color:Red;"></asp:Label>
                                    </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>


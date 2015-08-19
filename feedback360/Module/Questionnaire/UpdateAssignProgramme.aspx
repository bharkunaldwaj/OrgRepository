<%@ Page Title="Move Participant Programme" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="UpdateAssignProgramme.aspx.cs" Inherits="Module_Questionnaire_UpdateAssignProgramme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <%-- <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server"
                                title="<% $Resources:lblTopHeading %>" align="absmiddle" />
                            <asp:Label ID="lblTopHeading" runat="server" Text="<% $Resources:lblTopHeading %>"></asp:Label>
                        </h3>
                        <div class="clear">
                        </div>
                    </div>
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
                        <tr>
                            <td>
                                <div align="center">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="Large" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="userform">
                        <div id="divAccount" runat="server" visible="false">
                            <fieldset class="fieldsetform">
                                <legend>
                                    <asp:Label ID="lblAccountDetails" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="35%">
                                            <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>  "
                                                SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
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
                                            <asp:Label ID="lblGeneralDetails" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                            <tr>
                                                <td width="15%" valign="top">
                                                    <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="35%" valign="top">
                                                    <asp:Label ID="lblProjectText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:Rq1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td width="15%" valign="top">
                                                <asp:Label ID="lblProgramme" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="35%" valign="top">
                                                <asp:Label ID="lblProgrammeText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:RequiredFieldValidator1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                           
                                            <tr id="trTargetPerson" runat="server">
                                                <td valign="top">
                                                    <asp:Label ID="lblTargetPerson" runat="server" Text="<% $Resources:lblTargetPerson %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td valign="top">
                                                    
                                                    <div style="height:240px;overflow-y:auto;overflow-x:hidden;border:solid 1px #dddddd;" >
                                                    <asp:CheckBoxList ID="ddlTargetPerson" runat="server" 
                                                        CellSpacing="2" CellPadding="2"  >
                                                    </asp:CheckBoxList>
                                                    </div>
                                                    
                                                    <%--<asp:DropDownList ID="ddlTargetPerson" runat="server" lSetFocusOnError="True" Display="None"
                                                        Style="width: 155px" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTargetPerson_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                    <%--<asp:RequiredFieldValidator ID="Rq3" runat="server" ErrorMessage="<% $Resources:Rq3 %>"
                                                        ControlToValidate="ddlTargetPerson" SetFocusOnError="True" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td valign="top">
                                                <asp:Label ID="lblNewProgramme" runat="server" Text="<% $Resources:lblNewProgramme %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td valign="top">
                                                <asp:DropDownList ID="ddlNewProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True" >
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<% $Resources:RequiredFieldValidator2 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlNewProgramme" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <span class="style3">
                            <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <br />
                        <div align="center">
                            <asp:ImageButton ID="imbAssign" ImageUrl="~/Layouts/Resources/images/Save.png" ToolTip="Assign"
                                runat="server" OnClick="imbAssign_Click" ValidationGroup="group1" />&nbsp;
                            <asp:ImageButton ID="imbReset" ImageUrl="~/Layouts/Resources/images/reset.png" ToolTip="Reset"
                                runat="server" OnClick="imbReset_Click" />&nbsp;
                        </div>
                    </div>
                    <!-- start user form -->
                </div>
            </div>
       <%-- </ContentTemplate>
        <Triggers>
            
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

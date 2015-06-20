<%@ Page Title="Assign Questionnaire to Participants" Language="C#" AutoEventWireup="true"
    CodeFile="AssignQstnPaticipant.aspx.cs" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    Inherits="Survey_Module_Questionnaire_AssignQstnPaticipant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" alt="<% $Resources:lblTopHeading %>"
                                align="absmiddle" />
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
                    <div class="userform">
                        <div id="divAccount" runat="server" visible="false">
                            <fieldset class="fieldsetform">
                                <legend><asp:Label ID="lblAccountDetails" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label> </legend>
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:lblRq21 %>  "
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
                                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:lblRq1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td width="15%" valign="top">
                                                </td>
                                                <td width="35%" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="15%" valign="top">
                                                    <asp:Label ID="lblProgramme" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="35%" valign="top">
                                                    <asp:DropDownList ID="ddlProgramme" Style="width: 155px" runat="server" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:RequiredFieldValidator1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td width="15%" valign="top">
                                                    <asp:Label ID="lblQuestionnaire" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="35%" valign="top">
                                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq2" runat="server" ErrorMessage="<% $Resources:Rq2 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlQuestionnaire" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblDescription" runat="server" Text="<% $Resources:lblDescription %>"></asp:Label>
                                                </td>
                                                <td valign="top" colspan="2">
                                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" SkinID="txtarea300X3"
                                                        Rows="3" />
                                                    <div class="maxlength-msg">
                                                        <asp:Label ID="lblMaxLength" runat="server" Text="<% $Resources:lblMaxLength %>"></asp:Label></div>
                                                </td>
                                                <td valign="top">
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:Label ID="lblCandidateList" runat="server" Text="<% $Resources:lblCandidateList %>"></asp:Label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="lblCandidateNumber" runat="server" Text="<% $Resources:lblCandidateNumber %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="5%">
                                                    <asp:TextBox ID="txtCandidateNo" MaxLength="2" onkeypress="return NumberOnly(this);"
                                                        runat="server" SkinID="ph1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="Rq19" runat="server" ErrorMessage="<% $Resources:Rq19 %>"
                                                        ControlToValidate="txtCandidateNo" ValidationGroup="group2" SetFocusOnError="True">&nbsp;</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="ressequence" ControlToValidate="txtCandidateNo"
                                                        ErrorMessage="<% $Resources:ressequence %>" ValidationExpression="^[0-9][\d]*"
                                                        runat="server" ValidationGroup="group2" SetFocusOnError="True" Text="*" ForeColor="White" />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtCandidateNo"
                                                        ErrorMessage="<% $Resources:RegularExpressionValidator1 %>" ValidationExpression="^[0-9][\d]*"
                                                        runat="server" ValidationGroup="group2" SetFocusOnError="True" Text="*" ForeColor="White" />
                                                    <asp:RangeValidator ID="valTxtRange" ControlToValidate="txtCandidateNo" Type="Integer"
                                                        MinimumValue="1" MaximumValue="99" ErrorMessage="<% $Resources:valTxtRange %>"
                                                        ValidationGroup="group2" SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />
                                                </td>
                                                <td width="25%">
                                                    <asp:ImageButton ID="imbSubmit" ImageUrl="~/Layouts/Resources/images/submit-s.png"
                                                        runat="server" OnClick="imbSubmit_Click" ValidationGroup="group2" />
                                                </td>
                                                <td width="30%">
                                                    <label>
                                                        <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;&nbsp;<span class="style3"><asp:Label ID="lblExcelType" runat="server" Text="<% $Resources:lblExcelType %>"></asp:Label></span>
                                                    </label>
                                                </td>
                                                <td width="15%">
                                                    <asp:ImageButton ID="ImgUpload" runat="server" ImageUrl="~/Layouts/Resources/images/import-s.png"
                                                        OnClick="ImgUpload_click" OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" valign="top">
                                                    <div class="candidatelist">
                                                        <asp:Repeater ID="rptrCandidateList" runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                    <tr>
                                                                        <th width="7%">
                                                                            <asp:Label ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                                        </th>
                                                                        <th width="20%">
                                                                            <asp:Label ID="lblFirstName" runat="server" Text="<% $Resources:lblFirstName %>"></asp:Label>
                                                                        </th>
                                                                        <th width="30%">
                                                                            <asp:Label ID="lblLastName" runat="server" Text="<% $Resources:lblLastName %>"></asp:Label>
                                                                        </th>
                                                                        <th width="45%">
                                                                            <asp:Label ID="lblEmailID" runat="server" Text="<% $Resources:lblEmailID %>"></asp:Label>
                                                                        </th>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td width="5%">
                                                                        <%# Container.ItemIndex + 1  %>.
                                                                    </td>
                                                                    <td width="20%">
                                                                        <asp:TextBox ID="txtFirstName" Text='<%# Eval("Relationship") %>' runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:TextBox ID="txtLastName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                                                                    </td>
                                                                    <td width="45%">
                                                                        <asp:TextBox ID="txtEmailID" SkinID="email" runat="server" Text='<%# Eval("EmailID") %>'></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <span class="style3"><asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <br />
                        <div align="center">
                            <asp:ImageButton ID="imbAssign" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server"
                                OnClick="imbAssign_Click" ValidationGroup="group1" />&nbsp;
                            <asp:ImageButton ID="imbReset" ImageUrl="~/Layouts/Resources/images/reset.png" runat="server"
                                OnClick="imbReset_Click" />
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

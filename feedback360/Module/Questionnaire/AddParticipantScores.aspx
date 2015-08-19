﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="AddParticipantScores.aspx.cs" Inherits="Module_Questionnaire_AddParticipantScores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <%-- <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server"
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:lblRq21 %>"
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
                                                <td>
                                                    <asp:Label ID="lblProgramme" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblProgrammeText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblQuestionnaire" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblQuestionnaireText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        OnSelectedIndexChanged="ddlQuestionnaire_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq2" runat="server" ErrorMessage="<% $Resources:lblRq2 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlQuestionnaire" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr id="trTargetPerson" runat="server">
                                                <td valign="top">
                                                    <asp:Label ID="lblTargetPerson" runat="server" Text="<% $Resources:lblTargetPerson %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td valign="top">
                                                    <asp:DropDownList ID="ddlTargetPerson" runat="server" lSetFocusOnError="True" Display="None" AutoPostBack="true"
                                                        Style="width: 155px" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTargetPerson_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq3" runat="server" ErrorMessage="<% $Resources:lblRq3 %>"
                                                        ControlToValidate="ddlTargetPerson" SetFocusOnError="True" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblMonthYear1" runat="server" Text="<% $Resources:lblMonthYear1 %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td valign="top">
                                                    <asp:DropDownList ID="ddlScoreMonth1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScoreMonth1_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="1">January</asp:ListItem>
                                                        <asp:ListItem Value="2">February</asp:ListItem>
                                                        <asp:ListItem Value="3">March</asp:ListItem>
                                                        <asp:ListItem Value="4">April</asp:ListItem>
                                                        <asp:ListItem Value="5">May</asp:ListItem>
                                                        <asp:ListItem Value="6">June</asp:ListItem>
                                                        <asp:ListItem Value="7">July</asp:ListItem>
                                                        <asp:ListItem Value="8">August</asp:ListItem>
                                                        <asp:ListItem Value="9">September</asp:ListItem>
                                                        <asp:ListItem Value="10">October</asp:ListItem>
                                                        <asp:ListItem Value="11">November</asp:ListItem>
                                                        <asp:ListItem Value="12">December</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlScoreYear1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScoreYear1_SelectedIndexChanged">
                                                        <%--<asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="2008">2008</asp:ListItem>
                                                        <asp:ListItem Value="2009">2009</asp:ListItem>
                                                        <asp:ListItem Value="2010">2010</asp:ListItem>
                                                        <asp:ListItem Value="2011">2011</asp:ListItem>
                                                        <asp:ListItem Value="2012">2012</asp:ListItem>
                                                        <asp:ListItem Value="2013">2013</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator2 %>"
                                                        ControlToValidate="ddlScoreMonth1" SetFocusOnError="True" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator3 %>"
                                                        ControlToValidate="ddlScoreYear1" SetFocusOnError="True" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="lblMonthYear2" runat="server" Text="<% $Resources:lblMonthYear2 %>"></asp:Label><span
                                                        class="style3"></span>
                                                </td>
                                                <td valign="top">
                                                    <asp:DropDownList ID="ddlScoreMonth2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScoreMonth2_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="1">January</asp:ListItem>
                                                        <asp:ListItem Value="2">February</asp:ListItem>
                                                        <asp:ListItem Value="3">March</asp:ListItem>
                                                        <asp:ListItem Value="4">April</asp:ListItem>
                                                        <asp:ListItem Value="5">May</asp:ListItem>
                                                        <asp:ListItem Value="6">June</asp:ListItem>
                                                        <asp:ListItem Value="7">July</asp:ListItem>
                                                        <asp:ListItem Value="8">August</asp:ListItem>
                                                        <asp:ListItem Value="9">September</asp:ListItem>
                                                        <asp:ListItem Value="10">October</asp:ListItem>
                                                        <asp:ListItem Value="11">November</asp:ListItem>
                                                        <asp:ListItem Value="12">December</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlScoreYear2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScoreYear2_SelectedIndexChanged">
                                                        <%--<asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="2008">2008</asp:ListItem>
                                                        <asp:ListItem Value="2009">2009</asp:ListItem>
                                                        <asp:ListItem Value="2010">2010</asp:ListItem>
                                                        <asp:ListItem Value="2011">2011</asp:ListItem>
                                                        <asp:ListItem Value="2012">2012</asp:ListItem>
                                                        <asp:ListItem Value="2013">2013</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator4 %>"
                                                        ControlToValidate="ddlScoreMonth2" SetFocusOnError="True" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator5 %>"
                                                        ControlToValidate="ddlScoreYear2" SetFocusOnError="True" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>--%>
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
                                                <td colspan="5" valign="top">
                                                    <div class="candidatelist">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Repeater ID="rptrCategoryList" OnItemDataBound="rptrCategoryList_ItemDataBound"
                                                                        runat="server">
                                                                        <HeaderTemplate>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                                <tr>
                                                                                    <th width="10%">
                                                                                        <asp:Label ID="lblCandidateListSrNo" runat="server" Text="<% $Resources:lblCandidateListSrNo %>"></asp:Label>
                                                                                    </th>
                                                                                    <th width="60%">
                                                                                        <asp:Label ID="lblCandidateListCategory" runat="server" Text="<% $Resources:lblCandidateListCategory %>"></asp:Label>
                                                                                    </th>
                                                                                    <th width="30%" align="center">
                                                                                        <asp:Label ID="lblCandidateListPrevScore" runat="server" Text="<% $Resources:lblCandidateListPrevScore %>"></asp:Label>
                                                                                    </th>
                                                                                </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <%# Container.ItemIndex + 1  %>.
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>' Visible="false"></asp:Label>
                                                                                    <%# Eval("CategoryName")%>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:TextBox ID="txtScore1" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                        Text='<%# Eval("Score1")%>'></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                                <td>
                                                                    <asp:Repeater ID="rptrPreviousScore2" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                                <tr>
                                                                                    <th width="100%" align="center">
                                                                                        <asp:Label ID="lblCandidateListPrevScore2" runat="server" Text="<% $Resources:lblCandidateListPrevScore2 %>"></asp:Label>
                                                                                    </th>
                                                                                </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>' Visible="false"></asp:Label>
                                                                                    <asp:TextBox ID="txtScore2" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                        Text='<%# Eval("Score2")%>'></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
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
                        <br />
                        <div align="center">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                        <br />
                    </div>
                    <!-- start user form -->
                </div>
            </div>
       <%-- </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

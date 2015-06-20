<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="EmailCandidate.aspx.cs" Inherits="Survey_Module_Admin_EmailCandidate" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/category.png" runat="server" title="<% $Resources:lblHeading %>"
                        align="absmiddle" />
                    <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <%--<asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
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
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="Div1" runat="server" class="validation-align">
                                    <span class="style3">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="divAccount" runat="server" visible="true">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:lblRq21 %>  "
                                            SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="13%">
                                        <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompanyName %>"></asp:Label>
                                    </td>
                                    <td width="38%">
                                        <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="10%" valign="top">
                                    <asp:Label ID="lblProjectName" runat="server" Text="<% $Resources:lblProject %>"></asp:Label>
                                </td>
                                <td width="15%" valign="top">
                                    <asp:Label ID="lblParticipant" runat="server" Text=""></asp:Label>
                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:lblRq1 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="group1"
                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="10%" valign="top">
                                    <asp:Label ID="lblProgrammeName" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label>
                                </td>
                                <td width="15%" valign="top">
                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator1 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="group1"
                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="10%" valign="top">
                                    <asp:Label ID="lblParticipantHeading" runat="server" Text="<% $Resources:lblParticipant %>"></asp:Label>
                                </td>
                                <td width="15%" valign="top">
                                    <asp:DropDownList ID="ddlParticipant" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="ddlParticipant_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RqParticipant" runat="server" ErrorMessage="<% $Resources:lblRqParticipant %>"
                                        SetFocusOnError="True" ControlToValidate="ddlParticipant" ValidationGroup="group1"
                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="10%" valign="top">
                                </td>
                                <td width="10%" valign="top">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        ToolTip="Reset" OnClick="imbReset_Click" />
                                </td>
                                <td width="10%" valign="top">
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        ToolTip="Submit" OnClick="imbSubmit_Click" ValidationGroup="group1" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="grdvCandidateStatus" runat="server" DataSourceID="odsCandidateStatus"
                        AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                        OnSorting="grdvCandidateStatus_Sorting" OnRowDataBound="grdvCandidateStatus_RowDataBound"
                        EmptyDataText="<center><span class='style3'>No Record Found</span></center>"
                        OnRowCommand="grdvCandidateStatus_OnRowCommand">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="2%" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="myCheckBox" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:BoundField DataField="AssignmentID" SortExpression="AssignmentID"></asp:BoundField>
                            <asp:BoundField DataField="EndDate" SortExpression="EndDate"></asp:BoundField>--%>
                            
                            <asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:lblColleagueName %>"
                                SortExpression="CandidateName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="CandidateEmail" HeaderText="<% $Resources:lblCandidateEmail %>" SortExpression="CandidateEmail">
                                <ItemStyle Width="28%" />
                            </asp:BoundField>--%>
                            <%--<asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:lblColleagueName %>" SortExpression="CandidateName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="<% $Resources:lblCandidateEmail %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle Width="28%" />
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnQuestionnaireId" Value='<%# Eval("QuestionnaireID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCandidateId" Value='<%# Eval("AssignmentID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCandidateName" Value='<%# Eval("CandidateName") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrganizationName" Value='<%# Eval("OrganisationName") %>' />
                                    <asp:HiddenField runat="server" ID="hdnStartDate" Value='<%# Eval("StartDate") %>' />
                                    <asp:HiddenField runat="server" ID="hdnEndDate" Value='<%# Eval("EndDate") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRelationShip" Value='<%# Eval("Relationship") %>' />
                                    <asp:HiddenField runat="server" ID="hdnAsgnDetailID" Value='<%# Eval("AssignmentID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnAssiggnmentID" Value='<%# Eval("AssiggnmentID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnLoginID" Value='<%# Eval("LoginID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnPassword" Value='<%# Eval("Password") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCode" Value='<%# Eval("Code") %>' />
                                    <asp:HiddenField runat="server" ID="hdnTitle" Value='<%# Eval("Title") %>' />
                                    <asp:Label runat="server" ID="lblCandidateEmail" Text='<%# Eval("CandidateEmail") %>'> </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<% $Resources:lblSubmitStatus %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="6%" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="SubmitFlag" Text='<%# (Eval("SubmitFlag").ToString() == "True" ? "Yes" : "No" ) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="98%" border="0">
                        <tr>
                            <td align="center" width="30%">
                                <table border="0" width="100%">
                                </table>
                                <asp:Literal ID="litPagingSummary" runat="server"></asp:Literal>
                            </td>
                            <td align="right" width="50%">
                                <div class="paging">
                                    <asp:PlaceHolder ID="plcPaging" runat="server"></asp:PlaceHolder>
                                </div>
                            </td>
                        </tr>
                    </table>
                    
                    
            
                    <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="lblEmailTemplates" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblStart" runat="server" Text="<% $Resources:lblStart %>"></asp:Label><span
                                        class="style3">*</span>
                                </td>
                                <td width="35%">
                                    <asp:DropDownList ID="ddlEmailStart" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlEmailStart_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq14" runat="server" ValidationGroup="group2" ErrorMessage="<% $Resources:Rq14 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlEmailStart" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblSubject" runat="server" Text="<% $Resources:lblSubject %>"></asp:Label><span
                                        class="style3"></span>
                                </td>
                                <td width="35%">
                                    <asp:TextBox ID="lblEmailSubject" runat="server" Style="width: 795px" Text=""></asp:TextBox>
                                    <%--<asp:Label ID="lblEmailSubject" runat="server" ></asp:Label><span class="style3"></span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td width="15%" valign="top">
                                    <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblFAQ %>"></asp:Label><span
                                        class="style3"></span>
                                </td>
                                <td width="85%">
                                    <FCKeditorV2:FCKeditor ID="txtFaqText" runat="server" BasePath="~/fckeditor/" Width="800px"
                                        ToolbarSet="Feedback">
                                    </FCKeditorV2:FCKeditor>
                                    <%--<asp:TextBox ID="txtFaqText" TextMode="MultiLine" SkinID="txtarea500" Rows="5" runat="server"></asp:TextBox>--%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                
            <br />
            <div align="center">
                <asp:ImageButton ID="imbSend" runat="server" ImageUrl="~/Layouts/Resources/images/send.png"
                    OnClick="imbSend_Click" ToolTip="Send" ValidationGroup="group2" />
                &nbsp;
                <asp:ImageButton ID="imbcancel" runat="server" ImageUrl="~/Layouts/Resources/images/cancel.png"
                    OnClick="imbcancel_Click" ToolTip="Back to List" />
            </div>
            <br />
            <div align="center">
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text=""></asp:Label>
            </div>
            <br />
            <asp:ObjectDataSource ID="odsCandidateStatus" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                SelectMethod="GetdtAssignListNew" TypeName="Questionnaire_BAO.AssignQuestionnaire_BAO">
            </asp:ObjectDataSource>
            
           <%-- </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
            
            
            
        </div>
    </div>
</asp:Content>

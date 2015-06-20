<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignQstnPaticipantList.aspx.cs"
    Inherits="Survey_Module_Feedback_AssignQstnPaticipantList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Feedback 360</title>
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/reset.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/style.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/ddmenu.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/Calendar_360.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/Calendar.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/jquery-ui-1.7.2.custom.css" />

    <script src='<%= ResolveClientUrl("../../Layouts/Resources/js/common.js")%>' type="text/javascript"></script>

    <script type="text/javascript" src='<%= ResolveClientUrl("../../Layouts/Resources/js/GeneralFunctions.js") %>'></script>

</head>
<body>
    <form id="frmFeedback" runat="server">
    <div id="divQuesPart" runat="server" style="height: 500px; overflow-y: scroll;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="bodytextcontainer">
            <div class="innercontainer">
                <!-- start heading logout -->
                <div class="Survey_topheadingdetails">
                    <h3>
                        <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblToolTip %>" 
                            align="absmiddle" />
                            <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                        </h3>
                    <div class="clear">
                    </div>
                </div>
                <!-- end heading logout -->
                
                        <div class="searchgrid">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="25%">
                                        <asp:Label ID="lblCode" runat="server" Text="<%$ Resources:lblCode %>"></asp:Label>                                       
                                        <asp:Label ID="lblAccountCode" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td width="35%">
                                        <asp:Label ID="lblPrjoectName" runat="server" Text="<% $Resources:lblPrjoectName %>"></asp:Label>                                       
                                        <asp:Label ID="lblProjectName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td width="35%">
                                        <asp:Label ID="lblPrgName" runat="server" Text="<% $Resources:lblPrgName %>"></asp:Label>                                       
                                        <asp:Label ID="lblProgrammeName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td width="5%">
                                        <asp:ImageButton ID="imbExportData" runat="server" title="Export to Excel" 
                                            ImageUrl="~/Layouts/Resources/images/export.png" 
                                            onclick="imbExportData_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        
                <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                        
                        <%--<!-- Search Grid -->
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="17%">
                                    Questionnaire Name
                                </td>
                                <td width="22%">
                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="11%">
                                    Project Name
                                </td>
                                <td width="23%">
                                  <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                >
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                
                                            </asp:DropDownList>
                                </td>
                                <td width="12%">
                                </td>
                                <td width="5%">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        OnClick="imbReset_Click" ToolTip="Reset" />
                                </td>
                                <td width="5%">
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        OnClick="imbSubmit_Click" ToolTip="Submit" />
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                        <!-- grid list -->
                        <asp:GridView ID="grdvAssignQuestionnaire" runat="server" DataSourceID="odsAssignQstnParticipant"
                            AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                            OnSorting="grdvAssignQuestionnaire_Sorting" OnRowCommand="grdvAssignQuestionnaire_RowCommand"
                            OnRowDataBound="grdvAssignQuestionnaire_RowDataBound" DataKeyNames="AssignmentID"
                            EmptyDataText="<% $Resources:lblNoRecordFound %>">
                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                Visible="false" />
                            <Columns>
                                <asp:BoundField DataField="AssignmentID" HeaderText="<% $Resources:grdvAssignmentID %>" SortExpression="AssignmentID"
                                    Visible="False" />
                                <%--<asp:BoundField DataField="Code" HeaderText="Account Code" SortExpression="Code">
                                    <ItemStyle Width="11%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProgrammeName" HeaderText="Programme Name" SortExpression="ProgrammeName">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="QSTNName" HeaderText="Questionnaire Name" SortExpression="QSTNName">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="FirstName" HeaderText="<% $Resources:grdvFName %>" SortExpression="FirstName">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LastName" HeaderText="<% $Resources:grdvLName %>" SortExpression="LastName">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmailID" HeaderText="<% $Resources:grdvEmail %>">
                                    <ItemStyle Width="26%" />
                                </asp:BoundField>
                                <asp:TemplateField ControlStyle-Width="12%" HeaderText="<% $Resources:grdvTotalCandidates %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCandidateCount" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="12%" HeaderText="<% $Resources:grdvCompletedCandidates %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmissionCount" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="12%" HeaderText="<% $Resources:grdvSelfAssessment %>" >
                                    <ItemTemplate   >
                                        <asp:Label ID="lblSelfAssessment" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="SendMail" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="2%" Text="<img id='imgSendMail' runat='server' src='../../Layouts/Resources/images/sendemail.png' title='Re-send Email' />">
                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                </asp:ButtonField>
                                <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="2%" DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />">
                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                </asp:CommandField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssignID" runat="server" Text='<%# Eval("AssignmentID1") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblAccountID" runat="server" Text='<%# Eval("AccountID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblTargetPersonID" runat="server" Text='<%# Eval("UserID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblProgrammeID" runat="server" Text='<%# Eval("ProgrammeID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblProjectID" runat="server" Text='<%# Eval("ProjecctID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0">
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
                        <br />
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
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsAssignQstnParticipant" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                            SelectMethod="GetdtAssignPartiList" TypeName="Questionnaire_BAO.AssignQstnParticipant_BAO"
                            DeleteMethod="DeleteAssignQuestionnaire"></asp:ObjectDataSource>
                            
                        <rsweb:ReportViewer ID="rview" runat="server">
                        </rsweb:ReportViewer>
                            
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

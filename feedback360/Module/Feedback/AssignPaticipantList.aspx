<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignPaticipantList.aspx.cs" Inherits="Module_Feedback_AssignPaticipantList" %>

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
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblToolTip %>"  align="absmiddle" />
                    <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                    </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
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
                    <div id="divQuesPart" runat="server" style="height:425px;overflow-y:scroll;" >
                    <asp:GridView ID="grdvAssignQuestionnaire" runat="server" DataSourceID="odsAssignQstnParticipant" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"  OnSorting="grdvAssignQuestionnaire_Sorting"
                        DataKeyNames="AssignmentID" EmptyDataText="<% $Resources:lblNoRecordFound %>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                          
                            <asp:BoundField DataField="AssignmentID" HeaderText="<% $Resources:grdvAssignmentID %>" SortExpression="AssignmentID"
                                Visible="False" />
                            <asp:BoundField DataField="Code" HeaderText="<% $Resources:grdvCode %>" SortExpression="Code">
                                <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="<% $Resources:grdvProjectName %>" SortExpression="Title">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="QSTNName" HeaderText="<% $Resources:grdvQtnrName %>" SortExpression="QSTNName">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:grdvCandidateName %>" SortExpression="CandidateName">
                                <ItemStyle Width="39%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RelationShip" HeaderText="<% $Resources:grdvRelationship %>" SortExpression="RelationShip">
                                <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CandidateEmail" HeaderText="<% $Resources:grdvEmail %>">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                           
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
                    
                    </div>
                    
                    
                    <br />
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsAssignQstnParticipant" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                         SelectMethod="GetdtAssignProgrammePartiList" TypeName="Questionnaire_BAO.AssignQstnParticipant_BAO">
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    </form>
</body>
</html>
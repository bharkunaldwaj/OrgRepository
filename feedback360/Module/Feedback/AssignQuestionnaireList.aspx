<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignQuestionnaireList.aspx.cs"
    Inherits="Module_Feedback_AssignQuestionnaireList" %>

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
    <div id="divQues" runat="server" style="height: 450px; overflow-y: scroll;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="bodytextcontainer">
            <div class="innercontainer">
                <!-- start heading logout -->
                <div class="topheadingdetails">
                    <h3>
                        <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblToolTip %>"
                            align="absmiddle" />
                            <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                        </h3>
                    <div class="clear"> 
                    </div>
                </div>
                <!-- end heading logout -->
                <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                        
                        <!-- grid list -->
                        <asp:GridView ID="grdvAssignQuestionnaire" runat="server" DataSourceID="odsAssignQuestionnaire"
                            AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                            OnSorting="grdvAssignQuestionnaire_Sorting" OnRowDataBound="grdvAssignQuestionnaire_RowDataBound"
                            DataKeyNames="AssignmentID" EmptyDataText="<% $Resources:lblNoRecordFound %>"
                            OnRowCommand="grdvAssignQuestionnaire_RowCommand" >
                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                Visible="false" />
                            <Columns>
                                <asp:BoundField DataField="AssignmentID" HeaderText="<% $Resources:grdvAssignmentID %>" SortExpression="AssignmentID"
                                    Visible="false" />
                                <asp:BoundField DataField="Code" HeaderText="<% $Resources:grdvCode %>" SortExpression="Code">
                                    <ItemStyle Width="12%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Title" HeaderText="<% $Resources:grdvProjectName %>" SortExpression="Title">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="QSTNName" HeaderText="<% $Resources:grdvQstnrName %>" SortExpression="QSTNName">
                                    <ItemStyle Width="100" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:grdvColleagueName %>" SortExpression="CandidateName">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="RelationShip" HeaderText="<% $Resources:grdvRelation %>" SortExpression="RelationShip">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>--%>
                                
                                <asp:TemplateField HeaderText="<% $Resources:grdvRelation %>" >
                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlRelationship" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                                                            
                               <%-- <asp:BoundField DataField="CandidateEmail" HeaderText="<% $Resources:grdvEmail %>">
                                    <ItemStyle Width="25%" />
                                </asp:BoundField>--%>
                                 <asp:TemplateField HeaderText="<% $Resources:grdvEmail %>">
                                    <ItemTemplate>
                                        <asp:TextBox style="width:180px" ID="txtEmail" runat="server" Text='<%# Eval("CandidateEmail") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="<% $Resources:lblSubmitStatus %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="SubmitFlag" Text=<%# (Eval("SubmitFlag").ToString() == "True" ? "Yes" : "No" ) %>  ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                                <asp:ButtonField CommandName="SendMail" ButtonType="Link" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%"
                                    Text="<img id='imgSendMail' runat='server' src='../../Layouts/Resources/images/sendemail.png' title='Re-send Email' />" >
                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                </asp:ButtonField>
                                <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="2%" DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />">
                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                </asp:CommandField>
                                
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:HiddenField id="hdnRelationship" runat="server" Value=<%# Eval("RelationShip") %> />
                                        
                                        <asp:Label id="lblAssignID" runat="server" Text=<%# Eval("AssignID") %> visible="false" ></asp:Label>
                                        <asp:Label id="lblAccountID" runat="server" Text=<%# Eval("AccountID") %> visible="false" ></asp:Label>
                                        <asp:Label id="lblTargetPersonID" runat="server" Text=<%# Eval("TargetPersonID") %> visible="false" ></asp:Label>
                                        <asp:Label id="lblAssignmentID" runat="server" Text=<%# Eval("AssignmentID") %> visible="false" ></asp:Label>
                                        <asp:Label id="lblProjectID" runat="server" Text=<%# Eval("ProjectID") %> visible="false" ></asp:Label>
                                        <asp:Label id="lblEmailSendFlag" runat="server" Text=<%# Eval("EmailSendFlag") %> visible="false" ></asp:Label>
                                        
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
                        <br />
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="imbSave" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server"
                                ToolTip="Save" onclick="imbSave_Click" />&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsAssignQuestionnaire" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                            DeleteMethod="DeleteAssignQuestionnaire" SelectMethod="GetdtAssignList" TypeName="Questionnaire_BAO.AssignQuestionnaire_BAO">
                        </asp:ObjectDataSource>
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

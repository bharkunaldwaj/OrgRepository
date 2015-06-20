<%@ Page Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" AutoEventWireup="true" CodeFile="AssignQstnPaticipantList.aspx.cs" Inherits="Module_Questionnaire_AssignQstnPaticipantList" Title="Assign Participant Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div id="divFaqText" runat="server" style="height:450px;overflow-y:scroll;" >
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/assign_questionnaire.png" title="Assign Questionnaire Management" align="absmiddle" />
                    Assign Participant Management</h3>
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
                    <asp:GridView ID="grdvAssignQuestionnaire" runat="server" DataSourceID="odsAssignQstnParticipant" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"  OnSorting="grdvAssignQuestionnaire_Sorting"
                        DataKeyNames="AssignmentID" EmptyDataText="<center><span class='style3'>No Record Found</span></center>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="AssignmentID" HeaderText="AssignmentID" SortExpression="AssignmentID"
                                Visible="False" />
                            <asp:BoundField DataField="Code" HeaderText="Account Code" SortExpression="Code">
                                <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="Project Name" SortExpression="Title">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="QSTNName" HeaderText="Questionnaire Name" SortExpression="QSTNName">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmailID" HeaderText="Email">
                                <ItemStyle Width="20%" />
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
                    <br />
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsAssignQstnParticipant" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                         SelectMethod="GetdtAssignPartiList" TypeName="Questionnaire_BAO.AssignQstnParticipant_BAO">
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    </div>
</asp:Content>


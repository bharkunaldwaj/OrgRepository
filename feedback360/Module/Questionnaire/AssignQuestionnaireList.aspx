<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignQuestionnaireList.aspx.cs" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" Inherits="Module_Questionnaire_AssignQuestionnaireList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/assign_questionnaire.png" title="Assign Questionnaire Management" align="absmiddle" />
                    Assign Questionnaire Management</h3>
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
                    

                    <!-- grid list -->
                    <asp:GridView ID="grdvAssignQuestionnaire" runat="server" DataSourceID="odsAssignQuestionnaire" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"  OnSorting="grdvAssignQuestionnaire_Sorting"
                        DataKeyNames="AssignmentID" EmptyDataText="<center><span class='style3'>No Record Found</span></center>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="AssignmentID" HeaderText="AssignmentID" SortExpression="AssignmentID"
                                Visible="False" />
                            <asp:BoundField DataField="Code" HeaderText="Account Code" SortExpression="Code">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="Project Name" SortExpression="Title">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="ProgrammeName" HeaderText="Programme Name" SortExpression="ProgrammeName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="QSTNName" HeaderText="Questionnaire Name" SortExpression="QSTNName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="CandidateName" HeaderText="Candidate Name" SortExpression="CandidateName">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RelationShip" HeaderText="Relationship" SortExpression="RelationShip">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CandidateEmail" HeaderText="Email">
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
                    <br />
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsAssignQuestionnaire" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                         SelectMethod="GetdtAssignList" TypeName="Questionnaire_BAO.AssignQuestionnaire_BAO">
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

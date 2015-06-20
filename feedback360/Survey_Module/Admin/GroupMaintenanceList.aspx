<%@ Page Title="Group Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" AutoEventWireup="true" CodeFile="GroupMaintenanceList.aspx.cs" Inherits="Survey_Module_Admin_GroupMaintenanceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/account.png" runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />
                    <asp:Label ID="lblGroupManagement" runat="server" Text="<% $Resources:lblGroupManagement %>"></asp:Label>
                   </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->

            <!-- grid list -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    <!-- grid list -->
                    <asp:GridView ID="grdvGroup" runat="server" DataSourceID="odsGroup" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" OnRowDataBound="grdvGroup_RowDataBound"
                        DataKeyNames="GroupID" EmptyDataText="<% $Resources:lblNoRecordFound %>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="GroupID" HeaderText="<% $Resources:lblGroupID %>" SortExpression="GroupID"
                                Visible="False" />
                            <asp:TemplateField HeaderText="Name" SortExpression="GroupName">
                                <ItemStyle Width="25%" />
                                <ItemTemplate>
                                    <a href="GroupMaintenance.aspx?Mode=R&GroupID=<%# Eval("GroupID") %>">
                                        <%# Eval("GroupName")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="Description" HeaderText="<% $Resources:lblDesc%>" >
                                <ItemStyle Width="70%" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="GroupMaintenance.aspx?Mode=E&GroupID=<%# Eval("GroupID") %>">
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />" >
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png" ToolTip="Add New Group"
                                    runat="server" OnClick="ibtnAddNew_Click" />
                            </td>
                            <td align="center" width="30%">
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
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsGroup" runat="server" 
                        DataObjectTypeName="Administration_BE.Survey_Group_BE" DeleteMethod="DeleteGroup" 
                        SelectMethod="GetdtGroupList" TypeName="Administration_BAO.Survey_Group_BAO" >
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
    
</asp:Content>


<%@ Page Title="User Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="AccountUserList.aspx.cs" Inherits="Module_Admin_AccountUserList" %>

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
                    <img src="../../Layouts/Resources/images/user.png" runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />
                    <asp:Label ID="lblUserManagement" runat="server" Text="<% $Resources:lblUserManagement %>"></asp:Label>
                    </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
           <%-- <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
                
                
                 <div id="divAccount" runat="server" visible="false">
                    <fieldset class="fieldsetform">
                    <legend> <asp:Label ID="lblAccount" runat="server" Text="<% $Resources:lblAccount %>"></asp:Label>
                     </legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label>
                            </td>
                            <td width="36%">
                                <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" 
                                    AppendDataBoundItems="True" 
                                    onselectedindexchanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true" >
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    
                                </asp:DropDownList>
                              
                            </td>
                            <td width="13%">
                                <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                            </td>
                            <td width="38%">
                                 <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                       
                    </table>
                </fieldset>
                </div>
                    <!-- start search -->
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="11%">
                                    <asp:Label ID="lblFname" runat="server" Text="<% $Resources:lblFname %>"></asp:Label>                                    
                                </td>
                                <td width="22%">
                                    <asp:TextBox ID="txtUserName" MaxLength="25" runat="server"></asp:TextBox>
                                </td>
                                <td width="11%">
                                    <asp:Label ID="lblLoginId" runat="server" Text="<% $Resources:lblLoginId %>"></asp:Label>                                    
                                </td>
                                <td width="23%">
                                    <asp:TextBox ID="txtLoginId" MaxLength="25" runat="server"></asp:TextBox>
                                </td>
                                <td width="12%">
                                    <asp:Label ID="lblGroup" runat="server" Text="<% $Resources:lblGroup %>"></asp:Label>                                    
                                </td>
                                <td width="21%">
                                    <asp:DropDownList ID="ddlGroup" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="2">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        OnClick="imbReset_Click" ToolTip="Reset" />
                                    <br />
                                    
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        OnClick="imbSubmit_Click" ToolTip="Submit" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLName" runat="server" Text="<% $Resources:lblLName %>"></asp:Label>                                    
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastName" MaxLength="25" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- start search -->
                    <!-- grid list -->
                    <!-- grid list -->
                    <asp:GridView ID="grdvAccountUser" runat="server" DataSourceID="odsAccountUser" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" OnRowDataBound="grdvAccountUser_RowDataBound"
                        DataKeyNames="UserID" EmptyDataText="<% $Resources:lblNoRecordFound %>" >
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="UserID" HeaderText="<% $Resources:grdvUserId %>" SortExpression="UserID" Visible="False" />
                            <asp:TemplateField HeaderText="Login ID" SortExpression="LoginID">
                                <ItemStyle Width="15%" />
                                <ItemTemplate>
                                    <a href="AccountUser.aspx?Mode=R&UsrId=<%# Eval("UserID") %>">
                                        <%# Eval("LoginID") %>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Code" HeaderText="<% $Resources:grdvAccountCode %>" SortExpression="Code">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="<% $Resources:grdvName %>" SortExpression="UserName">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GroupName" HeaderText="<% $Resources:grdvGroup %>">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmailID" HeaderText="<% $Resources:grdvEmailID %>">
                                <ItemStyle Width="31%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="AccountUser.aspx?Mode=E&UsrId=<%# Eval("UserID") %>">
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
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png"
                                    runat="server" OnClick="ibtnAddNew_Click" ToolTip="Add New User" />
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
                    <asp:ObjectDataSource ID="odsAccountUser" runat="server" 
                        DataObjectTypeName="Admin_BE.AccountUser_BE" DeleteMethod="DeleteAccountUser" 
                        SelectMethod="GetdtAccountUserListNew" TypeName="Admin_BAO.AccountUser_BAO" >
                    </asp:ObjectDataSource>
               <%-- </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>

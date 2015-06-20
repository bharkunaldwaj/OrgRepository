<%@ Page Title="Account Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="AccountList.aspx.cs" Inherits="Module_Admin_AccountList" %>

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
                    <img src="../../Layouts/Resources/images/account.png" runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />                    
                    <asp:Label ID="lblAccountHeading" runat="server" Text="<% $Resources:lblAccountHeading %>"></asp:Label>
                    </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            
             <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
            
            <!-- start search -->
            <div class="searchgrid">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="3%">                            
                            <asp:Label ID="lblCode" runat="server" Text="<% $Resources:lblCode %>"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="txtAccountCode" MaxLength="25" runat="server"></asp:TextBox>
                        </td>
                        
                        <td width="10%">
                            <asp:Label ID="lblOrganisationName" runat="server" Text="<% $Resources:lblOrganisationName %>"></asp:Label>                            
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="txtAccountName" MaxLength="25" runat="server"></asp:TextBox>
                        </td>
                        
                        <td width="5%">
                            <%--Login ID--%>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="txtLoginID" Visible="false" MaxLength="25" runat="server"></asp:TextBox>
                        </td>
                        
                        <td width="7%" align="center">
                            <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                             title="Reset"   OnClick="imbReset_Click" />
                        </td>
                        <td width="8%" align="center">
                            <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                             title="Submit"   OnClick="imbSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <!-- start search -->
            <!-- grid list -->
           
                    <!-- grid list -->
                    <asp:GridView ID="grdvAccount" runat="server" DataSourceID="odsAccount" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" OnRowDataBound="grdvAccount_RowDataBound"
                        DataKeyNames="AccountID" EmptyDataText="<% $Resources:lblNoRecordFound %>" >
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="AccountID" HeaderText="<% $Resources:grdvAccountAccountId %>" SortExpression="AccountID"
                                Visible="False" />
                            <asp:TemplateField HeaderText="<% $Resources:grdvAccountCode %>" SortExpression="Code">
                                <ItemStyle Width="20%" />
                                <ItemTemplate>
                                    <a href="Accounts.aspx?Mode=R&AccId=<%# Eval("AccountID") %>">
                                        <%# Eval("Code") %>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="LoginID" HeaderText="Login ID" SortExpression="LoginID">
                                <ItemStyle Width="25%" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="OrganisationName" HeaderText="<% $Resources:grdvAccountOrgName %>" SortExpression="OrganisationName">
                                <ItemStyle Width="35%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmailID" HeaderText="<% $Resources:grdvAccountEmailId %>">
                                <ItemStyle Width="35%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="Accounts.aspx?Mode=E&AccId=<%# Eval("AccountID") %>">
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />"  >
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png"
                                 title="Add New Account"   runat="server" OnClick="ibtnAddNew_Click" />
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
                    <asp:ObjectDataSource ID="odsAccount" runat="server" 
                        DataObjectTypeName="Admin_BE.Account_BE" DeleteMethod="DeleteAccount" 
                        SelectMethod="GetdtAccountList" TypeName="Admin_BAO.Account_BAO" >
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>

<%@ Page Title="Email Templates Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="EmailTemplatesList.aspx.cs" Inherits="Module_Admin_EmailTemplatesList" %>

<%@ Import Namespace="System.Data" %>
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
                    <img src="../../Layouts/Resources/images/email.png" runat="server" title="<% $Resources:lblToolTip %>"
                        align="absmiddle" />
                    <asp:Label ID="lblEmailTemplate" runat="server" Text="<% $Resources:lblEmailTemplate %>"></asp:Label>
                </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
          <%--  <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
                    <div id="divAccount" runat="server" visible="false">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblAccount" runat="server" Text="<% $Resources:lblAccount %>"></asp:Label>
                            </legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="13%">
                                        <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                    </td>
                                    <td width="38%">
                                        <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                        <asp:Button ID="Button1" runat="server" Text="Button" Visible="false" Enabled="false"
                                            OnClick="Button1_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <!-- grid list -->
                    <asp:GridView ID="grdvEmailTemplates" runat="server" DataSourceID="odsEmailTemplate"
                        AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                        OnSorting="grdvEmailTemplates_Sorting" OnRowDataBound="grdvEmailTemplates_RowDataBound"
                        DataKeyNames="EmailTemplateID" EmptyDataText="<center><span class='style3'>No Record Found</span></center>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="EmailTemplateID" HeaderText="<% $Resources:grdvEmailEmailTempID %>"
                                SortExpression="EmailTemplateID" Visible="False" />
                            <asp:TemplateField HeaderText="Title" SortExpression="Title">
                                <ItemStyle Width="15%" />
                                <ItemTemplate>
                                    <a href='EmailTemplates.aspx?Mode=R&amp;EmailTempID=<%#((DataRowView)Container.DataItem)["EmailTemplateID"] %>'>
                                        <%#((DataRowView)Container.DataItem)["Title"] %>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Code" HeaderText="<% $Resources:grdvEmailAccountCode %>"
                                SortExpression="Code">
                                <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="<% $Resources:grdvEmailDesc %>"
                                SortExpression="Description" ItemStyle-Wrap="true">
                                <ItemStyle Width="25%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmailText" HeaderText="<% $Resources:grdvEmailText%>">
                                <ItemStyle Width="39%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href='EmailTemplates.aspx?Mode=E&amp;EmailTempID=<%#((DataRowView)Container.DataItem)["EmailTemplateID"] %>'>
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />">
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png"
                                    ToolTip="Add New Email Template" runat="server" OnClick="ibtnAddNew_Click" />
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
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsEmailTemplate" runat="server" DataObjectTypeName="Admin_BE.EmailTemplate_BE"
                        DeleteMethod="DeleteEmailTemplate" SelectMethod="GetdtEmailTemplateList" TypeName="Admin_BAO.EmailTemplate_BAO">
                    </asp:ObjectDataSource>
                    <!-- grid list -->
               <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>

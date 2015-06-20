<%@ Page Title="Category Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="CategoryList.aspx.cs" Inherits="Module_Questionnaire_CategoryList" %>

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
                    <img src="../../Layouts/Resources/images/category.png" title="Category Management" align="absmiddle" />
                    <asp:Label ID="lblExclude" runat="server" Text="<% $Resources:lblCatMgt %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="summary" runat="server" class="validation-align">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="group1" />
                                   
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
                    
                    <div id="divAccount" runat="server" visible="false">
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode1 %>"></asp:Label>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
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
                    <!-- Search Grid -->
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="17%">
                                    <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblQuestionnaireName %>"></asp:Label>
                                </td>
                                <td width="22%">
                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="11%">
                                    <asp:Label ID="Label3" runat="server" Text="<% $Resources:lblCategoryName %>"></asp:Label>
                                </td>
                                <td width="23%">
                                    <asp:TextBox ID="txtCategoryName" MaxLength="25" runat="server"></asp:TextBox>
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
                    </div>
                    <!-- grid list -->
                    <asp:GridView ID="grdvCategory" runat="server" DataSourceID="odsCategory" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" OnRowDataBound="grdvCategory_RowDataBound" OnSorting="grdvCategory_Sorting"
                        DataKeyNames="CategoryID" EmptyDataText="<center><span class='style3'>No Record Found</span></center>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="CategoryID" HeaderText="<% $Resources:lblCategoryID %>" SortExpression="CategoryID"
                                Visible="False" />
                            <asp:BoundField DataField="Code" HeaderText="<% $Resources:lblAccountCode %>" SortExpression="Code">
                                <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QSTNName" HeaderText="<% $Resources:lblQuestionnaireName1 %>" SortExpression="QSTNName">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<% $Resources:lblName %>" SortExpression="CategoryName">
                                <ItemStyle Width="10%" />
                                <ItemTemplate>
                                    <a href="Category.aspx?Mode=R&CatId=<%# Eval("CategoryID") %>">
                                        <%# Eval("CategoryName") %>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CategoryTitle" HeaderText="<% $Resources:lblCategoryTitle %>" SortExpression="CategoryTitle">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="<% $Resources:lblDescription %>" SortExpression="Description">
                                <ItemStyle Width="29%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sequence" HeaderText="<% $Resources:lblSequenceNo %>" SortExpression="Sequence">
                                <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExcludeFromAnalysis" HeaderText="<% $Resources:lblIsExcluded %>">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="Category.aspx?Mode=E&CatId=<%# Eval("CategoryID") %>">
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
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png" ToolTip="Add New Category"
                                    runat="server" OnClick="ibtnAddNew_Click" />
                            </td>
                            <td align="center" width="30%">
                                <table border="0" width="100%">
                                    <tr>
                                        <td width="5%" align="right">
                                            <asp:ImageButton ID="imbReSequence" runat="server" ImageUrl="~/Layouts/Resources/images/Re-Sequence.png" ToolTip="<% $Resources:lblReSequenceCategory %>"
                                                OnClick="imbReSequence_Click" ValidationGroup="group1"/>
                                        </td>
                                        <td width="5%" align="left">
                                            &nbsp;
                                            <asp:TextBox ID="txtSequenceIncrement" ToolTip="" MaxLength="2" SkinID="age" runat="server"></asp:TextBox>
                                            <asp:RangeValidator id="valTxtRange" ControlToValidate="txtSequenceIncrement" Type="Integer" MinimumValue="1" MaximumValue="10" ErrorMessage="<% $Resources:valTxtRange %>"   ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />
                                        </td>
                                    </tr>
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
                    <asp:ObjectDataSource ID="odsCategory" runat="server" DataObjectTypeName="Questionnaire_BE.Survey_Category_BE"
                        DeleteMethod="DeleteCategory" SelectMethod="GetdtnewCategoryList" TypeName="Questionnaire_BAO.Survey_Category_BAO">
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

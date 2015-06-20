<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" Culture="en-GB" UICulture="en-US"   CodeFile="ProgrammeList.aspx.cs" Inherits="Module_Questionnaire_ProgrammeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">

    <script type="text/javascript">

    

  

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/project.png" runat="server" title="<% $Resources:lblHeading %>" align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start search -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                
                
                 <div id="divAccount" runat="server" visible="false">
                    <fieldset class="fieldsetform">
                    <legend><asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label>
                            </td>
                            <td width="36%">
                                <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" 
                                    AppendDataBoundItems="True" 
                                    onselectedindexchanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true" >
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
                
                
                
                
                
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                
                                <td width="11%">
                                    <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblProjectName  %>"></asp:Label>
                                </td>
                                <td width="23%">
                                          <asp:DropDownList ID="ddlproject" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select Project Name</asp:ListItem>
                                       
                                    </asp:DropDownList>
                                </td>
                                <td width="12%">
                                    <asp:Label ID="Label3" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label> 
                                </td>
                                <td width="35%">
                                    <asp:TextBox ID="txtprogramme" MaxLength="25" runat="server"></asp:TextBox>
                                </td>
                                <td rowspan="2">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png" ToolTip="Reset"
                                        OnClick="imbReset_Click" />
                                  &nbsp;&nbsp;
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png" ToolTip="Submit"
                                        OnClick="imbSubmit_Click" />
                                </td>
                            </tr>
                         
                        </table>
                    </div>
                    <!-- start search -->
                    <!-- grid list -->
                    <!-- grid list -->
                    <asp:GridView ID="grdvProgramme" runat="server" DataSourceID="odsProgramme" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" OnSorting="grdvProgramme_Sorting"
                        OnRowDataBound="grdvProgramme_RowDataBound" DataKeyNames="ProgrammeID"
                        EmptyDataText="<% $Resources:lblNoRecordFound %>" >
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="ProgrammeID" HeaderText="<% $Resources:lblProgrammeID %>" SortExpression="ProgrammeID"
                                Visible="False" />
                            <%--<asp:TemplateField HeaderText="Sr. No.">
                                <ItemStyle Width="6%" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="<% $Resources:lblTitle %>" SortExpression="ProgrammeName">
                                <ItemStyle Width="16%" />
                                <ItemTemplate>
                                    <a href="Programme.aspx?Mode=R&PrgId=<%# Eval("ProgrammeID") %>">
                                        <%# Eval("ProgrammeName")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <%--<asp:BoundField DataField="Code"  HeaderText="<% $Resources:lblAccountCode %>" SortExpression="Code">
                            <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="<% $Resources:lblProjectName %>" SortExpression="Title">
                                <ItemStyle Width="18%" />
                            </asp:BoundField>--%>
                           
                           
                            <asp:BoundField DataField="StartDate" HeaderText="<% $Resources:lblStartDate %>" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EndDate" HeaderText="<% $Resources:lblEndDate %>" SortExpression="EndDate" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="Programme.aspx?Mode=E&PrgId=<%# Eval("ProgrammeID") %>">
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" 
                                DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />" >
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                            
                            <%--  <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                               <ItemTemplate>
                                    <a href="#"  onclick="javascript:window.open('../../Module/Feedback/AssignPaticipantList.aspx?AccountID=<%# Eval("AccountID") %>&ProgrammeID=<%# Eval("ProgrammeID") %>','','left=100,top=100,height=475,width=1000');"  >
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/view.png" title="View Candidates' List" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                               <ItemTemplate>
                                    <a href="#"  onclick="javascript:window.open('../../Module/Feedback/AssignQstnPaticipantList.aspx?AccountID=<%# Eval("AccountID") %>&PrgramId=<%# Eval("ProgrammeID") %>','','left=100,top=100,height=475,width=1000,menubar=no, resizable=yes');"  >
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/view.png" title="View Participants' List" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png" ToolTip="Add New Programme"
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
                    <asp:ObjectDataSource ID="odsProgramme" runat="server" 
                        DataObjectTypeName="Questionnaire_BE.Programme_BE" DeleteMethod="DeleteProgramme" 
                        SelectMethod="GetdtProgrammeListNew" TypeName="Questionnaire_BAO.Programme_BAO" >
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>
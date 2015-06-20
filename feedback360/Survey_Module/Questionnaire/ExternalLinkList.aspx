<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    Culture="en-GB" UICulture="en-US" CodeFile="ExternalLinkList.aspx.cs" Inherits="Survey_Module_Questionnaire_ExternalLinkList" %>

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
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/project.png" runat="server" title="<% $Resources:lblHeading %>"
                        align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="External Link Management"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start search -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    <div id="divAccount" runat="server" visible="false">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="13%">
                                        <%--<asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompanyName %>"></asp:Label>--%>
                                    </td>
                                    <td width="38%">
                                        <%--<asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>--%>
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
                                        <asp:DropDownList ID="ddlproject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="S">Select Project Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="12%">
                                        <asp:Label ID="lblCompanyName" runat="server" Text="<% $Resources:lblCompanyName  %>"></asp:Label>
                                    </td>
                                    <td width="35%">
                                      <asp:DropDownList ID="ddlCompany" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="S">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td rowspan="2">
                                        <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                            ToolTip="Reset" OnClick="imbReset_Click" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                            ToolTip="Submit" OnClick="imbSubmit_Click" />
                                    </td>
                                </tr>
                               
                                <tr>
                                 <td>
                                   <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                     <asp:DropDownList ID="ddlStatus" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="B" Selected="True">Both</asp:ListItem>
                                            <asp:ListItem Value="True">Active</asp:ListItem>
                                            <asp:ListItem Value="False">InActive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="grdvExternalLink" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" CssClass="grid" EmptyDataText="<% $Resources:lblNoRecordFound %>"
                            OnSorting="grdvExternalLink_Sorting" Width="100%">
                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                Visible="false" />
                            <Columns>
                                <asp:TemplateField HeaderText="Visit Link">
                                    <ItemStyle  />
                                    <ItemTemplate>
                                        <a href='<%# Eval("ExternalLink") %>'>Visit</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:BoundField DataField="ProgrammeName" HeaderText="Programme Name" SortExpression="ProgrammeName">
                                    <ItemStyle  />
                                </asp:BoundField>

                                 <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName">
                                    <ItemStyle />
                                </asp:BoundField>
                                  <asp:BoundField DataField="QuestionName" HeaderText="Questionnaires Name" SortExpression="QuestionName">
                                    <ItemStyle  />
                                </asp:BoundField>
                                <asp:BoundField DataField="ExternalLink" HeaderText="Link" SortExpression="ExternalLink">
                                    <ItemStyle  />
                                </asp:BoundField>

                                <asp:BoundField DataField="EmailTo" HeaderText="Email To" SortExpression="EmailTo">
                                    <ItemStyle  />
                                </asp:BoundField>
                               <asp:BoundField DataField="CustomEmail" HeaderText="Email" SortExpression="CustomEmail">
                                    <ItemStyle  />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmailTitle" HeaderText="Email Template Name" SortExpression="EmailTitle">
                                    <ItemStyle  />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderStyle-Width="10%" HeaderText="Status" SortExpression="Status">
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created On" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"
                                    SortExpression="CreatedDate">
                                    <ItemStyle  />
                                </asp:BoundField>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <a ID="lnkEdit" runat="Server" href='<%# "addExternallink.aspx?id=" + Eval("UniqueID") %>'>Edit</a>
                                   
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle VerticalAlign="Top" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnDisable" runat="Server" CommandArgument='<%# Eval("UniqueID") %>'
                                                      OnClick="lbtnDisable_Click" Text="Disable"  Visible='<%#Eval("Active") %>'></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnEnable" runat="Server" CommandArgument='<%# Eval("UniqueID") %>'
                                                      OnClick="lbtnEnable_Click" Text="Enable" Visible='<%#Convert.ToBoolean(Eval("Inactive")) %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle VerticalAlign="Top" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    <!-- start search -->
                    <!-- grid list -->
                    <!-- grid list -->
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png"
                                    ToolTip="Add New External Link" runat="server" OnClick="ibtnAddNew_Click" />
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
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>

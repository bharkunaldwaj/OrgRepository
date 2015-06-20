<%@ Page Title="Assign Templates" Language="C#" AutoEventWireup="true" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    CodeFile="AssignTemplates.aspx.cs" Inherits="Module_Questionnaire_AssignTemplates" %>

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
                    <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblTopHeading %>"
                        align="absmiddle" />
                    <asp:Label ID="lblTopHeading" runat="server" Text="<% $Resources:lblTopHeading %>"></asp:Label></h3>
            </div>
            <!-- end heading logout -->
            <!-- start user form -->
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
                    <div class="userform">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="3">
                                    <div id="divAccount" runat="server" visible="true">
                                        <fieldset class="fieldsetform">
                                            <legend><asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label> </legend>
                                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                                <tr>
                                                    <td width="13%">
                                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                                            class="style3">*</span>
                                                    </td>
                                                    <td width="20%">
                                                    
                                                    <asp:ListBox ID="ddlAccountCode" runat="server"  SelectionMode="Multiple" Style="width: 155px;height:150px"  >
                                                   
                                                 
                                                    </asp:ListBox>
                                                    
                                                    
                                                        <%--<asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                            OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                        </asp:DropDownList>--%>
                                                        <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Account Code Details "
                                                            SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                    </td>
                                                     <td width="67%" valign="bottom">
                                                    <span class="style3"><asp:Label ID="Label2" runat="server" Text="<% $Resources:lblNote %>"></asp:Label></span>
                                                    </td>
                                                   <%-- <td width="13%" visible="false">
                                                        <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                                    </td>
                                                    <td width="38%">
                                                        <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="49%" valign="top">
                                    <fieldset class="fieldsetform assign-question">
                                        <legend><asp:Label ID="Label3" runat="server" Text="<% $Resources:lblEmail %>"></asp:Label></legend>
                                        <div class="candidatelist">
                                            <table width="100%" border="0" cellpadding="3" cellspacing="1">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="grdvmail" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            GridLines="None">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="85%" HeaderText="" HeaderStyle-HorizontalAlign="left"
                                                                    ItemStyle-Width="85%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lblfmail" runat="server" Text='<%# Bind("Title") %> '></asp:Label>
                                                                        <asp:Label ID="lblfID" runat="server" Visible="false" Text='<%# Bind("EmailTemplateID") %> '></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="50%"></HeaderStyle>
                                                                    <ItemStyle Width="50%"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkmail" runat="server" EnableViewState="false" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="center" Width="15%"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="center" Width="15%"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </td>
                                <td width="1%">
                                    &nbsp;
                                </td>
                                <td width="49%" valign="top">
                                    <fieldset class="fieldsetform assign-question">
                                        <legend><asp:Label ID="Label4" runat="server" Text="<% $Resources:lblProjects %>"></asp:Label></legend>
                                        <div class="candidatelist">
                                            <table width="100%" border="0" cellpadding="3" cellspacing="1">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="grdvProjects" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            GridLines="None">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="85%" HeaderText="" HeaderStyle-HorizontalAlign="left"
                                                                    ItemStyle-Width="85%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lblfdesc" runat="server" Text='<%# Bind("Title") %> '></asp:Label>
                                                                        <asp:Label ID="lblpID" runat="server" Visible="false" Text='<%# Bind("ProjectID") %> '></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="50%"></HeaderStyle>
                                                                    <ItemStyle Width="50%"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkProject" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="center" Width="15%"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="center" Width="15%"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <span class="style3"><asp:Label ID="Label5" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span> 
                        <br> </br>
                        <div align="center">
                            <asp:ImageButton ID="ibtnAssign" ImageUrl="~/Layouts/Resources/images/Save.png" ToolTip="Save" runat="server"
                                OnClick="ibtnAssign_Click" ValidationGroup="group1" />&nbsp;
                            <asp:ImageButton ID="imbReset" ImageUrl="~/Layouts/Resources/images/reset.png" runat="server" ToolTip="Reset"
                                OnClick="imbReset_Click" />
                        </div>
                        <br />
                        <div align="center">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                        <br />
                    </div>
                    <!-- start user form -->
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

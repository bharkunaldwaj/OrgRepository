<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="ViewCandidateStatus.aspx.cs" Inherits="Module_Questionnaire_ViewCandidateStatus" %>

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
                    <img src="../../Layouts/Resources/images/category.png" runat="server" title="<% $Resources:lblHeading %>"
                        align="absmiddle" />
                    <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
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
                    <div id="divAccount" runat="server" visible="false">
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="Label2" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label>
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
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="lblProjectName" runat="server" Text="<% $Resources:lblProject %>"></asp:Label>
                                </td>
                                <td width="25%">
                                    <asp:Label ID="lblParticipant" runat="server" Text=""></asp:Label>
                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="10%">
                                    <asp:Label ID="lblProgrammeName" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label>
                                </td>
                                <td width="25%">
                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="10%">
                                    <asp:Label ID="lblParticipantHeading" runat="server" Text="<% $Resources:lblParticipant %>"></asp:Label>
                                </td>
                                <td width="25%">
                                    <asp:DropDownList ID="ddlParticipant" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="ddlParticipant_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="10%">
                                </td>
                                <td width="10%">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        ToolTip="Reset" OnClick="imbReset_Click" />
                                </td>
                                <td width="10%">
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        ToolTip="Submit" OnClick="imbSubmit_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="grdvCandidateStatus" runat="server" DataSourceID="odsCandidateStatus"
                        AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                        OnSorting="grdvCandidateStatus_Sorting" OnRowDataBound="grdvCandidateStatus_RowDataBound"
                        EmptyDataText="<center><span class='style3'>No Record Found</span></center>"
                        OnRowCommand="grdvCandidateStatus_OnRowCommand">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="FirstName" HeaderText="<% $Resources:lblParticipant %>" SortExpression="FirstName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProgrammeName" HeaderText="<% $Resources:lblProgramme %>" SortExpression="ProgrammeName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:lblColleagueName %>" SortExpression="CandidateName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<% $Resources:lblComplete %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                <ItemTemplate>
                                    <asp:Label ID="lblRelationShip" runat="server" Visible="false" Text='<%# Eval("RelationShip") %>'> </asp:Label>
                                    <asp:Label ID="lblComplete" runat="server" Visible="true" Text=""> </asp:Label>
                                    <asp:Label ID="lblQuestionnaireID" runat="server" Visible="false" Text='<%# Eval("QuestionnaireID") %>'> </asp:Label>
                                    <asp:Label ID="lblCandidateID" runat="server" Visible="false" Text='<%# Eval("AssignmentID") %>'> </asp:Label>
                                    <asp:Label ID="lblSubmitFlag" runat="server" Visible="false" Text='<%# Eval("SubmitFlag") %>'> </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Submit Status" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="rblstSubmitFlag" runat="server" RepeatLayout="Flow" AutoPostBack="true">
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Submit Status" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnStatus" runat="server" Text=""></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:CommandField ShowEditButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" EditText="Yes">
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>--%>
                            <asp:TemplateField HeaderText="<% $Resources:lblSubmitStatus %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="SubmitFlag" Text=<%# (Eval("SubmitFlag").ToString() == "True" ? "Yes" : "No" ) %>
                                        CommandName='<%# Eval("AssignmentID") %>' CommandArgument='<%# Eval("SubmitFlag") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="left" Width="45%" />
                                    <ItemTemplate>
                                        <table width="100%" bgcolor="gray" border="0">
                                            <tr>
                                                <td >
                                                    <table id="tbGraph" style="border:0px solid black;" bgcolor="blue"
                                                        runat="server" border="0" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td bgcolor="blue">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                    <table width="98%" border="0">
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
                    <asp:ObjectDataSource ID="odsCandidateStatus" runat="server" DataObjectTypeName="Questionnaire_BE.AssignQuestionnaire_BE"
                        SelectMethod="GetdtAssignListNew" TypeName="Questionnaire_BAO.AssignQuestionnaire_BAO">
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

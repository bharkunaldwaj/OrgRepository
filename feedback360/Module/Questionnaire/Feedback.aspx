<%@ Page Title="Feedback" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Module_Questionnaire_Feedback" %>

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
                    <img src="../../Layouts/Resources/images/Questionnaire.png" alt="Category" align="absmiddle" />
                    Feedback Questionnaire</h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnCandidateId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnQuestionCount" runat="server"></asp:HiddenField>
                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                Welcome Candidate
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divText" runat="server">
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblQuestionnaireText" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Repeater ID="rptrQuestionList" runat="server" OnItemDataBound="rptrQuestionList_ItemDataBound">
                                    <HeaderTemplate>
                                        <table width="100%" border="0" cellpadding="1" cellspacing="10">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="feedback-heading">
                                                Sequence Number
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "RowNumber")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="feedback-heading">
                                                Category
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "CategoryName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="feedback-heading">
                                                Title
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="feedback-heading">
                                                Description
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Description")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="feedback-heading">
                                                Answer
                                            </td>
                                            <td>
                                                <table border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblLowerLabel" runat="server" Text='<%# Eval("LowerLabel") %>' Font-Bold="true"
                                                                Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAnswer" runat="server" SkinID="txtarea500"></asp:TextBox>
                                                            <asp:RadioButtonList ID="rblAnswer" runat="server" CellSpacing="5" RepeatDirection="Horizontal"
                                                                CellPadding="5">
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            &nbsp;<asp:Label ID="lblUpperLabel" runat="server" Text='<%# Eval("UpperLabel") %>'
                                                                Font-Bold="true" Visible="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label ID="lblLowerBound" runat="server" Text='<%# Eval("LowerBound") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblUpperBound" runat="server" Text='<%# Eval("UpperBound") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblIncrement" runat="server" Text='<%# Eval("Increment") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQType" runat="server" Text='<%# Eval("QuestionTypeID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQId" runat="server" Text='<%# Eval("QuestionID") %>' Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="feedback-heading">
                                                Hint
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Hint")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <div id="divQuestionButton" runat="server">
                                    <table width="100%" border="0">
                                        <tr align="center">
                                            <asp:HiddenField ID="hdnCurrentCount" runat="server" />
                                            <td>
                                                <asp:ImageButton ID="imbPrevious" runat="server" ImageUrl="~/Layouts/Resources/images/previous.png"
                                                    OnClick="imbPrevious_Click" />
                                                &nbsp;
                                                <asp:ImageButton ID="imbNext" ImageUrl="~/Layouts/Resources/images/next.png" runat="server"
                                                    OnClick="imbNext_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divStartEndButton" runat="server">
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="imbStart" ImageUrl="~/Layouts/Resources/images/start.png" runat="server"
                                                    OnClick="imbStart_Click" />
                                                <asp:ImageButton ID="imbFinish" Visible="false" ImageUrl="~/Layouts/Resources/images/finish.png"
                                                    runat="server" OnClick="imbFinish_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

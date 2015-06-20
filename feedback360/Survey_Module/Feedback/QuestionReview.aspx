<%@ Page Language="C#" Title="Question Review" AutoEventWireup="true" CodeFile="QuestionReview.aspx.cs" Inherits="Survey_Module_Feedback_QuestionReview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/style.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/ddmenu.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/assign_questionnaire.png"  runat="server" title="<% $Resources:lblToolTip %>" 
                        align="absmiddle" />
                        <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                    </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    
                    <table border="0" cellspacing="0" cellpadding="5" width="100%">
                        <tr bgcolor="#4169E1">
                            <td width="85%">
                                <b><asp:Label ID="lblQuestionnaire" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label></b>&nbsp;<asp:Label ID="lblQuestionnaireName" runat="server" Text=""></asp:Label>
                            </td>
                            <td width="15%" align="center">
                                <asp:ImageButton ID="ibtnPrint" ImageUrl="~/Layouts/Resources/images/print.png" ToolTip="Print"
                                    runat="server" OnClientClick="window.print();"  />
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    <asp:Repeater ID="rptrQstCategory" runat="server" OnItemDataBound="rptrQstCategory_ItemDataBound">
                            <HeaderTemplate>
                                <table width="100%" border="0" cellspacing="0" cellpadding="5">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr bgcolor="#4169E1">
                                    <td>
                                    <asp:Label ID="lblCategoryID" Visible="false" runat="server" Text=<%# Eval("CategoryID") %> ></asp:Label>                                    
                                    <b><%# Eval("CategoryName") %></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Repeater ID="rptrQuestion" runat="server" >
                                            <HeaderTemplate>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="5">
                                                    <tr bgcolor="#ADD8E6">
                                                    <td width="5%">
                                                        <b><asp:Label ID="lblSequence" runat="server" Text="Sr. No"></asp:Label></b>
                                                        </td>
                                                        <td width="15%">
                                                        <b><asp:Label ID="lblType" runat="server" Text="<% $Resources:lblType %>"></asp:Label></b>
                                                        </td>
                                                        <td width="80%">
                                                        <b><asp:Label ID="lblDescription" runat="server" Text="<% $Resources:lblDescription %>"></asp:Label></b>
                                                        </td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                    <%# Eval("Sequence")%>
                                                    </td>
                                                    <td>
                                                    <%# Eval("Type")%>
                                                    </td>
                                                    <td>
                                                    <%# Eval("Description")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    
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
                    <br />
                    <table width="100%" border="0">
                    </table>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>

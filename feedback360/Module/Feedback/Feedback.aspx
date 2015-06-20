<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Module_Feedback_Feedback"
    ValidateRequest="false" %>
<%@Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Feedback 360</title>
    <!--[if IE 10]>   
     <meta http-equiv="x-ua-compatible" content="IE=9" /> 
     <![endif]-->
     <!--[if IE 11]>   
     <meta http-equiv="x-ua-compatible" content="IE=9" /> 
     <![endif]-->
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/reset.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/style.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/ddmenu.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/Calendar_360.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/Calendar.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/jquery-ui-1.7.2.custom.css" />

    <script src='<%= ResolveClientUrl("../../Layouts/Resources/js/common.js")%>' type="text/javascript"></script>

    <script type="text/javascript" src='<%= ResolveClientUrl("../../Layouts/Resources/js/GeneralFunctions.js") %>'></script>

    <script language="javascript" type="text/javascript">

        var TextAreaMaxLengthCheck = function(id, length) {
            length = length - 1;
            var val = document.getElementById(id).value;
             
            if (val.length <= length)
                return true;
            else {
                event.keyCode = 0;
            }
        }
        
    </script>

    <style type="text/css">
        .graphtext
        {
            color: white; /*#025273;*/
            text-align: center;
            font: bold 10px verdana;
            height: 20px;
        }
        
        /*span .cke_bottom{display:none;}
        span .cke_top{display:none;}*/
        span.cke_top.cke_reset_all{display:none;}
        span.cke_bottom.cke_reset_all{display:none;}
        /*.cke_top{display:none;}
        .cke_bottom {display:none;}*/
    </style>
</head>
<body>
    <form id="frmFeedback" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table id="maincontainer-feedback" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td width="10%" align="left">
                <asp:Image ID="imgHeader" Height="70" runat="server" />
            </td>
            <td id="tdHeader" runat="server" width="90%" align="right">
                <asp:Image ID="imgProjectLogo" Height="70" runat="server" />
            </td>
        </tr>
        <tr>
            <td id="tdMenuBar" colspan="2" runat="server" style="float: none !important">
                <b>Welcome
                    <asp:Label ID="lblUserName" runat="server" Text="<% $Resources:lblUserName %>"></asp:Label>
                </b>
            </td>
        </tr>
    </table>
    <div id="maincontainer-feedback">
        <div class="innercontainer">
            <div class="topheadingdetails-feedback">
                <h3>
                    <img src="../../Layouts/Resources/images/Questionnaire.png" runat="server" title="<% $Resources:lblToolTip %>"
                        align="absmiddle" />
                    <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                </h3>
                <div class="clear">
                </div>
            </div>
<%--            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                <div class="top-head">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="42%">
                                                <asp:Label ID="lblProjectName" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td width="51%">
                                                <asp:Label ID="lblParticipantName" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td width="7%">
                                                <asp:ImageButton ID="ibtnHelp" runat="server" title="Help" OnClientClick="ShowPopup();"
                                                    ImageUrl="~/Layouts/Resources/images/help.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="progress">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td class="number">
                                                <asp:Label ID="lblCompletionStatus" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="progress-bar">
                                                    <tr>
                                                        <td width="50%" class="pad">
                                                            <table id="tbGraph" class="progress-status" runat="server" border="0" cellpadding="1"
                                                                cellspacing="0">
                                                                <tr>
                                                                    <td class="bg">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divText" runat="server">
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblQuestionnaireText" runat="server" Text=""></asp:Label>
                                                <br />
                                                <asp:Label ID="cbxNotifyMail" runat="server" class="style3" Text='<% $Resources:cbxNotifyMail %>'></asp:Label>
                                                <%--<asp:CheckBox ID="cbxNotifyMail" runat="server" Visible="false" class="style3" Text="Tick the box to let your colleague know you have completed the questionnaire" />--%>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Repeater ID="rptrQuestionListMain" runat="server" OnItemDataBound="rptrQuestionListMain_ItemDataBound">
                                    <ItemTemplate>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="title-head">
                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID") %>' Visible="false"></asp:Label>
                                                    <%--<asp:Label ID="lblCategoryName" runat="server" Text=""></asp:Label>--%>
                                                    <asp:Label ID="lblCategoryTitle" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Repeater ID="rptrQuestionList" runat="server" OnItemDataBound="rptrQuestionList_ItemDataBound"
                                                        OnItemCommand="rptrQuestionList_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="5" class="feed-grid">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                            <%--width="20"--%>
                                                                <td valign="middle" width="2%">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Sequence") %>'></asp:Label>
                                                                    <%--<%# DataBinder.Eval(Container.DataItem, "Description")%>--%>
                                                                </td>
                                                                <%--width="400"--%>
                                                                <td valign="middle" width="40%" title="<%# DataBinder.Eval(Container.DataItem, "Hint")%>">
                                                                    <asp:Label ID="lblQuestionText" runat="server" Text=""></asp:Label>
                                                                    <%--<%# DataBinder.Eval(Container.DataItem, "Description")%>--%>
                                                                </td>
                                                                <td valign="middle">
                                                                <%--width="640"--%>
                                                                    <table border="0" width="64%" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <asp:Label ID="lblLowerLabel" runat="server" Text='<%# Eval("LowerLabel") %>' Font-Bold="true"
                                                                                    Visible="true"></asp:Label>
                                                                            </td>
                                                                            <td class="number-label">
                                                                                <asp:RadioButtonList ID="rblAnswer" RepeatLayout="Table" runat="server" CellSpacing="0"
                                                                                    RepeatDirection="Horizontal" Width="100%" CellPadding="0">
                                                                                </asp:RadioButtonList>
                                                                                <%--<asp:TextBox ID="txtAnswer" runat="server" SkinID="txtarea500" Rows="2" onkeypress='TextAreaMaxLengthCheck(this.id,<%# Eval("LengthMAX") %>)'></asp:TextBox>--%>
                                                                                <CKEditor:CKEditorControl ID="txtAnswers" 
                                                                                    Width="300px" Height="100px" BasePath="~/ckeditorNew/" 
                                                                                    AutoCompleteType="None"
                                                                                    AutoParagraph="false" 
                                                                                    ScaytAutoStartup="true" 
                                                                                    BrowserContextMenuOnCtrl="true" 
                                                                                    ForcePasteAsPlainText="false"
                                                                                    IgnoreEmptyParagraph="true" 
                                                                                    ContentsLangDirection="Ltr" 
                                                                                    EnableTabKeyTools="false"
                                                                                    EnterMode="BR"
                                                                                    Entities="false"
                                                                                    PasteFromWordNumberedHeadingToList="false"
                                                                                    PasteFromWordRemoveStyles="true"
                                                                                    onkeypress='TextAreaMaxLengthCheck(this.id,<%# Eval("LengthMAX") %>)' runat="server"></CKEditor:CKEditorControl>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;<asp:Label ID="lblUpperLabel" runat="server" Text='<%# Eval("UpperLabel") %>'
                                                                                    Font-Bold="true" Visible="true"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;<asp:RadioButton ID="rbtnNotApplicable" Text="<% $Resources:rbtnNotApplicable %>"
                                                                                    runat="server" AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <table id="tblGraph" runat="server" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td width="5">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 98px; *width: 102px;" bgcolor="#229C6F" class="graphtext">
                                                                                            Foundation
                                                                                        </td>
                                                                                        <td style="width: 102px; *width: 107px;" bgcolor="#3167FF" class="graphtext">
                                                                                            Developing
                                                                                        </td>
                                                                                        <td style="width: 100px; *width: 105px;" bgcolor="#FF9C00" class="graphtext">
                                                                                            Advanced
                                                                                        </td>
                                                                                        <td style="width: 125px; *width: 122px;" bgcolor="#FF0000" class="graphtext">
                                                                                            Expert
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%-- 
                                                                    bgcolor="#229C6F"  bgcolor="#3167FF"   bgcolor="#FF9C00"   bgcolor="#FF0000"
                                                                       --%>
                                                                    <asp:Label ID="lblValidation" runat="server" Text='<%# Eval("Validation") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLowerBound" runat="server" Text='<%# Eval("LowerBound") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblUpperBound" runat="server" Text='<%# Eval("UpperBound") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblIncrement" runat="server" Text='<%# Eval("Increment") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQType" runat="server" Text='<%# Eval("QuestionTypeID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQId" runat="server" Text='<%# Eval("QuestionID") %>' Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblHint" runat="server" Text=""></asp:Label>
                                                                    <%--<%# DataBinder.Eval(Container.DataItem, "Hint")%>--%>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr class="alternate">
                                                                <td valign="middle" width="2%">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Sequence") %>'></asp:Label>
                                                                    <%--<%# DataBinder.Eval(Container.DataItem, "Description")%>--%>
                                                                </td>
                                                                <td valign="middle" width="40%" title="<%# DataBinder.Eval(Container.DataItem, "Hint")%>">
                                                                    <asp:Label ID="lblQuestionText" runat="server" Text=""></asp:Label>
                                                                    <%--<%# DataBinder.Eval(Container.DataItem, "Description")%>--%>
                                                                </td>
                                                                <td valign="middle">
                                                                    <table border="0" width="64%" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <asp:Label ID="lblLowerLabel" runat="server" Text='<%# Eval("LowerLabel") %>' Font-Bold="true"
                                                                                    Visible="true"></asp:Label>
                                                                            </td>
                                                                            <td class="number-label">
                                                                                <asp:RadioButtonList ID="rblAnswer" RepeatLayout="Table" runat="server" CellSpacing="0"
                                                                                    RepeatDirection="Horizontal" Width="100%" CellPadding="0">
                                                                                </asp:RadioButtonList>
                                                                                <%--<asp:TextBox ID="txtAnswer" runat="server" SkinID="txtarea500" Rows="2" onkeypress='TextAreaMaxLengthCheck(this.id,<%# Eval("LengthMAX") %>)'></asp:TextBox>--%>
                                                                                <CKEditor:CKEditorControl ID="txtAnswers" 
                                                                                    Width="300px" Height="100px" BasePath="~/ckeditorNew/" 
                                                                                    AutoCompleteType="None"
                                                                                    AutoParagraph="false" 
                                                                                    ScaytAutoStartup="true" 
                                                                                    BrowserContextMenuOnCtrl="true" 
                                                                                    ForcePasteAsPlainText="false"
                                                                                    IgnoreEmptyParagraph="true" 
                                                                                    ContentsLangDirection="Ltr" 
                                                                                    EnableTabKeyTools="false"
                                                                                    EnterMode="BR"
                                                                                    Entities="false"
                                                                                    PasteFromWordNumberedHeadingToList="false"
                                                                                    PasteFromWordRemoveStyles="true"
                                                                                    onkeypress='TextAreaMaxLengthCheck(this.id,<%# Eval("LengthMAX") %>)' runat="server"></CKEditor:CKEditorControl>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;<asp:Label ID="lblUpperLabel" runat="server" Text='<%# Eval("UpperLabel") %>'
                                                                                    Font-Bold="true" Visible="true"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;<asp:RadioButton ID="rbtnNotApplicable" Text="<% $Resources:rbtnNotApplicable %>"
                                                                                    runat="server" AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <table id="tblGraph" runat="server" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td width="5">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 98px; width: 102px;" bgcolor="#229C6F" class="graphtext">
                                                                                            Foundation
                                                                                        </td>
                                                                                        <td style="width: 102px; width: 107px;" bgcolor="#3167FF" class="graphtext">
                                                                                            Developing
                                                                                        </td>
                                                                                        <td style="width: 100px; width: 105px;" bgcolor="#FF9C00" class="graphtext">
                                                                                            Advanced
                                                                                        </td>
                                                                                        <td style="width: 125px; width: 122px;" bgcolor="#FF0000" class="graphtext">
                                                                                            Expert
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%-- 
                                                                    bgcolor="#229C6F"  bgcolor="#3167FF"   bgcolor="#FF9C00"   bgcolor="#FF0000"
                                                                       --%>
                                                                    <asp:Label ID="lblValidation" runat="server" Text='<%# Eval("Validation") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLowerBound" runat="server" Text='<%# Eval("LowerBound") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblUpperBound" runat="server" Text='<%# Eval("UpperBound") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblIncrement" runat="server" Text='<%# Eval("Increment") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQType" runat="server" Text='<%# Eval("QuestionTypeID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQId" runat="server" Text='<%# Eval("QuestionID") %>' Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblHint" runat="server" Text=""></asp:Label>
                                                                    <%--<%# DataBinder.Eval(Container.DataItem, "Hint")%>--%>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <table width="100%" border="0" cellpadding="0" cellspacing="5">
                                            <tr>
                                                <td>
                                                
                                                    <%--<asp:Label ID="lblNote" runat="server" Text="<% $Resources:lblNote %>" ForeColor="Red"></asp:Label>--%>
                                                    <asp:Label ID="lblNote" runat="server" Text="Note:&lt;br&gt;1. Select or enter N/A for No Answer" ForeColor="Red"></asp:Label>
                                                    <asp:Label ID="lblNote2" runat="server" Text="2. Place mouse over a question for a hint." ForeColor="Red" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <div id="divQuestionButton" runat="server">
                                    <table width="100%" border="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div align="center">
                                                  <%--  <asp:ImageButton ID="imbStart" title="Start" Visible="false" ImageUrl="~/Layouts/Resources/images/start.png"
                                                        runat="server" OnClick="imbStart_Click" />&nbsp;
                                                    <asp:ImageButton ID="imbPrevious" Visible="false" title="Previous" runat="server"
                                                        ImageUrl="~/Layouts/Resources/images/previous.png" OnClick="imbPrevious_Click" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="imbNext" title="Next" Visible="false" ImageUrl="~/Layouts/Resources/images/next.png"
                                                        runat="server" OnClick="imbNext_Click" />&nbsp;
                                                    <asp:ImageButton ID="imbFinish" Visible="false" title="finish" ImageUrl="~/Layouts/Resources/images/finish.png"
                                                        runat="server" OnClick="imbFinish_Click" />--%>

                                                    <asp:LinkButton ID="imbStart" title="Start" Visible="false" 
                                                        runat="server" OnClick="imbStart_Click" ><img src="~/Layouts/Resources/images/start.png" runat="server" /></asp:LinkButton>
                                                    <asp:LinkButton ID="imbPrevious" Visible="false" title="Previous" runat="server"
                                                        ImageUrl="~/Layouts/Resources/images/previous.png" OnClick="imbPrevious_Click"><img src="~/Layouts/Resources/images/previous.png" runat="server" /></asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="imbNext" title="Next" Visible="false" ImageUrl="~/Layouts/Resources/images/next.png"
                                                        runat="server" OnClick="imbNext_Click"><img  src="~/Layouts/Resources/images/next.png" runat="server" /></asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="imbFinish" Visible="false" title="finish" ImageUrl="~/Layouts/Resources/images/finish.png"
                                                        runat="server" OnClick="imbFinish_Click" ><img id="Img1"  src="~/Layouts/Resources/images/finish.png" runat="server" /></asp:LinkButton>

                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div align="center">
                                                    <asp:Label ID="lblMessage" CssClass="style3" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--<div id="divStartEndButton" runat="server">
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnValidationCheck" runat="server" Value=""></asp:HiddenField>
               <%-- </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <asp:HiddenField ID="hdnIncrementValue" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnCandidateId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnClientName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnFirstName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnLastName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnQuestionCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnQuestionnaireId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnProjectId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnCurrentCount" runat="server" />
        <asp:HiddenField ID="hdnAccountId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnRelationship" runat="server"></asp:HiddenField>
        <div class="clear">
        </div>
        <div id="divMessage">
            <asp:Label ID="lblUnAuthorisedMessage" CssClass="style3" runat="server" Text=""></asp:Label>
        </div>
        <div id="footer">
            <asp:Label ID="lblFooter" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <script type="text/javascript">

        function ShowPopup() {
            var path = "ProjectFAQ.aspx?ProjectId=" + document.getElementById('hdnProjectId').value;

            window.open(path, '', 'left=100,top=100,height=475,width=1000');
        }
    
    </script>

    </form>
</body>
</html>

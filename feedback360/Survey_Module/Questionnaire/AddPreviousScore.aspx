<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    Culture="en-GB" UICulture="en-US" ValidateRequest="false" AutoEventWireup="true"
    CodeFile="AddPreviousScore.aspx.cs" Inherits="Survey_Module_Questionnaire_AddPreviousScore" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <script type="text/javascript" language="javascript">

    /*
        function ClearFileUpload() {

            var fil = document.getElementById("FileUpload");

            fil.select();

            n = fil.createTextRange();

            n.execCommand('delete');

            fil.focus();

        }


        function pageLoad() {

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtStartDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, minDate: '0d', defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
            });


            $(document).ready(function () {
                $("#ctl00_cphMaster_dtEndDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, minDate: '0d', defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtRemainderDate1").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtRemainderDate2").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtRemainderDate3").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtAvailableFrom").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtAvailableTo").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtPartReminder1").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function () {
                $("#ctl00_cphMaster_dtPartReminder2").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });
        }
    */

        function RemoveImage() {
            document.getElementById('ctl00_cphMaster_hdnRemoveImage').value = "";
        }

    </script>
    <style type="text/css">
        .gallerycontainer
        {
            position: relative; /*Add a height attribute and set to largest image's height to prevent overlaying*/
        }
        .thumbnail img
        {
            border: 1px solid white;
            margin: 0 5px 5px 0;
        }
        .thumbnail:hover
        {
            background-color: transparent;
        }
        .thumbnail:hover img
        {
            border: 1px solid blue;
        }
        .thumbnail span
        {
            /*CSS for enlarged image*/
            position: absolute;
            background-color: lightyellow;
            padding: 5px;
            left: -1000px;
            border: 1px dashed gray;
            visibility: hidden;
            color: black;
            text-decoration: none;
        }
        .thumbnail span img
        {
            /*CSS for enlarged image*/
            border-width: 0;
            padding: 2px;
        }
        .thumbnail:hover span
        {
            /*CSS for enlarged image*/
            visibility: visible;
            top: 0;
            left: 2px; /*position where enlarged image should offset horizontally */
            z-index: 50;
        }
    </style>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <div class="Survey_topheadingdetails">
                <h3>
                    <img id="Img1" src="../../Layouts/Resources/images/project_create.png" runat="server"
                        alt="<% $Resources:lblheader %>" align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
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
            <asp:HiddenField ID="hdnPassword" runat="server" />
            <asp:HiddenField ID="hdnimage" runat="server" />
            <asp:HiddenField ID="hdnReminder2" runat="server" />
            <div class="userform">
                <div id="divAccount" runat="server" visible="true">
                    <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                        class="style3">*</span>
                                </td>
                                <td width="35%">
                                    <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %> "
                                        SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="15%">
                                    <%--<asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>--%>
                                </td>
                                <td width="35%">
                                    <%--<asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>--%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <%--<asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label5" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label>
                            </legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%">
                                        <%-- <asp:Label ID="lblName" runat="server" Text="<% $Resources:lblName %>"></asp:Label><span
                                            class="style3">*</span>--%>
                                        <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <%--  <asp:TextBox ID="txtName" MaxLength="25" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq2" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq2 %> "
                                            SetFocusOnError="True" ControlToValidate="txtName">&nbsp;</asp:RequiredFieldValidator>--%>
                                        <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq1 %> "
                                            SetFocusOnError="True" ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlCompany" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rqCompany %> " SetFocusOnError="True" ControlToValidate="ddlCompany"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblProgrammeName" runat="server" Text="<% $Resources:lblProgrammeName %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlProgrammeName" runat="server" AppendDataBoundItems="true"
                                            OnSelectedIndexChanged="ddlProgrammeName_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProgrammeName" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rfvProgrammeName %> " SetFocusOnError="True" ControlToValidate="ddlProgrammeName"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblClientName" runat="server" Text="<% $Resources:lblClientName %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtClientName0" runat="server" MaxLength="50" 
                                            AutoPostBack="True" ontextchanged="txtClientName0_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rfvClientName0 %> " SetFocusOnError="True" ControlToValidate="txtClientName0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblScore1Title" runat="server" Text="<% $Resources:lblScore1Title %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtScore1Title" runat="server" MaxLength="50" 
                                            AutoPostBack="True" ontextchanged="txtScore1Title_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvScore1Title" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rfvScore1Title %> " SetFocusOnError="True" ControlToValidate="txtScore1Title">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblScore2Title" runat="server" Text="<% $Resources:lblScore2Title %>"></asp:Label><span
                                           <%-- class="style3">*</span>--%>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtScore2Title" runat="server" MaxLength="50" 
                                            AutoPostBack="True" ontextchanged="txtScore2Title_TextChanged"></asp:TextBox>
                                     <%--   <asp:RequiredFieldValidator ID="rfvScore2Title" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rfvScore2Title %> " SetFocusOnError="True" ControlToValidate="txtScore2Title">&nbsp;</asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="4">
                                        <asp:ImageButton ID="imbFindOld" runat="server" ImageUrl="~/Layouts/Resources/images/save.png"
                                            OnClick="imbFindOld_Click" ToolTip="Save" ValidationGroup="group1" Visible=false />
                                    </td>
                                </tr>
                                <tr>
                                <!-- 1.0.0.1.2 -->
                                    <td valign="top" colspan="3">
                                        <asp:Label ID="lblFileUpdload" Text="Upload Excel File for Previous Score" runat="server"></asp:Label>
                                        <asp:CheckBox ID="chkUploadFile" runat="server" Text="" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:FileUpload ID="fleUploadScoreExcel" runat="server" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:ImageButton ID="imgBtnUploadScoreExcel" runat="server" ImageUrl="~/Layouts/Resources/images/import-s.png" 
                                            CausesValidation="true" ValidationGroup="group1"
                                            OnClick="imgBtnUploadScoreExcel_Click" OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';"/>
                                    </td>
                                <!-- 1.0.0.1.2 -->
                                </tr>
                                <tr>
                                    <td valign="top" colspan="4" align=center>
                                        <a href="../../SampleDocs/PreviousScore.xlsx">Click here to download the sample excel to add previous score</a>
                                  </td>
                                   
                                </tr>
                            </table>
                        </fieldset>
                   <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="analysis_Panel">
                            <fieldset id="analysis_fieldset" class="fieldsetform">
                                <legend>
                                    <asp:Label ID="Label7" runat="server" Text="<% $Resources:lblAnalysisSheet %>"></asp:Label></legend>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Label ID="Label9" runat="server" Text="ANALYSIS- I " BorderStyle="None" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                ForeColor="#FF6600"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin: 0 auto; text-align: center" width="63%" border="0" cellspacing="0"
                                    cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="Repeater0" runat="server" EnableTheming="true" OnItemDataBound="Repeater0_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table width="100%" style="text-align: center" border="0" cellspacing="0" cellpadding="0"
                                                        class="grid">
                                                        <tr>
                                                            <th width="5%" align="center">
                                                                <asp:Label Style="text-align: center" ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                            </th>
                                                            <th width="95%" align="left">
                                                                <asp:Label Style="text-align: center" ID="score_ratings" runat="server" Text="<% $Resources:lblCategory %>"></asp:Label>
                                                            </th>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%" border="1" cellspacing="0" cellpadding="0" class="grid">
                                                        <tr style="width: 100%">
                                                            <td width="5%" align="center">
                                                                <%# Container.ItemIndex + 1  %>.
                                                            </td>
                                                            <td width="95%" align="left">
                                                                <span style="display: none"><%# Eval("CategoryID")%>
                                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>'></asp:Label></span>
                                                                <asp:Label Style="width: 97%" ID="txt_category" Text='<%# Eval("Category_Detail") %>'
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td align="left">
                                                            </td>
                                                            <td>
                                                                <asp:Repeater ID="rptrQuestionPreviousScoresAn1" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                            <tr>
                                                                                <th width="5%">
                                                                                    <asp:Label ID="lblQuestionListSrNo" runat="server" Text="<% $Resources:lblQuestionListSrNo %>"></asp:Label>
                                                                                </th>
                                                                                <th width="35%">
                                                                                    <asp:Label ID="lblQuestionList" runat="server" Text="<% $Resources:lblQuestionList %>"></asp:Label>
                                                                                </th>
                                                                                <th width="30%" align="center">
                                                                                    <asp:Label ID="lblQuestionListPrevScore1" runat="server" Text="<% $Resources:lblQuestionListPrevScore %>"></asp:Label>
                                                                                </th>
                                                                                <th width="30%" align="center">
                                                                                    <asp:Label ID="lblQuestionListPrevScore2" runat="server" Text="<% $Resources:lblQuestionListPrevScore2 %>"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%# Container.ItemIndex + 1  %>.
                                                                            </td>
                                                                            <td>
                                                                                <div style="display: none">
                                                                                    <asp:Label ID="lblQuestionnaireID" runat="server" Text='<%# Eval("QuestionID")%>'></asp:Label></div>
                                                                                <%# Eval("Description")%>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtScore1" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                    Text='<%# Eval("Score1")%>'></asp:TextBox>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtScore2" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                    Text='<%# Eval("Score2")%>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblErrorMsg" ForeColor="Red" runat="server" Text="*Please enter any number other than zero"
                                                        Visible="false">
                                                    </asp:Label>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Label ID="Label10" runat="server" Text="ANALYSIS- II " BorderStyle="None" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                ForeColor="#FF6600"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin: 0 auto; text-align: center" width="63%" border="0" cellspacing="0"
                                    cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="Repeater1" runat="server" EnableTheming="true" Visible="true" OnItemDataBound="Repeater1_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                        <tr>
                                                            <th width="5%">
                                                                <asp:Label Style="text-align: center" ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                            </th>
                                                            <th width="95%">
                                                                <asp:Label ID="score_ratings" runat="server" Text="<% $Resources:lblCategory %>"></asp:Label>
                                                            </th>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                        <tr style="width: 100%">
                                                            <td width="5%" align="center">
                                                                <%# Container.ItemIndex + 1  %>.
                                                            </td>
                                                            <td width="95%" align="left">
                                                                <span style="display: none">
                                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>'></asp:Label></span>
                                                                <asp:Label Style="width: 97%" ID="txt_category" Text='<%# Eval("Category_Detail") %>'
                                                                    runat="server"></asp:Label>
                                                                <%--<asp:RequiredFieldValidator ID="txt_category_validator1" runat="server" ValidationGroup="group1" ErrorMessage="Can not keep the table-entry empty."
                                            SetFocusOnError="True" ControlToValidate="txt_category">&nbsp;</asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td align="left">
                                                            </td>
                                                            <td>
                                                                <asp:Repeater ID="rptrQuestionPreviousScoresAn2" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                            <tr>
                                                                                <th width="5%">
                                                                                    <asp:Label ID="lblQuestionListSrNo" runat="server" Text="<% $Resources:lblQuestionListSrNo %>"></asp:Label>
                                                                                </th>
                                                                                <th width="35%">
                                                                                    <asp:Label ID="lblQuestionList" runat="server" Text="<% $Resources:lblQuestionList %>"></asp:Label>
                                                                                </th>
                                                                                <th width="30%" align="center">
                                                                                    <asp:Label ID="lblQuestionListPrevScore1" runat="server" Text="<% $Resources:lblQuestionListPrevScore %>"></asp:Label>
                                                                                </th>
                                                                                <th width="30%" align="center">
                                                                                    <asp:Label ID="lblQuestionListPrevScore2" runat="server" Text="<% $Resources:lblQuestionListPrevScore2 %>"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%# Container.ItemIndex + 1  %>.
                                                                            </td>
                                                                            <td>
                                                                                 <div style="display: none">
                                                                                    <asp:Label ID="lblQuestionnaireID" runat="server" Text='<%# Eval("QuestionID")%>'></asp:Label></div>
                                                                                <%# Eval("Description")%>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtScore1" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                    Text='<%# Eval("Score1")%>'></asp:TextBox>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtScore2" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                    Text='<%# Eval("Score2")%>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblErrorMsg1" ForeColor="Red" runat="server" Text="*Please enter any number other than zero"
                                                        Visible="false"></asp:Label>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Label ID="Label12" runat="server" Text="ANALYSIS- III " BorderStyle="None" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                ForeColor="#FF6600"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin: 0 auto; text-align: center" width="63%" border="0" cellspacing="0"
                                    cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="Repeater2" runat="server" EnableTheming="true" Visible="true" OnItemDataBound="Repeater2_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                        <tr>
                                                            <th width="5%">
                                                                <asp:Label Style="text-align: center" ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                            </th>
                                                            <th width="95%">
                                                                <asp:Label ID="score_ratings" runat="server" Text="<% $Resources:lblCategory %>"></asp:Label>
                                                            </th>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                        <tr style="width: 100%">
                                                            <td width="5%" align="center">
                                                                <%# Container.ItemIndex + 1  %>.
                                                            </td>
                                                            <td width="95%" align="left">
                                                                <span style="display: none">
                                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>'></asp:Label></span>
                                                                <asp:Label Style="width: 97%" ID="txt_category" Text='<%# Eval("Category_Detail") %>'
                                                                    runat="server"></asp:Label>
                                                                <%--<asp:RequiredFieldValidator ID="txt_category_validator2" runat="server" ValidationGroup="group1" ErrorMessage="Can not keep the table-entry empty."
                                            SetFocusOnError="True" ControlToValidate="txt_category">&nbsp;</asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td align="left">
                                                            </td>
                                                            <td>
                                                                <asp:Repeater ID="rptrQuestionPreviousScoresAn3" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                            <tr>
                                                                                <th width="5%">
                                                                                    <asp:Label ID="lblQuestionListSrNo" runat="server" Text="<% $Resources:lblQuestionListSrNo %>"></asp:Label>
                                                                                </th>
                                                                                <th width="35%">
                                                                                    <asp:Label ID="lblQuestionList" runat="server" Text="<% $Resources:lblQuestionList %>"></asp:Label>
                                                                                </th>
                                                                                <th width="30%" align="center">
                                                                                    <asp:Label ID="lblQuestionListPrevScore1" runat="server" Text="<% $Resources:lblQuestionListPrevScore %>"></asp:Label>
                                                                                </th>
                                                                                <th width="30%" align="center">
                                                                                    <asp:Label ID="lblQuestionListPrevScore2" runat="server" Text="<% $Resources:lblQuestionListPrevScore2 %>"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%# Container.ItemIndex + 1  %>.
                                                                            </td>
                                                                            <td>
                                                                                 <div style="display: none">
                                                                                    <asp:Label ID="lblQuestionnaireID" runat="server" Text='<%# Eval("QuestionID")%>'></asp:Label></div>
                                                                                <%# Eval("Description")%>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtScore1" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                    Text='<%# Eval("Score1")%>'></asp:TextBox>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtScore2" runat="server" SkinID="ph3" MaxLength="5" onkeypress="return DecimalOnly(this);"
                                                                                    Text='<%# Eval("Score2")%>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblErrorMsg2" ForeColor="Red" runat="server" Text="*Please enter any number other than zero"
                                                        Visible="false"></asp:Label>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                        <span class="style3">
                            <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>


                        <br>
                         <div>
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td style="width: 39%">
                                    </td>
                                    <td style="width: 61%" align="left">
                                        <asp:Label ID="lbl_save_message" runat="server" Text="" ForeColor="Red" Visible="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div align="center">
                            <asp:ImageButton ID="imbSave" runat="server" ImageUrl="~/Layouts/Resources/images/save.png"
                                OnClick="imbSave_Click" ToolTip="Save" ValidationGroup="group1" />
                            &nbsp;
                            <asp:ImageButton ID="imbcancel" runat="server" ImageUrl="~/Layouts/Resources/images/cancel.png"
                                OnClick="imbcancel_Click" ToolTip="Back to List" />
                            &nbsp;
                            <asp:ImageButton ID="imbBack" runat="server" CausesValidation="true" ImageUrl="~/Layouts/Resources/images/Back.png"
                                OnClick="imbBack_Click" ToolTip="Back to List" Visible="false" />
                        </div>
                       
                        </div>
                        <!-- start user form -->
                        </div> </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="imbSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <script type="text/javascript">

//                if (document.getElementById('ctl00_cphMaster_hdnimage').value != "") {
//                    document.getElementById('imagelogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
//                }
//                else {
//                    document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
//                }

            </script>
        </div>
    </div>
</asp:Content>

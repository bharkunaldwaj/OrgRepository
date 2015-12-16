<%@ Page Title="Create New Company" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    ValidateRequest="false" AutoEventWireup="true" CodeFile="Company.aspx.cs" Culture="en-GB"
    UICulture="en-US" Inherits="Survey_Module_Questionnaire_Company" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      <script type="text/javascript" src="../../ckeditorn/ckeditor.js"></script>
     <script src="../../Layouts/Resources/js/Common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Layouts/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript" src="../../Layouts/Resources/js/tinymice.js"></script>
    <script type="text/javascript">
        //        Editor();
    </script>
    <script type="text/javascript">

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

        }

        function RemoveQuestImage() {
            document.getElementById('ctl00_cphMaster_qstFileUpload').value = "";
            document.getElementById('ctl00_cphMaster_hdnQuestimage').value = "";
        }
        function RemoveReportImage() {
            document.getElementById('ctl00_cphMaster_reportFileUpload').value = "";
            document.getElementById('ctl00_cphMaster_hdnReportimage').value = "";
        }

    </script>
    <%--<script type="text/javascript" language="javascript">

        function checkInput() {
            var tb1 = document.getElementById('<%= Finish_emailID_Txtbox.ClientID %>');
            var tb2 = document.getElementById('<%= Finish_emailID_Txtbox.ClientID %>');
            if (tb1.value == "" || tb2.value == "") {
                alert('');
                return false;

            }
            else
            { return true; }
        }

    </script>--%>
    <style type="text/css">
        /*Credits: Dynamic Drive CSS Library */
        /*URL: http://www.dynamicdrive.com/style/ */
        
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
            background-color: yellow;
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
   <%-- <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="Survey_topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/project_create.png" runat="server" alt="<% $Resources:lblToolTip %>"
                                align="absmiddle" />
                            <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label></h3>
                        <div class="clear">
                        </div>
                    </div>
                    <!-- end heading logout -->
                    <!-- start user form -->
                    <table border="0" width="100%">
                        <tr>
                            <td style="width: 35%">
                            </td>
                            <td>
                                <div id="summary" runat="server">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="group1" />
                                </div>
                            </td>
                            <td style="width: 30%">
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnPassword" runat="server" />
                    <asp:HiddenField ID="hdnReportimage" runat="server" />
                    <asp:HiddenField ID="hdnQuestimage" runat="server" />
                    <div class="userform">
                        <div id="divAccount" runat="server" visible="true">
                            <fieldset class="fieldsetform">
                                <legend>
                                    <asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>"
                                                SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:TextBox ID="txtcompanyname" runat="server" Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblGeneralDetail" runat="server" Text="<% $Resources:lblGeneralDetail %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:RequiredFieldValidator3 %>" SetFocusOnError="True"
                                            ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="15%">
                                    </td>
                                    <td width="35%">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTitle" runat="server" Text="<% $Resources:lblTitle %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTitle" MaxLength="25" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq3" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq3 %>"
                                            SetFocusOnError="True" ControlToValidate="txtTitle">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblProjectManager" runat="server" Text="<% $Resources:lblProjectManager %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProjectManager" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq4" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq4 %>"
                                            SetFocusOnError="True" ControlToValidate="ddlProjectManager" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" rowspan="2">
                                        <asp:Label ID="lblDesc" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label>
                                    </td>
                                    <td valign="top" rowspan="2">
                                        <asp:TextBox ID="txtDescription" runat="server" SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" /><div
                                            class="maxlength-msg">
                                            <asp:Label ID="lblCharactersLimit" runat="server" Text="<% $Resources:lblCharactersLimit %>"></asp:Label></div>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblStatus" runat="server" Text="<% $Resources:lblStatus %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Style="width: 155px">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="2">Inactive</asp:ListItem>
                                            <asp:ListItem Value="3">Suspended</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq1 %>"
                                            SetFocusOnError="True" ControlToValidate="ddlStatus" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Finish_emailID_Txtbox"
                                            Display="None" EnableClientScript="true" ErrorMessage="               Please enter the email-ID."
                                            SetFocusOnError="True" ValidationGroup="group1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ControlToValidate="Finish_emailID_Txtbox" Display="None" EnableClientScript="true"
                                            ErrorMessage="Incorrect Email format in 'Finish Email-ID'(Correct Example: YourId@somedomain.com)"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="group1"></asp:RegularExpressionValidator>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        Finish Email-ID <span class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="Finish_emailID_Txtbox" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                        Send Finish E-mail
                                    </td>
                                    <td valign="top">
                                        <asp:CheckBox ID="Finish_Email_Chkbox" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                
                                <td>Finish Auto E-mail Template </td><td><asp:DropDownList ID="ddlEmailTemplate" runat="server" AppendDataBoundItems="true"
                                            Width="200px">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="group1"
                                            ErrorMessage="Please Select Email Template" SetFocusOnError="True" ControlToValidate="ddlEmailTemplate"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator></td><td colspan=2></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblQuestLogo" runat="server" Text="<% $Resources:lblQuestLogo %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                            <tr>
                                                <td width="70%" valign="top">
                                                    <asp:FileUpload ID="qstFileUpload" runat="Server" />
                                                </td>
                                                <td width="20%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp;
                                                        <a class="thumbnail" href="#thumb">Preview<span><img id="imgQuestlogo" src="" /><br />
                                                            Image</span></a>
                                                    </div>
                                                </td>
                                                <td width="10%" valign="top">
                                                    <asp:HiddenField ID="hdnRemoveQuestImage" runat="server" Value="0" />
                                                    <a href="javascript:void(0)" onclick="RemoveQuestImage();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <asp:Label ID="Label3" runat="server" Text="<% $Resources:lblRecSize %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblReportLogo" runat="server" Text="<% $Resources:lblReportLogo %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                            <tr>
                                                <td width="70%" valign="top">
                                                    <asp:FileUpload ID="reportFileUpload" runat="Server" />
                                                </td>
                                                <td width="20%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp;
                                                        <a class="thumbnail" href="#thumb">Preview<span><img id="imgReportlogo" src="" /><br />
                                                            Image</span></a>
                                                    </div>
                                                </td>
                                                <td width="10%" valign="top">
                                                    <asp:HiddenField ID="hdnRemoveReportImage" runat="server" Value="0" />
                                                    <a href="javascript:void(0)" onclick="RemoveReportImage();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblRecSize %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblEmailTemplates" runat="server" Text="<% $Resources:lblEmailTemplates %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="lblStart" runat="server" Text="<%$ Resources:lblStart %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlEmailStart" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq14" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq14 %>"
                                            SetFocusOnError="True" ControlToValidate="ddlEmailStart" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblReminder1" runat="server" Text="<% $Resources:lblReminder1 %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlEmailRemainder1" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq15" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq15 %>"
                                            SetFocusOnError="True" ControlToValidate="ddlEmailRemainder1" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="lblReminder2" runat="server" Text="<% $Resources:lblReminder2 %>"></asp:Label>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlEmailRemainder2" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblReminder3" runat="server" Text="<% $Resources:lblReminder3 %>"></asp:Label>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlEmailRemainder3" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblFaqDetails" runat="server" Text="<% $Resources:lblFaqDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%" valign="top">
                                        <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblFAQ %>"></asp:Label><span
                                            class="style3"></span>
                                    </td>
                                    <td width="85%">
                                        <%--<FCKeditorV2:FCKeditor ID="txtFaqText" runat="server" BasePath="~/fckeditor/" Width="800px"
                                            ToolbarSet="Feedback">
                                        </FCKeditorV2:FCKeditor>--%>
                                         <div style="width: 100%;">
                                            <textarea id="txtFaqText" runat="server" rows="10" cols="80" style="width: 90%;"
                                                clientidmode="Static">
                                        </textarea>
                                        </div>
                                        <%--<asp:TextBox ID="txtFaqText" TextMode="MultiLine" SkinID="txtarea500" Rows="5" runat="server"></asp:TextBox>--%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label>
                        <br>
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
                        <br>
                    </div>
                    <!-- start user form -->
                </div>
            </div>
       <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imbSave" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">

        //document.getElementById('ctl00_cphMaster_txtPassowrd').value = document.getElementById('ctl00_cphMaster_hdnPassword').value;
        
    </script>

    <script type="text/javascript">

        if (document.getElementById('ctl00_cphMaster_hdnReportimage').value != "") {
            document.getElementById('imgReportlogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnReportimage').value;
        }
        else {
            document.getElementById('imgReportlogo').src = "../../UploadDocs/noImage.jpg ";
        }


        if (document.getElementById('ctl00_cphMaster_hdnQuestimage').value != "") {
            document.getElementById('imgQuestlogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnQuestimage').value;
        }
        else {
            document.getElementById('imgQuestlogo').src = "../../UploadDocs/noImage.jpg ";
        }

    </script>
      <script type="text/javascript">
         
           BindEditor(document.getElementById('txtFaqText'));
            
        </script>
</asp:Content>

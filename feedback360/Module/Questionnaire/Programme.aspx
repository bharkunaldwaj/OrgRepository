<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    Culture="en-GB" UICulture="en-US" ValidateRequest="false" AutoEventWireup="true"
    CodeFile="Programme.aspx.cs" Inherits="Module_Questionnaire_Programme" %>
    <%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">

        function pageLoad() {

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtStartDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, minDate: '0d', defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
            });


            $(document).ready(function() {
                $("#ctl00_cphMaster_dtEndDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, minDate: '0d', defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtRemainderDate1").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtRemainderDate2").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtRemainderDate3").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtAvailableFrom").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtAvailableTo").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtPartReminder1").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtPartReminder2").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', minDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100'
                });
            });
        }

        function RemoveImage() {
            document.getElementById('ctl00_cphMaster_hdnRemoveImage').value = "";
        }

        function ValidateFCK(source, args) {
            var fckBody = FCKeditorAPI.GetInstance('<%=txtInstructionText.ClientID %>');
            args.IsValid = fckBody.GetXHTML(true) != "";
        }
    
    </script>

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
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/project_create.png" runat="server" alt="<% $Resources:lblheader %>"
                                align="absmiddle" />
                            <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label></h3>
                        <div class="clear">
                        </div>
                    </div>
                    <!-- end heading logout -->
                    <!-- start user form -->
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
                    <asp:HiddenField ID="hdnRemoveLogoImage" runat="server" />
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
                                            <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label5" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label>
                            </legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="lblName" runat="server" Text="<% $Resources:lblName %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:TextBox ID="txtName" MaxLength="25" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq2" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq2 %> "
                                            SetFocusOnError="True" ControlToValidate="txtName">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq1 %> "
                                            SetFocusOnError="True" ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblDesc" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDescription" runat="server" SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" /><div
                                            class="maxlength-msg">
                                            <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblMaxChar %>"></asp:Label></div>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblClientName" runat="server" Text="<% $Resources:lblClientName %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtClientName" MaxLength="50" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblProgrammeLogo" runat="server" Text="<% $Resources:lblProgrammeLogo %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                            <tr>
                                                <td width="70%" valign="top">
                                                    <asp:FileUpload ID="FileUpload" runat="Server" />
                                                </td>
                                                <td width="20%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="imagelogo" src="" /><br />
                                                            Image</span></a>
                                                    </div>
                                                </td>
                                                <td width="10%" valign="top">
                                                    <asp:HiddenField ID="hdnRemoveImage" runat="server" Value="0" />
                                                    <a href="#" onclick="RemoveImage();"  ><img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <asp:Label ID="Label3" runat="server" Text="<% $Resources:lblRecSize %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblColleaguesNo" runat="server" Text="<% $Resources:lblColleaguesNo %>"></asp:Label>
                                        <span class="style3">*</span>                                        
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtColleaguesNo" MaxLength="2" runat="server" onKeyPress="return NumberOnly(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvColleague" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:rfvColleague %> "
                                            SetFocusOnError="True" ControlToValidate="txtColleaguesNo" InitialValue="">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label8" runat="server" Text="<% $Resources:lblReportTopLogo %>"></asp:Label></td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                            <tr>
                                                <td width="70%" valign="top">
                                                    <asp:FileUpload ID="FileUploadReportLogo" runat="Server" />
                                                </td>
                                                <td width="20%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="ReportLogoImage" src="" /><br />
                                                            Image</span></a>
                                                    </div>
                                                </td>
                                                <td width="10%" valign="top">
                                                    <asp:HiddenField ID="hdnRemoveReportImage" runat="server" Value="0" />
                                                    <a href="#" onclick="RemoveReportImage();"  ><img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <asp:Label ID="Label9" runat="server" Text="<% $Resources:lblRepLogoSize %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table></td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label4" runat="server" Text="<% $Resources:lblSetDates %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="lblStartDate" runat="server" Text="<% $Resources:lblStartDate %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%" align="left" class="calimg">
                                        <asp:TextBox ID="dtStartDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtStartDate','txtStartDate');"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq7" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq7 %> "
                                            ControlToValidate="txtStartDate">&nbsp;</asp:RequiredFieldValidator>
                                        <div style="display: none">
                                            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblCloseDate" runat="server" Text="<% $Resources:lblCloseDate %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%" class="calimg">
                                        <asp:TextBox ID="dtEndDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtEndDate','txtEndDate');"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq8" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq8 %>  "
                                            SetFocusOnError="True" ControlToValidate="txtEndDate">&nbsp;</asp:RequiredFieldValidator>
                                        <div style="display: none">
                                            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                        </div>
                                        <asp:CompareValidator runat="server" ID="enddate" ValidationGroup="group1" ControlToValidate="txtEndDate"
                                            ControlToCompare="txtStartDate" Operator="GreaterThan" Type="Date" ErrorMessage="<% $Resources:enddate %>"
                                            Text="*" ForeColor="White" />
                                    </td>
                                </tr>
                                                                <tr>
                                    <td>
                                        <asp:Label ID="lblPartReminder1" runat="server" Text="<% $Resources:lblPartReminder1 %>"></asp:Label><span
                                            class="style3"></span>
                                    </td>
                                    <td class="calimg">
                                        <asp:TextBox ID="dtPartReminder1" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtPartReminder1','txtPartReminder1');"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:RequiredFieldValidator1 %>    "
                                            SetFocusOnError="True" ControlToValidate="txtPartReminder1">&nbsp;</asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<% $Resources:CustomValidator1 %>"
                                            Display="Dynamic" ControlToValidate="txtPartReminder1" ValidateEmptyText="true" Text="*" 
                                            OnServerValidate="ValCusPartReminder1" ValidationGroup="group1" />
                                        <div style="display: none">
                                            <asp:TextBox ID="txtPartReminder1" runat="server"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="CompareValidator9" ControlToValidate="txtPartReminder1"
                                                ControlToCompare="txtStartDate" ValidationGroup="group1" Operator="GreaterThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator9 %>" Text="*" ForeColor="White" />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPartReminder2" runat="server" Text="<% $Resources:lblPartReminder2 %>"></asp:Label><span
                                            class="style3"></span>
                                    </td>
                                    <td class="calimg">
                                        <asp:TextBox ID="dtPartReminder2" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtPartReminder2','txtPartReminder2');"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:RequiredFieldValidator2 %> "
                                            SetFocusOnError="True" ControlToValidate="txtPartReminder2">&nbsp;</asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<% $Resources:CustomValidator2 %>"
                                            Display="Dynamic" ControlToValidate="txtPartReminder2" ValidateEmptyText="true" Text="*" 
                                            OnServerValidate="ValCusPartReminder2" ValidationGroup="group1" />
                                        <div style="display: none">
                                            <asp:TextBox ID="txtPartReminder2" runat="server"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="CompareValidator10" ControlToValidate="txtPartReminder2"
                                                ControlToCompare="txtStartDate" ValidationGroup="group1" Operator="GreaterThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator10 %>" Text="*" ForeColor="White" />
                                        </div>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblReminder_Date_1" runat="server" Text="<% $Resources:lblReminder_Date_1 %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td class="calimg">
                                        <asp:TextBox ID="dtRemainderDate1" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtRemainderDate1','txtRemainderDate1');"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq9" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq9 %> "
                                            SetFocusOnError="True" ControlToValidate="txtRemainderDate1">&nbsp;</asp:RequiredFieldValidator>
                                        <div style="display: none">
                                            <asp:TextBox ID="txtRemainderDate1" runat="server"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="CompareValidator1" ValidationGroup="group1"
                                                ControlToValidate="txtRemainderDate1" ControlToCompare="txtStartDate" Operator="GreaterThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator1 %>" Text="*" ForeColor="White" />
                                            <asp:CompareValidator runat="server" ID="CompareValidator6" ValidationGroup="group1"
                                                ControlToValidate="txtRemainderDate1" ControlToCompare="txtEndDate" Operator="LessThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator6 %>" Text="*" ForeColor="White" />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReminder_Date_2" runat="server" Text="<% $Resources:lblReminder_Date_2 %>"></asp:Label>
                                    </td>
                                    <td class="calimg">
                                        <asp:TextBox ID="dtRemainderDate2" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtRemainderDate2','txtRemainderDate2');"></asp:TextBox>
                                        <div style="display: none">
                                            <asp:TextBox ID="txtRemainderDate2" runat="server"></asp:TextBox>
                                            <asp:CustomValidator ID="CusReminder2" runat="server" ErrorMessage="<% $Resources:CusReminder2 %>"
                                                Display="Dynamic" ControlToValidate="txtRemainderDate2" ValidateEmptyText="true"
                                                OnServerValidate="ValCusReminder2" ValidationGroup="group1" />
                                            <asp:CompareValidator runat="server" ID="CompareValidator2" ControlToValidate="txtRemainderDate2"
                                                ValidationGroup="group1" ControlToCompare="txtRemainderDate1" Operator="GreaterThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator2 %>" Text="*" ForeColor="White" />
                                            <asp:CompareValidator runat="server" ID="CompareValidator7" ValidationGroup="group1"
                                                ControlToValidate="txtRemainderDate2" ControlToCompare="txtEndDate" Operator="LessThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator7 %>" Text="*" ForeColor="White" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReminder_Date_3" runat="server" Text="<% $Resources:lblReminder_Date_3 %>"></asp:Label>
                                    </td>
                                    <td class="calimg">
                                        <asp:TextBox ID="dtRemainderDate3" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                            SkinID="dob" onchange="SetDTPickerDate('dtRemainderDate3','txtRemainderDate3');"></asp:TextBox>
                                        <div style="display: none">
                                            <asp:TextBox ID="txtRemainderDate3" runat="server"></asp:TextBox>
                                            <asp:CustomValidator ID="CusReminder3" runat="server" ErrorMessage="<% $Resources:CusReminder3 %>"
                                                Display="Dynamic" ControlToValidate="txtRemainderDate3" ValidateEmptyText="true"
                                                OnServerValidate="ValCusReminder3" ValidationGroup="group1" />
                                            <asp:CompareValidator runat="server" ID="CompareValidator3" ControlToValidate="txtRemainderDate3"
                                                ControlToCompare="txtRemainderDate2" ValidationGroup="group1" Operator="GreaterThan"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator3 %>" Text="*" ForeColor="White" />
                                            <asp:CompareValidator runat="server" ID="CompareValidator8" ValidationGroup="group1"
                                                ControlToValidate="txtRemainderDate3" ControlToCompare="txtEndDate" Operator="LessThanEqual"
                                                Type="Date" ErrorMessage="<% $Resources:CompareValidator8 %>" Text="*" ForeColor="White" />
                                        </div>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <caption>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblReportFrom" runat="server" 
                                                Text="<% $Resources:lblReportFrom %>"></asp:Label>
                                            <span class="style3"></span>
                                        </td>
                                        <td class="calimg">
                                            <asp:TextBox ID="dtAvailableFrom" runat="server" MaxLength="15" 
                                                onchange="SetDTPickerDate('dtAvailableFrom','txtAvailableFrom');" 
                                                ReadOnly="true" SkinID="dob" Width="80"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="Rq12" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq12 %>    "
                                            SetFocusOnError="True" ControlToValidate="txtAvailableFrom">&nbsp;</asp:RequiredFieldValidator>--%>
                                            <asp:CustomValidator ID="CustomValidator3" runat="server" 
                                                ControlToValidate="txtAvailableFrom" Display="Dynamic" 
                                                ErrorMessage="<% $Resources:Rq12 %>" 
                                                OnServerValidate="ValCusReportAvailableFrom" Text="*" ValidateEmptyText="true" 
                                                ValidationGroup="group1" />
                                            <div style="display: none">
                                                <asp:TextBox ID="txtAvailableFrom" runat="server"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator4" runat="server" 
                                                    ControlToCompare="txtEndDate" ControlToValidate="txtAvailableFrom" 
                                                    ErrorMessage="<% $Resources:CompareValidator4 %>" ForeColor="White" 
                                                    Operator="GreaterThan" Text="*" Type="Date" ValidationGroup="group1" />
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReportTo" runat="server" Text="<% $Resources:lblReportTo %>"></asp:Label>
                                            <span class="style3"></span>
                                        </td>
                                        <td class="calimg">
                                            <asp:TextBox ID="dtAvailableTo" runat="server" MaxLength="15" 
                                                onchange="SetDTPickerDate('dtAvailableTo','txtAvailableTo');" ReadOnly="true" 
                                                SkinID="dob" Width="80"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="Rq13" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq13 %> "
                                            SetFocusOnError="True" ControlToValidate="txtAvailableTo">&nbsp;</asp:RequiredFieldValidator>--%>
                                            <asp:CustomValidator ID="CustomValidator4" runat="server" 
                                                ControlToValidate="txtAvailableTo" Display="Dynamic" 
                                                ErrorMessage="<% $Resources:Rq13 %>" OnServerValidate="ValCusReportAvailableTo" 
                                                Text="*" ValidateEmptyText="true" ValidationGroup="group1" />
                                            <div style="display: none">
                                                <asp:TextBox ID="txtAvailableTo" runat="server"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator5" runat="server" 
                                                    ControlToCompare="txtAvailableFrom" ControlToValidate="txtAvailableTo" 
                                                    ErrorMessage="<% $Resources:CompareValidator5 %>" ForeColor="White" 
                                                    Operator="GreaterThan" Text="*" Type="Date" ValidationGroup="group1" />
                                            </div>
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                        </fieldset>

                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label7" runat="server" Text="<% $Resources:lblInstruction %>"></asp:Label></legend>                            

                                <div>
                                <FCKeditorV2:FCKeditor ID="txtInstructionText" runat="server" BasePath="~/fckeditor/" Width="800px"
                                        ToolbarSet="Instructions">
                                    </FCKeditorV2:FCKeditor>
                                    <asp:CustomValidator ID="rfvInstruction" runat="server" ErrorMessage="<% $Resources:rfvInstruction %>" ControlToValidate="txtInstructionText"
                                    ClientValidationFunction="ValidateFCK" ValidateEmptyText="True"></asp:CustomValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvInstruction" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:rfvInstruction %> "
                                            SetFocusOnError="True" ControlToValidate="txtInstructionText" InitialValue="">&nbsp;</asp:RequiredFieldValidator>--%>
                                </div>
                        </fieldset>

                        <span class="style3">
                            <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <br>
                        <div align="center">
                            <asp:ImageButton ID="imbSave" runat="server" ImageUrl="~/Layouts/Resources/images/save.png"
                                OnClick="imbSave_Click" ToolTip="Save" ValidationGroup="group1" />
                            &nbsp;
                            <asp:ImageButton ID="imbcancel" runat="server" ImageUrl="~/Layouts/Resources/images/cancel.png"
                                OnClick="imbcancel_Click" ToolTip="Back to List" CausesValidation="false" />
                            &nbsp;
                            <asp:ImageButton ID="imbBack" runat="server" CausesValidation="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                                OnClick="imbBack_Click" ToolTip="Back to List" Visible="false" />
                        </div>
                        <br>
                    </div>
                    <!-- start user form -->
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imbSave" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        if (document.getElementById('ctl00_cphMaster_hdnimage').value != "") {
            document.getElementById('imagelogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
        }
        else {
            document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
        }

       // alert(document.getElementById('ctl00_cphMaster_hdnRemoveLogoImage').value);
       // alert(document.getElementById('ReportLogoImage').src);
        if (document.getElementById('ctl00_cphMaster_hdnRemoveLogoImage').value != "") {
            document.getElementById('ReportLogoImage').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnRemoveLogoImage').value;
           //alert(document.getElementById('ReportLogoImage').src);
        }
        else {
            document.getElementById('ReportLogoImage').src = "../../UploadDocs/noImage.jpg ";
        }
       // alert(document.getElementById('ReportLogoImage').src);

    </script>

</asp:Content>

<%@ Page Title="Create New Project" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="Projects.aspx.cs" Culture="en-GB" UICulture="en-US" Inherits="Survey_Module_Questionnaire_Projects" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

<script type="text/javascript" src="../../Layouts/tinymce/jscripts/tiny_mce/tiny_mce.js" ></script>

    <script type="text/javascript" src="../../Layouts/Resources/js/tinymice.js" ></script>

    <script type="text/javascript">
//        Editor();
    </script>
    
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

.gallerycontainer{
position: relative;
/*Add a height attribute and set to largest image's height to prevent overlaying*/
}

.thumbnail img{
border: 1px solid white;
margin: 0 5px 5px 0;
}

.thumbnail:hover{
background-color: transparent;
}

.thumbnail:hover img{
border: 1px solid blue;
}

.thumbnail span{ /*CSS for enlarged image*/
position: absolute;
background-color: yellow;
padding: 5px;
left: -1000px;
border: 1px dashed gray;
visibility: hidden;
color: black;
text-decoration: none;
}

.thumbnail span img{ /*CSS for enlarged image*/
border-width: 0;
padding: 2px;
}

.thumbnail:hover span{ /*CSS for enlarged image*/
visibility: visible;
top: 0;
left: 2px; /*position where enlarged image should offset horizontally */
z-index: 50;
}

</style>
    
   <asp:UpdatePanel ID="updPanel" runat="server"  >
    <ContentTemplate>

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
          <td style="width:35%"></td>
          <td><div id="summary"  runat="server">
          
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                DisplayMode="BulletList"  ShowSummary="true" ValidationGroup = "group1"  />
            </div></td> 
            <td style="width:30%"></td>           
             </tr>
            </table>
            
            <asp:HiddenField ID="hdnPassword" runat="server" />
            <asp:HiddenField ID="hdnimage" runat="server" />
            <div class="userform">
            
                <div id="divAccount" runat="server" visible="true">
                    <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td width="35%">
                                <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" 
                                    AppendDataBoundItems="True" 
                                    onselectedindexchanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true" >
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>"
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
                    <legend><asp:Label ID="lblGeneralDetail" runat="server" Text="<% $Resources:lblGeneralDetail %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblQuestionnaire" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td width="35%">
                                <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:RequiredFieldValidator3 %>"
                                    SetFocusOnError="True" ControlToValidate="ddlQuestionnaire" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblReference" runat="server" Text="<% $Resources:lblReference %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td width="35%">
                                <asp:TextBox ID="txtReference" MaxLength="25" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq2" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq2 %>"
                                    SetFocusOnError="True" ControlToValidate="txtReference">&nbsp;</asp:RequiredFieldValidator>
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
                                <asp:TextBox ID="txtDescription" runat="server" SkinID="txtarea300X3" Rows="3" TextMode="MultiLine"
                                    /><div class="maxlength-msg"><asp:Label id="lblCharactersLimit"  runat="server" Text="<% $Resources:lblCharactersLimit %>"></asp:Label></div>
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
                                
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="Finish_emailID_Txtbox" Display="None" 
                                    EnableClientScript="true" ErrorMessage="               Please enter the email-ID." 
                                    SetFocusOnError="True" ValidationGroup="group1"></asp:RequiredFieldValidator>--%>
                                
                                </td>
                            <td valign="top">
                                &nbsp;<%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                    runat="server" ControlToValidate="Finish_emailID_Txtbox" Display="None" 
                                    EnableClientScript="true" 
                                    ErrorMessage="Incorrect Email format in 'Finish Email-ID'(Correct Example: YourId@somedomain.com)" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ValidationGroup="group1"></asp:RegularExpressionValidator>--%>
                                <br />
                                
                                </td>
                        </tr>
                        <%--<tr>
                       <td valign="top">
                       Finish Email-ID
                                <span
                                    class="style3">*</span>
                       </td>
                       <td valign="top">
                           <asp:TextBox ID="Finish_emailID_Txtbox" runat="server" Width="300px"></asp:TextBox>
                            </td>
                       <td valign="top">Send Finish E-mail</td>
                       <td valign="top">
                       <asp:CheckBox ID="Finish_Email_Chkbox" runat="server" />
                       </td>
                       
                        </tr>--%>
                        
                    </table>
                </fieldset>
                <%--<fieldset class="fieldsetform">
                    <legend>Set Dates</legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblStartDate" runat="server" Text="<% $Resources:lblStartDate %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td width="35%" align="left" class="calimg">
                                <asp:TextBox ID="dtStartDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                    SkinID="dob" onchange="SetDTPickerDate('dtStartDate','txtStartDate');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq7" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Start Date "
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
                                <asp:RequiredFieldValidator ID="Rq8" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select End Date "
                                    SetFocusOnError="True" ControlToValidate="txtEndDate">&nbsp;</asp:RequiredFieldValidator>
                                <div style="display: none">
                                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                </div>
                                <asp:CompareValidator runat="server" id="enddate" ValidationGroup="group1" controltovalidate="txtEndDate" controltocompare="txtStartDate" operator="GreaterThan" type="Date" errormessage="Start date should be greater than end date" Text="*" ForeColor="White"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblReminder_Date_1" runat="server" Text="<% $Resources:lblReminder_Date_1 %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td class="calimg">
                                <asp:TextBox ID="dtRemainderDate1" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                    SkinID="dob" onchange="SetDTPickerDate('dtRemainderDate1','txtRemainderDate1');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq9" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Reminder 1 Date"
                                    SetFocusOnError="True" ControlToValidate="txtRemainderDate1">&nbsp;</asp:RequiredFieldValidator>
                                <div style="display: none">
                                    <asp:TextBox ID="txtRemainderDate1" runat="server"></asp:TextBox>
                                    
                                        <asp:CompareValidator runat="server" id="CompareValidator1" ValidationGroup="group1" controltovalidate="txtRemainderDate1" controltocompare="txtStartDate" operator="GreaterThan" type="Date" errormessage="Reminder 1 date should be greater than start date" Text="*" ForeColor="White"/>
                                        <asp:CompareValidator runat="server" id="CompareValidator6" ValidationGroup="group1" controltovalidate="txtRemainderDate1" controltocompare="txtEndDate" operator="LessThan" type="Date" errormessage="Reminder 1 date should be less than  end date" Text="*" ForeColor="White"/>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblReminder_Date_2" runat="server" Text="<% $Resources:lblReminder_Date_2 %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td class="calimg">
                                <asp:TextBox ID="dtRemainderDate2" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                    SkinID="dob" onchange="SetDTPickerDate('dtRemainderDate2','txtRemainderDate2');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq10" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Reminder 2 Date"
                                    SetFocusOnError="True" ControlToValidate="txtRemainderDate2">&nbsp;</asp:RequiredFieldValidator>
                                <div style="display: none">
                                    <asp:TextBox ID="txtRemainderDate2" runat="server"></asp:TextBox>
                                    <asp:CompareValidator runat="server" id="CompareValidator2" controltovalidate="txtRemainderDate2" ValidationGroup="group1" controltocompare="txtRemainderDate1" operator="GreaterThan" type="Date" errormessage="Reminder 2 date should be greater than Reminder 1 date"  Text="*" ForeColor="White"/>
                                     <asp:CompareValidator runat="server" id="CompareValidator7" ValidationGroup="group1" controltovalidate="txtRemainderDate2" controltocompare="txtEndDate" operator="LessThan" type="Date" errormessage="Reminder 2 date should be less than  end date" Text="*" ForeColor="White"/>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblReminder_Date_3" runat="server" Text="<% $Resources:lblReminder_Date_3 %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td class="calimg">
                                <asp:TextBox ID="dtRemainderDate3" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                    SkinID="dob" onchange="SetDTPickerDate('dtRemainderDate3','txtRemainderDate3');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq11" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Reminder 3 Date"
                                    SetFocusOnError="True" ControlToValidate="txtRemainderDate3">&nbsp;</asp:RequiredFieldValidator>
                                <div style="display: none">
                                    <asp:TextBox ID="txtRemainderDate3" runat="server"></asp:TextBox>
                                    <asp:CompareValidator runat="server" id="CompareValidator3" controltovalidate="txtRemainderDate3" controltocompare="txtRemainderDate2" ValidationGroup="group1" operator="GreaterThan" type="Date" errormessage="Reminder 3 date should be greater than Reminder 2 date" Text="*" ForeColor="White"/>
                                    <asp:CompareValidator runat="server" id="CompareValidator8" ValidationGroup="group1" controltovalidate="txtRemainderDate3" controltocompare="txtEndDate" operator="LessThanEqual" type="Date" errormessage="Reminder 3 date should be less than or equal to end date" Text="*" ForeColor="White"/>
                                
                                </div>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblReportFrom" runat="server" Text="<% $Resources:lblReportFrom %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td class="calimg">
                                <asp:TextBox ID="dtAvailableFrom" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                    SkinID="dob" onchange="SetDTPickerDate('dtAvailableFrom','txtAvailableFrom');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq12" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Report Available From "
                                    SetFocusOnError="True" ControlToValidate="txtAvailableFrom">&nbsp;</asp:RequiredFieldValidator>
                                <div style="display: none">
                                    <asp:TextBox ID="txtAvailableFrom" runat="server"></asp:TextBox>
                                    
                                     <asp:CompareValidator runat="server" id="CompareValidator4" controltovalidate="txtAvailableFrom" controltocompare="txtEndDate" ValidationGroup="group1" operator="GreaterThan" type="Date" errormessage="Report available from date should be greater than end date" Text="*" ForeColor="White"/>
                                    
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblReportTo" runat="server" Text="<% $Resources:lblReportTo %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td class="calimg">
                                <asp:TextBox ID="dtAvailableTo" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                    SkinID="dob" onchange="SetDTPickerDate('dtAvailableTo','txtAvailableTo');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Rq13" runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Report Available To "
                                    SetFocusOnError="True" ControlToValidate="txtAvailableTo">&nbsp;</asp:RequiredFieldValidator>
                                <div style="display: none">
                                    <asp:TextBox ID="txtAvailableTo" runat="server"></asp:TextBox>
                                    
                                    
                                      <asp:CompareValidator runat="server" id="CompareValidator5" controltovalidate="txtAvailableTo" controltocompare="txtAvailableFrom" ValidationGroup="group1" operator="GreaterThan" type="Date" errormessage="Report available to date should be greater than Report available from" Text="*" ForeColor="White"/>
                                    
                                </div>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblEmailTemplates" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label></legend>
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
                        <tr>
                            <td width="15%">
                                <span
                                    class="style3"></span>
                            </td>
                            <td width="35%">
                                <asp:RequiredFieldValidator ID="Rq18" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq18 %>"
                                    SetFocusOnError="True" ControlToValidate="ddlEmailAvailable" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td width="15%" colspan="2">
                                &nbsp;</td>
                        </tr>
                    </table>
                </fieldset>--%>
                
                
                
                <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblFaqDetails" runat="server" Text="<% $Resources:lblFaqDetails %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="15%" valign="top">
                                <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblFAQ %>"></asp:Label><span
                                    class="style3"></span>
                            </td>
                            <td width="85%">
                                <FCKeditorV2:FCKeditor ID="txtFaqText" runat="server" BasePath="~/fckeditor/" Width="800px" ToolbarSet="Feedback" >
                                </FCKeditorV2:FCKeditor>
                                <%--<asp:TextBox ID="txtFaqText" TextMode="MultiLine" SkinID="txtarea500" Rows="5" runat="server"></asp:TextBox>--%>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label>
                
        
                <br>
                
                <div align="center">
                    <asp:ImageButton ID="imbSave" runat="server" 
                        ImageUrl="~/Layouts/Resources/images/save.png" OnClick="imbSave_Click" 
                        ToolTip="Save" ValidationGroup="group1" />
                    &nbsp;
                    <asp:ImageButton ID="imbcancel" runat="server" 
                        ImageUrl="~/Layouts/Resources/images/cancel.png" OnClick="imbcancel_Click" 
                        ToolTip="Back to List" />
                    &nbsp;
                    <asp:ImageButton ID="imbBack" runat="server" CausesValidation="true" 
                        ImageUrl="~/Layouts/Resources/images/Back.png" onclick="imbBack_Click" 
                        ToolTip="Back to List" Visible="false" />
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

        //document.getElementById('ctl00_cphMaster_txtPassowrd').value = document.getElementById('ctl00_cphMaster_hdnPassword').value;
        
    </script>
    
    <script type="text/javascript">

//        if (document.getElementById('ctl00_cphMaster_hdnimage').value != "" )
//        {
//            document.getElementById('imagelogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
//        }
//        else
//        {
//            document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
//        }   
        
    </script>
    
    <script type="text/javascript">
        
        function SetImage() 
        {
//            if (document.getElementById('ctl00_cphMaster_hdnimage').value != "") {
//                document.getElementById('imagelogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
//            }
//            else {
//                document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
//            }
        }
    
    </script>
    
</asp:Content>

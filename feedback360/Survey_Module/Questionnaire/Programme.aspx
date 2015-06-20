<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    Culture="en-GB" UICulture="en-US" ValidateRequest="false" AutoEventWireup="true"
    CodeFile="Programme.aspx.cs" Inherits="Survey_Module_Questionnaire_Programme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <script type="text/javascript" language="javascript">


        function ClearFileUpload() {

            var fil = document.getElementById("FileUpload");

            fil.select();

            n = fil.createTextRange();

            n.execCommand('delete');

            fil.focus();

        }


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
                      
                        
                    <%--    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                        --%>
                        
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
                                        <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="true" 
                                            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq1 %> "
                                            SetFocusOnError="True" ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                      <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                         <asp:DropDownList ID="ddlCompany" runat="server" AppendDataBoundItems="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:rqCompany %> "
                                            SetFocusOnError="True" ControlToValidate="ddlCompany" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
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
                                                    <a href="#" onclick="RemoveImage();">
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
                                    <td>
                                                  <asp:Label ID="lblDesc" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label>
                                                </td>
                                                <td>
                                                   <asp:TextBox ID="txtDescription" runat="server" SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" /><div
                                            class="maxlength-msg">
                                            <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblMaxChar %>"></asp:Label></div>
                                                </td>
                                </tr>
                            </table>
                        </fieldset>
                   <%--   </ContentTemplate>
                      </asp:UpdatePanel>
                  
                  
                  
                  
                  
                  
                  
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                      --%>
                      
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
                                            SkinID="dob" onchange="SetDTPickerDate('dtEndDate','txtEndDate');" 
                                            ontextchanged="dtEndDate_TextChanged"></asp:TextBox>
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
                            </table>
                        </fieldset>
                       <%-- </ContentTemplate>
                                 </asp:UpdatePanel>
        
                        
                        
                        
                        
                        
                        
                        
                        
                        
                        
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
                        
                        <asp:Panel runat="server" ID="analysis_Panel">
                        <fieldset id="analysis_fieldset" class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label7" runat="server" Text="<% $Resources:lblAnalysisSheet %>"></asp:Label></legend>
                            
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td colspan="5" >
                                        <br />
                                        <asp:Label ID="Label9" runat="server" Text="ANALYSIS- I " BorderStyle="None"
                                            Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" ForeColor="#FF6600"></asp:Label>
                                    </td>
                                    
                                </tr>
                                <tr>
                                
                                    <td style="width:18%">
                                        &nbsp;<asp:Label ID="lblName1" runat="server" Text="Enter the Name"></asp:Label>
                                        &nbsp;</td>
                                    <td  align="left" style="width:32%">
                                           <asp:TextBox ID="Txt_name_Analysis1" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:18%">
                                        <asp:Label ID="lbl_catgory_Analysis1" runat="server" 
                                            Text="No. of Analysis Categories"></asp:Label>
                                    </td>
                                    <td align="left" style="width:7%">
                                        <asp:TextBox ID="txt_catagory_Analysis1" runat="server" style="width:50px;"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:25%">
                                    
                                        <asp:ImageButton ID="imbSubmit1" runat="server" Height="21px" 
                                            ImageUrl="~/Layouts/Resources/images/submit-s.png" 
                                            OnClick="imbSubmit1_Click" />
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_catagory_Analysis1" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            
                            <table style="margin:0 auto;text-align:center" width="63%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                
                                    <td>
                                      <asp:Repeater ID="Repeater0" runat="server" EnableTheming="true" Visible="false" 
                                            onitemdatabound="Repeater0_ItemDataBound">
                                            <HeaderTemplate>
                                                <table width="100%" style="text-align:center" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                    <tr>
                                                        <th width="20%" align="center">
                                                            <asp:Label  style="text-align:center"  ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                        </th>
                                                        <th width="80%" align="center">
                                                            <asp:Label style="text-align:center" ID="score_ratings" runat="server" Text="<% $Resources:lblCategory %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                <tr style="width: 100%">
                                                    <td width="20%" align="center">
                                                        <%# Container.ItemIndex + 1  %>.
                                                    </td>
                                                    <td width="80%">
                                                        <asp:TextBox style="width:97%"  ID="txt_category" Text='<%# Eval("Category_Detail") %>' runat="server"></asp:TextBox>
                                                        
                                                        <asp:RequiredFieldValidator ID="txt_category_validator0" runat="server" ValidationGroup="group1" ErrorMessage="Can not keep the table-entry empty."
                                            SetFocusOnError="True" ControlToValidate="txt_category">&nbsp;</asp:RequiredFieldValidator>
                                                        
                                                    </td>
                                                </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            
                                            <asp:Label ID="lblErrorMsg" ForeColor="Red" runat="server" Text="*Please enter any number other than zero" Visible="false">
        </asp:Label>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                                                    
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                <td colspan="5" >
                                        <br />
                                        <asp:Label ID="Label10" runat="server" Text="ANALYSIS- II " BorderStyle="None"
                                            Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" ForeColor="#FF6600"></asp:Label>
                                    </td>
                                                                       
                                </tr>
                                <tr>
                                
                                    <td style="width:18%">
                                        &nbsp;<asp:Label ID="Lbl_name_Analysis2" runat="server" Text="Enter the Name"></asp:Label>
                                        &nbsp;</td>
                                    <td  align="left" style="width:32%">
                                           <asp:TextBox ID="txt_name_Analysis2" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:18%">
                                        <asp:Label ID="Lbl_category_Analysis2" runat="server" Text="No. of Analysis Categories"></asp:Label>
                                    </td>
                                    <td align="left" style="width:7%">
                                        <asp:TextBox ID="txt_category_Analysis2" runat="server" style="width:50px;"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:25%">
                                        <asp:ImageButton ID="imbSubmit2" runat="server" Height="21px" 
                                            ImageUrl="~/Layouts/Resources/images/submit-s.png" 
                                            OnClick="imbSubmit2_Click" />
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_category_Analysis2" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender  >
                                    </td>
                                </tr>
                            </table>
                            <table style="margin:0 auto;text-align:center" width="63%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:Repeater ID="Repeater1" runat="server" EnableTheming="true" 
                                            Visible="False" onitemdatabound="Repeater1_ItemDataBound">
                                            <HeaderTemplate>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                    <tr>
                                                        <th width="20%">
                                                            <asp:Label style="text-align:center"  ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                        </th>
                                                        <th width="80%">
                                                            <asp:Label  ID="score_ratings" runat="server" Text="<% $Resources:lblCategory %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                <tr style="width: 100%">
                                                    <td width="20%" align="center">
                                                        <%# Container.ItemIndex + 1  %>.
                                                    </td>
                                                    <td width="80%">
                                                        <asp:TextBox style="width:97%"  ID="txt_category" Text='<%# Eval("Category_Detail") %>' runat="server"></asp:TextBox>
                                                        
                                                        <asp:RequiredFieldValidator ID="txt_category_validator1" runat="server" ValidationGroup="group1" ErrorMessage="Can not keep the table-entry empty."
                                            SetFocusOnError="True" ControlToValidate="txt_category">&nbsp;</asp:RequiredFieldValidator>
                                                        
                                                        
                                                    </td>
                                                </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                             <asp:Label ID="lblErrorMsg1" ForeColor="Red" runat="server" Text="*Please enter any number other than zero" Visible="false"></asp:Label>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            
                            
                            
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                      <td colspan="5" >
                                        <br />
                                        <asp:Label ID="Label12" runat="server" Text="ANALYSIS- III " BorderStyle="None"
                                            Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" ForeColor="#FF6600"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td style="width:18%">
                                        &nbsp;<asp:Label ID="Lbl_name_Analysis3" runat="server" Text="Enter the Name"></asp:Label>
                                        &nbsp;</td>
                                    <td  align="left" style="width:32%">
                                           <asp:TextBox ID="txt_name_Analysis3" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:18%">
                                        <asp:Label ID="lbl_category_Analysis3" runat="server" Text="No. of Analysis Categories"></asp:Label>
                                    </td>
                                    <td align="left" style="width:7%">
                                        <asp:TextBox ID="txt_category_Analysis3" runat="server" style="width:50px;"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:25%">
                                        <asp:ImageButton ID="imbSubmit3" runat="server" Height="21px" 
                                            ImageUrl="~/Layouts/Resources/images/submit-s.png" 
                                            OnClick="imbSubmit3_Click" />
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_category_Analysis3" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender  >
                                    </td>
                                </tr>
                            </table>
                            <table style="margin:0 auto;text-align:center" width="63%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:Repeater ID="Repeater2" runat="server" EnableTheming="true" 
                                            Visible="False" onitemdatabound="Repeater2_ItemDataBound">
                                            <HeaderTemplate>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                    <tr>
                                                        <th width="20%">
                                                            <asp:Label style="text-align:center"  ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                        </th>
                                                        <th width="80%">
                                                            <asp:Label  ID="score_ratings" runat="server" Text="<% $Resources:lblCategory %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                <tr style="width: 100%">
                                                    <td width="20%" align="center">
                                                        <%# Container.ItemIndex + 1  %>.
                                                    </td>
                                                    <td width="80%">
                                                        <asp:TextBox style="width:97%"  ID="txt_category" Text='<%# Eval("Category_Detail") %>' runat="server"></asp:TextBox>
                                                        
                                                     <asp:RequiredFieldValidator ID="txt_category_validator2" runat="server" ValidationGroup="group1" ErrorMessage="Can not keep the table-entry empty."
                                            SetFocusOnError="True" ControlToValidate="txt_category">&nbsp;</asp:RequiredFieldValidator>
                                                     
                                                        
                                                    </td>
                                                </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                             <asp:Label ID="lblErrorMsg2" ForeColor="Red" runat="server" Text="*Please enter any number other than zero" Visible="false"></asp:Label>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>
                        </fieldset></asp:Panel>
                        
                        
                        <span class="style3">
                            <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
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
                        <div>
                        <table style="width:100%">
                        <tr style="width:100%">
                        <td style="width:39%"></td>
                        <td style="width:61%" align="left">
                            <asp:Label ID="lbl_save_message" runat="server" Text="Label" ForeColor="Red" 
                                Visible="False"></asp:Label>
                          
                            </td>
                        </tr>
                        </table>
                        </div>
                    </div>
                    <!-- start user form -->
                </div>
            </div>
        
       <%-- </ContentTemplate>
     
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="imbSave" />
        </Triggers>
    </asp:UpdatePanel>--%>

    <script type="text/javascript">

        if (document.getElementById('ctl00_cphMaster_hdnimage').value != "") {
            document.getElementById('imagelogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
        }
        else {
            document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
        }

    </script>

</asp:Content>

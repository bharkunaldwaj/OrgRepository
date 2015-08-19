<%@ Page Title="Set up your colleagues" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Async="true" CodeFile="AssignQuestionnaire.aspx.cs"
    Inherits="Module_Questionnaire_AssignQuestionnaire" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script src="../../Layouts/Resources/js/AssignQuestionnaire.js" type="text/javascript"></script>
    <script type="text/javascript">


        function DeleteConfirmation(txtBox) {
            debugger;
            var txt = document.getElementById(txtBox);

            if (txt != null) {
                var result = confirm("Are you sure you want to delete " + txt.value + "?")

                if (result == true)
                    return true;
            }

            return false;
        }

    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMaster" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            xPos = $get('divCandidateList').scrollLeft;
            yPos = $get('divCandidateList').scrollTop;
        }
        function EndRequestHandler(sender, args) {
            $get('divCandidateList').scrollLeft = xPos;
            $get('divCandidateList').scrollTop = yPos;
        }

        function ShowPopup() {
            var path = "../Feedback/ProjectFAQ.aspx?ProjectId=" + document.getElementById('<%=hdnProjectId.ClientID %>').value;

            window.open(path, '', 'left=100,top=100,height=475,width=1000');
        }
    </script>
    <%--       <script language="javascript" type="text/javascript">

      var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        function InitializeRequest(sender, args) 
        {
            var btn = document.getElementById('<%=imbAssign.ClientID%>');
            btn.disabled = true;
       }
        function EndRequest(sender, args) {
            var btn = document.getElementById('<%=imbAssign.ClientID%>');
            btn.disabled = false;
        } 
        </script>
    --%>
    <asp:hiddenfield id="hdnProjectId" runat="server">
    </asp:hiddenfield>
   <%-- <asp:updatepanel id="updPanel" runat="server">
        <contenttemplate>
            <input type="hidden" id="div_position" name="div_position" />
            <rsweb:ReportViewer ID="rview" runat="server" Height="0" Width="">
            </rsweb:ReportViewer>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server"
                                title="<% $Resources:lblTopHeading %>" align="absmiddle" alt="" />
                            <asp:label id="lblTopHeading" runat="server" text="<% $Resources:lblTopHeading %>"></asp:label>
                            <asp:imagebutton id="ibtnHelp" runat="server" title="Help" cssclass="HeadingInfoHelp"
                                onclientclick="ShowPopup();" imageurl="~/Layouts/Resources/images/help.png" />
                        </h3>
                        <div class="clear">
                        </div>
                    </div>
                    <!-- end heading logout -->
                    <!-- start user form -->
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="summary" runat="server" class="validation-align">
                                    <asp:validationsummary id="ValidationSummary1" runat="server" displaymode="BulletList"
                                        showsummary="true" validationgroup="VGroupX" />
                                    <asp:validationsummary id="ValidationSummary2" runat="server" displaymode="BulletList"
                                        showsummary="true" validationgroup="group2" />
                                    <asp:label id="lblvalidation" runat="server" forecolor="Red" text=""></asp:label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<br />--%>
                                <div align="center">
                                    <asp:label id="lblMessage" runat="server" forecolor="Red" font-size="Large" text="">
                                    </asp:label>
                                </div>
                                <%--<br />--%>
                                <asp:label id="lblInstruction" runat="server" text=""></asp:label>
                            </td>
                        </tr>
                    </table>
                    <div class="userform">
                        <div id="divAccount" runat="server" visible="false">
                            <fieldset class="fieldsetform">
                                <legend>
                                    <asp:label id="lblAccountDetails" runat="server" text="<% $Resources:lblAccountDetails %>">
                                    </asp:label></legend>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td width="15%">
                                            <asp:label id="lblAccountcode" runat="server" text="<% $Resources:lblAccountcode %>">
                                            </asp:label><span class="style3">*</span>
                                        </td>
                                        <td width="35%">
                                            <asp:dropdownlist id="ddlAccountCode" runat="server" style="width: 155px" onselectedindexchanged="ddlAccountCode_SelectedIndexChanged"
                                                autopostback="true">
                                                <asp:listitem value="0">Select</asp:listitem>
                                            </asp:dropdownlist>
                                            <asp:requiredfieldvalidator id="Rq21" runat="server" validationgroup="VGroup" errormessage="<% $Resources:Rq21 %>  "
                                                setfocusonerror="True" controltovalidate="ddlAccountCode" initialvalue="0">&nbsp;</asp:requiredfieldvalidator>
                                        </td>
                                        <td width="15%">
                                            <asp:label id="lblCompany" runat="server" text="<% $Resources:lblCompany %>"></asp:label>
                                        </td>
                                        <td width="35%">
                                            <asp:label id="lblcompanyname" runat="server" text=""></asp:label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:label id="lblGeneralDetails" runat="server" text="<% $Resources:lblGeneralDetails %>">
                                            </asp:label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                            <tr>
                                                <td width="15%" valign="top">
                                                    <asp:label id="lblProject" runat="server" text="<% $Resources:lblProject %>"></asp:label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="35%" valign="top">
                                                    <asp:label id="lblProjectText" runat="server" text=""></asp:label>
                                                    <asp:dropdownlist id="ddlProject" runat="server" style="width: 155px" autopostback="true"
                                                        onselectedindexchanged="ddlProject_SelectedIndexChanged">
                                                        <asp:listitem value="0">Select</asp:listitem>
                                                    </asp:dropdownlist>
                                                    <asp:requiredfieldvalidator id="Rq1" runat="server" errormessage="<% $Resources:Rq1 %>"
                                                        setfocusonerror="True" controltovalidate="ddlProject" validationgroup="VGroupX"
                                                        initialvalue="0">&nbsp;</asp:requiredfieldvalidator>
                                                </td>
                                                <td width="15%" valign="top">
                                                </td>
                                                <td width="35%" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:label id="lblProgramme" runat="server" text="<% $Resources:lblProgramme %>"></asp:label>
                                                    <span class="style3">*</span>
                                                </td>
                                                <td>
                                                    <asp:label id="lblProgrammeText" runat="server" text=""></asp:label>
                                                   <%-- <asp:updatepanel id="UpdatePanelProgram" runat="server">
                                                        <contenttemplate>--%>
                                                    <asp:dropdownlist id="ddlProgramme" runat="server" style="width: 155px" Autopostback="true"
                                                        onselectedindexchanged="ddlProgramme_SelectedIndexChanged">
                                                        <asp:listitem value="0">Select</asp:listitem>
                                                    </asp:dropdownlist>
                                                    <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" errormessage="<% $Resources:RequiredFieldValidator1 %>"
                                                        setfocusonerror="True" controltovalidate="ddlProgramme" validationgroup="VGroupX"
                                                        initialvalue="0">&nbsp;</asp:requiredfieldvalidator>
                                                          <%-- </contenttemplate>
                                                        <triggers>
                                                    <asp:asyncpostBacktrigger ControlID="ddlProject"  />
                                                    </triggers>
                                                    </asp:updatepanel>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr id="trTargetPerson" runat="server">
                                                <td valign="top" style="width: 15%;">
                                                    <asp:label id="lblTargetPerson" runat="server" text="<% $Resources:lblTargetPerson %>">
                                                    </asp:label><span class="style3">*</span>
                                                </td>
                                                <td valign="top" colspan="3">
                                                   <%-- <asp:updatepanel id="UpdatePanelTargetPeron" runat="server">
                                                        <contenttemplate>--%>
                                                    <asp:dropdownlist id="ddlTargetPerson" runat="server" 
                                                        Autopostback="true" style="width: 155px" onselectedindexchanged="ddlTargetPerson_SelectedIndexChanged">
                                                        <asp:listitem value="0">Select</asp:listitem>
                                                    </asp:dropdownlist>
                                                   
                                                    <asp:Requiredfieldvalidator id="Rq3" runat="server" errormessage="<% $Resources:Rq3 %>"
                                                        controltovalidate="ddlTargetPerson" setfocusonerror="True" validationgroup="VGroupX"
                                                        display="Dynamic" initialvalue="0">&nbsp;</asp:Requiredfieldvalidator>
                                                        <%-- </contenttemplate>
                                                        <triggers>
                                                    <asp:asyncpostBacktrigger ControlID="ddlProgramme"  />
                                                    </triggers>
                                                    </asp:updatepanel>--%>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblDescription" runat="server" Text="<% $Resources:lblDescription %>"></asp:Label>
                                                </td>
                                                <td valign="top" colspan="2">
                                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" SkinID="txtarea300X3"
                                                        Rows="3" />
                                                    <div class="maxlength-msg">
                                                        (Max. 1000 characters)</div>
                                                </td>
                                                <td valign="top">
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                       <%-- <asp:updatepanel id="updPanel" runat="server">
                            <contenttemplate>--%>
                                             <input type="hidden" id="div_position" name="div_position" />
                                            <rsweb:ReportViewer ID="rview" runat="server" Height="0" Width="">
                                               </rsweb:ReportViewer>
                            <table width="100%">
                            <tr>
                                <td valign="top">
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:label id="lblCandidateList" runat="server" text="<% $Resources:lblCandidateList %>"> </asp:label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                            <tr>
                                                <%--<td width="15%">
                                                    <asp:Label ID="lblCandidateNumber" runat="server" Text="<% $Resources:lblCandidateNumber %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="10%">
                                                    <asp:TextBox ID="txtCandidateNo" MaxLength="2" onkeypress="return NumberOnly(this);"
                                                        runat="server" SkinID="ph1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="Rq19" runat="server" ErrorMessage="<% $Resources:Rq19 %>"
                                                        ControlToValidate="txtCandidateNo" ValidationGroup="group2" SetFocusOnError="True">&nbsp;</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="ressequence" ControlToValidate="txtCandidateNo"
                                                        ErrorMessage="<% $Resources:ressequence %>" ValidationExpression="^[0-9][\d]*"
                                                        runat="server" ValidationGroup="group2" SetFocusOnError="True" Text="*" ForeColor="White" />
                                                    <asp:RangeValidator ID="valTxtRange" ControlToValidate="txtCandidateNo" Type="Integer"
                                                        MinimumValue="1" MaximumValue="99" ErrorMessage="<% $Resources:valTxtRange %>"
                                                        ValidationGroup="group2" SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />
                                                </td>--%>
                                                <td colspan="3">
                                                    <asp:imagebutton id="imbSubmit" imageurl="~/Layouts/Resources/images/add-more-btn.png"
                                                        runat="server" onclick="imbSubmit_Click" />&nbsp;
                                                    <asp:ImageButton id="ImageButtonSaveAll" imageurl="~/Layouts/Resources/images/save.png" causeValidation="False"
                                                        runat="server" onclick="ImageButtonSaveAll_Click" onclientclick="return ValidateRepeaterCandidateList();"
                                                        tooltip="Save All" />
                                                </td>
                                                <%--<td width="35%">
                                                    <label>
                                                        <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;&nbsp;<span class="style3">(Excel
                                                            type only)</span>
                                                    </label>
                                                </td>
                                                <td width="15%">
                                                    <asp:ImageButton ID="ImgUpload" runat="server" ImageUrl="~/Layouts/Resources/images/import-s.png"
                                                        OnClick="ImgUpload_click" OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';" />
                                                </td>--%>
                                                <td width="45%" colspan="2">
                                                    <table id="tblParticipantUpload" runat="server" width="100%" border="0" cellspacing="0"
                                                        cellpadding="0">
                                                        <tr>
                                                            <td width="30%">
                                                                <label>
                                                                    <asp:fileupload id="FileUpload1" runat="server" />&nbsp;&nbsp;<span class="style3">
                                                                        <asp:label id="lblExcelType" runat="server" text="<% $Resources:lblExcelType %>"></asp:label></span>
                                                                </label>
                                                            </td>
                                                            <td width="15%">
                                                                <asp:imagebutton id="ImgUpload" runat="server" imageurl="~/Layouts/Resources/images/import-s.png"
                                                                    onclick="ImgUpload_click" onclientclick="javascript:document.forms[0].encoding = 'multipart/form-data';" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" valign="top">
                                               
                                                    <div id="divCandidateList" class="candidatelist" style="height: 242px">
                                                        <asp:repeater id="rptrCandidateList" onitemdatabound="rptrCandidateList_ItemDataBound"
                                                            onitemcommand="rptrCandidateList_ItemCommand" runat="server">
                                                            
                                                            <headertemplate>
                                                                <table width="100%" id="RepeatorCandidateList" border="0" cellspacing="0" cellpadding="0"
                                                                    class="grid">
                                                                    <tr>
                                                                        <th width="7%">
                                                                            <asp:Label ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                                        </th>
                                                                        <th width="15%">
                                                                            <asp:Label ID="lblRelationship" runat="server" Text="<% $Resources:lblRelationship %>"></asp:Label>
                                                                        </th>
                                                                        <th width="20%">
                                                                            <asp:Label ID="lblName" runat="server" Text="<% $Resources:lblName %>"></asp:Label>
                                                                        </th>
                                                                        <th width="31%">
                                                                            <asp:Label ID="lblEmailID" runat="server" Text="<% $Resources:lblEmailID %>"></asp:Label>
                                                                        </th>
                                                                        <%-- <th width="5%">
                                                                            <asp:Label ID="lblSubmitFlag" runat="server" Text="<% $Resources:lblSubmitFlag %>"></asp:Label>
                                                                        </th>--%>
                                                                        <th width="7%">
                                                                            <asp:Label ID="lblSave" runat="server" Text="<% $Resources:lblSave %>"></asp:Label>
                                                                        </th>
                                                                        <th width="10%">
                                                                            <asp:Label ID="lblCompletionPertiontage" runat="server" Text="<% $Resources:lblCompletionPertiontage %>"></asp:Label>
                                                                        </th>
                                                                        <th width="5%">
                                                                            <%--<asp:Label ID="lbl" runat="server" Text="<% $Resources:lblEmailID %>"></asp:Label>--%>
                                                                        </th>
                                                                        <th width="5%">
                                                                            <%--<asp:Label ID="lbl" runat="server" Text="<% $Resources:lblEmailID %>"></asp:Label>--%>
                                                                        </th>
                                                                    </tr>
                                                                   <%-- </table>--%>
                                                            </headertemplate>
                                                            <itemtemplate>
                                                              <%--<table width="100%"  border="0" cellspacing="0" cellpadding="0"
                                                                    class="grid">--%>
                                                                <tr>
                                                                    <td>
                                                                        <%# Container.ItemIndex + 1  %>.
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRelationship" runat="server" ValidationGroup="VGroup" AppendDataBoundItems="True">
                                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hddRelationShip" Value='<%# Eval("Relationship") %>' runat="server" />
                                                                        <asp:RequiredFieldValidator ID="rqfRelationShip" ControlToValidate="ddlRelationship"
                                                                            ErrorMessage="*" Display="Dynamic" InitialValue="0" ValidationGroup="VGroup"
                                                                            runat="server"> </asp:RequiredFieldValidator>
                                                                        <%--<asp:TextBox ID="txtRelationship" Text='<%# Eval("Relationship") %>' runat="server"></asp:TextBox>--%>
                                                                        <%--<asp:RequiredFieldValidator ID="rqval1" runat="server" ValidationGroup="group1" ErrorMessage="Please enter relationship"
                                                                            SetFocusOnError="True" ControlToValidate="txtRelationship">&nbsp;</asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtName" Style="width: 150px" runat="server" ValidationGroup="VGroup"
                                                                            Text='<%# Eval("Name") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfqTxtName" ControlToValidate="txtName" ErrorMessage="*"
                                                                            Display="Dynamic" ValidationGroup="VGroup" runat="server"> </asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEmailID" Style="width: 255px" SkinID="email" ValidationGroup="VGroup"
                                                                            runat="server" Text='<%# Eval("EmailID") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfqTxtEmailID" ControlToValidate="txtEmailID" ErrorMessage="*"
                                                                            Display="Dynamic" ValidationGroup="VGroup" runat="server"> </asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imbSaveColleague" ImageUrl="../../Layouts/Resources/images/saveIcon.png"
                                                                            Visible="false" CausesValidation="true" ValidationGroup="VGroup" CommandName="Assign"
                                                                            runat="server" />
                                                                        <asp:ImageButton ID="imbSaveOnlyColleague" ImageUrl="../../Layouts/Resources/images/saveIcon.png"
                                                                            Visible="false" CausesValidation="true" AlternateText="Save Only" ValidationGroup="VGroup"
                                                                            CommandName="Save" runat="server" />
                                                                        <asp:Image ID="imgColleagueSaved" src="../../Layouts/Resources/images/tick.png" Visible="false"
                                                                            runat="server" AlternateText="Save and Email" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCompletion" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imbReSendEmail" ImageUrl="../../Layouts/Resources/images/sendemail.png"
                                                                            CommandName="SendEmail" ToolTip="Re-send Email" runat="server" AlternateText="Re-send Email" />
                                                                        <asp:Label ID="lblAssignID" runat="server" Text='<%# Eval("AssignID") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblAccountID" runat="server" Text='<%# Eval("AccountID") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblTargetPersonID" runat="server" Text='<%# Eval("TargetPersonID") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblAssignmentID" runat="server" Text='<%# Eval("AssignmentID") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblProjectID" runat="server" Text='<%# Eval("ProjectID") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblEmailSendFlag" runat="server" Text='<%# Eval("EmailSendFlag") %>'
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imbDeleteColleague" ImageUrl="../../Layouts/Resources/images/delete.png"
                                                                            CommandName="Delete" CommandArgument='<%# Eval("AssignmentID")%>' ToolTip="Delete"
                                                                            runat="server" AlternateText="Delete" />
                                                                    </td>
                                                                </tr>
                                                               
                                                            </itemtemplate>
                                                            <footertemplate>
                                                              </table>
                        </footertemplate> </asp:repeater>
                    </div>
                    </td> </tr> </table> </fieldset> </td> </tr>
                    <tr>
                        <td>
                            <label id="lblReportMSG" text="" style="font-weight: bold" runat="server" visible="false">
                            </label>
                            <asp:imagebutton id="imbReportDownload" imageurl="~/Layouts/Resources/images/pdf.jpg"
                                onclick="imbReportDownload_Click" runat="server" visible="false" />
                        </td>
                    </tr>
                    </table>
                    <div>
                        <table width="100%" style="margin-top: 5px;">
                            <tr style="margin-bottom: 5px;">
                                <td width="50%">
                                    <span class="style3">
                                        <asp:label id="lblMandatory" runat="server" text="<% $Resources:lblMandatory %>"></asp:label></span>
                                </td>
                                <td rowspan="4">
                                    <asp:imagebutton id="imbSelfAssessment" enabled="false" imageurl="~/Layouts/Resources/images/self-assessment-btn.png"
                                        tooltip="Self Assessment" runat="server" causesvalidation="false" onclick="imbSelfAssessment_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="style3">
                                        <asp:label id="lblAddMore" runat="server" text="<% $Resources:lblAddMore %>"></asp:label></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="style3 imagealign">
                                        <asp:label id="lblResendEmail1" runat="server" text="<% $Resources:lblResendEmail1 %>">
                                        </asp:label>
                                        <img src="../../Layouts/Resources/images/sendemail.png" />
                                        <asp:label id="lblResendEmail2" runat="server" text="<% $Resources:lblResendEmail2 %>">
                                        </asp:label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="style3 imagealign">
                                        <asp:label id="lblDelete1" runat="server" text="<% $Resources:lblDelete1 %>"></asp:label>
                                        <img src="../../Layouts/Resources/images/delete.png" style="margin-top: 2px;" />
                                        <asp:label id="lblDelete2" runat="server" text="<% $Resources:lblDelete2 %>"></asp:label>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div align="center">
                        <%--<asp:ImageButton ID="imbSelfAssessment" Enabled="false" ImageUrl="~/Layouts/Resources/images/self-assessment-btn.png" ToolTip="Self Assessment"
                                runat="server"  CausesValidation="false" OnClick="imbSelfAssessment_Click" />--%>
                        <%--&nbsp;
                            <asp:ImageButton ID="imbAssign" ImageUrl="~/Layouts/Resources/images/Save.png" ToolTip="Assign"
                                runat="server" OnClick="imbAssign_Click" ValidationGroup="group1" />&nbsp;
                            <asp:ImageButton ID="imbReset" ImageUrl="~/Layouts/Resources/images/reset.png" ToolTip="Reset"
                                runat="server" OnClick="imbReset_Click" />&nbsp;
                            <asp:ImageButton ID="imbView" ImageUrl="~/Layouts/Resources/images/View-Candidate.png"
                                ToolTip="View Colleagues' List" runat="server" OnClick="imbView_Click" />--%>
                    </div>
                    <br />
                    <div align="center">
                       <%-- <asp:updateprogress id="Up1" runat="Server">
                            <progresstemplate>
                                    <img src="../../Layouts/Resources/images/loading.gif" alt="Please wait..." />
                                </progresstemplate>
                        </asp:updateprogress>--%>
                        <asp:label id="lblMessage2" runat="server" forecolor="Red" text=""></asp:label>
                    </div>
                    <br />
                </div>
                <!-- start user form -->
            </div>
            </div>
            <%--
            <cc1:ModalPopupExtender ID="mp1" runat="server" TargetControlID="imbSelfAssessment" PopupControlID="pnlSelfAssessment" Enabled="true"
    CancelControlID="Button2" BackgroundCssClass="Background" >
</cc1:ModalPopupExtender>

<asp:Panel ID="pnlSelfAssessment" CssClass="Popup" style="width: 850px; height: 500px;display:none"  runat="server" align="center" >
    <iframe id="ifSelfAssessment" style="width: 850px; height: 475px;"  src="../../Module/Feedback/Feedback.aspx" runat="server"></iframe>
   <br/>
    <asp:Button ID="Button2" runat="server" Text="Close" />
</asp:Panel>--%>
       <%-- </contenttemplate>
        <triggers>
            <asp:asyncPostBackTrigger ControlID="ImgUpload" />
            <asp:asyncPostBackTrigger ControlID="imbReportDownload" />
            <asp:asyncPostBackTrigger ControlID="ImageButtonSaveAll" />
             <asp:asyncPostBackTrigger ControlID="ddlProgramme" />
             <asp:asyncPostBackTrigger ControlID="ddlTargetPerson" />
            
        </triggers>
    </asp:updatepanel>--%>
    <div>
        <asp:chart id="Chart1" runat="server" width="810px" height="370px" visible="false"
            imagelocation="~/TempImages/ChartPic_#SEQ(300,3)" imagetype="Png" palette="None"
            borderdashstyle="none" borderwidth="2">
            <titles>
                <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" Text="" ForeColor="26, 59, 105">
                </asp:Title>
            </titles>
            <legends>
                <asp:Legend IsTextAutoFit="False" Name="Default" BackColor="#FAFA9D" Font="Trebuchet MS, 8.25pt, style=Bold"
                    Alignment="Far" BorderColor="LightGray" BorderWidth="4" ShadowColor="LightGray"
                    ShadowOffset="5" ItemColumnSpacing="20">
                    <Position Y="18.08253" Height="12.23021" Width="26.34047" X="72.73474"></Position>
                </asp:Legend>
            </legends>
            <borderskin skinstyle="None" backcolor="White"></borderskin>
            <%--<series >
								<asp:Series MarkerBorderColor="64, 64, 64" MarkerSize="9" Name="Series1" ChartType="Radar" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="1"></asp:Series>
								<asp:Series MarkerBorderColor="64, 64, 64" MarkerSize="9" Name="Series2" ChartType="Radar" BorderColor="180, 26, 59, 105" Color="220, 252, 180, 65" ShadowOffset="1"></asp:Series>
							</series>--%>
            <chartareas>
                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                    BackColor="White" ShadowColor="Transparent">
                    <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                        WallWidth="0" IsClustered="False" />
                    <Position Y="15" Height="90" Width="78" X="5"></Position>
                    <%--<AxisY LineColor="64, 64, 64, 64" Minimum="3" Maximum="10" Interval="1">--%>
                    <AxisY LineColor="64, 64, 64, 64" Interval="1">
                        <%-- Change Scale & CategoryName FontSize and FontColor(here:ForeColor="#666666") select Below labelstyle go property --%>
                        <LabelStyle Font="Trebuchet MS, 7.25pt, style=Bold" IntervalOffsetType="Number" IntervalType="Number"
                            ForeColor="#333333" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                        <MajorTickMark Size="0.1" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 7.25pt, style=Bold" ForeColor="#333333" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                </asp:ChartArea>
            </chartareas>
        </asp:chart>
    </div>
                                        </table>
                                        </table>
</asp:content>

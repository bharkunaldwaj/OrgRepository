<%@ Page Title="Set up your colleagues" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" MaintainScrollPositionOnPostback="true"
     AutoEventWireup="true" CodeFile="AssignQuestionnaire.aspx.cs" Inherits="Module_Questionnaire_AssignQuestionnaire" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>

    <cc1:ToolkitScriptManager  ID="ScriptManager1" runat="server">
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
<asp:HiddenField ID="hdnProjectId" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="updPanel" runat="server">    
        <ContentTemplate>
        <input type="hidden" id="div_position" name="div_position" />
        <rsweb:ReportViewer ID="rview" runat="server" Height="0" Width="">
                    </rsweb:ReportViewer>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblTopHeading %>"
                                align="absmiddle" />                                
                            <asp:Label ID="lblTopHeading" runat="server" Text="<% $Resources:lblTopHeading %>"></asp:Label>
                            <asp:ImageButton ID="ibtnHelp" runat="server" title="Help" CssClass="HeadingInfoHelp" OnClientClick="ShowPopup();"
                                                    ImageUrl="~/Layouts/Resources/images/help.png" />
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
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="VGroupX" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="group2" />
                                    <asp:Label ID="lblvalidation" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <%--<br />--%>
                                    <div align="center">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="Large" Text=""></asp:Label>
                                    </div>
                                <%--<br />--%>

                                <asp:Label ID="lblInstruction" runat="server" Text=""></asp:Label>

                               <%-- <b>
                                <asp:Label ID="lblInstruction" runat="server" Text="<% $Resources:lblInstruction %>"></asp:Label>
                                
                                <br /><asp:Label ID="lblInstruction1" runat="server" Text="<% $Resources:lblInstruction1 %>"></asp:Label>  
                                <br /><asp:Label ID="lblInstruction2" runat="server" Text="<% $Resources:lblInstruction2 %>"></asp:Label>
                                <br /><asp:Label ID="lblInstruction3" runat="server" Text="<% $Resources:lblInstruction3 %>"></asp:Label>
                                <br /><asp:Label ID="lblInstruction4" runat="server" Text="<% $Resources:lblInstruction4 %>"></asp:Label> 
                                <br /><asp:Label ID="lblInstruction5" runat="server" Text="<% $Resources:lblInstruction5 %>"></asp:Label>  
                                </b>--%>
                            </td>
                        </tr>
                    </table>
                    <div class="userform">
                        <div id="divAccount" runat="server" visible="false">
                            <fieldset class="fieldsetform">
                                <legend><asp:Label ID="lblAccountDetails" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="VGroup" ErrorMessage="<% $Resources:Rq21 %>  "
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
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:Label ID="lblGeneralDetails" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                            
                                            <tr>
                                                <td width="15%" valign="top">
                                                    <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="35%" valign="top">
                                                    <asp:Label ID="lblProjectText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:Rq1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="VGroupX"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                    
                                                </td>
                                                <td width="15%" valign="top">
                                                </td>
                                                <td width="35%" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProgramme" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblProgrammeText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" 
                                                        AppendDataBoundItems="True" AutoPostBack="true"
                                                        onselectedindexchanged="ddlProgramme_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:RequiredFieldValidator1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="VGroupX"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <%--<asp:Label ID="lblQuestionnaire" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label><span
                                                        class="style3">*</span>--%>
                                                </td>
                                                <td>
                                                    <%--<asp:Label ID="lblQuestionnaireText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq2" runat="server" ErrorMessage="<% $Resources:Rq2 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlQuestionnaire" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr id="trTargetPerson" runat="server">
                                                <td valign="top">
                                                    <asp:Label ID="lblTargetPerson" runat="server" Text="<% $Resources:lblTargetPerson %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td valign="top" colspan="3">
                                                    <asp:DropDownList ID="ddlTargetPerson" runat="server" lSetFocusOnError="True" Display="None" AutoPostBack="true"
                                                        Style="width: 155px" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTargetPerson_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq3" runat="server" ErrorMessage="<% $Resources:Rq3 %>"
                                                        ControlToValidate="ddlTargetPerson" SetFocusOnError="True" ValidationGroup="VGroupX" Display="Dynamic"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
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
                            <tr>
                                <td valign="top">
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:Label ID="lblCandidateList" runat="server" Text="<% $Resources:lblCandidateList %>"></asp:Label></legend>
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
                                                <td colspan="3" >
                                                    <asp:ImageButton ID="imbSubmit" ImageUrl="~/Layouts/Resources/images/add-more-btn.png"
                                                        runat="server" OnClick="imbSubmit_Click"/>
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
                                                                    <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;&nbsp;<span class="style3">
                                                                    <asp:Label ID="lblExcelType" runat="server" Text="<% $Resources:lblExcelType %>"></asp:Label></span>
                                                                </label>
                                                            </td>
                                                            <td width="15%">
                                                                <asp:ImageButton ID="ImgUpload" runat="server" ImageUrl="~/Layouts/Resources/images/import-s.png"
                                                                    OnClick="ImgUpload_click" OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" valign="top">
                                                    <div id="divCandidateList" class="candidatelist" style="height:242px">
                                                        <asp:Repeater ID="rptrCandidateList" OnItemDataBound="rptrCandidateList_ItemDataBound" OnItemCommand="rptrCandidateList_ItemCommand"
                                                            runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
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
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%# Container.ItemIndex + 1  %>.
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRelationship" runat="server" ValidationGroup="VGroup" AppendDataBoundItems="True">
                                                                            <asp:ListItem Value="0" >Select</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hddRelationShip" Value='<%# Eval("Relationship") %>' runat="server" />
                                                                        <asp:RequiredFieldValidator ID="rqfRelationShip" ControlToValidate="ddlRelationship" 
                                                                        ErrorMessage="*" Display=Dynamic InitialValue="0" ValidationGroup="VGroup" runat = "server">
                                                                        </asp:RequiredFieldValidator>
                                                                                        <%--<asp:TextBox ID="txtRelationship" Text='<%# Eval("Relationship") %>' runat="server"></asp:TextBox>--%>
                                                                        <%--<asp:RequiredFieldValidator ID="rqval1" runat="server" ValidationGroup="group1" ErrorMessage="Please enter relationship"
                                                                            SetFocusOnError="True" ControlToValidate="txtRelationship">&nbsp;</asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtName" style="width:150px" runat="server"  ValidationGroup="VGroup" Text='<%# Eval("Name") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfqTxtName" ControlToValidate="txtName" 
                                                                        ErrorMessage="*" Display=Dynamic ValidationGroup="VGroup" runat = "server">
                                                                        </asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEmailID" style="width:255px" SkinID="email"  ValidationGroup="VGroup" runat="server" Text='<%# Eval("EmailID") %>'></asp:TextBox>
                                                                         <asp:RequiredFieldValidator ID="rfqTxtEmailID" ControlToValidate="txtEmailID" 
                                                                        ErrorMessage="*" Display=Dynamic ValidationGroup="VGroup" runat = "server">
                                                                        </asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <%--<td width="5%">
                                                                        <asp:Label ID="lblSubmitStatus" Text='<%# Eval("SubmitFlag") %>' runat="server"></asp:Label>
                                                                    </td>--%>
                                                                    <td>
                                                                      
                                                                        <asp:ImageButton ID="imbSaveColleague" ImageUrl="../../Layouts/Resources/images/saveIcon.png"
                                                                            Visible="false" CausesValidation="true" ValidationGroup="VGroup"  CommandName="Assign" runat="server" />
                                                                        <asp:ImageButton ID="imbSaveOnlyColleague" ImageUrl="../../Layouts/Resources/images/saveIcon.png"
                                                                            Visible="false" CausesValidation="true" AlternateText="Save Only" ValidationGroup="VGroup"  CommandName="Save" runat="server" />
                                                                        <asp:Image ID="imgColleagueSaved" src="../../Layouts/Resources/images/tick.png" Visible="false"
                                                                            runat="server" AlternateText="Save and Email"/>
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
                                                                            CommandName="Delete" CommandArgument='<%# Eval("AssignmentID")%>' ToolTip="Delete" runat="server" AlternateText="Delete" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblReportMSG" text="" style="font-weight:bold" runat="server" visible="false"></label>
                                    <asp:ImageButton ID="imbReportDownload" ImageUrl="~/Layouts/Resources/images/pdf.jpg" OnClick="imbReportDownload_Click" runat="server" visible="false"/>
                                </td>
                            </tr>
                        </table>
                        <div>
                        <table width="100%" style="margin-top:5px;">
                            <tr style="margin-bottom:5px;">
                                <td width="50%">
                                <span class="style3" ><asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                                </td>
                                <td rowspan="4"><asp:ImageButton ID="imbSelfAssessment" Enabled="false" ImageUrl="~/Layouts/Resources/images/self-assessment-btn.png" ToolTip="Self Assessment"
                                runat="server"  CausesValidation="false" OnClick="imbSelfAssessment_Click" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="style3"><asp:Label ID="lblAddMore" runat="server" Text="<% $Resources:lblAddMore %>"></asp:Label></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="style3 imagealign"><asp:Label ID="lblResendEmail1" runat="server" Text="<% $Resources:lblResendEmail1 %>"></asp:Label>
                                    <img src="../../Layouts/Resources/images/sendemail.png" />
                                    <asp:Label ID="lblResendEmail2" runat="server" Text="<% $Resources:lblResendEmail2 %>"></asp:Label>
                                    </span>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="style3 imagealign"><asp:Label ID="lblDelete1" runat="server" Text="<% $Resources:lblDelete1 %>"></asp:Label>
                                    <img src="../../Layouts/Resources/images/delete.png" style="margin-top:2px;" />
                                    <asp:Label ID="lblDelete2" runat="server" Text="<% $Resources:lblDelete2 %>"></asp:Label>
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
                            <asp:UpdateProgress ID="Up1" runat="Server" AssociatedUpdatePanelID="updPanel">
                            <ProgressTemplate>
                                <img src="../../UploadDocs/Send1.gif" alt="Please wait..." />
                            </ProgressTemplate>
                            </asp:UpdateProgress>                        
                            <asp:Label ID="lblMessage2" runat="server" ForeColor="Red" Text=""></asp:Label>
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

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgUpload" />
            <asp:PostBackTrigger ControlID="imbReportDownload" />
        </Triggers>        
       
    </asp:UpdatePanel>

    <div>
         <asp:Chart ID="Chart1" runat="server" Width="810px" Height="370px" Visible="false"
                                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png" Palette="None"
                                BorderDashStyle="none" BorderWidth="2">
                                <Titles>
                                    <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" Text="" ForeColor="26, 59, 105">
                                    </asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend IsTextAutoFit="False" Name="Default" BackColor="#FAFA9D" Font="Trebuchet MS, 8.25pt, style=Bold"
                                        Alignment="Far" BorderColor="LightGray" BorderWidth="4" ShadowColor="LightGray"
                                        ShadowOffset="5" ItemColumnSpacing="20">
                                        <Position Y="18.08253" Height="12.23021" Width="26.34047" X="72.73474"></Position>
                                    </asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="None" BackColor="White"></BorderSkin>
                                <%--<series >
								<asp:Series MarkerBorderColor="64, 64, 64" MarkerSize="9" Name="Series1" ChartType="Radar" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="1"></asp:Series>
								<asp:Series MarkerBorderColor="64, 64, 64" MarkerSize="9" Name="Series2" ChartType="Radar" BorderColor="180, 26, 59, 105" Color="220, 252, 180, 65" ShadowOffset="1"></asp:Series>
							</series>--%>
                                <ChartAreas>
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
                                </ChartAreas>
                            </asp:Chart>
    </div>


</asp:Content>

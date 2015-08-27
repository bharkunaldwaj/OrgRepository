<%@ Page Title="Set up your colleagues" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="AssignQuestionnaire.aspx.cs" Inherits="Module_Questionnaire_AssignQuestionnaire" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
   
    <script language="javascript" type="text/javascript">

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
    
    
    
    
   <%-- <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="Survey_topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblTopHeading %>"
                                align="absmiddle" />
                            <asp:Label ID="lblTopHeading" runat="server" Text="<% $Resources:lblTopHeading %>"></asp:Label>
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
                                        ShowSummary="true" ValidationGroup="group1" />
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
                                <b>
                                <asp:Label ID="lblInstruction" runat="server" Text="<% $Resources:lblInstruction %>"></asp:Label>
                                
                                <br /><asp:Label ID="lblInstruction1" runat="server" Text="<% $Resources:lblInstruction1 %>"></asp:Label>  
                                <br /><asp:Label ID="lblInstruction2" runat="server" 
                                    Text="<%$ Resources:lblInstruction2 %>"></asp:Label>
                                <br /><asp:Label ID="lblInstruction3" runat="server" Text="<% $Resources:lblInstruction3 %>"></asp:Label>
                                <br /><asp:Label ID="lblInstruction4" runat="server" Text="<% $Resources:lblInstruction4 %>"></asp:Label> 
                                <br /><asp:Label ID="lblInstruction5" runat="server" Text="<% $Resources:lblInstruction5 %>"></asp:Label>  
                                </b>
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>  "
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
                                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="group1"
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
                                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblQuestionnaire" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblQuestionnaireText" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" 
                                                        AppendDataBoundItems="True" 
                                                        >
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq2" runat="server" ErrorMessage="<% $Resources:Rq2 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlQuestionnaire" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr id="trTargetPerson" runat="server">
                                                <td valign="top">
                                                    &nbsp;</td>
                                                <td valign="top">
                                                    
                                                </td>
                                                <td valign="top">
                                                </td>
                                                <td valign="top">
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
                                                <td width="15%" valign="top">
                                                    <asp:Label ID="lblCandidateNumber" runat="server" Text="<% $Resources:lblCandidateNumber %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="10%" valign="top">
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
                                                </td>
                                                <td width="25%" valign="top">
                                                    <asp:ImageButton ID="imbSubmit" ImageUrl="~/Layouts/Resources/images/submit-s.png"
                                                        runat="server" OnClick="imbSubmit_Click" ValidationGroup="group2" 
                                                        Height="21px" />
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
                                                                <br />
                                                                <a href="../../UploadDocs/Survey_Sample_Participant.xlsx">
                                        <asp:Label ID="Label4" runat="server" Text="Download Sample Excel"></asp:Label></a>
                                                                
                                                            </td>
                                                            <td width="15%" valign="top">
                                                                <asp:ImageButton ID="ImgUpload" runat="server" ImageUrl="~/Layouts/Resources/images/import-s.png"
                                                                    OnClick="ImgUpload_click" OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" valign="top">
                                                    <div class="candidatelist">
                                                      <asp:Repeater ID="rptrCandidateList" OnItemDataBound="rptrCandidateList_ItemDataBound" runat="server">
                                                            <HeaderTemplate>
                                                               <asp:Table ID="Table1" runat="server" width="100%" BorderWidth="0" cellspacing="0" cellpadding="0" Class="grid">
                                                                    <asp:TableRow runat="server" ID="HeaderRow">
                                                                    <asp:TableHeaderCell width="6%" ID="HeaderCell1" runat="server">
                                                                            <asp:Label ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                                        </asp:TableHeaderCell>
                                                                        <asp:TableHeaderCell width="12%" ID="HeaderCell2" runat="server">
                                                                            <asp:Label ID="lblAnalysis1" runat="server" Text="<% $Resources:lblAnalysis1 %>"></asp:Label>
                                                                        </asp:TableHeaderCell>
                                                                        <asp:TableHeaderCell width="12%" ID="HeaderCell3" runat="server">
                                                                            <asp:Label ID="lblAnalysis2" runat="server" Text="<% $Resources:lblAnalysis2 %>"></asp:Label>
                                                                        </asp:TableHeaderCell>
                                                                        <asp:TableHeaderCell width="12%" ID="HeaderCell4" runat="server">
                                                                            <asp:Label ID="lblAnalysis3" runat="server" Text="<% $Resources:lblAnalysis3 %>"></asp:Label>
                                                                        </asp:TableHeaderCell>
                                                                        <asp:TableHeaderCell width="20%" ID="HeaderCell5" runat="server">
                                                                            <asp:Label ID="lblName" runat="server" Text="<% $Resources:lblName %>"></asp:Label>
                                                                        </asp:TableHeaderCell>
                                                                        <asp:TableHeaderCell width="32%" ID="HeaderCell6" runat="server">
                                                                            <asp:Label ID="lblEmailID" runat="server" Text="<% $Resources:lblEmailID %>"></asp:Label>
                                                                        </asp:TableHeaderCell>
                                                                    </asp:TableRow>
                                                                    </asp:Table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            <asp:Table ID="Table2" runat="server" Class="grid" Width="100%">
                                                                 <asp:TableRow ID="ItemRow">
                                                                 <asp:TableCell width="6%" ID="ItemCell1" runat="server">
                                                                        <%# Container.ItemIndex + 1  %>.
                                                                    </asp:TableCell>
                                                                    <asp:TableCell width="12%" ID="ItemCell2" runat="server">
                                                                        <asp:DropDownList ID="ddlAnalysis1" runat="server" AppendDataBoundItems="True">
                                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                       
                                                                    </asp:TableCell>
                                                                    
                                                                    
                                                                     <asp:TableCell width="12%" ID="ItemCell3" runat="server">
                                                                        <asp:DropDownList ID="ddlAnalysis2" runat="server" AppendDataBoundItems="True">
                                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                       
                                                                    </asp:TableCell>
                                                                    
                                                                    <asp:TableCell width="12%" ID="ItemCell4" runat="server">
                                                                        <asp:DropDownList ID="ddlAnalysis3" runat="server" AppendDataBoundItems="True">
                                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                       
                                                                    </asp:TableCell>
                                                                    
                                                                    <asp:TableCell width="20%" ID="ItemCell5" runat="server">
                                                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                                                                    </asp:TableCell>
                                                                    
                                                                    <asp:TableCell width="32%" ID="ItemCell6" runat="server">
                                                                        <asp:TextBox ID="txtEmailID" SkinID="email" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:TextBox>
                                                                    </asp:TableCell>
                                                                    
                                                               </asp:TableRow>
                                                               </asp:Table>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                               
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <span class="style3"><asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <br />
                        <div align="center">
                            <asp:ImageButton ID="imbAssign" Enabled="false" ImageUrl="~/Layouts/Resources/images/Save.png" ToolTip="Assign"
                                runat="server" OnClick="imbAssign_Click" ValidationGroup="group1" />&nbsp;
                            <asp:ImageButton ID="imbReset" ImageUrl="~/Layouts/Resources/images/reset.png" ToolTip="Reset"
                                runat="server" OnClick="imbReset_Click" />&nbsp;
                            <asp:ImageButton ID="imbView" ImageUrl="~/Layouts/Resources/images/view-participants.png"
                                ToolTip="View Participant's List" runat="server" OnClick="imbView_Click" />
                        </div>
                        <br />
                        <div align="center">
                        
                     <%--   <asp:UpdateProgress ID="Up1" runat="Server" AssociatedUpdatePanelID="updPanel" DynamicLayout="false">
    <ProgressTemplate>
        <img src="../../UploadDocs/Send1.gif" alt="Please wait..." />
    </ProgressTemplate>
    </asp:UpdateProgress>--%>
                       
                            <asp:Label ID="lblMessage2" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                        <br />
                    </div>
                    <!-- start user form -->
                </div>
            </div>
      <%--  </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="ImgUpload" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

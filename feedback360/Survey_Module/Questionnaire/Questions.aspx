


      <%@ Page Title="Create New Question" Language="C#" 

MasterPageFile="~/Layouts/MasterPages/Survey.master"  AutoEventWireup="true" CodeFile="Questions.aspx.cs" 

Inherits="Survey_Module_Questionnaire_Questions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">

<script type="text/javascript">

        function ChangeSettings() {
            var index =
document.getElementById('ctl00_cphMaster_ddlQuestionType').selectedIndex;

            if
(document.getElementById('ctl00_cphMaster_ddlQuestionType').options[index].value == "1") {
                document.getElementById('divFreeText').style.display = "block";
                document.getElementById('divRange').style.display = "none";
            }
            else if
(document.getElementById('ctl00_cphMaster_ddlQuestionType').options[index].value == "2") {
                document.getElementById('divFreeText').style.display = "none";
                document.getElementById('divRange').style.display = "block";
            }
        }
    
    </script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img id="Img1" src="../../Layouts/Resources/images/create_question.png" 

runat="server" alt="<% $Resources:lblToolTip %>"
                        align="absmiddle" />
                     <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblHeading 

%>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
           <%-- <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
                    <!-- start user form -->
                    
                    <table border="0" width="100%">
            <tr>
            <td > <div id="summary"  runat="server" class="validation-align">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            DisplayMode="BulletList"  ShowSummary="true" ValidationGroup = 

"group1"  />
            </div></td>            
            </tr>
            </table>
                    
                    
                    
                  
                    <div class="userform">
                    
                    
                       <div id="divAccount" runat="server" visible="false">
                    <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblAccountDetail" runat="server" Text="<% 

$Resources:lblAccountDetail %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblAccountcode" runat="server" Text="<% 

$Resources:lblAccountcode %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td width="36%">
                                <asp:DropDownList ID="ddlAccountCode" runat="server" 

Style="width: 155px" 
                                    AppendDataBoundItems="True" 
                                    

onselectedindexchanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true" >
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Rq21" runat="server" 

ValidationGroup="group1" ErrorMessage=" Please Select Account Code Details "
                                    SetFocusOnError="True" 

ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td width="13%">
                                <asp:Label ID="lblCompany" runat="server" Text="<% 

$Resources:lblCompany %>"></asp:Label>
                            </td>
                            <td width="38%">
                                 <asp:Label ID="lblcompanyname" runat="server"  
 

Text=""></asp:Label>
                            </td>
                        </tr>
                       
                    </table>
                </fieldset>
                </div>
                    
                    
                    
                    
                    
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="lblGeneralDetail" runat="server" Text="<% 

$Resources:lblGeneralDetail %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblQuestionnaire" runat="server" 

Text="<% $Resources:lblQuestionnaire %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlQuestionnaire" 

runat="server" Style="width: 155px" 
                                            AppendDataBoundItems="True" AutoPostBack="true" 
                                            

onselectedindexchanged="ddlQuestionnaire_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq5" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq5 %>"
                                            SetFocusOnError="True" 

ControlToValidate="ddlQuestionnaire" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblQuestionCategory" runat="server" 

Text="<% $Resources:lblQuestionCategory %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlQuestionCategory" 

runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq2" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq2 %>"
                                            SetFocusOnError="True" 

ControlToValidate="ddlQuestionCategory" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSequence" runat="server" Text="<% 

$Resources:lblSequence %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSequence" SkinID="age" 

onKeyPress="return NumberOnly(this);"
                                            MaxLength="2" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq8" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq8 %>"
                                            SetFocusOnError="True" 

ControlToValidate="txtSequence">&nbsp;</asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator
                                        ID="regsequence" ControlToValidate="txtSequence" 

ErrorMessage="Please enter number in sequence " ValidationExpression="^[0-9][\d]*"
                                        runat="server" ValidationGroup="group1" 

SetFocusOnError="True" Text="*" ForeColor="White" />
                                    <asp:RangeValidator id="RangeValidator2" 

ControlToValidate="txtSequence" Type="Integer" MinimumValue="1" MaximumValue="99" 

ErrorMessage="<% $Resources:RangeValidator2 %>"   ValidationGroup="group1" 

SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />
                                   
                                   
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValidation" runat="server" 

Text="<% $Resources:lblValidation %>"></asp:Label><span class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlValidation" runat="server" 

Style="width: 155px">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">None</asp:ListItem>
                                            <asp:ListItem Value="2">Light</asp:ListItem>
                                            <asp:ListItem Value="3">Strong</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq3" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq3 %>"
                                            SetFocusOnError="True" 

ControlToValidate="ddlValidation" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td width="13%">
                                    </td>
                                    <td width="36%">
                                       
                                    </td>
                                    <td width="14%">
                                        <asp:Label ID="lblTitle" runat="server" Text="<% 

$Resources:lblTitle %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="37%">
                                     <asp:TextBox ID="txtTitle" MaxLength="25" 

runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq7" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq7 %>"
                                            SetFocusOnError="True" 

ControlToValidate="txtTitle">&nbsp;</asp:RequiredFieldValidator>
                                        
                                    </td>
                                </tr>--%>
                                <tr>
                                   <td>
                                        <asp:Label ID="lblQuestionType" runat="server" 

Text="<% $Resources:lblQuestionType %>"></asp:Label>&nbsp;<span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlQuestionType" 

runat="server" AppendDataBoundItems="True" AutoPostBack="true"
                                            

OnSelectedIndexChanged="ddlQuestionType_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">Free Text</asp:ListItem>
                                            <asp:ListItem Value="2">Range</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq1 %>"
                                            SetFocusOnError="True" 

ControlToValidate="ddlQuestionType" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        &nbsp;&nbsp;
                                        <asp:DropDownList ID="DropDownList1" runat="server"  
                                            DataTextField="Range_Name" Visible="False" DataValueField="Range_Name" 
                                            >
                                        </asp:DropDownList>
                                     
                                      <%--  <asp:LinkButton ID="Lbtn_select_range" 

runat="server" Visible="False">Select the range</asp:LinkButton>--%>
                                        &nbsp;&nbsp;
                                    <%--    <asp:LinkButton ID="Lbtn_change" 

runat="server" Visible="False">(Change)</asp:LinkButton>--%>
                                        <%--<asp:CustomValidator ID="CustomValidator1" runat="server" 
                                            ControlToValidate="DropDownList1" ErrorMessage="Range selection is mandatory." 
                                            onservervalidate="CustomValidator1_ServerValidate" Display="Dynamic">Range selection is mandatory.</asp:CustomValidator>--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Enabled="false" runat="server" 

ValidationGroup="group1" ErrorMessage="Range selection is mandatory."
                                            SetFocusOnError="True" 

ControlToValidate="DropDownList1" InitialValue="0" >&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="14%">
                                        <asp:Label ID="lblTokens" runat="server" Text="<% 

$Resources:lblTokens %>"></asp:Label><span
                                            class="style3"></span>
                                    </td>
                                    <td width="37%">
                                        <asp:DropDownList ID="ddlTokens" runat="server" 

Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">First 

Name</asp:ListItem>
                                            <asp:ListItem Value="2">Last Name</asp:ListItem>
                                            <asp:ListItem Value="3">Full Name</asp:ListItem>
                                        </asp:DropDownList>
<%--                                        <asp:RequiredFieldValidator ID="Rq4" 

runat="server" ValidationGroup="group1" ErrorMessage=" Please Select Tokens Details "
                                            SetFocusOnError="True" 

ControlToValidate="ddlTokens" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblQuestionText" runat="server" 

Text="<% $Resources:lblQuestionText %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtQuestionText" runat="server" 

SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" />
                                        <asp:RequiredFieldValidator ID="Rq17" runat="server" 

ValidationGroup="group1" ErrorMessage="<% $Resources:Rq17 %>"
                                            SetFocusOnError="True" 

ControlToValidate="txtQuestionText">&nbsp;</asp:RequiredFieldValidator>
                                        <div class="maxlength-msg"><asp:Label 

id="lblCharactersLimit"  runat="server" Text="<% $Resources:lblCharactersLimit 

%>"></asp:Label></div>
                                    </td>
                                    <td valign="top">
                                        <div>
                                            <asp:Label ID="lblUsageHint" runat="server" 

Text="<% $Resources:lblUsageHint %>"></asp:Label></div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUsageHint" runat="server" 

SkinID="txtarea300X3" Rows="3" TextMode="MultiLine"
                                             />
                                        <div class="maxlength-msg"><asp:Label 

id="lblCharactersLimit1"  runat="server" Text="<% $Resources:lblCharactersLimit1 

%>"></asp:Label></div>
                                    </td>
                                </tr>
                                
                            </table>
                        </fieldset>
                        <div id="divFreeText" runat="server">
                            <fieldset class="fieldsetform">
                                <legend>Free Text Settings</legend>
                                <table width="100%" border="0" cellspacing="5" 

cellpadding="0">
                                    <tr>
                                        <td width="13%">
                                            <asp:Label ID="lblMinLength" runat="server" 

Text="<% $Resources:lblMinLength %>"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtMinLength" SkinID="age" 

onKeyPress="return NumberOnly(this);"
                                                MaxLength="3" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="Rq11" 

runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq11 %>"
                                                SetFocusOnError="True" 

ControlToValidate="txtMinLength">&nbsp;</asp:RequiredFieldValidator>
                                                
                                        <asp:RegularExpressionValidator
                                        ID="RegMinLgt" ControlToValidate="txtMinLength" 

ErrorMessage="<% $Resources:RegMinLgt %>" ValidationExpression="^[0-9][\d]*"
                                        runat="server" ValidationGroup="group1" 

SetFocusOnError="True" Text="*" ForeColor="White" />
                                        </td>
                                        <td width="13%">
                                            <asp:Label ID="lblMaxLength" runat="server" 

Text="<% $Resources:lblMaxLength %>"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="29%">
                                            <asp:TextBox ID="txtMaxLength" SkinID="age" 

onKeyPress="return NumberOnly(this);"
                                                MaxLength="3" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="Rq12" 

runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq12 %>"
                                                SetFocusOnError="True" 

ControlToValidate="txtMaxLength">&nbsp;</asp:RequiredFieldValidator>
                                                
                                                   <asp:RegularExpressionValidator
                                        ID="RegMaxLgt" ControlToValidate="txtMaxLength" 

ErrorMessage="<% $Resources:RegMaxLgt %>" ValidationExpression="^[0-9][\d]*"
                                        runat="server" ValidationGroup="group1" 

SetFocusOnError="True" Text="*" ForeColor="White" />
                                        </td>
                                        <td width="7%">
                                            <asp:Label ID="lblMultiline" runat="server" 

Text="<% $Resources:lblMultiline %>"></asp:Label>
                                        </td>
                                        <td width="8%">
                                            <asp:CheckBox ID="chkmultiline" runat="server" 

/>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <asp:Label ID="lblMandatory" runat="server" Text="<%$Resources:lblMandatory %>"></asp:Label>
                       
                        
                                    
                       
                       
                        
                        </div>
                        
                        
                        
                        
                        <div align="center">
                            <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="Save"
                                ImageUrl="~/Layouts/Resources/images/Save.png" OnClick="ibtnSave_Click" 
                                ValidationGroup="group1" />
                            &nbsp;
                            <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Back to List"
                                ImageUrl="~/Layouts/Resources/images/Cancel.png" OnClick="ibtnCancel_Click" />
                            <asp:ImageButton ID="imbBack" runat="server" CausesValidation="false" 
                                ImageUrl="~/Layouts/Resources/images/Back.png" ToolTip="Back to List"
                                PostBackUrl="~/Survey_Module/Questionnaire/QuestionList.aspx" Visible="false" />
                        </div>
                        
                        
                        
                        
                        
                    <!-- start user form -->
          <%--      </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>

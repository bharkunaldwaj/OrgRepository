<%@ Page Title="Create New Category" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="Category.aspx.cs" Inherits="Module_Questionnaire_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">

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

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/category_create.png" alt="Create New  Category"
                        align="absmiddle" />
                      <asp:Label ID="lblheader" runat="server" Text="Create New Category"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start user form -->
           <%-- <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
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
                    <div class="userform">
                        <div id="divAccount" runat="server" visible="false">
                            <fieldset class="fieldsetform">
                                <legend><asp:Label ID="Label2" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                                <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                    <tr>
                                        <td width="13%">
                                            <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="36%">
                                            <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>  "
                                                SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="13%">
                                            <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                        </td>
                                        <td width="38%">
                                            <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="Label3" runat="server" Text="<% $Resources:lblCategoryDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblQuestionnaire %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlQuestionnaire" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq5" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq5 %>  "
                                            SetFocusOnError="True" ControlToValidate="ddlQuestionnaire" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblSequence" runat="server" Text="<% $Resources:lblSequence %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtSequence" onKeyPress="return NumberOnly(this);" SkinID="grdvgoto"
                                            runat="server" MaxLength="2"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rq2" runat="server" ControlToValidate="txtSequence"
                                            ErrorMessage="<% $Resources:rq2 %>  " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                       <%-- <asp:RegularExpressionValidator ID="ressequence" ControlToValidate="txtSequence"
                                            ErrorMessage="<% $Resources:ressequence %>" ValidationExpression="^[0-9][\d]*" Display="Dynamic"
                                            runat="server" ValidationGroup="group1" SetFocusOnError="True" Text="Enter Number only." ForeColor="Red" />--%>
                                             <asp:RangeValidator id="valTxtRange" ControlToValidate="txtSequence" Type="Integer"
                                              MinimumValue="1" MaximumValue="99" ErrorMessage="<% $Resources:valTxtRange %>"   Display="Dynamic"
                                               ValidationGroup="group1" SetFocusOnError="True" Text="Enter Number only." ForeColor="Red" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" valign="top">
                                        <asp:Label ID="lblName" runat="server" Text="<% $Resources:lblName %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td width="35%" valign="top">
                                        <asp:TextBox ID="txtCategoryName" MaxLength="35" runat="server"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rq1" runat="server" ControlToValidate="txtCategoryName"
                                            ErrorMessage="<% $Resources:rq1 %>  " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="15%" valign="top">
                                        <asp:Label ID="lblTitle" runat="server" Text="<% $Resources:lblTitle %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td width="35%" valign="top">
                                        <asp:TextBox ID="txtCategoryTitle" MaxLength="50" runat="server"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rqCatTitle" runat="server" ControlToValidate="txtCategoryTitle"
                                            ErrorMessage="<% $Resources:rqCatTitle %>  " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblDescription" runat="server" Text="<% $Resources:lblDescription %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                                            SkinID="txtarea300X3" onkeypress="javascript:TextAreaMaxLengthCheck(this.id,999);"
                                            Text="" Rows="3" /><div class="maxlength-msg">
                                                <asp:Label ID="Label5" runat="server" Text="<% $Resources:divMaxChar %>"></asp:Label></div>
                                        &nbsp;<asp:RequiredFieldValidator ID="rq3" runat="server" ControlToValidate="txtDescription"
                                            ErrorMessage="Please Enter Description " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblExclude" runat="server" Text="<% $Resources:lblExclude %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:CheckBox ID="chkExcludeFromAnalysis" runat="server" />
                                    </td>
                                </tr>
                                <tr  id="ReportDescription" runat="server" visible="false">
                                <td valign="top">
                                        <asp:Label ID="LabeReportDescription" runat="server" Text="<% $Resources:Report_Category_Description %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                     <td valign="top">
                                        <asp:TextBox ID="TextBoxReportDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                                            SkinID="txtarea300X3" onkeypress="javascript:TextAreaMaxLengthCheck(this.id,500);"
                                            Text="" Rows="3" /><div class="maxlength-msg">
                                                <asp:Label ID="LabelReportDataCount" runat="server" Text="<% $Resources:Label_Max_500_Char%>"></asp:Label></div>
                                       <%-- &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidatorReportDescription" runat="server" ControlToValidate="TextBoxReportDescription"
                                            ErrorMessage="<% $Resources:Enter_Report_Description %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                                <tr >
                                <td valign="top">
                                        <asp:Label ID="LabelQuestionnaireDescription" runat="server" Text="<% $Resources:Questionnaire_Category_Description %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                     <td valign="top">
                                        <asp:TextBox ID="TextBoxQuestionnaireDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                                            SkinID="txtarea300X3" onkeypress="javascript:TextAreaMaxLengthCheck(this.id,500);"
                                            Text="" Rows="3" /><div class="maxlength-msg">
                                                <asp:Label ID="LabelQuestionnaireDataCount" runat="server" Text="<% $Resources:Label_Max_500_Char%>"></asp:Label></div>
                                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidatorQuestionnaireDescription" runat="server" ControlToValidate="TextBoxQuestionnaireDescription"
                                            ErrorMessage="<% $Resources:Enter_Questionnaire_Description %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                            </table>
                        </fieldset>
                        <span class="style3"><asp:Label ID="Label4" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <div align="center">
                            <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/Layouts/Resources/images/Save.png"
                                OnClick="ibtnSave_Click" ValidationGroup="group1" ToolTip="Save" />
                            &nbsp;
                            <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/Layouts/Resources/images/Cancel.png"
                                OnClick="ibtnCancel_Click" ToolTip="Back to List" />
                            <asp:ImageButton ID="imbBack" runat="server" CausesValidation="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                                PostBackUrl="~/Module/Questionnaire/CategoryList.aspx" ToolTip="Back to List" Visible="false" />
                        </div>
                        <br>
                        </br>
                        <br>
                        </br>
                            <div align="center">
                                <span class="style3">
                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>
                            </div>
                        
                    </div>
               <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            <!-- start user form -->
        </div>
    </div>
</asp:Content>

<%@ Page Title="Create New Questionnaire" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="Questionnaire.aspx.cs" Inherits="Survey_Module_Questionnaire_Questionnaire" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <script type="text/javascript" src="../../ckeditorn/ckeditor.js"></script>
     <script src="../../Layouts/Resources/js/Common.js" type="text/javascript"></script>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/Questionnaire_new.png" runat="server" alt="<% $Resources:lblToolTip %>" 
                        align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label>
                    </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start user form -->
            <%--<asp:UpdatePanel ID="updPanel" runat="server">
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
                                <legend><asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>"
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
                            <legend><asp:Label ID="lblGeneralDetails" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblType" runat="server" Text="<% $Resources:lblType %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlType" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">360 Questionnaire</asp:ListItem>
                                            <asp:ListItem Value="2">Survey Questionnaire</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:Rq1 %>"
                                            SetFocusOnError="True" ControlToValidate="ddlType" ValidationGroup="group1" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="13%">
                                        &nbsp;
                                    </td>
                                    <td width="38%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblQuestCode" runat="server" Text="<% $Resources:lblQuestCode %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtquestionnairecode" MaxLength="25" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq3" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq3 %>"
                                            SetFocusOnError="True" ControlToValidate="txtquestionnairecode">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblQuestName" runat="server" Text="<% $Resources:lblQuestName %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtquestionnairename" MaxLength="25" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq4" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq4 %>"
                                            SetFocusOnError="True" ControlToValidate="txtquestionnairename">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblDescription" runat="server" Text="<% $Resources:lblDescription %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" />
                                        <div class="maxlength-msg"><asp:Label id="lblCharactersLimit"  runat="server" Text="<% $Resources:lblCharactersLimit %>"></asp:Label>
                                            </div>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblDisplayCategory" runat="server" Text="<% $Resources:lblDisplayCategory %>" ></asp:Label><span
                                    class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDisplayCategory" MaxLength="2" onKeyPress="return NumberOnly(this);" SkinID="age" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:RequiredFieldValidator1 %>"
                                            SetFocusOnError="True" ControlToValidate="txtDisplayCategory">&nbsp;</asp:RequiredFieldValidator>
                                        <%--<asp:DropDownList ID="ddlProject" runat="server" style="width:155px" AppendDataBoundItems="True" >
                    <asp:ListItem Value="0">Select</asp:ListItem>
             </asp:DropDownList>
             
              <asp:RequiredFieldValidator ID="Rq5" runat="server" ValidationGroup= "group1"  ErrorMessage="Please Select Project Details" SetFocusOnError="True" ControlToValidate="ddlproject"  InitialValue="0">&nbsp;</asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="lblPrologueEditor" runat="server" Text="<% $Resources:lblPrologueEditor %>" ></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%" valign="top">
                                        <asp:Label ID="lblPrologueDesc" runat="server" Text="<% $Resources:lblPrologueDesc %>"></asp:Label>
                                    </td>
                                    <td width="87%" valign="top">
                                       <%-- <FCKeditorV2:FCKeditor ID="txtPrologueEditor" runat="server" BasePath="~/fckeditor/"
                                            Width="800px" ToolbarSet="Feedback">
                                        </FCKeditorV2:FCKeditor>--%>
                                        <div style="width: 100%;">
                                            <textarea id="txtPrologueEditor" runat="server" rows="10" cols="80" style="width: 90%;"
                                                clientidmode="Static">
                                        </textarea>
                                        </div>
                                        <%--<asp:TextBox ID="txtPrologueEditor" runat="server" SkinID="txtarea500" Rows="5" TextMode="MultiLine" 
                                        Text="" />--%>
                                        <div class="maxlength-msg"><asp:Label id="lblCharactersLimit1"  runat="server" Text="<% $Resources:lblCharactersLimit1 %>"></asp:Label>
                                            </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="lblEpilogueEditor" runat="server" Text="<% $Resources:lblEpilogueEditor %>" ></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%" valign="top">
                                        <asp:Label ID="lblEpilogueDesc" runat="server" Text="<% $Resources:lblEpilogueDesc %>"></asp:Label>
                                    </td>
                                    <td width="87%" valign="top">
                                       <%-- <FCKeditorV2:FCKeditor ID="txtEpilogueEditor" runat="server" BasePath="~/fckeditor/"
                                            Width="800px" ToolbarSet="Feedback">
                                        </FCKeditorV2:FCKeditor>--%>
                                        <div style="width: 100%;">
                                            <textarea id="txtEpilogueEditor" runat="server" rows="10" cols="80" style="width: 90%;"
                                                clientidmode="Static">
                                        </textarea>
                                        </div>
                                        <%--<asp:TextBox ID="txtEpilogueEditor" runat="server" SkinID="txtarea500" Rows="5" TextMode="MultiLine" 
                                       Text=""  />--%>
                                        <div class="maxlength-msg"><asp:Label id="lblCharactersLimit2"  runat="server" Text="<% $Resources:lblCharactersLimit2 %>"></asp:Label>
                                            </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label>
                        
                        <div align="center">
                            <asp:ImageButton ID="ibtnSave" ImageUrl="~/Layouts/Resources/images/save.png" runat="server"
                                OnClick="ibtnSave_Click" ValidationGroup="group1" />&nbsp;
                            <asp:ImageButton ID="ibtnCancel" ImageUrl="~/Layouts/Resources/images/cancel.png"
                                runat="server" OnClick="ibtnCancel_Click" />&nbsp;
                            <asp:ImageButton ID="imbBack" Visible="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                                CausesValidation="false" runat="server" PostBackUrl="~/Survey_Module/Questionnaire/QuestionnaireList.aspx" />
                        </div>
                        <br />
                    </div>
                    <!-- start user form -->
                <%--</ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>

     <script type="text/javascript">

         BindEditor(txtPrologueEditor);
         BindEditor(txtEpilogueEditor);

     </script>
</asp:Content>

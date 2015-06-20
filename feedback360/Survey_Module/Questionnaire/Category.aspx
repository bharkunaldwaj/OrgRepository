<%@ Page Title="Create New Category" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="Category.aspx.cs" Inherits="Module_Questionnaire_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <script language="javascript" type="text/javascript">

        var TextAreaMaxLengthCheck = function (id, length) {

            length = length - 1;
            var val = document.getElementById(id).value;
            if (val.length <= length)
                return true;
            else {

                event.keyCode = 0;


            }
        }

        function RemoveQuestImage() {
            document.getElementById('ctl00_cphMaster_introImageFileUpload').value = "";
            document.getElementById('<%=hdnQuestImage.ClientID %>').value = "";
            document.getElementById('<%=imgQuestlogo.ClientID %>').src = "";
            
        }
        function RemoveReportPdf() {
            document.getElementById('<%=hdnRemoveIntroPdf.ClientID %>').value = "";
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
 <%--   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
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
                                <legend>
                                    <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
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
                            <legend>
                                <asp:Label ID="Label3" runat="server" Text="<% $Resources:lblCategoryDetails %>"></asp:Label></legend>
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
                                        <asp:RegularExpressionValidator ID="ressequence" ControlToValidate="txtSequence"
                                            ErrorMessage="<% $Resources:ressequence %>" ValidationExpression="^[0-9][\d]*"
                                            runat="server" ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" />
                                        <asp:RangeValidator ID="valTxtRange" ControlToValidate="txtSequence" Type="Integer"
                                            MinimumValue="1" MaximumValue="99" ErrorMessage="<% $Resources:valTxtRange %>"
                                            ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />
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
                                        <asp:TextBox ID="txtCategoryTitle" MaxLength="35" runat="server"></asp:TextBox>
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
                                            ErrorMessage="<% $Resources:Rq21 %> Please Enter Description " SetFocusOnError="True"
                                            ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblExclude" runat="server" Text="<% $Resources:lblExclude %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:CheckBox ID="chkExcludeFromAnalysis" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblIntroImage" runat="server" Text="<% $Resources:lblIntroImage %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                            <tr>
                                                <td width="70%" valign="top">
                                                    <asp:FileUpload ID="introImageFileUpload" runat="Server" />
                                                </td>
                                                <td width="20%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="imgQuestlogo" src=""
                                                            runat="server" /><br />
                                                            Image</span></a>
                                                    </div>
                                                </td>
                                                <td width="10%" valign="top">
                                                    <asp:HiddenField ID="hdnQuestImage" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnRemoveQuestImage" runat="server" Value="0" />
                                                    <a href="javascript:void(0)" onclick="RemoveQuestImage();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblRecSize %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblIntroPdf" runat="server" Text="<% $Resources:lblIntroPdf %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                            <tr>
                                                <td width="70%" valign="top">
                                                    <asp:FileUpload ID="pdfFileUpload" runat="Server" />
                                                </td>
                                                <td width="20%" valign="top">
                                                    <asp:LinkButton ID="lnkPdf" runat="server" Text="Download" 
                                                        onclick="lnkPdf_Click"></asp:LinkButton>
                                                </td>
                                                <td width="10%" valign="top">
                                                    <asp:HiddenField ID="hdnRemoveIntroPdf" runat="server" Value="0" />
                                                    <a href="javascript:void(0)" onclick="RemoveReportPdf();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <%--<asp:Label ID="lblPdfSize" runat="server" Text="<% $Resources:lblPdfSize %>"></asp:Label>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <span class="style3">
                            <asp:Label ID="Label4" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <div align="center">
                            <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/Layouts/Resources/images/Save.png"
                                OnClick="ibtnSave_Click" ValidationGroup="group1" ToolTip="Save" />
                            &nbsp;
                            <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/Layouts/Resources/images/Cancel.png"
                                OnClick="ibtnCancel_Click" ToolTip="Back to List" />
                            <asp:ImageButton ID="imbBack" runat="server" CausesValidation="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                                PostBackUrl="~/Survey_Module/Questionnaire/CategoryList.aspx" ToolTip="Back to List"
                                Visible="false" />
                        </div>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <div align="center">
                            <span class="style3">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </span>
                        </div>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                    </div>
              <%--  </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ibtnSave" />
                     <asp:PostBackTrigger ControlID="lnkPdf" />
                </Triggers>
            </asp:UpdatePanel>--%>
            <!-- start user form -->
        </div>
    </div>
</asp:Content>

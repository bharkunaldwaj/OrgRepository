<%@ Page Title="Create New Email Templates" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    ValidateRequest="false" AutoEventWireup="true" CodeFile="EmailTemplates.aspx.cs"
    Inherits="Survey_Module_Admin_EmailTemplates" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">

        var TextAreaMaxLengthCheck = function(id, length) {

            length = length - 1;
            var val = document.getElementById(id).value;
            if (val.length <= length)
                return true;
            else {

                event.keyCode = 0;


            }
        }

        function GenerateText() {
            //document.getElementById('divEmailOutput').innerHTML=document.getElementById('ctl00_cphMaster_txtEmailText').value;
            document.getElementById('ctl00_cphMaster_hdnEmailText').innerHTML = document.getElementById('ctl00_cphMaster_txtEmailText').value;
        }

        function CheckData() {
            var str = document.getElementById('ctl00_cphMaster_txtEmailText').value;
            str = str.replace(/^[\s]+/, '').replace(/[\s]+$/, '');

            if (str == "") {
                document.getElementById('divMessage').innerHTML = "Please Enter Email Text";
                return false;
            }
        }
    
    </script>
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
        background-color: lightyellow;
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

    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/create_email.png" runat="server" title="<% $Resources:lblToolTip %>" 
                        align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label>
                </h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start user form -->
            <table border="0" width="100%">
                <tr>
                    <td>
                        <div id="Div1" runat="server" class="validation-align">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                ShowSummary="true" ValidationGroup="group1" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <div id="divMessage" class="validation-align">
                        </div>
                        <asp:Label ID="lblMessage" class="style3" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
          <%--  <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
                    <asp:HiddenField ID="hdnimage" runat="server" />
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
                            <legend><asp:Label ID="lblGeneralDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblTitle" runat="server" Text="<% $Resources:lblTitle %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="36%">
                                        <asp:TextBox ID="txttitle" MaxLength="50" runat="server"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rq1" runat="server" ControlToValidate="txttitle"
                                            ErrorMessage="<% $Resources:rq1 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="13%">
                                        &nbsp;
                                    </td>
                                    <td width="38%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="13%" valign="top">
                                        <asp:Label ID="lblDesc" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" onkeypress="javascript:TextAreaMaxLengthCheck(this.id,999);"
                                            SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" />
                                        <div class="maxlength-msg">
                                            <asp:Label id="lblCharactersLimit"  runat="server" Text="<% $Resources:lblCharactersLimit %>"></asp:Label></div>
                                    </td>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="lblEmailDetail" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%" valign="top">
                                        <asp:Label ID="lblSubject" runat="server" Text="<% $Resources:lblSubject %>">"></asp:Label>
                                    </td>
                                    <td width="87%" valign="top">
                                        <asp:TextBox ID="txtSubject" MaxLength="200" SkinID="txtarea500" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="13%" valign="top">
                                        <asp:Label ID="lblEmailText" runat="server" Text="<% $Resources:lblEmailText %>">"></asp:Label>
                                    </td>
                                    <td width="87%" valign="top">
                                        <FCKeditorV2:FCKeditor ID="txtEmailText" runat="server" BasePath="~/fckeditor/" Width="800px"
                                            Value=" " ToolbarSet="Feedback">
                                        </FCKeditorV2:FCKeditor>
                                        
                                        <div id="div1" class="maxlength-msg">
                                            <asp:Label id="lblCharactersLimit1"  runat="server" Text="<% $Resources:lblCharactersLimit1 %>"></asp:Label>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="13%" valign="top">
                                        <asp:Label ID="lblEmailImage" runat="server" Text="<% $Resources:lblEmailImage %>">"></asp:Label>
                                    </td>
                                    <td width="87%" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="5%"><asp:FileUpload ID="FileUpload" runat="Server" /></td>
                                        <td>
                                        <div class="gallerycontainer">&nbsp;
                                            <a class="thumbnail" href="#thumb">Preview<span><img id="imagelogo" src="" /><br />Image</span></a>
                                        </div>
                                        </td>
                                    </tr
                                    <tr>
                                        <td colspan="2" class="style3"><asp:Label ID="lblSize" runat="server" Text="<% $Resources:lblSize %>"></asp:Label></td>
                                    </tr>
                                </table>
                                    </td>
                                </tr>

                                </tr>
                                <tr>
                                    
                                     <td width="13%" valign="top">
                                        <asp:Label ID="Label1" runat="server" Text="Preview E-mail"></asp:Label>
                                    </td>
                                    <td width="87%" valign="top">
                                        <asp:TextBox ID="txtEmail" placeholder="abc@xyz.com" MaxLength="225" Width="250px"  runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="v1" ErrorMessage="E-Mail is required." runat="server" ValidationGroup="preview" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                        </td></tr>

                            </table>
                        </fieldset>
                        <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label>                        
                        <br />
                        <center>
                        </center>
                        <br>
                            <br></br>
                            <div align="center">
                                &nbsp;<asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/Layouts/Resources/images/Save.png"
                                    OnClick="ibtnSave_Click" OnClientClick="" ToolTip="Save" ValidationGroup="group1" />
                                &nbsp;
                                  <asp:ImageButton ID="btnSend" runat="server" ValidationGroup="preview"  OnClick="previewEmail_Click" ImageUrl="~/Layouts/Resources/images/preview-email-btn.png "
                                      ToolTip="Preview E-mail" />
                                      &nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/Layouts/Resources/images/Cancel.png"
                                    OnClick="ibtnCancel_Click" ToolTip="Back to list" />
                                <asp:ImageButton ID="imbBack" runat="server" CausesValidation="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                                    OnClick="imbBack_Click" ToolTip="Back to list" Visible="false" />
                            </div>
                        </br>
                    </div>
                    <br />
                </ContentTemplate>
               <%-- <Triggers>
                    <asp:PostBackTrigger ControlID="ibtnSave" />
                </Triggers>
            </asp:UpdatePanel>--%>
            
            <!-- start user form -->
        </div>
    </div>

    
    <script type="text/javascript">
        if (document.getElementById('ctl00_cphMaster_hdnimage').value != "") {
            document.getElementById('imagelogo').src = "../../EmailImages/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
        }
        else {
            document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
        }
    </script>
    
    

</asp:Content>

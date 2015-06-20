<%@ Page Title="Create New Account" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="Accounts.aspx.cs" Inherits="Survey_Module_Admin_Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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

        function RemoveImage() 
        {
            document.getElementById('ctl00_cphMaster_hdnRemoveImage').value = "";
        }
    </script>

    <style type="text/css">
        /*Credits: Dynamic Drive CSS Library *//*URL: http://www.dynamicdrive.com/style/ */.gallerycontainer
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
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="Survey_topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/create_account.png"  runat="server" alt="<% $Resources:lblToolTip %>"
                                align="absmiddle" />
                            <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label> </h3>
                        <div class="clear">
                        </div>
                    </div>
                    <!-- end heading logout -->
                    <!-- start user form -->
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="Div1" runat="server" class="validation-align">
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="group1" />
                                    <span class="style3">
                                        <asp:Label ID="lblfilemsg" runat="server" Text=""></asp:Label></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="Div2" runat="server" class="validation-align">
                                    <span class="style3">
                                        <asp:Label ID="lblusermsg" runat="server" Text=""></asp:Label></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnPassword" runat="server" />
                    <asp:HiddenField ID="hdnimage" runat="server" />
                    <div class="userform">
                        <%--  <fieldset class="fieldsetform">
                    <legend>
                        <asp:Label ID="lblGeneralDetails" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        
                        <tr>
                            <td>
                                <asp:Label ID="lblPassword" runat="server" Text="<% $Resources:lblPassword %>"></asp:Label><span
                                    class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" TextMode="Password" MaxLength="25" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq12" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage=" Please Enter Password " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator></td>
                            
                         
                             <asp:RegularExpressionValidator ID="Rq89" runat="server" ValidationGroup="group1" ControlToValidate="txtPassword" SetFocusOnError="True" Text="*" ForeColor="White" ErrorMessage="Password length must be of Minimum 6 characters" ValidationExpression=".{5}.*" />
                            
                            
                            
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>--%>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblCompanyDetails" runat="server" Text="<% $Resources:lblCompanyDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblCode" runat="server" Text="<% $Resources:lblCode %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="36%">
                                        <asp:TextBox ID="txtCode" MaxLength="5" runat="server"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="Rq10" runat="server" ControlToValidate="txtCode"
                                            ErrorMessage="<% $Resources:Rq10 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="13%">
                                        <%--<asp:Label ID="lblLoginID" runat="server" Text="<% $Resources:lblLoginID %>"></asp:Label><span
                                    class="style3">*</span>--%>
                                    </td>
                                    <td width="38%">
                                        <%--<asp:TextBox ID="txtLoginID" MaxLength="25" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="Rq11" runat="server" ControlToValidate="txtLoginID"
                                    ErrorMessage=" Please Enter Login Id " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                            --%>
                                    </td>
                                    <%--                        <asp:RegularExpressionValidator ID="Rq88" runat="server" ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" ControlToValidate="txtLoginID" ErrorMessage="Login ID length must be of Minimum 5 characters" ValidationExpression=".{4}.*" />
--%>
                                </tr>
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblOrganisationName" runat="server" Text="<% $Resources:lblOrganisationName %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="36%">
                                        <asp:TextBox ID="txtOrganisationName" MaxLength="25" runat="server"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="Rq13" runat="server" ControlToValidate="txtOrganisationName"
                                            ErrorMessage="<% $Resources:Rq13 %>" SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="13%">
                                        <asp:Label ID="lblType" runat="server" Text="<% $Resources:lblType %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="38%">
                                        <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">Super Admin</asp:ListItem>
                                            <asp:ListItem Value="2">Company</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;<asp:RequiredFieldValidator ID="Rq14" runat="server" ControlToValidate="ddlType"
                                            ErrorMessage="<% $Resources:Rq14 %>" SetFocusOnError="True" InitialValue="0" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblDescription" runat="server" Text="<% $Resources:lblDescription %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDescription" runat="server" onkeypress="javascript:TextAreaMaxLengthCheck(this.id,999);"
                                            SkinID="txtarea300X3" Rows="3" TextMode="MultiLine" />
                                        <div class="maxlength-msg">
                                            <asp:Label id="lblCharactersLimit"  runat="server" Text="<% $Resources:lblCharactersLimit %>"></asp:Label>
                                            </div>
                                    </td>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" Text="<% $Resources:lblEmail %>"></asp:Label><span
                                            class="style3"></span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" MaxLength="80" runat="server" SkinID="email"></asp:TextBox>
                                        <%--&nbsp;<asp:RequiredFieldValidator ID="Rq15" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage=" Please Enter Email " SetFocusOnError="True" ValidationGroup="group1">&nbsp;
                                        </asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmail"
                                            ErrorMessage="Invalid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            runat="server" ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" />--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblWebsite" runat="server" Text="<% $Resources:lblWebsite %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWebsite" MaxLength="80" runat="server" SkinID="email"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Rq49" ControlToValidate="txtWebsite" ErrorMessage="<% $Resources:Rq49 %>"
                                            ValidationExpression="(http(s)?://((w{3})+\.)+([\w-]+\.)+(\w{2,4})+(/[\w- ./?%&=]*)?)|((w{3})+\.)+([\w-]+\.)+(\w{2,4})+(/[\w- ./?%&=]*)?"
                                            runat="server" ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCopyRight" runat="server" Text="<% $Resources:lblCopyRight %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCopyRight" MaxLength="250" runat="server" SkinID="txtarea500"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCopyRight"
                                            ErrorMessage="<% $Resources:RequiredFieldValidator1 %> " SetFocusOnError="True" ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStatus" runat="server" Text="<% $Resources:lblStatus %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="2">Suspended</asp:ListItem>
                                            <asp:ListItem Value="3">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;<asp:RequiredFieldValidator ID="Rq17" runat="server" ControlToValidate="ddlStatus"
                                            ErrorMessage="<% $Resources:Rq17 %> " SetFocusOnError="True" InitialValue="0"
                                            ValidationGroup="group1">&nbsp;</asp:RequiredFieldValidator>
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
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblDisplayDetails" runat="server" Text="<% $Resources:lblDisplayDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblCompanyLogo" runat="server" Text="<% $Resources:lblCompanyLogo %>"></asp:Label>
                                    </td>
                                    <td colspan="3" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="5%" valign="top">
                                                    <asp:FileUpload ID="fuplCompanyLogo" runat="Server" />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="fuplCompanyLogo"
                                                        ErrorMessage="<% $Resources:RegularExpressionValidator1 %>" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|doc|DOC|png|PNG|bmp|BMP|jpeg|JPEG)$"
                                                        runat="server" ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" />
                                                </td>
                                                <td valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp; <a class="thumbnail" href="#thumb" >Preview<span><img id="imagelogo" src="" /><br />
                                                            Image</span></a>
                                                    </div>
                                                </td>
                                                <td valign="top">
                                                    <asp:HiddenField ID="hdnRemoveImage" runat="server" Value="0" />
                                                    <a href="#" onclick="RemoveImage();"  ><img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                </td>
                                                <td  valign="top" class="style3" colspan="2">(Recomended Size (Width X Height): 450 X 70)<asp:Label ID="lblUploadFileName" Visible="false" runat="server" Text=""></asp:Label></td>            
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblHeaderBackgroundColor" runat="server" Text="<% $Resources:lblHeaderBackgroundColor %>"></asp:Label>
                                    </td>
                                    <td width="36%">
                                        <%--<input class="color {valueElement:'ctl00_cphMaster_txtBannerBGColor'}" style="height: 15px;
                                            width: 75px;" readonly="true" />--%>
                                        <asp:TextBox ID="txtBannerBGColor" SkinID="zip" MaxLength="7" runat="server" Text=""
                                             />
                                    </td>
                                    <td width="13%">
                                        <asp:Label ID="lblMenuBackgroundColor" runat="server" Text="<% $Resources:lblMenuBackgroundColor %>"></asp:Label>
                                    </td>
                                    <td  width="38%">
                                        <%--<input class="color {valueElement:'ctl00_cphMaster_txtMenuBGColor'}" style="height: 15px;
                                            width: 75px;" readonly="true" />--%>
                                        <asp:TextBox ID="txtMenuBGColor" SkinID="zip" MaxLength="7" runat="server" Text=""  />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label>
                        
                        <br />
                        <div align="center">
                            <asp:ImageButton ID="imbSave" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server"
                                OnClick="imbSave_Click" ValidationGroup="group1" ToolTip="Save" />&nbsp;
                            <asp:ImageButton ID="imbCancel" ImageUrl="~/Layouts/Resources/images/Cancel.png"
                                CausesValidation="false" runat="server" OnClick="imbCancel_Click" ToolTip="Back to list" />
                            <asp:ImageButton ID="imbBack" Visible="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                                CausesValidation="false" runat="server" PostBackUrl="~/Survey_Module/Admin/AccountList.aspx"
                                ToolTip="Back to list" />
                        </div>
                        <br />
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

        //document.getElementById('ctl00_cphMaster_txtPassword').value = document.getElementById('ctl00_cphMaster_hdnPassword').value;
        
    </script>

    <script type="text/javascript">

        if (document.getElementById('ctl00_cphMaster_hdnimage').value != "") {
            document.getElementById('imagelogo').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimage').value;
        }
        else {
            document.getElementById('imagelogo').src = "../../UploadDocs/noImage.jpg ";
        }
        
    </script>

</asp:Content>

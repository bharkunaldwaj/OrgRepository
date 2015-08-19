<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportManagement.aspx.cs"
    ValidateRequest="false" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    Inherits="Module_Reports_ReportManagement" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">

        function RemoveImage(i) {

            if (i == 1) {
                document.getElementById('ctl00_cphMaster_hdnTopImage').value = "";
            }
            else if (i == 2) {
                document.getElementById('ctl00_cphMaster_hdnMiddleImage').value = "";
            }
            else if (i == 3) {
                document.getElementById('ctl00_cphMaster_hdnBottomImage').value = "";
            }
            else if (i == 4) {
                document.getElementById('ctl00_cphMaster_hdnRightImage').value = "";
            }
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
   <%-- <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/view-report.png" runat="server" title="<% $Resources:lblToolTip %>"
                                align="absmiddle" />
                            <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                        </h3>
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
                    
                    <div id="divAccount" runat="server" visible="false">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
                            <div id="Div4" style="margin: 0 auto; padding: 10px;">
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
                                            <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %>"
                                                SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompanyName %>"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblProjectDetail" runat="server" Text="<% $Resources:lblProjectDetail %>"></asp:Label></legend>
                            <div id="Div1" style="margin: 0 auto; padding: 10px;">
                                <table border="0" cellspacing="5" cellpadding="0" width="100%">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="35%">
                                            <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1"
                                                ErrorMessage="<% $Resources:RequiredFieldValidator1 %>" SetFocusOnError="True"
                                                ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="15%">
                                            &nbsp;
                                        </td>
                                        <td width="35%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div id="divReportSettings" runat="server">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblRerportSetting" runat="server" Text="<% $Resources:lblRerportSetting %>"></asp:Label></legend>
                            <div id="Div5" style="margin: 0 auto; padding: 10px;">
                                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="lblReportType" runat="server" Text="<% $Resources:lblReportType %>"></asp:Label><span
                                                class="style3">*</span>
                                        </td>
                                        <td width="35%">
                                            <asp:DropDownList ID="ddlReportType" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                <asp:ListItem Value="1">Report1</asp:ListItem>
                                                <asp:ListItem Value="2">Report2</asp:ListItem>
                                                <asp:ListItem Value="3">Report3</asp:ListItem>
                                                <asp:ListItem Value="4">Report4</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="group1"
                                                ErrorMessage="<% $Resources:RequiredFieldValidator2 %>" SetFocusOnError="True"
                                                ControlToValidate="ddlReportType" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHeading1" runat="server" Text="<% $Resources:lblHeading1 %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPageHeading1" MaxLength="75" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHeading2" runat="server" Text="<% $Resources:lblHeading2 %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPageHeading2" MaxLength="75" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHeading3" runat="server" Text="<% $Resources:lblHeading3 %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPageHeading3" MaxLength="75" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHeadingColour" runat="server" Text="<% $Resources:lblHeadingColour %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPageHeadingColor" MaxLength="25" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCopyright" runat="server" Text="<% $Resources:lblCopyright %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPageCopyright" MaxLength="500" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblConclusionHeading" runat="server" Text="<% $Resources:lblConclusionHeading %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConclusionHeading" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblTopImage" runat="server" Text="<% $Resources:lblTopImage %>"></asp:Label>
                                        </td>
                                        <td valign="top" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="20%" valign="top">
                                                        <asp:FileUpload ID="fuplTopImage" runat="Server" />
                                                    </td>
                                                    <td width="10%" valign="top">
                                                        <div class="gallerycontainer">
                                                            &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="ImgTopImage" runat="server"
                                                                src="" /><br />
                                                            </span></a>
                                                        </div>
                                                    </td>
                                                    <td width="5%" valign="top">
                                                        <asp:HiddenField ID="hdnImgTopImage" runat="server" />
                                                        <asp:HiddenField ID="hdnTopImage" runat="server" Value="0" />
                                                        <a href="#" onclick="RemoveImage(1);">
                                                            <img src="../../Layouts/Resources/images/remove.png" title="Remove Top Image" /></a>
                                                    </td>
                                                    <td width="65%" valign="top" class="style3"  >
                                                        (Recomended Size (Width X Height): 331 X 128)<asp:Label ID="lblUploadFileName" Visible="false"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblMiddleImage" runat="server" Text="<% $Resources:lblMiddleImage %>"></asp:Label>
                                        </td>
                                        <td valign="top" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="20%" valign="top">
                                                        <asp:FileUpload ID="fuplMiddleImage" runat="Server" />
                                                    </td>
                                                    <td width="10%" valign="top">
                                                        <div class="gallerycontainer">
                                                            &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="ImgMiddleImage" runat="server"
                                                                src="" /><br />
                                                            </span></a>
                                                        </div>
                                                    </td>
                                                    <td width="5%" valign="top">
                                                        <asp:HiddenField ID="hdnImgMiddleImage" runat="server" />
                                                        <asp:HiddenField ID="hdnMiddleImage" runat="server" Value="0" />
                                                        <a href="#" onclick="RemoveImage(2);">
                                                            <img src="../../Layouts/Resources/images/remove.png" title="Remove Middle Image" /></a>
                                                    </td>
                                                    <td width="65%" valign="top" class="style3"  >
                                                        (Recomended Size (Width X Height): 331 X 242)<asp:Label ID="Label2" Visible="false"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblBottomImage" runat="server" Text="<% $Resources:lblBottomImage %>"></asp:Label>
                                        </td>
                                        <td valign="top" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="20%" valign="top">
                                                        <asp:FileUpload ID="fuplBottomImage" runat="Server" />
                                                    </td>
                                                    <td width="10%" valign="top">
                                                        <div class="gallerycontainer">
                                                            &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="ImgBottomImage" runat="server"
                                                                src="" /><br />
                                                            </span></a>
                                                        </div>
                                                    </td>
                                                    <td width="5%" valign="top">
                                                        <asp:HiddenField ID="hdnImgBottomImage" runat="server" />
                                                        <asp:HiddenField ID="hdnBottomImage" runat="server" Value="0" />
                                                        <a href="#" onclick="RemoveImage(3);">
                                                            <img src="../../Layouts/Resources/images/remove.png" title="Remove Bottom Image" /></a>
                                                    </td>
                                                    <td width="65%" valign="top" class="style3"  >
                                                        (Recomended Size (Width X Height): 331 X 120)<asp:Label ID="Label3" Visible="false"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                             <asp:Label ID="lblRightImage" runat="server" Text="<% $Resources:lblRightImage %>"></asp:Label></td>
                                        <td valign="top" colspan="3" align=Left>
                                             <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="20%" valign="top">
                                                        <asp:FileUpload ID="FileUploadRightImage" runat="Server" />
                                                    </td>
                                                    <td width="10%" valign="top">
                                                        <div class="gallerycontainer">
                                                            &nbsp;  <a class="thumbnail" href="#thumb">Preview<span><img id="ImgRightImage"  runat="server"
                                                                src="" /><br />
                                                            </span></a>
                                                        </div>
                                                    </td>
                                                    <td width="5%" valign="top">
                                                        <asp:HiddenField ID="hdnImgRightImage" runat="server" />
                                                        <asp:HiddenField ID="hdnRightImage" runat="server" Value="0" />
                                                        <a href="#" onclick="RemoveImage(4);">
                                                            <img src="../../Layouts/Resources/images/remove.png" title="Remove Bottom Image" /></a>
                                                    </td>
                                                    <td width="65%" valign="top" class="style3"  >
                                                        (Recomended Size (Width X Height): 120 X 745)<asp:Label ID="Label4" Visible="false"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            &nbsp;</td>
                                        <td align="Right" colspan="3" valign="top">
                                            <div class="gallerycontainer" style="padding-right:130px">
                                                <asp:LinkButton ID="LinkPreview" runat="server" onclick="LinkPreview_Click">Preview</asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblIntroduction" runat="server" Text="<% $Resources:lblIntroduction %>"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <%--<asp:TextBox ID="txtPageIntroduction" MaxLength="2000"  SkinID="txtarea300X3" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
                                            <FCKeditorV2:FCKeditor ID="txtPageIntroduction" runat="server" BasePath="~/fckeditor/"
                                                Width="700px" Value=" " ToolbarSet="Feedback">
                                            </FCKeditorV2:FCKeditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblConclus" runat="server" Text="<% $Resources:lblConclus %>"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <%--<asp:TextBox ID="txtPageConclusion" MaxLength="5000"  SkinID="txtarea300X3" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
                                            <FCKeditorV2:FCKeditor ID="txtPageConclusion" runat="server" BasePath="~/fckeditor/"
                                                Width="700px" Value=" " ToolbarSet="Feedback">
                                            </FCKeditorV2:FCKeditor>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div id="reportselection" runat="server">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblReportSelection" runat="server" Text="<% $Resources:lblReportSelection %>"></asp:Label></legend>
                            <div id="pnlForm" style="margin: 0 auto; padding: 10px;">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td valign="top" width="50%">
                                            <div runat="server" id="coverpage">
                                                <span>
                                                    <input id="chkCoverPage" runat="server" type="checkbox" /><asp:Label ID="lblCoverPage"
                                                        runat="server" Text="<% $Resources:lblCoverPage %>"></asp:Label></span>
                                            </div>
                                            <br>
                                            <div runat="server" id="reportintro">
                                                <asp:CheckBox ID="chkReportIntro" runat="server" /><asp:Label ID="lblReportIntro"
                                                    runat="server" Text="<% $Resources:lblReportIntro %>"></asp:Label></span>
                                            </div>
                                            <div style="margin: 15px 0 0 0;" runat="server" id="radarchart">
                                                <span>
                                                    <asp:CheckBox ID="chkRadarChart" runat="server" /><asp:Label ID="lblRadarChart" runat="server"
                                                        Text="<% $Resources:lblRadarChart %>"></asp:Label></span>
                                            </div>
                                            <div style="margin: 15px 0 0 0;" runat="server" id="catintro">
                                                <span>
                                                    <asp:CheckBox ID="chkCategoryIntro" runat="server" AutoPostBack="true" OnCheckedChanged="chkCategoryIntro_CheckedChanged" /><asp:Label
                                                        ID="lblCategoryIntro" runat="server" Text="<% $Resources:lblCategoryIntro %>"></asp:Label></span>
                                                <div style="margin: 0 0 0 20px;" runat="server" id="catQstlist">
                                                    <div>
                                                        <span>
                                                            <asp:CheckBox ID="chkCatQstlist" runat="server" AutoPostBack="true" OnCheckedChanged="chkCatQstlist_CheckedChanged" /><asp:Label
                                                                ID="lblCatQstlist" runat="server" Text="<% $Resources:lblCatQstlist %>"></asp:Label></span>
                                                    </div>
                                                    <div>
                                                        <span>
                                                            <asp:CheckBox ID="chkCatQstChart" runat="server" AutoPostBack="true" OnCheckedChanged="chkCatQstChart_CheckedChanged" /><asp:Label
                                                                ID="lblCatQstChart" runat="server" Text="<% $Resources:lblCatQstChart %>"></asp:Label></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="margin: 15px 0 0 0;" runat="server" id="catQstText">
                                                <span>
                                                    <input id="chkCatQstText" runat="server" type="checkbox" /><asp:Label ID="lblCatQstText"
                                                        runat="server" Text="<% $Resources:lblCatQstText %>"></asp:Label></span>
                                            </div>
                                            <div style="margin: 15px 0 0 0;" runat="server" id="benchconclusion">
                                                <span>
                                                    <input id="chkBenchConclusionPage" runat="server" type="checkbox" /><asp:Label ID="lblBecnhConclusion"
                                                        runat="server" Text="<% $Resources:lblBecnhConclusion %>"></asp:Label></span>
                                            </div>
                                            <br>
                                            <div style="margin: 15px 0 0 0;" runat="server" id="conclusion">
                                                <span>
                                                    <input id="chkConclusion" runat="server" type="checkbox" /><asp:Label ID="lblConclusion"
                                                        runat="server" Text="<% $Resources:lblConclusion %>"></asp:Label></span>
                                            </div>
                                            <br>
                                            <div runat="server" id="benchscoregraph">
                                                <span>
                                                    <input id="chkBenchMark" runat="server" type="checkbox" /><asp:Label ID="lblBenchMark"
                                                        runat="server" Text="<% $Resources:lblBenchMark %>"></asp:Label></span>
                                            </div>
                                            <br>
                                            <div runat="server" id="prevscr">
                                                <span>
                                                    <input id="chkPreviousScore" runat="server" type="checkbox" /><asp:Label ID="lblPreviousScore"
                                                        runat="server" Text="<% $Resources:lblPreviousScore %>"></asp:Label></span>
                                            </div>
                                            <div style="margin: 15px 0 0 0;" runat="server" id="hlrange">
                                                <table cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td width="5%">
                                                            <asp:Label ID="lblConHighLowRange1" runat="server" Text="<% $Resources:lblConHighLowRange1 %>"></asp:Label>
                                                        </td>
                                                        <td width="5%">
                                                            <asp:TextBox ID="txtConHighLowRange" SkinID="age" MaxLength="25" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td width="45%">
                                                            <asp:Label ID="lblConHighLowRange2" runat="server" Text="<% $Resources:lblConHighLowRange2 %>"></asp:Label>
                                                        </td>
                                                        <td width="20%">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                        <td valign="top" width="50%">
                                            <div style="margin: 25px 0 20px 20px;">
                                                <asp:Label ID="lblSelectionGroup" runat="server" Text="<% $Resources:lblSelectionGroup %>"></asp:Label>
                                                <div style="margin: 0 0 0 12px;" runat="server" id="selfname">
                                                    <span>
                                                        <input id="chkSelfNameGrp" runat="server" type="checkbox" /><asp:Label ID="lblSelfNameGrp"
                                                            runat="server"></asp:Label></span>
                                                </div>
                                                <div style="margin-left: 10px;" runat="server" id="grouplist">
                                                    <span id="chklReportResponseSelectionList">
                                                        <asp:CheckBoxList ID="chkGroupList" runat="server" AppendDataBoundItems="true">
                                                        </asp:CheckBoxList>
                                                    </span>
                                                </div>
                                                <div style="margin: 0 0 0 12px;" runat="server" id="benchrelation">
                                                    <span>
                                                        <input id="chkBenchMarkGrp" runat="server" type="checkbox" /><asp:Label ID="lblBenckMarkGrp"
                                                            runat="server" Text="<% $Resources:lblBenckMarkGrp %>"></asp:Label></span>
                                                </div>
                                                <div style="margin: 0 0 0 12px;" runat="server" id="prggroup">
                                                    <span>
                                                        <input id="chkProgrammeGrp" runat="server" type="checkbox" /><asp:Label ID="lblProgramme"
                                                            runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label></span>
                                                </div>
                                                <div style="margin: 0 0 0 12px;" runat="server" id="fullprojgrp">
                                                    <span>
                                                        <input id="chkFullPrjGrp" runat="server" type="checkbox" /><asp:Label ID="lblFullPrjGrp"
                                                            runat="server" Text="<% $Resources:lblFullPrjGrp %>"></asp:Label></span>
                                                </div>
                                                <div style="margin-top: 10px;">
                                                    <span>
                                                        <asp:Label ID="lblavailable" runat="server"></asp:Label></span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div>
                        <div id="Div2" runat="server" class="validation-align">
                            <td width="10%">
                                <asp:ImageButton ID="imbSubmit" runat="server" CausesValidation="true" ImageUrl="~/Layouts/Resources/images/submit.png"
                                    ToolTip="Submit" ValidationGroup="group1" OnClick="imbSubmit_Click" />
                            </td>
                            <td width="10%">
                                <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                    ToolTip="Reset" OnClick="imbReset_Click" />
                            </td>
                        </div>
                        <div id="Div3" runat="server" class="validation-align">
                            <span class="style3">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>
                        </div>
                        <rsweb:ReportViewer ID="rview" runat="server" Height="0">
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </div>
       <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imbSubmit" />
            <asp:PostBackTrigger ControlID="LinkPreview" />

        </Triggers>
    </asp:UpdatePanel>--%>

    <script type="text/javascript">
        
        if (document.getElementById('ctl00_cphMaster_ImgTopImage')) {
            if (document.getElementById('ctl00_cphMaster_hdnImgTopImage').value != "") {
                document.getElementById('ctl00_cphMaster_ImgTopImage').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnImgTopImage').value;
            }

            else {
                document.getElementById('ctl00_cphMaster_ImgTopImage').src = "../../UploadDocs/noImage.jpg ";
            }
        }

        if (document.getElementById('ctl00_cphMaster_ImgMiddleImage')) {
            if (document.getElementById('ctl00_cphMaster_hdnImgMiddleImage').value != "") {
                document.getElementById('ctl00_cphMaster_ImgMiddleImage').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnImgMiddleImage').value;
            }

            else {
                document.getElementById('ctl00_cphMaster_ImgMiddleImage').src = "../../UploadDocs/noImage.jpg ";
            }
        }

        if (document.getElementById('ctl00_cphMaster_ImgBottomImage')) {
            if (document.getElementById('ctl00_cphMaster_hdnImgBottomImage').value != "") {
                document.getElementById('ctl00_cphMaster_ImgBottomImage').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnImgBottomImage').value;
            }

            else {
                document.getElementById('ctl00_cphMaster_ImgBottomImage').src = "../../UploadDocs/noImage.jpg ";
            }
        }

        if (document.getElementById('ctl00_cphMaster_ImgRightImage')) {
            if (document.getElementById('ctl00_cphMaster_hdnImgRightImage').value != "") {
                document.getElementById('ctl00_cphMaster_ImgRightImage').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnImgRightImage').value;
            }

            else {
                document.getElementById('ctl00_cphMaster_ImgRightImage').src = "../../UploadDocs/noImage.jpg ";
            }
        }

    </script>

    <%--<script type="text/javascript">

        function SetImage() {
            if (document.getElementById('ImgMiddleImage')) {
                if (document.getElementById('ctl00_cphMaster_hdnImgMiddleImage').value != "") {
                    document.getElementById('ImgMiddleImage').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnImgMiddleImage').value;
                }
                else {
                    document.getElementById('ImgMiddleImage').src = "../../UploadDocs/noImage.jpg ";
                }
            }
        }
    
    </script>--%>

</asp:Content>

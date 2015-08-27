<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportManagement.aspx.cs"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    Inherits="Module_Reports_ReportManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>--%>
<%@Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
   <%-- <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>--%>
    <script type="text/javascript">

        function RemoveImage(i) {debugger;
            var imageID = i;
            if (imageID == "1") {
                document.getElementById('ctl00_cphMaster_hdnTopImage').value = "";
            }

           if (imageID == "2") {
                document.getElementById('ctl00_cphMaster_hdnMiddleImage').value = "";
            }

           if (imageID == "3") {
                document.getElementById('ctl00_cphMaster_hdnBottomImage').value = "";
            }
        }

        function removefrontpdf() {
            document.getElementById('<%=hdnFrontPDF.ClientID %>').value = "";
        }
        function RemoveQuestImage() {
            document.getElementById('ctl00_cphMaster_fuScoreTable').value = "";
            document.getElementById('<%=hdnimgScoreTable.ClientID %>').value = "";
            document.getElementById('<%=imgScoreTable.ClientID %>').src = "";
        }

        function RemoveFooterImage() {
            document.getElementById('ctl00_cphMaster_fuFooter').value = "";
            document.getElementById('ctl00_cphMaster_hdnimgFooter').value = "";
            document.getElementById('ctl00_cphMaster_imgFooter').src = "";
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
 <%--   <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div>
                    <!-- start heading logout -->
                    <div class="Survey_topheadingdetails">
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
                                    <td width="85%">
                                        <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:RequiredFieldValidator1 %>" SetFocusOnError="True"
                                            ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
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
                                            <asp:ListItem Value="5">Report5</asp:ListItem>
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
                                                    <asp:FileUpload ID="fuplTopImage" runat="Server" EnableViewState="true" />
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
                                                <td width="65%" valign="top" class="style3">
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
                                                    <asp:FileUpload ID="fuplMiddleImage" runat="Server" EnableViewState="true" />
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
                                                <td width="65%" valign="top" class="style3">
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
                                                    <asp:FileUpload ID="fuplBottomImage" runat="Server" EnableViewState="true" />
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
                                                <td width="65%" valign="top" class="style3">
                                                    (Recomended Size (Width X Height): 331 X 120)<asp:Label ID="Label3" Visible="false"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        Show Highest/Lowest Scores Count
                                    </td>
                                    <td valign="top" colspan="3" align="left">
                                         <asp:TextBox ID="txtRadarGraphCategoryCount" MaxLength="75" runat="server" Width="10px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td valign="top" colspan="3" align="Right">
                                        <div class="gallerycontainer" style="padding-right: 130px">
                                            <asp:LinkButton ID="LinkPreview" runat="server" OnClick="LinkPreview_Click">Preview</asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblIntroduction" runat="server" Text="<% $Resources:lblIntroduction %>"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <%--<FCKeditorV2:FCKeditor ID="txtPageIntroduction" runat="server" BasePath="~/fckeditor/"
                                            Width="700px" Value=" " ToolbarSet="Feedback">
                                        </FCKeditorV2:FCKeditor>--%>
                                         <CKEditor:CKEditorControl ID="txtPageIntroduction"  Width="700px"  BasePath="~/ckeditor"  runat="server"></CKEditor:CKEditorControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblConclus" runat="server" Text="<% $Resources:lblConclus %>"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                          <CKEditor:CKEditorControl ID="txtPageConclusion" runat="server" BasePath="~/ckeditor/"
                                            Width="700px"  >
                                        </CKEditor:CKEditorControl><%--Value=" " ToolbarSet="Feedback"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblFrontPdf" runat="server" Text="<% $Resources:lblFrontPdf %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="20%" valign="top">
                                                    <asp:FileUpload ID="pdfFileUpload" runat="Server" />
                                                </td>
                                                <td width="10%" valign="top">
                                                 <div class="gallerycontainer">
                                                    &nbsp;&nbsp;<asp:LinkButton ID="lnkbtnFrontPdf" runat="server" OnClick="lnkbtnFrontPdf_Click">Preview</asp:LinkButton>
                                                </div>
                                                </td>
                                                <td width="10%" valign="top">
                                                 &nbsp;&nbsp;&nbsp;&nbsp;<asp:HiddenField ID="hdnFrontPdfFileName" runat="server" />
                                                    <asp:HiddenField ID="hdnFrontPDF" runat="server" Value="0" />
                                                    <a href="javascript:removefrontpdf(1);" ><img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>
                                                    <%--<asp:HiddenField ID="hdnRemoveReportImage" runat="server" Value="0" />--%>
                                                    <%--<a href="javascript:void(0)" onclick="RemoveReportImage();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Image" /></a>--%>
                                                </td>
                                                <td width="60%" class="style3"></td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="2">
                                                    <%--<asp:Label ID="lblPdfSize" runat="server" Text="<% $Resources:lblPdfSize %>"></asp:Label>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                    </td>
                                    <td valign="top">
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblScoreTableImage %>"></asp:Label>
                                    </td>
                                    <td valign="top" colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="20%" valign="top">
                                                    <asp:FileUpload ID="fuScoreTable" runat="Server" EnableViewState="true" />
                                                </td>
                                                <td width="10%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="imgScoreTable" runat="server"
                                                            src="" /><br />
                                                        </span></a>
                                                    </div>
                                                </td>
                                                <td width="5%" valign="top">
                                                    <asp:HiddenField ID="hdnimgScoreTable" runat="server" Value="" />
                                                    <a href="javascript:void(0)" onclick="RemoveQuestImage();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Score Table Image" /></a>
                                                </td>
                                                <td width="65%" valign="top" class="style3">
                                                    (Recommended Size (Width X Height): 700 X 350)<asp:Label ID="Label7" Visible="false"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblFooter" runat="server" Text="<% $Resources:lblFooterImage %>"></asp:Label>
                                    </td>
                                    <td valign="top" colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="20%" valign="top">
                                                    <asp:FileUpload ID="fuFooter" runat="Server" EnableViewState="true" />
                                                </td>
                                                <td width="10%" valign="top">
                                                    <div class="gallerycontainer">
                                                        &nbsp; <a class="thumbnail" href="#thumb">Preview<span><img id="imgFooter" runat="server"
                                                            src="" /><br />
                                                        </span></a>
                                                    </div>
                                                </td>
                                                <td width="5%" valign="top">
                                                    <asp:HiddenField ID="hdnimgFooter" runat="server" Value="" />
                                                    <a href="javascript:void(0)" onclick="RemoveFooterImage();">
                                                        <img src="../../Layouts/Resources/images/remove.png" title="Remove Footer Image" /></a>
                                                </td>
                                                <td width="65%" valign="top" class="style3">
                                                    (Recommended Size (Width X Height): 600 X 50)<asp:Label ID="Label9" Visible="false"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
     <%--   </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlAccountCode" />
            <asp:PostBackTrigger ControlID="ddlProject" />
            <asp:PostBackTrigger ControlID="ddlReportType" />
        </Triggers>
    </asp:UpdatePanel>--%>
   <%-- <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>--%>
            <div id="reportselection" runat="server">
                <fieldset class="fieldsetform" visible="true">
                    <legend>
                        <asp:Label ID="lblReportSelection" runat="server" Text="<% $Resources:lblReportSelection %>"></asp:Label></legend>
                    <div id="pnlForm" style="margin: 0 auto; padding: 10px;">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <div runat="server" id="reportintro">
                                        <asp:CheckBox ID="chkReportIntro" runat="server" />
                                        <asp:Label ID="lblReportIntro" runat="server" Text="<% $Resources:lblReportIntro %>"></asp:Label>
                                    </div>
                                </td>
                                <td valign="top" width="50%" rowspan="3">
                                    <span>
                                        <input id="AnalysisI_Chkbox" runat="server" type="checkbox" /><asp:Label ID="AnalysisI_Label"
                                            runat="server" Text="Analysis I"></asp:Label>
                                    </span>
                                    <br />
                                    <span>
                                        <input id="AnalysisII_Chkbox" runat="server" type="checkbox" /><asp:Label ID="Label1"
                                            runat="server" Text="Analysis II"></asp:Label>
                                    </span>
                                    <br />
                                    <span>
                                        <input id="AnalysisIII_Chkbox" runat="server" type="checkbox" /><asp:Label ID="Label4"
                                            runat="server" Text="Analysis III"></asp:Label>
                                    </span>
                                    <br />
                                    <span>
                                        <input id="Programme_Avg_Chkbox" runat="server" type="checkbox" /><asp:Label ID="Label5"
                                            runat="server" Text="Programme Average"></asp:Label>
                                    </span>
                                    <br />
                                    <span>
                                        <input id="chkFullPrjGrp" runat="server" type="checkbox" /><asp:Label ID="lblFullPrjGrp"
                                            runat="server" Text="<% $Resources:lblFullPrjGrp %>"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="div_coverpage">
                                        <div runat="server" id="coverpage">
                                            <span>
                                                <input id="chkCoverPage" runat="server" type="checkbox" /><asp:Label ID="lblCoverPage"
                                                    runat="server" Text="<% $Resources:lblCoverPage %>"></asp:Label></span>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>
                                        <input id="chkShowScoreRespondents" runat="server" type="checkbox" />
                                        <asp:Label ID="lblShowScoreRespondents" runat="server" Text="<% $Resources:lblShowScoreRespondents %>"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="catintro" runat="server" style="margin: 15px 0 0 0;">
                                        <span>
                                            <asp:CheckBox ID="chkCategoryIntro" runat="server" AutoPostBack="true" OnCheckedChanged="chkCategoryIntro_CheckedChanged" />
                                            <asp:Label ID="lblCategoryIntro" runat="server" Text="<% $Resources:lblCategoryIntro %>"></asp:Label>
                                        </span>
                                        <div id="catQstlist" runat="server" style="margin: 0 0 0 20px;">
                                            <div>
                                                <span>
                                                    <asp:CheckBox ID="chkCatQstlist" runat="server" AutoPostBack="true" OnCheckedChanged="chkCatQstlist_CheckedChanged" />
                                                    <asp:Label ID="lblCatQstlist" runat="server" Text="<% $Resources:lblCatQstlist %>"></asp:Label>
                                                </span>
                                            </div>
                                            <div>
                                                <span>
                                                    <asp:CheckBox ID="chkCatQstChart" runat="server" AutoPostBack="true" OnCheckedChanged="chkCatQstChart_CheckedChanged" />
                                                    <asp:Label ID="lblCatQstChart" runat="server" Text="<% $Resources:lblCatQstChart %>"></asp:Label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="catQstText">
                                        <asp:CheckBox ID="chkConclusion" runat="server" />
                                        <asp:Label ID="lblConclusion" runat="server" Text="<% $Resources:lblConclusion %>"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <span>
                                        <input id="chkBoxFreeText" runat="server" type="checkbox" />
                                        <asp:Label ID="freeTextLbl" runat="server" Text="Free Text Responses"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>
                                        <input id="chkPrvScore1" runat="server" type="checkbox" />
                                        <asp:Label ID="lblPrvScore1" runat="server" Text="Show Previous Score 1"></asp:Label>
                                    </span>
                                </td>
                                <td>
                                    <span>
                                        <input id="chkPrvScore2" runat="server" type="checkbox" />
                                        <asp:Label ID="lblPrvScore2" runat="server" Text="Show Previous Score 2"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>
                                        <input id="chkRadar" runat="server" type="checkbox" />
                                        <asp:Label ID="lblRadar" runat="server" Text="Show Radar Graph"></asp:Label>
                                    </span>
                                </td>
                                <td>
                                    <span>
                                        <input id="chkTable" runat="server" type="checkbox" />
                                        <asp:Label ID="lblTable" runat="server" Text="Show Table"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>
                                        <input id="chkBarGraph" runat="server" type="checkbox" />
                                        <asp:Label ID="lblBarGraph" runat="server" Text="Show Bar Graph"></asp:Label>
                                    </span>
                                </td>
                                <td>
                                    <span>
                                        <input id="chkLineChart" runat="server" type="checkbox" />
                                        <asp:Label ID="lblLineChart" runat="server" Text="Show Line Chart"></asp:Label>
                                    </span>
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
            </div> </div>
    <%--    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imbSubmit" />
            <asp:PostBackTrigger ControlID="lnkbtnFrontPdf" />
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


        if (document.getElementById('ctl00_cphMaster_hdnimgScoreTable').value != "") {
            document.getElementById('ctl00_cphMaster_imgScoreTable').src = "../../UploadDocs/" + document.getElementById('ctl00_cphMaster_hdnimgScoreTable').value;
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="ExportData.aspx.cs" Inherits="Module_Reports_ExportData" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <%-- <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
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
                    <div id="bodytextcontainerrr">
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" class="validation-align">
                                        <%--<span class="style3">
                                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>--%>
                                        <asp:Label ID="lbl_no_data_to_export_message" runat="server" ForeColor="#FF3300"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="divAccount" runat="server" visible="false">
                            <fieldset class="fieldsetform">
                                <legend>
                                    <asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
                                <div id="Div3" style="margin: 0 auto; padding: 10px;">
                                    <table border="0" cellspacing="5" cellpadding="0" width="100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label><span
                                                    class="style3"> *</span>
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
                                                <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                            </td>
                                            <td width="35%">
                                                <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </div>
                        <div class="searchgrid">
                            <%--<fieldset class="fieldsetform">
                                  <legend>Project Details </legend>                                  
                                  <div id="Div2" style="margin: 0 auto; padding: 10px;">--%>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="7%" valign="top">
                                        <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label>
                                        <span style="vertical-align: text-top" class="style3">*</span>
                                    </td>
                                    <td width="12%" valign="top">
                                        <asp:DropDownList ID="ddlProject" runat="server" Style="width: 140px" AppendDataBoundItems="True"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:RequiredFieldValidator1 %>" SetFocusOnError="True"
                                            ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="10%" valign="top">
                                        <asp:Label ID="lblddlCompany" runat="server" Text="<% $Resources:lblddlCompany %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td width="13%" valign="top">
                                        <asp:DropDownList ID="ddlCompany" runat="server" Style="width: 140px" AppendDataBoundItems="True"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rfvCompany %>" SetFocusOnError="True" ControlToValidate="ddlCompany"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="10%" valign="top">
                                        <asp:Label ID="lblPrgramme" runat="server" Text="<% $Resources:lblPrgramme %>"></asp:Label>
                                        <span class="style3">*</span>
                                    </td>
                                    <td width="13%" valign="top">
                                        <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 140px" AppendDataBoundItems="True"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:RequiredFieldValidator2 %>" SetFocusOnError="True"
                                            ControlToValidate="ddlProgramme" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    </tr>
                                    <tr><td>&nbsp</td><td>&nbsp</td></tr>
                                    <tr>
                                        <td width="9%" valign="top" rowspan="2">
                                            &nbsp; Analysis <span class="style3">*</span>
                                        </td>
                                        <td width="13%" valign="top">
                                            <asp:DropDownList ID="DDList_analysis" runat="server" Style="width: 140px" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="group1"
                                            ErrorMessage="Please select Analysis" SetFocusOnError="True"
                                            ControlToValidate="DDList_analysis" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td width="8%" valign="top">
                                            &nbsp;&nbsp; Export<span class="style3"> *</span>
                                        </td>
                                        <td width="14%" valign="top">
                                            <asp:DropDownList ID="ddlExportType" runat="server" Style="width: 140px">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                <asp:ListItem Value="C">Export By Category</asp:ListItem>
                                                <asp:ListItem Value="Q">Export By Question</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="group1"
                                                ErrorMessage="<% $Resources:RequiredFieldValidator3 %>" SetFocusOnError="True"
                                                ControlToValidate="ddlExportType" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            &nbsp
                                        </td>
                                        <td width="14%" valign="top" align="center">
                                            <asp:ImageButton ID="imbExport" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                                ToolTip="Export" OnClick="imbExport_Click" ValidationGroup="group1" Height="24px"
                                                Width="72px" />
                                            <tr>
                                        </td>
                                    </tr>
                                    
                            </table>
                            <%-- </div> 
                                  </fieldset>--%>
                        </div>
                    </div>
                    <!-- grid list -->
                </div>
            </div>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imbExport" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <asp:GridView ID="GridView1" runat="server" BorderColor="Black" BorderStyle="Solid"
        Width="600px">
        <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
    </asp:GridView>
    <script type="text/javascript">

        function ShowPopUpPDF(fname) {
            var filename = "../../ReportGenerate/" + fname;
            window.open(filename, '', '');
        }
      
    </script>
</asp:Content>

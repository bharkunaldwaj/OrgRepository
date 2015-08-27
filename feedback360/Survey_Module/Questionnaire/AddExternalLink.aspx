<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    Culture="en-GB" UICulture="en-US" ValidateRequest="false" AutoEventWireup="true"
    CodeFile="AddExternalLink.aspx.cs" Inherits="Survey_Module_Questionnaire_AddExternalLink" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <script type="text/javascript" language="javascript">
        Sys.Application.add_load(EmailToChange);

        $(function () {

            EmailToChange();

        });
        function EmailToChange() {
            $("#ctl00_cphMaster_ddlEmailTo").change(function () {
                if ($("#ctl00_cphMaster_ddlEmailTo").val() == "Both" || $("#ctl00_cphMaster_ddlEmailTo").val() == "Email") {
                    $("[id$='txtCustomEmail']").show();
                    $("#ctl00_cphMaster_lblCustomEmail").show();

                }
                else {
                    $("#ctl00_cphMaster_txtCustomEmail").hide();
                    $("#ctl00_cphMaster_lblCustomEmail").hide();
                    //$("#ctl00_cphMaster_txtCustomEmail").val('');
                }
            });

        }
       
    </script>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <div class="Survey_topheadingdetails">
                <h3>
                    <img id="Img1" src="../../Layouts/Resources/images/project_create.png" runat="server"
                        align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="Add External Link"></asp:Label></h3>
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
            <asp:HiddenField ID="hdnPassword" runat="server" />
            <asp:HiddenField ID="hdnimage" runat="server" />
            <asp:HiddenField ID="hdnReminder2" runat="server" />
            <div class="userform">
                <div id="divAccount" runat="server" visible="true">
                    <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                        class="style3">*</span>
                                </td>
                                <td width="35%">
                                    <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 200px" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq21 %> "
                                        SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="15%">
                                    <%--<asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>--%>
                                </td>
                                <td width="35%">
                                    <%--<asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>--%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
               <%-- <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label5" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label>
                            </legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="15%">
                                        <%-- <asp:Label ID="lblName" runat="server" Text="<% $Resources:lblName %>"></asp:Label><span
                                            class="style3">*</span>--%>
                                        <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <%--  <asp:TextBox ID="txtName" MaxLength="25" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Rq2" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq2 %> "
                                            SetFocusOnError="True" ControlToValidate="txtName">&nbsp;</asp:RequiredFieldValidator>--%>
                                        <asp:DropDownList ID="ddlProject" runat="server" Width="200px" AppendDataBoundItems="true"
                                            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq1" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:Rq1 %> "
                                            SetFocusOnError="True" ControlToValidate="ddlProject" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" AppendDataBoundItems="true"
                                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rqCompany %> " SetFocusOnError="True" ControlToValidate="ddlCompany"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblProgrammeName" runat="server" Text="<% $Resources:lblProgrammeName %>"></asp:Label><span
                                            class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlProgrammeName" runat="server" AppendDataBoundItems="true"
                                            Width="200px" OnSelectedIndexChanged="ddlProgrammeName_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProgrammeName" runat="server" ValidationGroup="group1"
                                            ErrorMessage="<% $Resources:rfvProgrammeName %> " SetFocusOnError="True" ControlToValidate="ddlProgrammeName"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblEmailTo" runat="server" Text="Email To"></asp:Label><span class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlEmailTo" runat="server" Width="200px" AppendDataBoundItems="true">
                                            <asp:ListItem Value="Both">Both</asp:ListItem>
                                            <asp:ListItem Value="Email">Email</asp:ListItem>
                                            <asp:ListItem Value="Participant" Selected="True">Participant</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label4" runat="server" Text="Email Template"></asp:Label><span class="style3">*</span>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlEmailTemplate" runat="server" AppendDataBoundItems="true"
                                            Width="200px">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="group1"
                                            ErrorMessage="Please Select Email Template" SetFocusOnError="True" ControlToValidate="ddlEmailTemplate"
                                            InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblCustomEmail" runat="server" Text="Email-ID" Style="display: none"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtCustomEmail" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCustomEmail"
                                            Display="None" EnableClientScript="true" ErrorMessage="Incorrect Email format in 'Email-ID'(Correct Example: YourId@somedomain.com)"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="group1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblSendEmailAfterCompletion" runat="server" Text="Send e-mail after completion"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:CheckBox ID="chkSendEmailAfterCompletion" runat="server" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label3" runat="server" Text="Send the report to the participant"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:CheckBox ID="chkSendReportParticipant" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">Instructions for Participants:
                                    </td>
                                    <td valign="top" colspan="3">
                                        <asp:TextBox ID="txtInstructions" TextMode="MultiLine" runat="server" Style="width: 683px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        Link:</td>
                                    <td valign="top" colspan="3">
                                        <asp:TextBox ID="txtExternalLink" TextMode="MultiLine" runat="server" Style="width: 683px;" ReadOnly=true></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <span class="style3">
                            <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <br>
                        <div align="center">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Layouts/Resources/images/save.png"
                                OnClick="imbSave_Click" ToolTip="Save" ValidationGroup="group1" />
                            &nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Layouts/Resources/images/cancel.png"
                                OnClick="imbcancel_Click" ToolTip="Back to List" />
                            &nbsp;
                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="true" ImageUrl="~/Layouts/Resources/images/Back.png"
                                OnClick="imbBack_Click" ToolTip="Back to List" Visible="false" />
                        </div>
                        <div>
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td style="width: 39%">
                                    </td>
                                    <td style="width: 61%" align="left">
                                        <asp:Label ID="lblMessage" runat="server" Text="Label" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </div>
                        <!-- start user form -->
                        </div> </div>
                   <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
</asp:Content>

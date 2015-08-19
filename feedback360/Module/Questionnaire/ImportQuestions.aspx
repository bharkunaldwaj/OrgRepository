<%@ Page Title="Import Questions" Language="C#" AutoEventWireup="true" CodeFile="ImportQuestions.aspx.cs"
    MasterPageFile="~/Layouts/MasterPages/Feedback360.master" Inherits="Module_Questionnaire_ImportQuestions" %>

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
                    <div class="topheadingdetails">
                        <h3>
                            <img src="../../Layouts/Resources/images/Questionnaire_new.png" runat="server" alt="<% $Resources:lblImportQuestions %>"
                                align="absmiddle" />
                            <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblImportQuestions %>"></asp:Label></h3>
                        <div class="clear">
                        </div>
                    </div>
                    <!-- end heading logout -->
                    <!-- start user form -->
                    <div class="userform">
                        <fieldset class="fieldsetform">
                            <legend><asp:Label ID="Label1" runat="server" Text="<% $Resources:lblImportQuestions %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="38%" align="right">
                                        <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblSelectFile %>"></asp:Label>
                                    </td>
                                    <td width="62%">
                                        <label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;&nbsp;<span
                                    class="style3"><asp:Label ID="Label3" runat="server" Text="<% $Resources:lblExcel %>"></asp:Label></span>
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    
                                     <td  colspan ="2"  align="left">
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <a href="../../UploadDocs/sampleQuestion_360.xls">
                                        <asp:Label ID="Label4" runat="server" Text="<% $Resources:lblClickHere %>"></asp:Label></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div align="center">
                            <asp:ImageButton ID="ImgUpload" runat="server" ImageUrl="~/Layouts/Resources/images/Import.png"
                                OnClick="ImgUpload_click" />
                            &nbsp;&nbsp;<asp:ImageButton ID="imbcancel" runat="server" ImageUrl="~/Layouts/Resources/images/cancel.png"
                                PostBackUrl="~/Default.aspx" />
                        </div>
                    </div>
                    <!-- start user form -->
                </div>
            </div>
            <!-- start bodytext container -->
       <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgUpload" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

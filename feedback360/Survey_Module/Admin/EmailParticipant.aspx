<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="EmailParticipant.aspx.cs" Inherits="Survey_Module_Admin_EmailParticipant" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
     <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
    <div id="bodytextcontainer">
        <div class="innercontainer">
           
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/category.png" runat="server" title="<% $Resources:lblHeading %>"
                        align="absmiddle" />
                    <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
          
          
          
           
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="summary" runat="server" class="validation-align">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="group1" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                        ShowSummary="true" ValidationGroup="group2" />
                                    <asp:Label ID="lblvalidation" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div id="Div1" runat="server" class="validation-align">
                                    <span class="style3">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="divAccount" runat="server" visible="true">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></legend>
                            <table width="100%" border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                    <td width="13%">
                                        <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountCode %>"></asp:Label>
                                    </td>
                                    <td width="36%">
                                        <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                            OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Rq21" runat="server" ValidationGroup="group1" ErrorMessage="<% $Resources:lblRq21 %>  "
                                            SetFocusOnError="True" ControlToValidate="ddlAccountCode" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="13%">
                                        <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompanyName %>"></asp:Label>
                                    </td>
                                    <td width="38%">
                                        <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div class="searchgrid">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="lblProjectName" runat="server" Text="<% $Resources:lblProject %>"></asp:Label>
                                </td>
                                <td width="25%">
                                    <asp:Label ID="lblParticipant" runat="server" Text=""></asp:Label>
                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:lblRq1 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="group1"
                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="10%">
                                    <asp:Label ID="lblProgrammeName" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label>
                                </td>
                                <td width="25%">
                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        >
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:lblRequiredFieldValidator1 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlProgramme" ValidationGroup="group1"
                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td width="10%">
                                    &nbsp;
                                </td>
                                <td width="25%">
                                    &nbsp;
                                </td>
                                <td width="10%">
                                </td>
                                <td width="10%">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        ToolTip="Reset" OnClick="imbReset_Click" />
                                </td>
                                <td width="10%">
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        ToolTip="Submit" OnClick="imbSubmit_Click" ValidationGroup="group1" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divgridlist" style="overflow-y: auto; overflow-x: hidden; height: 200px; border: 1px;">
                    <asp:GridView ID="grdvCandidateStatus"  runat="server" DataSourceID="odsCandidateStatus"
                        AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                        OnSorting="grdvCandidateStatus_Sorting" OnRowDataBound="grdvCandidateStatus_RowDataBound"
                        EmptyDataText="<center><span class='style3'>No Record Found</span></center>"
                        OnRowCommand="grdvCandidateStatus_OnRowCommand">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="2%" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="myCheckBox" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                                                       
                            
                            <asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:lblColleagueName %>"
                                SortExpression="CandidateName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            
                            
                           <%-- <asp:BoundField DataField="CandidateEmail" HeaderText="<% $Resources:lblColleagueName %>"
                                SortExpression="CandidateEmail">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                                             --%>
                            
                                <asp:TemplateField HeaderText="<% $Resources:lblCandidateEmail %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle Width="28%" />
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnQuestionnaireID" Value='<%# Eval("QuestionnaireID") %>' />
                                    <asp:HiddenField runat="server" ID="CandidateName" Value='<%# Eval("CandidateName") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCandidateID" Value='<%# Eval("AsgnDetailID") %>' />
                                    <%-- <asp:HiddenField runat="server" ID="hdnPassword" Value='<%# Eval("Password") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCode" Value='<%# Eval("Code") %>' />
                                    <asp:HiddenField runat="server" ID="hdnTitle" Value='<%# Eval("Title") %>' />--%>
                                    <asp:Label runat="server" ID="lblParticipantEmail" Text='<%# Eval("CandidateEmail") %>'> </asp:Label>
                                </ItemTemplate>



                            </asp:TemplateField>
                            
                            
                              <asp:TemplateField HeaderText="<% $Resources:lblComplete %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="20%" />
                                <ItemTemplate>
                                    <asp:Label ID="lblComplete" runat="server" Visible="true" Text=""> </asp:Label>
                                    <asp:Label ID="lblQuestionnaireID" runat="server" Visible="false" Text='<%# Eval("QuestionnaireID") %>'> </asp:Label>
                                    <asp:Label ID="lblCandidateID" runat="server" Visible="false" Text='<%# Eval("AsgnDetailID") %>'> </asp:Label>
                                    <asp:Label ID="lblSubmitFlag" runat="server" Visible="false" Text='<%# Eval("SubmitFlag") %>'> </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<% $Resources:lblSubmitStatus %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="left" Width="30%" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="SubmitFlag" Text='<%# (Eval("SubmitFlag").ToString() == "True" ? "Yes" : "No" )%>'
                                        CommandName='<%# Eval("AsgnDetailID") %>' CommandArgument='<%# Eval("SubmitFlag") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                         
                        </Columns>
                    </asp:GridView>
                    <table width="98%" border="0">
                        <tr>
                            <td align="center" width="30%">
                                <table border="0" width="100%">
                                </table>
                                <asp:Literal ID="litPagingSummary" runat="server"></asp:Literal>
                            </td>
                            <td align="right" width="50%">
                                <div class="paging">
                                    <asp:PlaceHolder ID="plcPaging" runat="server"></asp:PlaceHolder>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </div>
                    
                    <fieldset class="fieldsetform">
                        <legend>
                            <asp:Label ID="lblEmailTemplates" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label></legend>
                        <table width="100%" border="0" cellspacing="5" cellpadding="0">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblStart" runat="server" Text="<% $Resources:lblStart %>"></asp:Label><span
                                        class="style3">*</span>
                                </td>
                                <td width="35%">
                                    <asp:DropDownList ID="ddlEmailStart" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlEmailStart_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Rq14" runat="server" ValidationGroup="group2" ErrorMessage="<% $Resources:Rq14 %>"
                                        SetFocusOnError="True" ControlToValidate="ddlEmailStart" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="lblSubject" runat="server" Text="<% $Resources:lblSubject %>"></asp:Label><span
                                        class="style3"></span>
                                </td>
                                <td width="35%">
                                    <asp:TextBox ID="lblEmailSubject" runat="server" Style="width: 795px" Text="" ></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td width="15%" valign="top">
                                    <asp:Label ID="Label6" runat="server" Text="<% $Resources:lblFAQ %>"></asp:Label><span
                                        class="style3"></span>
                                </td>
                                <td width="85%">
                                    <FCKeditorV2:FCKeditor ID="txtFaqText" runat="server" BasePath="~/fckeditor/" Width="800px"
                                        ToolbarSet="Feedback">
                                    </FCKeditorV2:FCKeditor>
                                    
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br>
                    <div align="center">
                        <asp:ImageButton ID="imbSend" runat="server" ImageUrl="~/Layouts/Resources/images/send.png"
                            OnClick="imbSend_Click" ToolTip="Send" ValidationGroup="group2" />
                        &nbsp;
                        <asp:ImageButton ID="imbcancel" runat="server" ImageUrl="~/Layouts/Resources/images/cancel.png"
                            OnClick="imbcancel_Click" ToolTip="Back to List" />
                    </div>
                    <br />
                    <div align="center">
                        <asp:Label ID="Label3" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </div>
                    <br />
                    <asp:ObjectDataSource ID="odsCandidateStatus" runat="server"
                        SelectMethod="GetdtAssignPartiList" 
                TypeName="Questionnaire_BAO.Survey_AssignQstnParticipant_BAO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlAccountCode" Name="accountID" 
                                PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="ddlProgramme" Name="programmeID" 
                                PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                
                
        </div>
    </div>
    
    
    </ContentTemplate>
                
            </asp:UpdatePanel>
</asp:Content>

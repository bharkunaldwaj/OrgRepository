<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignQuestionnaireList.aspx.cs"
    Inherits="Survey_Module_Feedback_AssignQuestionnaireList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Survey</title>
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/reset.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/style.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/ddmenu.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/Calendar_360.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/Calendar.css" />
    <link rel="stylesheet" type="text/css" href="../../Layouts/Resources/css/jquery-ui-1.7.2.custom.css" />

    <script src='<%= ResolveClientUrl("../../Layouts/Resources/js/common.js")%>' type="text/javascript"></script>
    
    <script type="text/javascript" src='<%= ResolveClientUrl("../../Layouts/Resources/js/GeneralFunctions.js") %>'></script>

</head>
<body>
    <form id="frmFeedback" runat="server">
    <div id="divQues" runat="server" style="height: 550px; overflow-y: scroll;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="bodytextcontainer">
            <div class="innercontainer">
                <!-- start heading logout -->
                <div class="Survey_topheadingdetails">
                    <h3>
                        <img src="../../Layouts/Resources/images/assign_questionnaire.png" runat="server" title="<% $Resources:lblToolTip %>"
                            align="absmiddle" />
                            <asp:Label ID="lblHeading" runat="server" 
                            Text="<%$ Resources:lblHeading %>"></asp:Label>
                        </h3>
                    <div class="clear"> 
                    </div>
                </div>
                <!-- end heading logout -->
                <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                   
                        <div class="searchgrid">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="33%" align="left">
                                        <asp:Label ID="lblProj" runat="server" Text="Project: "></asp:Label>                                       
                                        <asp:Label ID="lblProjectVal" runat="server" Text=""></asp:Label>
                                    </td>
                                   
                                    <td width="33%" align="left">
                                        <asp:Label ID="lblPrgName" runat="server" Text="Programme: "></asp:Label>                                       
                                        <asp:Label ID="lblProgrammeName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td width="34%" align="left">
                                        <asp:Label ID="lblQuestionair" runat="server" Text="Questionair: "></asp:Label>                                       
                                        <asp:Label ID="lblQuestionairVal" runat="server" Text=""></asp:Label>
                                    </td>
                                    <!--
                                    <td width="5%">
                                        <asp:ImageButton ID="imbExportData" runat="server" title="Export to Excel" 
                                            ImageUrl="~/Layouts/Resources/images/export.png" 
                                            />
                                    </td>
                                    -->
                                </tr>
                            </table>
                        </div>
                        <!-- grid list -->
                        <asp:GridView ID="grdvAssignQuestionnaire" runat="server" DataSourceID="odsAssignQuestionnaire"
                            AutoGenerateColumns="False" Width="100%" CssClass="grid" 
                            AllowPaging="True" AllowSorting="True"
                            OnSorting="grdvAssignQuestionnaire_Sorting" OnRowDataBound="grdvAssignQuestionnaire_RowDataBound"
                             EmptyDataText="<%$ Resources:lblNoRecordFound %>"
                            OnRowCommand="grdvAssignQuestionnaire_RowCommand" 
                             >
                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                Visible="false" />
                            <Columns>
                                
                                <asp:TemplateField HeaderText="AssignDetail ID" ControlStyle-Width="18%">
                                <ItemTemplate>
                                  <asp:Label ID="AsgnDetailID" runat="server" 
                                    Text='<%# Bind("AsgnDetailID") %>'>
                                  </asp:Label>
                                </ItemTemplate>
                                    <ControlStyle Width="15%" />
                              </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Participant Name">
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCandidateName" Text='<%# Eval("CandidateName") %>' runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                                                
                                <asp:TemplateField HeaderText="Analysis I">
                                <ItemStyle HorizontalAlign="Left" Width="13%" />
                                    <ItemTemplate>
                                        <asp:HiddenField id="hdnAnalysisI" runat="server" Value=<%# Eval("Analysis_I") %> />
                                        <asp:DropDownList ID="ddlAnalysisI" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Analysis II">
                                <ItemStyle HorizontalAlign="Left" Width="13%" />
                                    <ItemTemplate>
                                        <asp:HiddenField id="hdnAnalysisII" runat="server" Value=<%# Eval("Analysis_II") %> />
                                        <asp:DropDownList ID="ddlAnalysisII" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Analysis III">
                                <ItemStyle HorizontalAlign="Left" Width="13%" />
                                    <ItemTemplate>
                                        <asp:HiddenField id="hdnAnalysisIII" runat="server" Value=<%# Eval("Analysis_III") %> />
                                        <asp:DropDownList ID="ddlAnalysisIII" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Candidate Email">
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemTemplate>
                                        <asp:TextBox id="txtCandidateEmail" runat="server" Value=<%# Eval("CandidateEmail") %> />
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                                

                            <%--    <asp:BoundField DataField="CandidateEmail" HeaderText="Candidate Email">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>--%>
                                
                                <asp:TemplateField HeaderText="<% $Resources:lblSubmitStatus %>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="SubmitFlag" Text=<%# (Eval("SubmitFlag").ToString() == "True" ? "Yes" : "No" ) %>  ></asp:Label>
                                </ItemTemplate>
                                 </asp:TemplateField>
                            
                                <asp:ButtonField CommandName="SendMail" ButtonType="Link" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%"
                                    Text="<img id='imgSendMail' runat='server' src='../../Layouts/Resources/images/sendemail.png' title='Re-send Email' />" >
                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                </asp:ButtonField>
                                <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="2%" DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />">
                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0">
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
                        <br />
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
                        <br />
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="imbSave" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server" Visible="false"
                                ToolTip="Save" onclick="imbSave_Click" />&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsAssignQuestionnaire" runat="server" 
                            SelectMethod="GetColleaguesListView" 
                            TypeName="Questionnaire_BAO.Survey_AssignQuestionnaire_BAO" 
                            DeleteMethod="Survey_ManageCollegue">
                            <DeleteParameters>
                                <asp:Parameter Name="flag" Type="String" />
                                <asp:Parameter Name="AssignmentId" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:Parameter Name="projectID" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
    <p>
&nbsp;&nbsp;&nbsp;
    </p>
</body>
</html>

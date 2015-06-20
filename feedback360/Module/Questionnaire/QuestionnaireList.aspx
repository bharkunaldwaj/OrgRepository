<%@ Page Title="Questionnaire Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" AutoEventWireup="true" CodeFile="QuestionnaireList.aspx.cs" Inherits="Module_Questionnaire_QuestionnaireList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     
    <div id="bodytextcontainer">
        	<div class="innercontainer">
            	<!-- start heading logout -->
            		<div class="topheadingdetails">
                    	<h3><img src="../../Layouts/Resources/images/Questionnaire.png"  runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />
                    	<asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                        <div class="clear"></div>
                    </div>
                <!-- end heading logout -->
                

                
                <!-- grid list -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                
                 <div id="divAccount" runat="server" visible="false">
                    <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblAccountDetail %>"></asp:Label></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label>
                            </td>
                            <td width="36%">
                                <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" 
                                    AppendDataBoundItems="True" 
                                    onselectedindexchanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true" >
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    
                                </asp:DropDownList>
                              
                            </td>
                            <td width="13%">
                                <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany%>"></asp:Label>
                            </td>
                            <td width="38%">
                                 <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                       
                    </table>
                </fieldset>
                </div>
                
                
                
                    <!-- grid list -->
                    <asp:GridView ID="grdvQuestionnaire" runat="server" DataSourceID="odsQuestionnaire" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" 
                        OnRowDataBound="grdvQuestionnaire_RowDataBound" OnSorting="grdvQuestionnaire_Sorting"
                        DataKeyNames="QuestionnaireID" EmptyDataText="<% $Resources:lblNoRecordFound %>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="QuestionnaireID" HeaderText="<% $Resources:QuestionnaireID %>" SortExpression="QuestionnaireID"
                                Visible="False" />
                            <%--<asp:TemplateField HeaderText="Sr. No.">
                                <ItemStyle Width="6%" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Code" SortExpression="QSTNCode">
                                <ItemStyle Width="10%" />
                                <ItemTemplate>
                                    <a href="Questionnaire.aspx?Mode=R&QestId=<%# Eval("QuestionnaireID") %>">
                                        <%# Eval("QSTNCode")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Name" SortExpression="QSTNName">
                                <ItemStyle Width="15%" />
                                <ItemTemplate>
                                    <a href="Questionnaire.aspx?Mode=R&QestId=<%# Eval("QuestionnaireID") %>">
                                        <%# Eval("QSTNName")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:BoundField DataField="Code"  HeaderText="<% $Resources:grdvCode %>" SortExpression="Code">
                            <ItemStyle Width="11%" />
                            </asp:BoundField>                 
                            <asp:BoundField DataField="Name" HeaderText="<% $Resources:grdvType %>" >
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QSTNDescription" HeaderText="<% $Resources:grdvDescription %>" >
                                <ItemStyle Width="25%" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="Questionnaire.aspx?Mode=E&QestId=<%# Eval("QuestionnaireID") %>">
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link"  ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" 
                                DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />"  >
                                
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                            
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                               <ItemTemplate>
                                    <a href="#"  onclick="javascript:window.open('../../Module/Feedback/QuestionReview.aspx?QstnId=<%# Eval("QuestionnaireID") %>','','menubar=no,scrollbars=yes, resizable=yes');"  >
                                        <img id="imgView" runat="server" src="~/Layouts/Resources/images/view.png" title="View Questions' List" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                                
                        </Columns>
                        
                    </asp:GridView>
                    
                    <table width="100%" border="0" >
                    <tr>
                        <td align="left" width="20%">
                            <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png" ToolTip="Add New Questionnaire"
                                    runat="server" onclick="ibtnAddNew_Click" />
                        </td>
                        <td align="center" width="30%">
                            <asp:Literal ID="litPagingSummary" runat="server"></asp:Literal>
                        </td>
                        <td align="right" width="50%" >
                            <div class="paging" >
                                <asp:PlaceHolder ID="plcPaging" runat="server"></asp:PlaceHolder>
                            </div>
                        </td>
                    </tr>
                    </table>
                                       
                    <br />
                    
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsQuestionnaire" runat="server" 
                        DataObjectTypeName="Questionnaire_BE.Questionnaire_BE" 
                        DeleteMethod="DeleteQuestionnaire" SelectMethod="GetdtQuestionnaireList" 
                        TypeName="Questionnaire_BAO.Questionnaire_BAO" >
                    </asp:ObjectDataSource>
                   
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>  
  <!-- grid list -->
            	
            </div>
        </div>
     
</asp:Content>


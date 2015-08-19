<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" AutoEventWireup="true" CodeFile="MapCategoryRelationship.aspx.cs" Inherits="Survey_Module_Admin_MapCategoryRelationship" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img id="Img1" src="../../Layouts/Resources/images/project.png" runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />
                    <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start search -->
           <%-- <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                --%>
                
                 <div align="center">
                                        <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Red" Font-Size="Large" Text=""></asp:Label>
                                    </div>
<br><br>
   <div id="divAccount" runat="server" >
                    <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblAccountDetail" runat="server" Text="<% $Resources:lblGeneralDetails %>"></asp:Label><span
                                                        class="style3">*</span></legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label><span
                                                        class="style3">*</span>
                                </td>
                            <td width="36%">
                                <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" 
                                    AppendDataBoundItems="True" 
                                    onselectedindexchanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true" >
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    
                                </asp:DropDownList>
                              
                            </td>
                            <td width="13%">
                                <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompanyName%>"></asp:Label>:
                            </td>
                            <td width="38%">
                                 <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                                                <td width="13%" valign="top">
                                                   
                                                </td>
                                                <td width="36%" valign="top">
                                                   
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                           </tr>
                           <tr>
                                                <td width="13%" valign="top">
                                                    <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label><span
                                                        class="style3">*</span>
                                                </td>
                                                <td width="36%" valign="top">
                                                    <asp:DropDownList ID="ddlProject" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                        AutoPostBack="true" 
                                                        onselectedindexchanged="ddlProject_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="Rq1" runat="server" ErrorMessage="<% $Resources:lblRq1 %>"
                                                        SetFocusOnError="True" ControlToValidate="ddlProject" ValidationGroup="group1"
                                                        InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                           </tr>
                           <tr>
                                                <td width="13%" valign="top">
                                                   
                                                </td>
                                                <td width="36%" valign="top">
                                                   
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imbSubmit0" runat="server" 
                                                        ImageUrl="~/Layouts/Resources/images/submit-s.png" onclick="imbSubmit_Click" 
                                                        ValidationGroup="group2" />
                                                </td>
                           </tr>
                            
                        
                    </table>
                </fieldset>
                </div>
                      
                       <%-- <style>
                        .candidatelists td
                        {
                        	border:none;
                        }
                        </style>--%>
                        
                         <table width="100%" border="0" cellspacing="0" cellpadding="0" visible="false" id="tblNoData" runat="server">
                            <tr>
                                <td>
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblAssignCategory %>"></asp:Label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0" runat="server" id="Table2">
                                            <tr>
                                            <td><div class="candidatelists">
                        <asp:Label ID="lblNoData" runat="server"></asp:Label> 
                        </div>
                        </td>
                        </tr>
                        </table>
                        </fieldset>
                        </td>
                        </tr>
                        </table>
                             <table width="100%" border="0" cellspacing="0" cellpadding="0" visible="false" id="tblAssignCategories" runat="server">
                            <tr>
                                <td>
                                    <fieldset class="fieldsetform assign-question">
                                        <legend>
                                            <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblAssignCategory %>"></asp:Label></legend>
                                        <table width="100%" border="0" cellspacing="5" cellpadding="0" runat="server" id="tdRelationship">
                                            <tr>
                                            <td><div class="candidatelists">
                                             <asp:Repeater ID="rptrRelationList" runat="server" 
                                                    onitemdatabound="rptrRelationList_ItemDataBound"   >
                                                        <HeaderTemplate>
                                                     <table width="100%" border="0" cellpadding="0" cellspacing="0"  class="grid">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr  >
                                                                <td style="width:200px"> 
                                                                 
                                                               <asp:Label ID="RelationShip" Text='<%# Eval("RelationShip")%>' runat="server"></asp:Label>
                                                               
                                                                
                                                                </td>  
                                                                <td>  
                                                                <asp:DataList ID="dsCategories" runat="server" RepeatDirection="Horizontal"
                                                                   RepeatColumns="8" CellPadding="4" ForeColor="#333333" RepeatLayout="Table">
                                                                     <%--<HeaderTemplate>
                                                                   <table width="100%" border="0" cellpadding="0" cellspacing="0"  >
                                                                      <tr>
                                                                     </HeaderTemplate>--%>
                                                                     <AlternatingItemStyle BackColor="White" />
                                                                     <ItemStyle BackColor="#EFF3FB" />
                                                                     <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                     <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                 <ItemTemplate>
                                                                  <%-- <td style="border:none">--%>
                                                                   <asp:HiddenField ID="QuestionnaireID" Value='<%# Eval("QuestionnaireID")%>' runat="server"></asp:HiddenField>
                                                                    <asp:CheckBox ID="chkBoxCategory"  runat="server" Text='<%# Eval("CategoryName")%>'/>
                                                                    <asp:HiddenField ID="hdCategoryId" Value='<%# Eval("CategoryID")%>' runat="server" />
                                                                    
                                                                     
                                                                 <%--  </td>--%>
                                                                   
                                                                 </ItemTemplate>
                                                                     <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                 <AlternatingItemTemplate>
                                                                 <%--  <td style="border:none">--%>
                                                                   <asp:HiddenField ID="QuestionnaireID" Value='<%# Eval("QuestionnaireID")%>' runat="server"></asp:HiddenField>
                                                                    <asp:CheckBox ID="chkBoxCategory"  runat="server" Text='<%# Eval("CategoryName")%>'/>
                                                                    <asp:HiddenField ID="hdCategoryId" Value='<%# Eval("CategoryID")%>' runat="server" />
                                                                  <%-- </td>--%>
                                                                 </AlternatingItemTemplate>
                                                                <%-- <FooterTemplate>
                                                                 </tr>
                                                                 </table>
                                                                 </FooterTemplate>--%>
                                                                </asp:DataList>
                                                                
                                                               </td>
                                                            </tr>
                                                            
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        </table>
                                                        </FooterTemplate>
                                            </asp:Repeater> 
                                            </div>
                                            </td>
                                            </tr>
                                         
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            
                        </table>
                        
                        <span class="style3"><asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
                        <br />
                        <div align="center">
                            <asp:ImageButton ID="imbAssign" ImageUrl="~/Layouts/Resources/images/Save.png" 
                                runat="server" onclick="imbAssign_Click" Visible="false"
                                />&nbsp;
                            <asp:ImageButton ID="imbReset" Visible="false" ImageUrl="~/Layouts/Resources/images/reset.png" runat="server"
                               />
                        </div>
                        <br />
                        <div align="center">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                        <br />
                 <%--</ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>


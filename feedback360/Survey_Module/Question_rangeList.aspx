

<%@ Page Title="Question Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"  AutoEventWireup="true" CodeFile="Question_rangeList.aspx.cs" Inherits="Survey_Module_Questionnaire_QuestionList" %>

<script runat="server">

   
    
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
  <%--                  <img id="Img1" alt="Image not Found" src="../../Layouts/Resources/images/question.png"  runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />--%>
                    <img id="Img2" alt="Image not Found" src="../Layouts/Resources/images/question.png"  runat="server" title="my tooltip" align="absmiddle" />
                   <%-- <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>--%>
                    <asp:Label ID="lblHeading" runat="server" Text= "<%$Resources:lblHeading %>" ></asp:Label></h3>
            </div>
            <!-- end heading logout -->
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    
                    
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
                    <!-- grid list -->
                 
                   
                    
                    
                      <div class="clear">
                    </div>
                 
                   
                    
                    
                      <asp:GridView ID="grdvRangeList" runat="server" DataSourceID="odsProject" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"  DataKeyNames="Range_Id" EmptyDataText="No Record Found" 
                        onrowcommand="grdvRangeList_RowCommand" onsorting="grdvRangeList_Sorting" onrowdatabound="grdvRangeList_RowDataBound" 
                        >
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                           
                            <asp:TemplateField HeaderText="Range_Id" ControlStyle-Width="18%">
                                <ItemTemplate>
                                  <asp:Label ID="Range_Id" runat="server" 
                                    Text='<%# Bind("Range_Id") %>'>
                                  </asp:Label>
                                </ItemTemplate>
                                    <ControlStyle Width="15%" />
                              </asp:TemplateField>
                              
                           <asp:BoundField DataField="Range_Name" HeaderText="Range_Name" 
                                SortExpression="Range_Name" />
                            <asp:BoundField DataField="Range_Title" HeaderText="Range_Title" 
                                SortExpression="Range_Title" />
                            <asp:BoundField DataField="Range_upto" HeaderText="Range_upto" 
                                SortExpression="Range_upto" />
                             <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="Question_range.aspx?Mode=V&RangeId=<%# Eval("Range_Id") %>">
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                 
                            
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" 
                                DeleteText="<img id='imgDelete' runat='server' src='../Layouts/Resources/images/delete.png' title='Delete' />" >
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                    
                    
                    
                    
                                      
                    <table width="100%" border="0">
                        <caption>
                            <br />
                            <asp:ObjectDataSource ID="odsProject" runat="server" SelectMethod="Survey_UspGetRangeList" 
                              UpdateMethod="Survey_Edit_Range" DeleteMethod="Survey_Delete_Range"  TypeName="Questionnaire_BAO.Survey_Question_Range_BAO">
                                <DeleteParameters>
                                    <asp:Parameter Name="Range_Id" Type="Int32" />
                                </DeleteParameters>
                            </asp:ObjectDataSource>
                            &nbsp;<tr>
                                <td align="left" width="20%">
                                    <asp:ImageButton ID="ibtnAddNew" runat="server" 
                                        ImageUrl="~/Layouts/Resources/images/Add_New.png" onclick="ibtnAddNew_Click" 
                                        ToolTip="Add New Question" />
                                </td>
                                <td align="center" width="30%">
                                    <table border="0" width="100%">
                                        <tr>
                                            <td align="right" width="5%">
                                                &nbsp;</td>
                                            <td align="left" width="5%">
                                                &nbsp;
                                                <%--<asp:RangeValidator id="valTxtRange" ControlToValidate="txtSequenceIncrement" Type="Integer" MinimumValue="1" MaximumValue="10" ErrorMessage="<% $Resources:valTxtRange%>"   ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />--%>
                                                <%-- <asp:RangeValidator id="RangeValidator1" ControlToValidate="txtSequenceIncrement" Type="Integer" MinimumValue="1" MaximumValue="10" ErrorMessage="sorry !!error 420"   ValidationGroup="group1" SetFocusOnError="True" Text="*" ForeColor="White" runat="server" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Literal ID="litPagingSummary" runat="server"></asp:Literal>
                                </td>
                                <td align="right" width="50%">
                                    <div class="paging">
                                        <asp:PlaceHolder ID="plcPaging" runat="server"></asp:PlaceHolder>
                                    </div>
                                </td>
                            </tr>
                        </caption>
                    </table>
                    <br />
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>

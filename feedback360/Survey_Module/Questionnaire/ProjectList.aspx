<%@ Page Title="Project Management" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" AutoEventWireup="true" CodeFile="ProjectList.aspx.cs" Culture="en-GB" UICulture="en-US" Inherits="Survey_Module_Questionnaire_ProjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">

    <script type="text/javascript">

//        function pageLoad() {

//            $(document).ready(function() {
//            $("#ctl00_cphMaster_dtStartDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });

//            $("#ctl00_cphMaster_dtEndDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
//            });





//        }

  

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/project.png" runat="server" title="<% $Resources:lblToolTip %>" align="absmiddle" />
                    <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                <div class="clear">
                </div>
            </div>
            <!-- end heading logout -->
            <!-- start search -->
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
                                <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompanyName%>"></asp:Label>
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
                                <td width="11%">
                                    <asp:Label ID="lblStatus" runat="server" Text="<% $Resources:lblStatus%>"></asp:Label>
                                </td>
                                <td width="22%">
                                    <asp:DropDownList ID="ddlstatus" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="2">Inactive</asp:ListItem>
                                        <asp:ListItem Value="3">Suspended</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="11%">
                                    <asp:Label ID="lblRefrence" runat="server" Text="<% $Resources:lblRefrence%>"></asp:Label> 
                                </td>
                                <td width="23%">
                                    <asp:TextBox ID="txtreference" MaxLength="25" runat="server"></asp:TextBox>
                                </td>
                                <td width="12%">
                                    <asp:Label ID="lblTitle" runat="server" Text="<% $Resources:lblTitle%>"></asp:Label> 
                                </td>
                                <td width="21%">
                                    <asp:TextBox ID="txttitle" MaxLength="25" runat="server"></asp:TextBox>
                                </td>
                                <td rowspan="2">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png" ToolTip="Reset"
                                        OnClick="imbReset_Click" />
                                    <br />
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png" ToolTip="Submit"
                                        OnClick="imbSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblManager" runat="server" Text="<% $Resources:lblManager%>"></asp:Label> 
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlManager" runat="server" Style="width: 155px" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <%--<td>
                                    Start Date 
                                </td>
                                <td class="calimg">
                                    <asp:TextBox ID="dtStartDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                        SkinID="dob" onchange="SetDTPickerDate('dtStartDate','txtStartDate');"></asp:TextBox>
                                    <div style="display: none">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    End Date 
                                </td>
                                <td class="calimg">
                                    <asp:TextBox ID="dtEndDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                        SkinID="dob" onchange="SetDTPickerDate('dtEndDate','txtEndDate');"></asp:TextBox>
                                    <div style="display: none">
                                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                    </div>
                                </td>--%>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- start search -->
                    <!-- grid list -->
                    <!-- grid list -->
                    <asp:GridView ID="grdvProject" runat="server" DataSourceID="odsProject" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True" OnSorting="grdvProject_Sorting"
                        OnRowDataBound="grdvProject_RowDataBound" DataKeyNames="ProjectID"
                        EmptyDataText="<% $Resources:lblNoRecordFound %>" >
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="ProjectID" HeaderText="<% $Resources:grdvProjectID%>" SortExpression="ProjectID"
                                Visible="False" />
                            <%--<asp:TemplateField HeaderText="Sr. No.">
                                <ItemStyle Width="6%" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Title" SortExpression="Title">
                                <ItemStyle Width="16%" />
                                <ItemTemplate>
                                    <a href="Projects.aspx?Mode=R&PrjId=<%# Eval("ProjectID") %>">
                                        <%# Eval("Title")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:BoundField DataField="Code"  HeaderText="<% $Resources:grdvCode%>" SortExpression="Code">
                            <ItemStyle Width="11%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProjectStatus" HeaderText="<% $Resources:grdvStatus%>" SortExpression="ProjectStatus">
                                <ItemStyle Width="18%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Reference" HeaderText="<% $Resources:grdvRefrence%>" SortExpression="Reference">
                                <ItemStyle Width="18%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="finalname" HeaderText="<% $Resources:grdvManager%>" SortExpression="finalname">
                                <ItemStyle Width="21%" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <a href="Projects.aspx?Mode=E&PrjId=<%# Eval("ProjectID") %>">
                                        <img id="imgEdit" runat="server" src="~/Layouts/Resources/images/edit.png" title="Edit" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="2%" 
                                DeleteText="<img id='imgDelete' runat='server' src='../../Layouts/Resources/images/delete.png' title='Delete' />" >
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
                                <asp:ImageButton ID="ibtnAddNew" ImageUrl="~/Layouts/Resources/images/Add_New.png" ToolTip="Add New Project"
                                    runat="server" OnClick="ibtnAddNew_Click" />
                            </td>
                            <td align="center" width="30%">
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
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:ObjectDataSource ID="odsProject" runat="server" 
                        DataObjectTypeName="Questionnaire_BE.Survey_Project_BE" DeleteMethod="DeleteProject" 
                        SelectMethod="GetdtProjectListNew" TypeName="Questionnaire_BAO.Survey_Project_BAO" >
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>

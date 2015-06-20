<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="ReminderEmailHistory.aspx.cs" Inherits="Survey_Module_Admin_ReminderEmailHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">

        function pageLoad() {

            $(document).ready(function() {
                $("#ctl00_cphMaster_dtFromDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, minDate: '-360d', defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
            });


            $(document).ready(function() {
                $("#ctl00_cphMaster_dtToDate").datepicker({ showOn: 'button', buttonImage: '../../Layouts/Resources/images/cal1.gif', buttonImageOnly: true, minDate: '-360d', defaultDate: '0d', changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', yearRange: '-80:+100' });
            });

        } 

    </script>

    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img id="Img1" src="../../Layouts/Resources/images/user.png" runat="server" title="<% $Resources:lblReminderEmailHistory %>"
                        align="absmiddle" />
                    <asp:Label ID="lblReminderEmailHistory" runat="server" Text="<% $Resources:lblReminderEmailHistory %>"></asp:Label>
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
                                <td width="11%">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<% $Resources:lblFromDate %>"></asp:Label>
                                </td>
                                <td width="22%" align="left" class="calimg">
                                    <asp:TextBox ID="dtFromDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                        SkinID="dob" onchange="SetDTPickerDate('dtFromDate','txtFromDate');"></asp:TextBox>
                                    <div style="display: none">
                                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                                <td width="11%">
                                    <asp:Label ID="lblToDate" runat="server" Text="<% $Resources:lblToDate %>"></asp:Label>
                                </td>
                                <td width="22%" align="left" class="calimg">
                                    <asp:TextBox ID="dtToDate" Width="80" runat="server" MaxLength="15" ReadOnly="true"
                                        SkinID="dob" onchange="SetDTPickerDate('dtToDate','txtToDate');"></asp:TextBox>
                                    <div style="display: none">
                                        <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                                <td width="34%" align="right">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        OnClick="imbReset_Click" ToolTip="Reset" />
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        OnClick="imbSubmit_Click" ToolTip="Submit" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="grdvReminderMailHistory" runat="server" DataSourceID="odsReminderMailHistory"
                        AutoGenerateColumns="False" Width="100%" CssClass="grid" AllowPaging="True" AllowSorting="True"
                        OnRowDataBound="grdvReminderMailHistory_RowDataBound" DataKeyNames="RemId" EmptyDataText="<%$ Resources:lblNoRecordFound %>">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                            Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="Type" HeaderText="<% $Resources:Type %>" SortExpression="Type"
                                Visible="False" />                           
                            <asp:BoundField DataField="AccountName" HeaderText="<% $Resources:AccountName %>"
                                SortExpression="AccountName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ParticipantName" HeaderText="<% $Resources:ParticipantName %>"
                                SortExpression="ParticipantName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="CandidateName" HeaderText="<% $Resources:CandidateName %>"
                                SortExpression="CandidateName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="ProjectName" HeaderText="<% $Resources:ProjectName %>"
                                SortExpression="ProjectName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProgrammeName" HeaderText="<% $Resources:ProgrammeName %>"
                                SortExpression="ProgrammeName">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmailDate" HeaderText="<% $Resources:EmailDate %>" SortExpression="EmailDate"
                                DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="EmailStatus" HeaderText="<% $Resources:EmailStatus %>" SortExpression="EmailStatus" DataFormatString="{0:(1 ? 'Yes': 'No')}">
                <ItemStyle Width="10%" />
            </asp:BoundField>  --%>
                            <asp:TemplateField HeaderText="<% $Resources:Type %>">
                                <ItemStyle Width="15%" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnType" runat="server" Value='<%# Eval("Type") %>' />
                                    <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="<% $Resources:EmailStatus %>">
                                <ItemStyle Width="10%" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("EmailStatus") %>' />
                                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" width="20%">
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
                    <asp:ObjectDataSource ID="odsReminderMailHistory" runat="server"
                        SelectMethod="GetdtReminderEmailHistoryList" 
                        TypeName="Admin_BAO.Survey_ReminderEmailHistory_BAO">
                        <SelectParameters>
                            <asp:Parameter Name="sql" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
            <!-- grid list -->
        </div>
    </div>
</asp:Content>

<%@ Page Title="Create New Group" Language="C#" AutoEventWireup="true" CodeFile="GroupMaintenance.aspx.cs"
    Inherits="Module_Admin_GroupMaintenance" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <div id="bodytextcontainer">
        <div class="innercontainer">
            <!-- start heading logout -->
            <div class="topheadingdetails">
                <h3>
                    <img src="../../Layouts/Resources/images/create_email.png" runat="server" title="<% $Resources:lblToolTip %>"
                        align="absmiddle" />
                    <asp:Label ID="lblheader" runat="server" Text="<% $Resources:lblheader %>"></asp:Label> 
                </h3>
            </div>
            <div class="validation-align">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="" />
                <asp:Panel ID="pnlMsg" runat="server" Visible="false">
                </asp:Panel>
            </div>
            <!-- end heading logout -->
            <!-- start user form -->
            <div class="userform">
                <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblDetail" runat="server" Text="<% $Resources:lblDetail %>"></asp:Label> </legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%">
                                <asp:Label ID="lblTitle" runat="server" Text="<% $Resources:lblTitle %>"></asp:Label>
                                <span class="style3">*</span>
                            </td>
                            <td width="36%">
                                <asp:TextBox ID="txtGroupName" runat="server" CssClass="i-box"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ControlToValidate="txtGroupName"
                                    SetFocusOnError="True" ErrorMessage="<% $Resources:rfvGroupName %>" Text="*" ForeColor="White"></asp:RequiredFieldValidator>
                            </td>
                            <td width="13%">
                                <asp:Label ID="lblActive" runat="server" Text="<% $Resources:lblActive %>"></asp:Label>
                                
                            </td>
                            <td width="38%">
                                <asp:CheckBox ID="chkIsActive" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblDesc" runat="server" Text="<% $Resources:lblDesc %>"></asp:Label>
                                <span class="style3">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" SkinID="txtarea300X3" TextMode="MultiLine"
                                    Rows="3"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDescription"
                                    SetFocusOnError="True" ErrorMessage="<% $Resources:rfvDesc %>" Text="*" ForeColor="White"></asp:RequiredFieldValidator>
                                <div style="font: normal 10px verdana; color: #999">
                                <asp:Label id="lblCharactersLimit"  runat="server" Text="<% $Resources:lblCharactersLimit %>"></asp:Label>
                                
                                    </div>
                            </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="fieldsetform">
                    <legend><asp:Label ID="lblAccess" runat="server" Text="<% $Resources:lblAccess %>"></asp:Label>  </legend>
                    <table width="100%" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td width="13%" valign="top">
                                <asp:Label ID="lblRights" runat="server" Text="<% $Resources:lblRights %>"></asp:Label>                                
                            </td>
                            <td width="36%" valign="top">
                            <asp:Label runat="server" ID="Feedback_tree" Text="Feedback" ></asp:Label>
                                <asp:TreeView ID="tvGroupRights_Feedback" onclick="javascript: return OnTreeClick(event)"
                                    ShowCheckBoxes="All" ShowLines="false" ShowExpandCollapse="true" runat="server"
                                    ExpandDepth="0">
                                    <NodeStyle CssClass="padding:5px;" ForeColor="#017BAF" />
                                </asp:TreeView>
                            </td>
                            
                            
                            <td width="36%" valign="top">
                              <asp:Label runat="server" ID="Survey_tree" Text="Survey" ></asp:Label>
                                <asp:TreeView ID="tvGroupRights_Survey" onclick="javascript: return OnTreeClick(event)"
                                    ShowCheckBoxes="All" ShowLines="false" ShowExpandCollapse="true" runat="server"
                                    ExpandDepth="0">
                                    <NodeStyle CssClass="padding:5px;" ForeColor="#017BAF" />
                                </asp:TreeView>    
                            </td>
                            
                            <td width="36%" valign="top">
                              <asp:Label runat="server" ID="Label1" Text="Personality" ></asp:Label>
                                <asp:TreeView ID="tvGroupRights_Personality" onclick="javascript: return OnTreeClick(event)"
                                    ShowCheckBoxes="All" ShowLines="false" ShowExpandCollapse="true" runat="server"
                                    ExpandDepth="0">
                                    <NodeStyle CssClass="padding:5px;" ForeColor="#017BAF" />
                                </asp:TreeView>    
                            </td>
                            <td width="15%" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div align="center">
                    <asp:ImageButton ID="imbSave" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server"
                        OnClick="imbSave_Click" ToolTip="Save" />&nbsp;
                    <asp:ImageButton ID="imbCancel" ImageUrl="~/Layouts/Resources/images/Cancel.png"
                        runat="server" CausesValidation="false" OnClick="imbCancel_Click" ToolTip="Back to list" />
                    <asp:ImageButton ID="imbBack" Visible="false" ImageUrl="~/Layouts/Resources/images/Back.png"
                        CausesValidation="false" runat="server" PostBackUrl="~/Module/Admin/GroupMaintenanceList.aspx"
                        ToolTip="Back to list" />
                </div>
                <br />
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>
            </div>
            <!-- start user form -->
        </div>
    </div>
</asp:Content>

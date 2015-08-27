<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master" AutoEventWireup="true" CodeFile="Question_range.aspx.cs" Inherits="Survey_Module_Question_range" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4
        {
            width: 800px;
        }
        .style10
        {
            width: 300px;
        }
        .style11
        {
            width: 390px;
        }
        .style12
        {
            width: 487px;
        }
        .style13
        {
           width:190px;
        }
        .stylexx
        {
        }
        
        .style22
        {
            width: 130px;
        }
        
        
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>  
    
      <%--<asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>--%>
    <div class="innercontainer">
            <!-- start heading logout -->
            <div class="Survey_topheadingdetails">
                <h3>
                    <img id="Img1" alt="Image  Not available" src="../Layouts/Resources/images/question.png"
                    runat="server" title="<% $Resources:lblToolTip %>" align="middle" />
                <asp:Label ID="Label1" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label>
                    </h3>
                <div class="clear">
                </div>
            </div>
     
    
     
    
    
    <fieldset class="fieldsetform">
        <legend>
            <asp:Label ID="Label2" runat="server" Text="<% $Resources:lblRangeDetails %>"></asp:Label></legend>
          <table class="style4">
        <tr>
        <td class="style11" valign="top"> 
           
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
           
            <asp:Label ID="LblName" Text="Name" runat="server"></asp:Label>
            <span
                                            class="style3">* </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            </td>
            <td class="style22" colspan="2" dir="ltr" valign="top">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtName" ErrorMessage="RequiredFieldValidator">Name can't Be null.</asp:RequiredFieldValidator>
            </td>
            <td class="style12" valign="top">
                &nbsp;
                <asp:Label ID="lblTitle" Text="Title" runat="server" ></asp:Label>
                &nbsp;&nbsp;
                <asp:TextBox ID="txtTitle"  runat="server" Width="201px"></asp:TextBox>
            </td>
        </tr>
            <tr>
                <td class="style11"  valign="top">
                   
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   
                    <asp:Label ID="Label3" runat="server" Text="Select the Range"></asp:Label><span id="hidespan" runat="server"
                                            class="style3">* </span>
                    <br />
  
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  
                                                                    
                    <br />

                                                
                </td>
                <td valign="top" class="style13"> <asp:TextBox ID="txtTo" runat="server" Width="36px"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txtTo" ErrorMessage="RequiredFieldValidator">Range Cant Be Null.</asp:RequiredFieldValidator>
                </td>
                <td class="stylexx" valign="top" colspan="2" >&nbsp;&nbsp;&nbsp; <asp:ImageButton ID="imbSubmit" ImageUrl="~/Layouts/Resources/images/submit-s.png"
                    runat="server" onclick="imbSubmit_Click"/>
                    &nbsp;&nbsp;
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    <br />
                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                        ControlToValidate="txtTo" ErrorMessage="RangeValidator" MaximumValue="10" 
                        MinimumValue="1" Type="Integer">Range should be between 1 and 10.</asp:RangeValidator>
                &nbsp;
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                        FilterType="Numbers" TargetControlID="txtTo">
                    </asp:FilteredTextBoxExtender>
                </td>
               
            </tr>
        </table>
    </fieldset>
    <table>
        <tr>
            <td class="style10">
                <div >
                                     <asp:Repeater ID="rptrCandidateList" runat="server" EnableTheming="true">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid">
                                                                    <tr>
                                                                        <th width="5%">
                                                                            <asp:Label ID="lblSrNo" runat="server" Text="<% $Resources:lblSrNo %>"></asp:Label>
                                                                        </th>
                                                                        <th width="95%">
                                                                            <asp:Label ID="Rating_Text" runat="server" Text="<% $Resources:lblscore_ratings %>"></asp:Label>
                                                                        </th>
                                                                        </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr style="width:100%" >
                                                                    <td width="5%">
                                                                        <%# Container.ItemIndex + 1  %>.
                                                                    </td>
                                                                    <td width="95%">
                                                                        <asp:TextBox Width="1500px" ID="Rating_TextBox" Text='<%# Eval("Rating_Text") %>' runat="server"></asp:TextBox>
                                                                       
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
    <span class="style3">
        <asp:Label ID="lblMandatory" runat="server" Text="<% $Resources:lblMandatory %>"></asp:Label></span>
    <div align="center">
        <asp:ImageButton ID="imbAssign" ImageUrl="~/Layouts/Resources/images/Save.png" runat="server"
            ValidationGroup="group1" onclick="imbAssign_Click" Height="25px" />&nbsp;
        <asp:ImageButton ID="ImgBtn_Rset" ImageUrl="~/Layouts/Resources/images/reset.png"
            runat="server" onclick="ImgBtn_Rset_Click" />
    &nbsp;&nbsp;
    <asp:ImageButton ID="imbBack" runat="server" CausesValidation="true" ImageUrl="~/Layouts/Resources/images/Back.png"
                                OnClick="imbBack_Click" ToolTip="Back to List" Visible="true" />
    </div>
    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
    <br />
                    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
      <%-- </ContentTemplate>
    </asp:UpdatePanel>
       --%>
               
</asp:Content>

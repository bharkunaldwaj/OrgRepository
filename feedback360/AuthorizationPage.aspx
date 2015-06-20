<%----==========================================================================================
 Project              : DamcoWebTemplate
 File Name            : AuthorizationPage.aspx
 Program Description  : Design file of Authorization.aspx page
 Programmed By        : SubrataM
 Programmed On        : 25th May 2009
 Modification History :
 
==========================================================================================----%>
<%@ Page Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master" AutoEventWireup="true" 
    CodeFile="AuthorizationPage.aspx.cs" Inherits="AuthorizationPage" Title="Unauthorized Access" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div style="display: inline;">
        <p class="Center" runat="server" id="ConfirmationMsgArea">
        </p>
        <div id="txt" style="color: Gray;">
        </div>
    </div>
</asp:Content>

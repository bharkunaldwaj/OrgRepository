<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"
    AutoEventWireup="true" CodeFile="ViewList.aspx.cs" Inherits="Module_Reports_ViewList"
    ValidateRequest="false" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
  <%--  <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="36000" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>--%>
            <div id="bodytextcontainer">
                <div class="innercontainer">
                    <!-- start heading logout -->
                    <div class="Survey_topheadingdetails">
                        <h3>
                            <img id="Img1" src="../../Layouts/Resources/images/view-report.png" runat="server" title="<% $Resources:lblToolTip %>"
                                align="absmiddle" />
                            <asp:Label ID="lblHeading" runat="server" Text="<% $Resources:lblHeading %>"></asp:Label></h3>
                        <div  align="center" >
                        
                        
                   <%-- <asp:UpdateProgress Visible="true" ID="Up1" runat="Server" AssociatedUpdatePanelID="updPanel" >
    <ProgressTemplate>--%>
  <%--  <asp:Label ID="rptGenerateLbl" runat="server" Visible="false" Text="Wait while the Report is being Generated.." ></asp:Label>--%>
   <%-- <img id="progress_circle" src="../../UploadDocs/Send1.gif" visible="false" alt="Please wait..." />--%>
   <%-- </ProgressTemplate>
    </asp:UpdateProgress>  --%>
                     
                        </div>
                    </div>
                    <%--<div id="bodytextcontainer">--%>
                    <%--<table border="0" width="100%">
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" class="validation-align">
                                        <span class="style3">
                                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                        </table>--%>
                    <div id="divAccount" runat="server" visible="false">
                        <fieldset class="fieldsetform">
                            <legend>
                                <asp:Label ID="lblAccountDetails" runat="server" Text="<% $Resources:lblAccountDetails %>"></asp:Label></h3></legend>
                            <div id="Div3" style="margin: 0 auto; padding: 10px;">
                                <table border="0" cellspacing="5" cellpadding="0" width="100%">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="lblAccountcode" runat="server" Text="<% $Resources:lblAccountcode %>"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:DropDownList ID="ddlAccountCode" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                                OnSelectedIndexChanged="ddlAccountCode_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblCompany" runat="server" Text="<% $Resources:lblCompany %>"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:Label ID="lblcompanyname" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div class="searchgrid">
                        <%--<fieldset class="fieldsetform">
                                  <legend>Project Details </legend>                                  
                                  <div id="Div2" style="margin: 0 auto; padding: 10px;">--%>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="11%">
                                    <asp:Label ID="lblProject" runat="server" Text="<% $Resources:lblProject %>"></asp:Label>
                                </td>
                                <td width="23%">
                                    <asp:DropDownList ID="ddlProject" runat="server" 
                                        Style="width: 155px; height: 19px;" AppendDataBoundItems="True"
                                        AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="12%">
                                    <asp:Label ID="lblProgramme" runat="server" Text="<% $Resources:lblProgramme %>"></asp:Label>
                                </td>
                                <td width="21%">
                                    <asp:DropDownList ID="ddlProgramme" runat="server" Style="width: 155px" AppendDataBoundItems="True"
                                        AutoPostBack="False" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="2" align="right">
                                    <asp:ImageButton ID="imbReset" runat="server" ImageUrl="~/Layouts/Resources/images/reset.png"
                                        ToolTip="Reset" OnClick="imbReset_Click" />
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="imbSubmit" runat="server" ImageUrl="~/Layouts/Resources/images/submit.png"
                                        ToolTip="Submit" OnClick="imbSubmit_Click"  />
                                </td>
                            </tr>
                        </table>
                        <%-- </div> 
                                  </fieldset>--%>
                    </div>
                    <%--</div>--%>
                    <!-- grid list -->
                    
                    <br>
                    
                    <br />
                    <div align="center">
                    </div>
                    <rsweb:ReportViewer ID="rview" runat="server" Height="600" Width="800" >
                    </rsweb:ReportViewer>
                </div>
            </div>
            <div>
                <table class="sampleTable">
                    <tr>
                        <td width="412" class="tdchart">
                            <%-- Manage Chart height & width from below --%>
                            <asp:Chart ID="Chart1" runat="server" Width="810px" Height="370px" Visible="false"
                                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png" Palette="None"
                                BorderDashStyle="none" BorderWidth="2">
                                <Titles>
                                    <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" Text="" ForeColor="26, 59, 105">
                                    </asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend IsTextAutoFit="False" Name="Default" BackColor="#FAFA9D" Font="Trebuchet MS, 8.25pt, style=Bold"
                                        Alignment="Far" BorderColor="LightGray" BorderWidth="4" ShadowColor="LightGray"
                                        ShadowOffset="5" ItemColumnSpacing="20">
                                        <Position Y="18.08253" Height="12.23021" Width="26.34047" X="72.73474"></Position>
                                    </asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="None" BackColor="White"></BorderSkin>
                                <%--<series >
								<asp:Series MarkerBorderColor="64, 64, 64" MarkerSize="9" Name="Series1" ChartType="Radar" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="1"></asp:Series>
								<asp:Series MarkerBorderColor="64, 64, 64" MarkerSize="9" Name="Series2" ChartType="Radar" BorderColor="180, 26, 59, 105" Color="220, 252, 180, 65" ShadowOffset="1"></asp:Series>
							</series>--%>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                        BackColor="White" ShadowColor="Transparent">
                                        <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                            WallWidth="0" IsClustered="False" />
                                        <Position Y="15" Height="90" Width="78" X="5"></Position>
                                        <%--<AxisY LineColor="64, 64, 64, 64" Minimum="3" Maximum="10" Interval="1">--%>
                                        <AxisY LineColor="64, 64, 64, 64" Interval="1">
                                            <%-- Change Scale & CategoryName FontSize and FontColor(here:ForeColor="#666666") select Below labelstyle go property --%>
                                            <LabelStyle Font="Trebuchet MS, 7.25pt, style=Bold" IntervalOffsetType="Number" IntervalType="Number"
                                                ForeColor="#333333" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                            <MajorTickMark Size="0.1" />
                                        </AxisY>
                                        <AxisX LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 7.25pt, style=Bold" ForeColor="#333333" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divProcess" style="background-color: red; height: 50px; width: 50px; display: none;">
            </div>
            <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr id="az" runat="server">
                    <td align="center">
                        <asp:UpdateProgress runat="server" ID="PageUpdateProgress" AssociatedUpdatePanelID="updPanel">
                            <ProgressTemplate>
                                <div align="center" style="background-color: #000; top: 0px; left: 0px; bottom: 0px;
                                    right: 0px; padding-top: 20%; margin: 0; width: 100%; height: 100%; overflow: hidden;
                                    position: absolute; z-index: 1000; filter: alpha(opacity=50); opacity: 0.5;">
                                    <asp:Image ID="imgWait" runat="server" ImageUrl="~/Layouts/Resources/images/ajaxloading.gif"
                                        ImageAlign="Middle" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>--%>
      <%--  </ContentTemplate>
        <Triggers>
        
       <asp:PostBackTrigger ControlID="imbSubmit" />--%>
        <%--<asp:AsyncPostBackTrigger ControlID="imbSubmit"/>--%>
        <%--</Triggers>
   </asp:UpdatePanel>--%>
    </asp:Content>

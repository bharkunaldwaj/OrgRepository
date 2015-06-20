<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Feedback360.master"
    AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMaster" runat="Server">
    <!-- start bodytext container -->

    <script type="text/javascript">

        function WindowPopup() {

            var url = document.getElementById('ctl00_cphMaster_hdnLink').value;

            if (url != "")
                window.open(url, '', ',type=fullWindow,resizable=yes,menubar=yes,location=yes,toolbar=yes,status=yes,scrollbars=yes');
            else
                alert("No questionnaire has been assigned to you");

        }

    </script>

    <asp:HiddenField ID="hdnProjectId" runat="server"></asp:HiddenField>
    <div id="bodytextcontainer">
        <div class="welcomepage">
            <div class="watermark" id="divWaterMark" runat="server">
                <img alt="Image not found" src="Layouts/Resources/images/watermark.jpg" width="455" height="351" /></div>
            <div class="left-img">
                <img alt="Image not found" src="Layouts/Resources/images/360.jpg" width="224" height="482" /></div>
            <div id="divNavigation" runat="server">
                <div class="main-nav">
                    <ul>
                        <li><a href="Default.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/AssignQuestionnaire.png" width="48" height="48" /></div>
                            <div class="text">
                                360 Feedback<br />
                            </div>
                        </a></li>
                        <li><a href="Survey_Default.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/pending.png" width="48" height="48" /></div>
                            <div class="text">
                                Survey<br />
                            </div>
                        </a></li>
                        <li><a href="#">
                            <div class="icon">
                                <img src="Layouts/Resources/images/create-user.png" width="48" height="48" /></div>
                            <div class="text">
                                Personality<br />
                            </div>
                        </a></li>
                    </ul>
                </div>
            </div>
            <div id="divParticipant" runat="server">
                <asp:HiddenField ID="hdnLink" runat="server" 
                    onvaluechanged="hdnLink_ValueChanged" />
                <br />
                <br />
                <br />
                <asp:HiddenField ID="hdnLink0" runat="server" />
                <br />
                <br />
                <br />
                <div class="main-nav">
                    <ul>
                        <li><a href="#" onclick="ShowPopup();">
                            <div class="icon">
                                <img src="Layouts/Resources/images/help.png" width="48" height="48" /></div>
                            <div class="text">
                                Help</div>
                        </a></li>
                    </ul>
                </div>
            </div>
            <div id="divManager" runat="server" visible="false">
               
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function ShowPopup() {
            var path = "Module/Feedback/ProjectFAQ.aspx?ProjectId=" + document.getElementById('ctl00_cphMaster_hdnProjectId').value;

            window.open(path, '', 'left=100,top=100,height=475,width=1000');
        }
    
    </script>

    <!-- start bodytext container -->
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/MasterPages/Survey.master"  AutoEventWireup="true" CodeFile="Survey_Default.aspx.cs" Inherits="_Default" %>

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
                <img src="Layouts/Resources/images/Survey_watermark.jpg" width="455" height="351" /></div>
            <div class="left-img">
                <img src="Layouts/Resources/images/360.jpg" width="224" height="482" /></div>
            <div id="divNavigation" runat="server">
                <div class="main-nav">
                    <ul>
                        <li><a href="Survey_Module/Questionnaire/AssignQuestionnaire.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/AssignQuestionnaire.png" width="48" height="48" /></div>
                            <div class="text">
                                Set up your Participants</div>
                        </a></li>
                        <li><a href="Survey_Module/Questionnaire/ViewParticipantStatus.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/pending.png" width="48" height="48" /></div>
                            <div class="text">
                                View Completion Status</div>
                        </a></li>
                        <li><a href="Survey_Module/Reports/ViewList.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/view-report.png" width="48" height="48" /></div>
                            <div class="text">
                                View Reports</div>
                        </a></li>
                        <li><a href="Survey_Module/Questionnaire/Projects.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/Create-Project.png" width="48" height="48" /></div>
                            <div class="text">
                                Project Management</div>
                        </a></li>
                        <li><a href="Survey_Module/Questionnaire/Questionnaire.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/Create-Questionnaire.png" width="48" height="48" /></div>
                            <div class="text">
                                Questionnaire Management</div>
                        </a></li>
                        <li><a href="Survey_Module/Admin/AccountUser.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/create-user.png" width="48" height="48" /></div>
                            <div class="text">
                                User Management</div>
                        </a></li>
                    </ul>
                </div>
            </div>
            <div id="divParticipant" runat="server">
                <asp:HiddenField ID="hdnLink" runat="server" />
                <div class="main-nav">
                    <ul>
                        <li><a href="Survey_Module/Questionnaire/AssignQuestionnaire.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/AssignQuestionnaire.png" width="48" height="48" /></div>
                            <div class="text">
                                Set up your colleagues</div>
                        </a></li>
                        <li><a href="#" onclick="WindowPopup();">
                            <div class="icon">
                                <img src="Layouts/Resources/images/AssignQuestionnaire.png" width="48" height="48" /></div>
                            <div class="text">
                                Your Self Assessment</div>
                        </a></li>
                        <li><a href="Survey_Module/Questionnaire/ViewParticipantStatus.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/pending.png" width="48" height="48" /></div>
                            <div class="text">
                                View Completion Status</div>
                        </a></li>
                        <li><a href="Survey_Module/Reports/ViewList.aspx?Type=1">
                            <div class="icon">
                                <img src="Layouts/Resources/images/view-report.png" width="48" height="48" /></div>
                            <div class="text">
                                View Reports</div>
                        </a></li>
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
                <div class="main-nav">
                    <ul>
                        <li><a href="Survey_Module/Questionnaire/ViewParticipantStatus.aspx">
                            <div class="icon">
                                <img src="Layouts/Resources/images/pending.png" width="48" height="48" /></div>
                            <div class="text">
                                View Completion Status</div>
                        </a></li>
                        <li><a href="Survey_Module/Reports/ViewList.aspx?Type=1">
                            <div class="icon">
                                <img src="Layouts/Resources/images/view-report.png" width="48" height="48" /></div>
                            <div class="text">
                                View Reports</div>
                        </a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function ShowPopup() {
            var path = "Survey_Module/Feedback/ProjectFAQ.aspx?ProjectId=" + document.getElementById('ctl00_cphMaster_hdnProjectId').value;

            window.open(path, '', 'left=100,top=100,height=475,width=1000');
        }
    
    </script>

    <!-- start bodytext container -->
</asp:Content>

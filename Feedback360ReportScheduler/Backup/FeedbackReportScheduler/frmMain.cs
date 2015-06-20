using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Configuration;
using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing.Imaging;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

namespace FeedbackReportScheduler
{
    public partial class frmMain : Form
    {
        #region Globalvariable

        Project_BAO project_BAO = new Project_BAO();
        Programme_BAO programme_BAO = new Programme_BAO();
        AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
        AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
        ReportManagement_BE reportManagement_BE = new ReportManagement_BE();
        Category_BAO category_BAO = new Category_BAO();
        Category_BE category_BE = new Category_BE();

        string LogFilePath = string.Empty;
        string mimeType;
        string encoding;
        string fileNameExtension;
        string extension, deviceInfo, outputFileName = "";
        string[] streams;
        string defaultFileName = string.Empty;
        Warning[] warnings;

        DataTable dtCompanyName;
        DataTable dtGroupList;
        DataTable dtSelfName;
        DataTable dtReportsID;

        string strGroupList;
        string strFrontPage;
        string strReportIntroduction;
        string strConclusionPage;
        string strRadarChart;
        string strDetailedQst;
        string strCategoryQstlist;
        string strCategoryBarChart;
        string strFullProjGrp;
        string strSelfNameGrp;
        string strProgrammeGrp;
        string strReportName;
        string targetradarname = string.Empty;
        string targetradarPreviousScore = string.Empty;
        string targetradarBenchmark = string.Empty;
        string strConHighLowRange;
        string strReportType = string.Empty;
        string strPreScoreVisibility = string.Empty;
        string strStaticBarLabelVisibility = string.Empty;
        string strBenchMarkGrpVisibility = string.Empty;
        string strBenchMarkVisibility = string.Empty;
        string strBenchConclusionPageVisibility = string.Empty;
        string strTargetPersonID;
        string strProjectID;
        string strAccountID;
        string strProgrammeID;
        string strTotalCount;
        string strSubmitCount;
        string strSelfAssessment;

        string strAdmin;

        //Int32 pageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["GridPageSize"]);
        //Int32 pageDispCount = Convert.ToInt32(ConfigurationSettings.AppSettings["PageDisplayCount"]);

        int reportCount = 0;
        string pageNo = "";

        //string participantName;

        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            MakeReport();
        }

        #region ReportMethods

        public void MakeReport()
        {
            try
            {
                //678,546
                //string participantid = Convert.ToString(546);
                //GetDetailFromTargetPersonID(participantid);
                
                AssignQstnParticipant_BAO assignQstnParticipant_BAO = new AssignQstnParticipant_BAO();
                int submitCount = 0;

                DataTable dtParticipant = new DataTable();
                dtParticipant = reportManagement_BAO.GetParticipantData();

                if (dtParticipant != null && dtParticipant.Rows.Count > 0)
                {
                    for (int i = 0; i < dtParticipant.Rows.Count; i++)
                    {
                        submitCount = Convert.ToInt32(assignQstnParticipant_BAO.GetSubmissionCount(Convert.ToInt32(dtParticipant.Rows[i]["UserID"].ToString())));
                        GetDetailFromTargetPersonID(dtParticipant.Rows[i]["UserID"].ToString());
                        //GetDetailFromTargetPersonID("685");
                        if (submitCount != Convert.ToInt32(strSubmitCount))
                        {
                            if (strTargetPersonID != string.Empty && strAccountID != string.Empty && strProjectID != string.Empty && strReportName != string.Empty)
                            {
                                //Generate Report
                                btnExport("");

                                //Report Name Insert Into DB_Table (ReportManagement)
                                reportManagement_BE.AccountID = Convert.ToInt32(strAccountID);
                                reportManagement_BE.ProjectID = Convert.ToInt32(strProjectID);
                                reportManagement_BE.ProgramID = Convert.ToInt32(strProgrammeID);
                                reportManagement_BE.TargetPersonID = Convert.ToInt32(strTargetPersonID);
                                reportManagement_BE.TotalCount = Convert.ToInt32(assignQstnParticipant_BAO.GetCandidatesCount(Convert.ToInt32(dtParticipant.Rows[i]["UserID"].ToString())));
                                reportManagement_BE.SubmitCount = Convert.ToInt32(assignQstnParticipant_BAO.GetSubmissionCount(Convert.ToInt32(dtParticipant.Rows[i]["UserID"].ToString())));
                                reportManagement_BE.SelfAssessment = Convert.ToInt32(assignQstnParticipant_BAO.GetSelfAssessment(Convert.ToInt32(dtParticipant.Rows[i]["UserID"].ToString())));
                                reportManagement_BE.ReportName = strReportName + ".pdf";

                                reportManagement_BAO.AddParticipantReport(reportManagement_BE);

                                //This Code Will Delete RadarImage & Pdf After save
                                if (targetradarname != String.Empty)
                                    File.Delete(targetradarname);
                                if (targetradarPreviousScore != String.Empty)
                                    File.Delete(targetradarPreviousScore);
                                if (targetradarBenchmark != String.Empty)
                                    File.Delete(targetradarBenchmark);
                                //if (strReportName != String.Empty)
                                //    File.Delete(ConfigurationSettings.AppSettings["ReportPath"].ToString() + strReportName + ".pdf");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            this.Close();
        }

        protected void GetDetailFromTargetPersonID(string targetid)
        {
            try
            {
                strTargetPersonID = targetid;
                DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(strTargetPersonID));

                if (dtuserlist != null && dtuserlist.Rows.Count > 0)
                {
                    strProjectID = dtuserlist.Rows[0]["ProjecctID"].ToString();
                    strAccountID = dtuserlist.Rows[0]["AccountID"].ToString();
                    strProgrammeID = dtuserlist.Rows[0]["ProgrammeID"].ToString();
                    strTotalCount = dtuserlist.Rows[0]["TotalCount"].ToString();
                    strSubmitCount = dtuserlist.Rows[0]["SubmitCount"].ToString();
                    strSelfAssessment = dtuserlist.Rows[0]["SelfAssessment"].ToString();
                    strReportName = dtuserlist.Rows[0]["FirstName"].ToString() + dtuserlist.Rows[0]["LastName"].ToString() + '_' + strTargetPersonID;

                    strStaticBarLabelVisibility = dtuserlist.Rows[0]["Code"].ToString();

                    if (strTotalCount == "") strTotalCount = "0";
                    if (strSubmitCount == "") strSubmitCount = "0";
                    if (strSelfAssessment == "") strSelfAssessment = "0";
                }
                else
                {
                    strProjectID = "";
                    strAccountID = "";
                    strProgrammeID = "";
                    strTotalCount = "";
                    strSubmitCount = "";
                    strSelfAssessment = "";
                    strReportName = "";

                    strStaticBarLabelVisibility = "";

                    if (strTotalCount == "") strTotalCount = "0";
                    if (strSubmitCount == "") strSubmitCount = "0";
                    if (strSelfAssessment == "") strSelfAssessment = "0";
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void ControlToParameter(string projectid)
        {
            if (projectid != null)
            {
                DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                    // This parameter will Decide: which type of Report will Call                
                    if (dtreportsetting.Rows[0]["ReportType"].ToString() != string.Empty)
                        strReportType = dtreportsetting.Rows[0]["ReportType"].ToString();

                    if (dtreportsetting.Rows[0]["CoverPage"].ToString() != string.Empty)
                        strFrontPage = dtreportsetting.Rows[0]["CoverPage"].ToString();

                    if (dtreportsetting.Rows[0]["ReportIntroduction"].ToString() != string.Empty)
                        strReportIntroduction = dtreportsetting.Rows[0]["ReportIntroduction"].ToString();

                    if (dtreportsetting.Rows[0]["Conclusionpage"].ToString() != string.Empty)
                        strConclusionPage = dtreportsetting.Rows[0]["Conclusionpage"].ToString();

                    if (dtreportsetting.Rows[0]["RadarChart"].ToString() != string.Empty)
                        strRadarChart = dtreportsetting.Rows[0]["RadarChart"].ToString();

                    if (dtreportsetting.Rows[0]["QstTextResponses"].ToString() != string.Empty)
                        strDetailedQst = dtreportsetting.Rows[0]["QstTextResponses"].ToString();

                    if (dtreportsetting.Rows[0]["CatQstList"].ToString() != string.Empty)
                        strCategoryQstlist = dtreportsetting.Rows[0]["CatQstList"].ToString();

                    if (dtreportsetting.Rows[0]["CatDataChart"].ToString() != string.Empty)
                        strCategoryBarChart = dtreportsetting.Rows[0]["CatDataChart"].ToString();

                    if (dtreportsetting.Rows[0]["CandidateSelfStatus"].ToString() != string.Empty)
                        strSelfNameGrp = dtreportsetting.Rows[0]["CandidateSelfStatus"].ToString();

                    if (dtreportsetting.Rows[0]["FullProjectGrp"].ToString() != string.Empty)
                        strFullProjGrp = dtreportsetting.Rows[0]["FullProjectGrp"].ToString();

                    if (dtreportsetting.Rows[0]["ProgrammeGrp"].ToString() != string.Empty)
                        strProgrammeGrp = dtreportsetting.Rows[0]["ProgrammeGrp"].ToString();

                    if (dtreportsetting.Rows[0]["ProjectRelationGrp"].ToString() != string.Empty)
                        strGroupList = dtreportsetting.Rows[0]["ProjectRelationGrp"].ToString();

                    if (dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != string.Empty)
                        strConHighLowRange = dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString();

                    if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() != string.Empty)
                        strPreScoreVisibility = dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString();

                    if (dtreportsetting.Rows[0]["BenchMarkGrpVisible"].ToString() != string.Empty)
                        strBenchMarkGrpVisibility = dtreportsetting.Rows[0]["BenchMarkGrpVisible"].ToString();

                    if (dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() != string.Empty)
                        strBenchMarkVisibility = dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString();

                    if (dtreportsetting.Rows[0]["BenchConclusionpage"].ToString() != string.Empty)
                        strBenchConclusionPageVisibility = dtreportsetting.Rows[0]["BenchConclusionpage"].ToString();
                }
            }
        }

        protected void btnExport(string dirName)
        {
            try
            {
                Microsoft.Reporting.WinForms.ReportViewer rview = new Microsoft.Reporting.WinForms.ReportViewer();
                rview.ServerReport.ReportServerUrl = new Uri(ConfigurationSettings.AppSettings["ReportServerUrl"].ToString());
                string[] streamids;
                Microsoft.Reporting.WinForms.Warning[] warnings;
                string root = string.Empty;
                root = ConfigurationSettings.AppSettings["ReportPath"].ToString();

                /* Function : For Filling Paramters From Controls */
                ControlToParameter(strProjectID);

                //If strReportType = 1 Then FeedbackReport will Call
                //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
                if (strReportType == "1")
                {
                    DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                    int iAccountID = 0;
                    if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                    {


                        iAccountID = Convert.ToInt32(dtreportsetting.Rows[0]["AccountID"]);
                        /*
                         * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                         * & Making Entry in Table with Radarchatname
                         * & Calling in RDL (RadarImage)
                         */
                        if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                            Radar(strTargetPersonID, strGroupList);
                        else
                            targetradarname = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartNoImage" + ".jpg";

                        //Previous ScoreRadar Chart.
                        if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                            RadarPreviousScore(strTargetPersonID, strGroupList);
                        else
                            targetradarPreviousScore = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartNoImage" + ".jpg";

                        //BenchMark Radar Chart.
                        if (dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() == "1")
                            RadarBenchMark(strTargetPersonID);
                        else
                            targetradarBenchmark = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartNoImage" + ".jpg";
                    }

                    if (iAccountID == 68)
                    {
                        rview.ServerReport.ReportPath = "/Feedback360/CapitaFeedbackReport"; 
                    }
                    else
                    {
                        rview.ServerReport.ReportPath = "/Feedback360/FeedbackReport";
                    }
                    

                    System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter>();
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetPersonID", strTargetPersonID));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FrontPageVisibility", strFrontPage));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("RadarChartVisibility", strRadarChart));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("GroupList", strGroupList));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetRadarName", targetradarname));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BenchMarkGrpVisibility", strBenchMarkGrpVisibility));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetRadarNameBenchmark", targetradarBenchmark));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BenchMarkVisibility", strBenchMarkVisibility));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BenchConclusionVisibility", strBenchConclusionPageVisibility));
                    rview.ServerReport.SetParameters(paramList);
                    //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
                }
                else if (strReportType == "2")
                {
                    rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient1";

                    //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                    // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                    System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter>();
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetPersonID", strTargetPersonID));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FrontPageVisibility", strFrontPage));            
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FrontPageVisibility", "1"));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("GroupList", strGroupList));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("DetailedQstVisibility", "1"));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryQstlistVisibility", "1"));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryBarChartVisibility", "1"));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("SelfNameGrpVisibility", "1"));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FullProjGrpVisibility", "1"));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ProgrammeVisibility", "1"));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));            
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                    rview.ServerReport.SetParameters(paramList);
                    //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
                }
                else if (strReportType == "3")
                {
                    DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                    if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                    {
                        // if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                        RadarCPL(strTargetPersonID, strGroupList);
                        //Previous ScoreRadar Chart.
                        if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                            RadarPreviousScoreCPL(strTargetPersonID, strGroupList);
                        else
                            targetradarPreviousScore = "RadarChartNoImage";
                    }

                    rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient2";

                    //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                    // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                    System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter>();
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetPersonID", strTargetPersonID));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FrontPageVisibility", strFrontPage));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                    rview.ServerReport.SetParameters(paramList);
                    //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
                }
                else if (strReportType == "4")
                {
                    rview.ServerReport.ReportPath = "/Feedback360/CurFeedbackReport";

                    System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WinForms.ReportParameter>();
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetPersonID", strTargetPersonID));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FrontPageVisibility", strFrontPage));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("RadarChartVisibility", strRadarChart));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("GroupList", strGroupList));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetRadarName", targetradarname));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BenchMarkGrpVisibility", strBenchMarkGrpVisibility));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("TargetRadarNameBenchmark", targetradarBenchmark));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BenchMarkVisibility", strBenchMarkVisibility));
                    //paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("BenchConclusionVisibility", strBenchConclusionPageVisibility));
                    rview.ServerReport.SetParameters(paramList);
                    //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
                }

                rview.Visible = false;

                byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                //string PDF_path = root + dirName + "\\" + strReportName + ".pdf";
                string PDF_path = ConfigurationSettings.AppSettings["ReportPath"].ToString() + strReportName + ".pdf";
                FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create);
                objFs.Write(bytes, 0, bytes.Length);
                objFs.Close();
                objFs.Dispose();

                bytes = null;
                rview.Dispose();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion

        #region Radar Chart Method
        /*
        public void Radar(string strTargetPersonID, string strGroupList)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
            DataTable dtFullProjectData = reportManagement_BAO.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }

            string[] xValues1 = new string[dtFullProjectData.Rows.Count];
            double[] yValues1 = new double[dtFullProjectData.Rows.Count];
            for (int i = 0; i < dtFullProjectData.Rows.Count; i++)
            {
                xValues1[i] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[i] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtFullProjectData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullProjectData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtFullProjectData.Rows.Count > 0)
                Series2 = dtFullProjectData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtFullProjectData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarname = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
                Chart1.SaveImage(targetradarname, ImageFormat.Jpeg);

            //dtSelfData.Dispose();
            //Chart1.Dispose();             
        }*/

        public void Radar(string strTargetPersonID, string strGroupList)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
            DataTable dtFullProjectData = reportManagement_BAO.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }

            string[] xValues1 = new string[dtFullProjectData.Rows.Count];
            double[] yValues1 = new double[dtFullProjectData.Rows.Count];
            for (int i = 0; i < dtFullProjectData.Rows.Count; i++)
            {
                xValues1[i] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[i] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtFullProjectData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullProjectData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtFullProjectData.Rows.Count > 0)
                Series2 = dtFullProjectData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtFullProjectData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarname = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
                Chart1.SaveImage(targetradarname, ImageFormat.Jpeg);

            //dtSelfData.Dispose();
            //Chart1.Dispose();             
        }

        public void RadarCPL(string strTargetPersonID, string strGroupList)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
            DataTable dtFullProjectData = reportManagement_BAO.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                if (i == dtSelfData.Rows.Count - 1)
                {
                    xValues[0] = dtSelfData.Rows[i]["CategoryName"].ToString();
                    yValues[0] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
                }
                else
                {
                    xValues[i + 1] = dtSelfData.Rows[i]["CategoryName"].ToString();
                    yValues[i + 1] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
                }
            }

            string[] xValues1 = new string[dtFullProjectData.Rows.Count];
            double[] yValues1 = new double[dtFullProjectData.Rows.Count];
            for (int i = 0; i < dtFullProjectData.Rows.Count; i++)
            {
                if (i == dtFullProjectData.Rows.Count - 1)
                {
                    xValues1[0] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
                    yValues1[0] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
                }
                else
                {
                    xValues1[i + 1] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
                    yValues1[i + 1] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
                }

            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtFullProjectData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullProjectData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtFullProjectData.Rows.Count > 0)
                Series2 = dtFullProjectData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtFullProjectData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarname = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
                Chart1.SaveImage(targetradarname, ImageFormat.Jpeg);

            //dtSelfData.Dispose();
            //Chart1.Dispose();             
        }

        //uncomment this is the copied one doesn't work
        /*public void RadarPreviousScore(string strTargetPersonID, string strGroupList)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
            DataTable dtFullPreviousData = reportManagement_BAO.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }

            string[] xValues1 = new string[dtFullPreviousData.Rows.Count];
            double[] yValues1 = new double[dtFullPreviousData.Rows.Count];
            for (int i = 0; i < dtFullPreviousData.Rows.Count; i++)
            {
                xValues1[i] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[i] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtFullPreviousData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullPreviousData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtFullPreviousData.Rows.Count > 0)
                Series2 = dtFullPreviousData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);

            //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtFullPreviousData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarPreviousScore = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
                Chart1.SaveImage(targetradarPreviousScore, ImageFormat.Jpeg);
        }*/

        public void RadarPreviousScore(string strTargetPersonID, string strGroupList)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
            DataTable dtFullPreviousData = reportManagement_BAO.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }

            string[] xValues1 = new string[dtFullPreviousData.Rows.Count];
            double[] yValues1 = new double[dtFullPreviousData.Rows.Count];
            for (int i = 0; i < dtFullPreviousData.Rows.Count; i++)
            {
                xValues1[i] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[i] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtFullPreviousData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullPreviousData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtFullPreviousData.Rows.Count > 0)
                Series2 = dtFullPreviousData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);

            //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtFullPreviousData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarPreviousScore = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
                Chart1.SaveImage(targetradarPreviousScore, ImageFormat.Jpeg);
        }

        public void RadarPreviousScoreCPL(string strTargetPersonID, string strGroupList)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
            DataTable dtFullPreviousData = reportManagement_BAO.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                if (i == dtSelfData.Rows.Count - 1)
                {
                    xValues[0] = dtSelfData.Rows[i]["CategoryName"].ToString();
                    yValues[0] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
                }
                else
                {
                    xValues[i + 1] = dtSelfData.Rows[i]["CategoryName"].ToString();
                    yValues[i + 1] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
                }
            }

            string[] xValues1 = new string[dtFullPreviousData.Rows.Count];
            double[] yValues1 = new double[dtFullPreviousData.Rows.Count];
            for (int i = 0; i < dtFullPreviousData.Rows.Count; i++)
            {
                if (i == dtFullPreviousData.Rows.Count - 1)
                {
                    xValues1[0] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
                    yValues1[0] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
                }
                else
                {
                    xValues1[i + 1] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
                    yValues1[i + 1] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
                }
            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtFullPreviousData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullPreviousData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtFullPreviousData.Rows.Count > 0)
                Series2 = dtFullPreviousData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);

            //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtFullPreviousData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarPreviousScore = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
                Chart1.SaveImage(targetradarPreviousScore, ImageFormat.Jpeg);
        }

        public void RadarBenchMark(string strTargetPersonID)
        {
            Chart1.Series.Clear();
            string Series1 = string.Empty;
            string Series2 = string.Empty;
            DataTable dtSelfData = reportManagement_BAO.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "S");
            DataTable dtBenchMarkData = reportManagement_BAO.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "P");

            string[] xValues = new string[dtSelfData.Rows.Count];
            double[] yValues = new double[dtSelfData.Rows.Count];
            for (int i = 0; i < dtSelfData.Rows.Count; i++)
            {
                xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }

            string[] xValues1 = new string[dtBenchMarkData.Rows.Count];
            double[] yValues1 = new double[dtBenchMarkData.Rows.Count];
            for (int i = 0; i < dtBenchMarkData.Rows.Count; i++)
            {
                xValues1[i] = dtBenchMarkData.Rows[i]["CategoryName"].ToString();
                yValues1[i] = Convert.ToDouble(dtBenchMarkData.Rows[i]["Average"].ToString());
            }

            //Can Set Y-Axis Scale from here.
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
            if (dtSelfData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
            else
            {
                if (dtBenchMarkData.Rows.Count > 0)
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtBenchMarkData.Rows[0]["UpperBound"].ToString());
                else
                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
            }

            //Adding Series in RadarChart 
            if (dtSelfData.Rows.Count > 0)
                Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
            if (dtBenchMarkData.Rows.Count > 0)
                Series2 = dtBenchMarkData.Rows[0]["RelationShip"].ToString();

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series.Add(Series1);
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series.Add(Series2);

            // Defining Series Type
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

            //Change Color Of Graph
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(193, 255, 193); //(240, 128, 128);

            //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].BorderWidth = 1;
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2].BorderWidth = 1;

            // Populate series data
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

            // Set radar chart style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

            if (dtSelfData.Rows.Count > 0)
            {
                Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series1].BorderWidth = 1;
            }
            if (dtBenchMarkData.Rows.Count > 0)
            {
                Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
                Chart1.Series[Series2].BorderWidth = 1;
            }

            // Set circular area drawing style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

            // Set labels style
            if (dtSelfData.Rows.Count > 0)
                Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
            //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

            targetradarBenchmark = ConfigurationSettings.AppSettings["ReportPath"].ToString() + "RadarChartBenchMark" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
            if (dtBenchMarkData.Rows.Count > 0 || dtBenchMarkData.Rows.Count > 0)
                Chart1.SaveImage(targetradarBenchmark, ImageFormat.Jpeg);
        }

        #endregion

        private void Chart1_Click(object sender, EventArgs e)
        {

        }

        public void HandleException(Exception ex)
        {
            //ExceptionLogger.Write(ex.ToString());
            FileStream FS;
            StreamWriter SW;
            string fpath;

            string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                        "Error Application: Feedback 360 - UI" + Environment.NewLine + "," +
                        "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                        "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                        "Error Source: " + ex.Source + Environment.NewLine + "," +
                        "Error Message: " + ex.Message + Environment.NewLine;

            fpath = System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"].ToString() + "ErrorLog.txt";

            if (File.Exists(fpath))
            { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
            else
            { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

            string msg = str.Replace(",", "");

            SW = new StreamWriter(FS);
            SW.WriteLine(msg);

            SW.Close();
            FS.Close();

        }

    }
}

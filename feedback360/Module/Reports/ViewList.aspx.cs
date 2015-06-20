using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Text;
using Microsoft.Reporting.WebForms;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;
using Microsoft.ReportingServices;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Web.SessionState;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;


public partial class Module_Reports_ViewList : CodeBehindBase
{
    #region Globalvariable

    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string extension, deviceInfo, outputFileName = "";
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;
    WADIdentity identity;
    Project_BAO project_BAO = new Project_BAO();
    Programme_BAO programme_BAO = new Programme_BAO();
    AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
    ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
    ReportManagement_BE reportManagement_BE = new ReportManagement_BE();
    AssignQstnParticipant_BAO assignQstnParticipant_BAO = new AssignQstnParticipant_BAO();

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
    string strAdmin;
    int rptCandidateCount = 0;

    Category_BAO category_BAO = new Category_BAO();
    Category_BE category_BE = new Category_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int reportCount = 0;
    string pageNo = "";

    string selfStatus = "";
    string filename = "";
    
    //string participantName;
    #endregion

    protected System.Web.UI.WebControls.Label Label1;
    protected System.Web.UI.WebControls.Label Label2;
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.DropDownList ExplodedPointList;
    protected System.Web.UI.WebControls.Label Label4;
    protected System.Web.UI.WebControls.DropDownList HoleSizeList;

    protected void Page_Load(object sender, EventArgs e)
    {
        identity = this.Page.User.Identity as WADIdentity;
        int? grpID = identity.User.GroupID;

        //AssignQuestionnaire_BAO chk_user = new AssignQuestionnaire_BAO();
        //DataTable ddd = chk_user.chk_user_authority(grpID, 14);
        //if (Convert.ToInt32(ddd.Rows[0][0]) == 0)
        //{
        //    Response.Redirect("UnAuthorized.aspx");
        //}




       Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        try
        {
            System.GC.Collect();

            identity = this.Page.User.Identity as WADIdentity;

            grdvParticipantList.PageSize = 50;
            ManagePaging();

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (txtGoto != null)
                txtGoto.Text = pageNo;                      


            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                if (identity.User.GroupID == 1)
                {
                    divAccount.Visible = true;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    divAccount.Visible = false;
                }

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";

                Project_BAO project_BAO = new Project_BAO();

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
                string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

                if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
                {
                    ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ViewState["strAdmin"] = "N";
                    grdvParticipantList.AllowSorting = false;

                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                    DataTable dtParticipantInfo = new DataTable();
                    dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                    if (dtParticipantInfo.Rows.Count>0)
                    ddlProject.SelectedValue = dtParticipantInfo.Rows[0]["ProjecctID"].ToString();

                    DataTable dtProgramme = new DataTable();
                    dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

                    if (dtProgramme.Rows.Count > 0)
                    {
                        ddlProgramme.DataSource = dtProgramme;
                        ddlProgramme.DataTextField = "ProgrammeName";
                        ddlProgramme.DataValueField = "ProgrammeID";
                        ddlProgramme.DataBind();
                        if (dtParticipantInfo.Rows.Count>0)
                        ddlProgramme.SelectedValue = dtParticipantInfo.Rows[0]["ProgrammeID"].ToString();
                    }

                    ddlProject.Enabled = false;
                    ddlProgramme.Enabled = false;

                    odsReport.SelectParameters.Clear();
                    odsReport.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
                    odsReport.SelectParameters.Add("projectID", ddlProject.SelectedValue);
                    odsReport.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue);
                    odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
                    odsReport.Select();

                    odsReport.FilterParameters.Clear();
                    odsReport.FilterExpression = "UserID=" + identity.User.UserID.ToString();

                    plcPaging.Controls.Clear();
                }
                else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))
                {
                    ViewState["strAdmin"] = "N";
                    grdvParticipantList.AllowSorting = false;

                    DataTable dtManagerProject = new DataTable();
                    dtManagerProject = project_BAO.GetManagerProject(identity.User.Email, Convert.ToInt32(identity.User.AccountID));

                    if (dtManagerProject.Rows.Count > 0)
                    {
                        ddlProject.DataSource = dtManagerProject;
                        ddlProject.DataValueField = "ProjectID";
                        ddlProject.DataTextField = "Title";
                        ddlProject.DataBind();

                        //ddlProject.SelectedValue = dtManagerProject.Rows[0]["ProjectID"].ToString();
                    }

                    DataTable dtManagerProgramme = new DataTable();
                    dtManagerProgramme = project_BAO.GetManagerProgramme(identity.User.Email, Convert.ToInt32(identity.User.AccountID));

                    odsReport.SelectParameters.Clear();
                    odsReport.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
                    odsReport.SelectParameters.Add("projectID", ddlProject.SelectedValue);
                    odsReport.SelectParameters.Add("programmeID", null);
                    odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
                    odsReport.Select();

                    plcPaging.Controls.Clear();
                }
                else
                {
                    ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ViewState["strAdmin"] = "Y";
                    grdvParticipantList.AllowSorting = true;

                    odsReport.SelectParameters.Clear();
                    odsReport.SelectParameters.Add("accountID", null);
                    odsReport.SelectParameters.Add("projectID", null);
                    odsReport.SelectParameters.Add("programmeID", null);
                    odsReport.SelectParameters.Add("admin", strAdmin);
                    odsReport.Select();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    
    #region Radar Chart Method

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
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

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

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

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
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

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

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
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
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

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

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
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
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

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

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
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
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(193, 255, 193); //(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

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

        targetradarBenchmark = Server.MapPath("~\\UploadDocs\\") + "RadarChartBenchMark" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtBenchMarkData.Rows.Count > 0 || dtBenchMarkData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarBenchmark);
    }
   
    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion
    
    #endregion

    #region Image Button Function
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        FillGridData();
        ManagePaging();
    }

    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ResetControls();
    }

    #endregion   

    #region dropdown event
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetControls();
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            dtCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dtAccount = dtCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();


            if (ddlAccountCode.SelectedIndex > 0)
            {
                DataTable dtprojectlist = project_BAO.GetdtProjectList(Convert.ToString(companycode));

                if (dtprojectlist.Rows.Count > 0)
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));

                    ddlProject.DataSource = dtprojectlist;
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataBind();

                    ddlProgramme.SelectedValue = "0";
                }
                else
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            //FillGridData();
            ViewState["accid"] = ddlAccountCode.SelectedValue.ToString();
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;


        ViewState["prjid"] = ddlProject.SelectedValue.ToString();
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    #endregion

    #region ReportMethods

    protected void GetDetailFromTargetPersonID(string targetid)
    {
        strTargetPersonID = targetid;
        ViewState["strTargetPersonID"] = targetid;
        DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(strTargetPersonID));
        if (dtuserlist != null && dtuserlist.Rows.Count > 0)
        {
            int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);
            strProjectID = dtuserlist.Rows[0]["ProjectID"].ToString();
            ViewState["strProjectID"] = dtuserlist.Rows[0]["ProjectID"].ToString();
            
            DataTable project = project_BAO.GetdataProjectByID(projectid);
            if (project != null && project.Rows.Count > 0)
            {
                strAccountID = project.Rows[0]["AccountID"].ToString();
                ViewState["strAccountID"] = project.Rows[0]["AccountID"].ToString();
            }

            DataTable programme = reportManagement_BAO.GetdataProjectByID(projectid);
            if (programme != null && programme.Rows.Count > 0)
            {
                strProgrammeID = programme.Rows[0]["ProgrammeID"].ToString();
                ViewState["strProgrammeID"] = programme.Rows[0]["ProgrammeID"].ToString();
            }
        }
        else
        {   
            ViewState["strProjectID"] = Convert.ToString(ViewState["prjid"]);

            strAccountID = Convert.ToString(ViewState["accid"]);
            ViewState["strAccountID"] = strAccountID;

            int projectid = Convert.ToInt32(ViewState["prjid"]);
            DataTable programme = reportManagement_BAO.GetdataProjectByID(projectid);
            if (programme != null && programme.Rows.Count > 0)
            {
                strProgrammeID = programme.Rows[0]["ProgrammeID"].ToString();
                ViewState["strProgrammeID"] = programme.Rows[0]["ProgrammeID"].ToString();
            }
        }

        /*For Self Name by Using Target Person ID & Account ID*/
        dtSelfName = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(strAccountID), Convert.ToInt32(strTargetPersonID));
        if (dtSelfName != null && dtSelfName.Rows.Count > 0)
        {            
            strReportName = dtSelfName.Rows[0]["FirstName"].ToString() + dtSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyHHmmss");
            //ViewState["strReportName"] = dtSelfName.Rows[0]["FirstName"].ToString() + dtSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss");
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
          //  Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
            rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string root = string.Empty;
            root = Server.MapPath("~") + "\\ReportGenerate\\";

            /* Function : For Filling Paramters From Controls */
            ControlToParameter(strProjectID);

            if (ddlAccountCode.SelectedValue != string.Empty)
                strStaticBarLabelVisibility = ddlAccountCode.SelectedItem.ToString();
            else
                strStaticBarLabelVisibility = "";

            //If strReportType = 1 Then FeedbackReport will Call
            //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
            if (strReportType == "1")
            {
                DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                    /*
                     * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                     * & Making Entry in Table with Radarchatname
                     * & Calling in RDL (RadarImage)
                     */
                    if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                        Radar(strTargetPersonID, strGroupList);
                    else
                        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScore(strTargetPersonID, strGroupList);
                    else
                        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //BenchMark Radar Chart.
                    if (dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() == "1")
                        RadarBenchMark(strTargetPersonID);
                    else
                        targetradarBenchmark = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";
                }

                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReport";
               // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                if (identity.User.AccountID == 68 || ddlAccountCode.SelectedValue == "68")
                {
                    rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/CapitaFeedbackReport";
                }
                else
                {
                    rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReport";
                }

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("RadarChartVisibility", strRadarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", strGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarName", targetradarname));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchMarkGrpVisibility", strBenchMarkGrpVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNameBenchmark", targetradarBenchmark));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchMarkVisibility", strBenchMarkVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchConclusionVisibility", strBenchConclusionPageVisibility));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "2")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));            
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", strGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", "1"));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));            
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "3")
            {
                 //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
              //  rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //New Changes 
                //Changed by Amit Singh
                DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                   // if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                        RadarCPL(strTargetPersonID, strGroupList);
                   // else
                      //  targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScoreCPL(strTargetPersonID, strGroupList);
                    else
                        targetradarPreviousScore = "RadarChartNoImage";

                }

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarName", targetradarname));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "4") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
               // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/CurFeedbackReport";
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", strGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                rview.ServerReport.SetParameters(paramList);
            }         

//            rview.Visible = false;
            byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //string PDF_path = root + dirName + "\\" + strReportName + ".pdf";
            string PDF_path = root + strReportName + ".pdf";
            FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create);
            objFs.Write(bytes, 0, bytes.Length);
            objFs.Close();
            objFs.Dispose();


            bytes = null;
            System.GC.Collect();
            rview.Dispose();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    #endregion

    #region GridView

    protected void grdvParticipantList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "2")
            {
                if (e.Row.RowType != DataControlRowType.EmptyDataRow)
                    e.Row.Cells[5].Visible = false;
            }
            

            selfStatus = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnUserID = (HiddenField)e.Row.FindControl("hdnUserID");
                Label lblCandidateCount = (Label)e.Row.FindControl("lblCandidateCount");
                Label lblCompleted = (Label)e.Row.FindControl("lblCompleted");
                Label lblSelfAssessment = (Label)e.Row.FindControl("lblSelfAssessment");
                
                //Get report management data 
                DataTable dtParticipantInfo=new DataTable();
                dtParticipantInfo = assignQstnParticipant_BAO.GetParticipantReportInfo(Convert.ToInt32(hdnUserID.Value));
                rptCandidateCount = 0;

                if (dtParticipantInfo.Rows.Count > 0)
                {
                    if (dtParticipantInfo.Rows[0]["SubmitCount"].ToString() == "")
                        dtParticipantInfo.Rows[0]["SubmitCount"] = "0";

                    rptCandidateCount = Convert.ToInt32(dtParticipantInfo.Rows[0]["SubmitCount"].ToString());
                }
                else
                {
                    rptCandidateCount = 0;
                }

                if (lblCandidateCount != null)
                    lblCandidateCount.Text = assignQstnParticipant_BAO.GetCandidatesCount(Convert.ToInt32(hdnUserID.Value)).ToString();

                if (lblCompleted != null)
                    lblCompleted.Text = assignQstnParticipant_BAO.GetSubmissionCount(Convert.ToInt32(hdnUserID.Value)).ToString();

                if (lblSelfAssessment != null)
                {
                    selfStatus=assignQstnParticipant_BAO.GetSelfAssessment(Convert.ToInt32(hdnUserID.Value)).ToString();
                    if (selfStatus == "1")
                        lblSelfAssessment.Text = "Yes";
                    else
                        lblSelfAssessment.Text = "No";
                }

                if ((Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "1") || (Request.QueryString["Type"] == null))
                {
                    ImageButton ibtnReport = (ImageButton)e.Row.FindControl("ibtnReport");
                    filename = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32(hdnUserID.Value));

                    if (filename != "")
                        ibtnReport.Visible = true;
                    else
                        ibtnReport.Visible = false;

                    ImageButton ibtnEdit = (ImageButton)e.Row.FindControl("imgEdit");
                    if (Convert.ToInt32(lblCompleted.Text) == rptCandidateCount)
                        ibtnEdit.Visible = false;
                    else
                    {
                        ibtnEdit.Visible = true;
                        ibtnReport.Visible = false;
                    }
                }
   
                //string fpath = "http://localhost:2258/feedback360/reportgenerate/DrupalCMS.pdf";
                //ibtnReport.OnClientClick = "javascript:window.open ('" + fpath + "', 'mywindow','status=1,toolbar=1,location=0,menubar=0,resizable=0,scrollbars=0,height=100,width=100');";

                dtParticipantInfo.Dispose();
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void grdvParticipantList_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            ManagePaging();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void grdvParticipantList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32(e.CommandArgument)).ToString();

            string participantid = Convert.ToString(e.CommandArgument);
            GetDetailFromTargetPersonID(participantid);

            if (Convert.ToString(ViewState["strTargetPersonID"]) != string.Empty)
                strTargetPersonID = Convert.ToString(ViewState["strTargetPersonID"]);
            if (Convert.ToString(ViewState["strAccountID"]) != string.Empty)
                strAccountID = Convert.ToString(ViewState["strAccountID"]);
            if (Convert.ToString(ViewState["strProjectID"]) != string.Empty)
                strProjectID = Convert.ToString(ViewState["strProjectID"]);
            //if (Convert.ToString(ViewState["strReportName"]) != string.Empty)
            //    strReportName = Convert.ToString(ViewState["strReportName"]);
            if (Convert.ToString(ViewState["strProgrammeID"]) != string.Empty)
                strProgrammeID = Convert.ToString(ViewState["strProgrammeID"]);

            if (strTargetPersonID != null && strAccountID != null && strProjectID != null && strReportName != null)
            {
                btnExport(""); 
                try
                {
                    string root = Server.MapPath("~") + "\\ReportGenerate\\";
                    string openpdf = root + strReportName + ".pdf";
                    //Response.Write(openpdf);
                    //Response.Redirect("../../ReportGenerate/" + strReportName + ".pdf");

                    /*Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Clear();                    
                    
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + strReportName.Substring(0, strReportName.Length - 6) + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.TransmitFile(openpdf);
                    
                    Response.Flush();
                    Response.Close();*/
                    Response.Redirect("download.aspx?filename=" + strReportName);
                    

                    ////This Code Will Delete RadarImage & Pdf After save
                    //if (targetradarname != String.Empty)
                    //    File.Delete(targetradarname);
                    //if (targetradarPreviousScore != String.Empty)
                    //    File.Delete(targetradarPreviousScore);
                    //if (targetradarBenchmark != String.Empty)
                    //    File.Delete(targetradarBenchmark);
                    //if (strReportName != String.Empty)
                    //    File.Delete((root + strReportName + ".pdf"));
                }
                catch (Exception ex)
                { }
            }
        }
        else if (e.CommandName == "Report")
        {
            string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32(e.CommandArgument)).ToString();

            if (fName != "")
            {
                string root = Server.MapPath("~") + "\\ReportGenerate\\";
                string openpdf = root + fName;
                //Response.Write(openpdf);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=Feedback.pdf");
                Response.ContentType = "application/octet-stream";
                Response.TransmitFile(openpdf);

                Response.Flush();
                Response.Clear();
                Response.Close();
            }
        }
    }

    protected void grdvParticipantList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void grdvParticipantList_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    #endregion

    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {        
        try
        {
            string participantid = string.Empty;

            string root = Server.MapPath("~") + "\\ReportGenerate\\";
            string newDir = ddlAccountCode.SelectedItem.Text + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");

            DirectoryInfo drInfo = new DirectoryInfo(root);
            drInfo.CreateSubdirectory(newDir);
            
            foreach (GridViewRow row in grdvParticipantList.Rows)
            {
                CheckBox myCheckBox = (CheckBox)row.FindControl("myCheckBox");
                HiddenField hdnUserID = (HiddenField)row.FindControl("hdnUserID");

                if (myCheckBox != null && myCheckBox.Checked == true)
                {
                    participantid = hdnUserID.Value.ToString();
                    GetDetailFromTargetPersonID(participantid);

                    if (Convert.ToString(ViewState["strTargetPersonID"]) != string.Empty)
                        strTargetPersonID = Convert.ToString(ViewState["strTargetPersonID"]);
                    if (Convert.ToString(ViewState["strAccountID"]) != string.Empty)
                        strAccountID = Convert.ToString(ViewState["strAccountID"]);
                    if (Convert.ToString(ViewState["strProjectID"]) != string.Empty)
                        strProjectID = Convert.ToString(ViewState["strProjectID"]);
                    if (Convert.ToString(ViewState["strProgrammeID"]) != string.Empty)
                        strProgrammeID = Convert.ToString(ViewState["strProgrammeID"]);

                    if (strTargetPersonID != null && strAccountID != null && strProjectID != null && strReportName != null)
                    {

                        btnExport(newDir);
                        System.GC.Collect();
                    }
                }
            }

            string sPath = Server.MapPath("~") + "\\ReportGenerate\\" + newDir + "\\" ;
            string zipName = "EN-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".zip";
            ZipFiles(sPath, zipName, "");

            //This Code Will Delete All PDF
            ArrayList fils = new ArrayList();
            foreach (string file in Directory.GetFiles(sPath)) // add each file in directory
            {
                if (Path.GetExtension(file) == ".pdf")
                {
                    File.Delete(file);
                }
            }

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('../../Module/Reports/DownloadZip.aspx?dirName="+ newDir +"&filename=" + zipName + "',null,'left=500,top=400,height=20, width=30,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ');", true);


            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('../../Module/Feedback/AssignQstnPaticipantList.aspx?AccountID=1&PrgramId=1','','left=500,top=400,height=20,width=30,menubar=no, resizable=yes');", true);
           
        }
        catch (Exception ex)
        { }
    }

    #region Gridview Paging Related Methods

    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        //reportCount = reportManagement_BAO.GetReportManagementListCount(Convert.ToInt32(ViewState["accid"]), Convert.ToInt32(ViewState["prjid"]), Convert.ToInt32(ViewState["prgid"]), Convert.ToString(ViewState["strAdmin"]));

        //plcPaging.Controls.Clear();

        //if (reportCount > 0)
        //{

        //    // Variable declaration
        //    int numberOfPages;
        //    int numberOfRecords = reportCount;
        //    int currentPage = (grdvParticipantList.PageIndex);
        //    StringBuilder strSummary = new StringBuilder();

        //    // If number of records is more then the page size (specified in global variable)
        //    // Just to check either gridview have enough records to implement paging
        //    if (numberOfRecords > pageSize)
        //    {
        //        // Calculating the total number of pages
        //        numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
        //    }
        //    else
        //    {
        //        numberOfPages = 1;
        //    }


        //    // Creating a small summary for records.
        //    strSummary.Append("Displaying <b>");

        //    // Creating X f X Records
        //    int floor = (currentPage * pageSize) + 1;
        //    strSummary.Append(floor.ToString());
        //    strSummary.Append("</b>-<b>");
        //    int ceil = ((currentPage * pageSize) + pageSize);

        //    //let say you have 26 records and you specified 10 page size, 
        //    // On the third page it will return 30 instead of 25 as that is based on pageSize
        //    // So this check will see if the ceil value is increasing the number of records. Consider numberOfRecords
        //    if (ceil > numberOfRecords)
        //    {
        //        strSummary.Append(numberOfRecords.ToString());
        //    }
        //    else
        //    {
        //        strSummary.Append(ceil.ToString());
        //    }

        //    // Displaying Total number of records Creating X of X of About X records.
        //    strSummary.Append("</b> of <b>");
        //    strSummary.Append(numberOfRecords.ToString());
        //    strSummary.Append("</b> records</br>");


        //    litPagingSummary.Text = ""; // strSummary.ToString();


        //    //Variable declaration 
        //    //these variables will used to calculate page number display
        //    int pageShowLimitStart = 1;
        //    int pageShowLimitEnd = 1;

        //    // Just to check, either there is enough pages to implement page number display logic.
        //    if (pageDispCount > numberOfPages)
        //    {
        //        pageShowLimitEnd = numberOfPages; // Setting the end limit to the number of pages. Means show all page numbers
        //    }
        //    else
        //    {
        //        if (currentPage > 4) // If page index is more then 4 then need to less the page numbers from start and show more on end.
        //        {
        //            //Calculating end limit to show more page numbers
        //            pageShowLimitEnd = currentPage + (int)(Math.Floor((decimal)pageDispCount / 2));
        //            //Calculating Start limit to hide previous page numbers
        //            pageShowLimitStart = currentPage - (int)(Math.Floor((decimal)pageDispCount / 2));
        //        }
        //        else
        //        {
        //            //Simply Displaying the 10 pages. no need to remove / add page numbers
        //            pageShowLimitEnd = pageDispCount;
        //        }
        //    }

        //    // Since the pageDispCount can be changed and limit calculation can cause < 0 values 
        //    // Simply, set the limit start value to 1 if it is less
        //    if (pageShowLimitStart < 1)
        //        pageShowLimitStart = 1;


        //    //Dynamic creation of link buttons

        //    // First Link button to display with paging
        //    LinkButton objLbFirst = new LinkButton();
        //    objLbFirst.Click += new EventHandler(objLb_Click);
        //    //objLbFirst.Text = "First";
        //    objLbFirst.CssClass = "first";
        //    objLbFirst.ToolTip = "First Page";
        //    objLbFirst.ID = "lb_FirstPage";
        //    objLbFirst.CommandName = "pgChange";
        //    objLbFirst.EnableViewState = true;
        //    objLbFirst.CommandArgument = "1";

        //    //Previous Link button to display with paging
        //    LinkButton objLbPrevious = new LinkButton();
        //    objLbPrevious.Click += new EventHandler(objLb_Click);
        //    //objLbPrevious.Text = "Previous";
        //    objLbPrevious.CssClass = "previous";
        //    objLbPrevious.ToolTip = "Previous Page";
        //    objLbPrevious.ID = "lb_PreviousPage";
        //    objLbPrevious.CommandName = "pgChange";
        //    objLbPrevious.EnableViewState = true;
        //    objLbPrevious.CommandArgument = currentPage.ToString();


        //    //of course if the page is the 1st page, then there is no need of First or Previous
        //    if (currentPage == 0)
        //    {
        //        objLbFirst.Enabled = false;
        //        objLbPrevious.Enabled = false;
        //    }
        //    else
        //    {
        //        objLbFirst.Enabled = true;
        //        objLbPrevious.Enabled = true;
        //    }

        //    plcPaging.Controls.Add(new LiteralControl("<table border=0><tr><td valign=middle>"));

        //    //Adding control in a place holder
        //    plcPaging.Controls.Add(objLbFirst);
        //    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;")); // Just to give some space 
        //    plcPaging.Controls.Add(objLbPrevious);
        //    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));


        //    // Creatig page numbers based on the start and end limit variables.
        //    for (int i = pageShowLimitStart; i <= pageShowLimitEnd; i++)
        //    {
        //        if ((Page.FindControl("lb_" + i.ToString()) == null) && i <= numberOfPages)
        //        {
        //            LinkButton objLb = new LinkButton();
        //            objLb.Click += new EventHandler(objLb_Click);
        //            objLb.Text = i.ToString();
        //            objLb.ID = "lb_" + i.ToString();
        //            objLb.CommandName = "pgChange";
        //            objLb.ToolTip = "Page " + i.ToString();
        //            objLb.EnableViewState = true;
        //            objLb.CommandArgument = i.ToString();

        //            if ((currentPage + 1) == i)
        //            {
        //                objLb.CssClass = "active";
        //                objLb.Enabled = false;

        //            }

        //            plcPaging.Controls.Add(objLb);
        //            //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));
        //        }
        //    }

        //    // Last Link button to display with paging
        //    LinkButton objLbLast = new LinkButton();
        //    objLbLast.Click += new EventHandler(objLb_Click);
        //    //objLbLast.Text = "Last";
        //    objLbLast.CssClass = "last";
        //    objLbLast.ToolTip = "Last Page";
        //    objLbLast.ID = "lb_LastPage";
        //    objLbLast.CommandName = "pgChange";
        //    objLbLast.EnableViewState = true;
        //    objLbLast.CommandArgument = numberOfPages.ToString();

        //    // Next Link button to display with paging
        //    LinkButton objLbNext = new LinkButton();
        //    objLbNext.Click += new EventHandler(objLb_Click);
        //    //objLbNext.Text = "Next";
        //    objLbNext.CssClass = "next";
        //    objLbNext.ToolTip = "Next Page";
        //    objLbNext.ID = "lb_NextPage";
        //    objLbNext.CommandName = "pgChange";
        //    objLbNext.EnableViewState = true;
        //    objLbNext.CommandArgument = (currentPage + 2).ToString();

        //    //of course if the page is the last page, then there is no need of last or next
        //    if ((currentPage + 1) == numberOfPages)
        //    {
        //        objLbLast.Enabled = false;
        //        objLbNext.Enabled = false;
        //    }
        //    else
        //    {
        //        objLbLast.Enabled = true;
        //        objLbNext.Enabled = true;
        //    }

        //    // Adding Control to the place holder
        //    plcPaging.Controls.Add(objLbNext);
        //    plcPaging.Controls.Add(objLbLast);

        //    plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));
        //    TextBox objtxtGoto = new TextBox();
        //    objtxtGoto.ID = "txtGoto";
        //    objtxtGoto.ToolTip = "Enter Page No.";
        //    objtxtGoto.MaxLength = 2;
        //    objtxtGoto.SkinID = "grdvgoto";
        //    objtxtGoto.Attributes.Add("onKeypress", "javascript:return NumberOnly(this);");
        //    objtxtGoto.Text = pageNo;
        //    plcPaging.Controls.Add(objtxtGoto);

        //    plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));

        //    ImageButton objIbtnGo = new ImageButton();
        //    objIbtnGo.ID = "ibtnGo";
        //    objIbtnGo.ToolTip = "Goto Page";
        //    objIbtnGo.ImageUrl = "~/Layouts/Resources/images/go-btn.png";
        //    objIbtnGo.Click += new ImageClickEventHandler(objIbtnGo_Click);
        //    plcPaging.Controls.Add(objIbtnGo);

        //    plcPaging.Controls.Add(new LiteralControl("</td></tr></table>"));
        //}
    }

    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, reportCount };
    }

    protected override void LoadViewState(object savedState)
    {
        object[] myState = (object[])savedState;
        if (myState[0] != null)
            base.LoadViewState(myState[0]);

        if (myState[1] != null)
        {
            ManagePaging();
        }

    }

    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton objlb = (LinkButton)sender;

        grdvParticipantList.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvParticipantList.DataBind();

        ManagePaging();

    }

    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
        if (txtGoto.Text.Trim() != "")
        {
            pageNo = txtGoto.Text;
            plcPaging.Controls.Clear();

            grdvParticipantList.PageIndex = Convert.ToInt32(txtGoto.Text.Trim()) - 1;
            grdvParticipantList.DataBind();
            ManagePaging();

            txtGoto.Text = pageNo;
        }
    }

    #endregion    

    #region Grid Method

    public void FillGridData()
    {
        odsReport.SelectParameters.Clear();
        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
        string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

        if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
        {
            odsReport.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            odsReport.SelectParameters.Add("projectID", ddlProject.SelectedValue);
            odsReport.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue);
            odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
            odsReport.Select();

            odsReport.FilterExpression = "UserID=" + identity.User.UserID.ToString();
            odsReport.FilterParameters.Clear();
            plcPaging.Controls.Clear();
        }
        else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))
        {
            odsReport.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            odsReport.SelectParameters.Add("projectID", ddlProject.SelectedValue);
            odsReport.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue);
            odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
            odsReport.Select();
        }
        else
        {
            odsReport.SelectParameters.Add("accountID", Convert.ToString(ViewState["accid"]));
            odsReport.SelectParameters.Add("projectID", Convert.ToString(ViewState["prjid"]));
            odsReport.SelectParameters.Add("programmeID", Convert.ToString(ViewState["prgid"]));
            odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
            odsReport.Select();
        }

    }       

    protected void ResetControls()
    {
        try
        {
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
            {
                odsReport.SelectParameters.Clear();
                odsReport.SelectParameters.Add("accountID", null);
                odsReport.SelectParameters.Add("projectID", null);
                odsReport.SelectParameters.Add("programmeID", null);
                odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
                odsReport.Select();

                ddlProject.SelectedValue = "0";
                ddlProgramme.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #endregion

    #region ZipCode

    public static void ZipFiles(string inputFolderPath, string outputPathAndFile, string password)
    {
        ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
        int TrimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
        // find number of chars to remove     // from orginal file path
        TrimLength += 1; //remove '\'
        FileStream ostream;
        byte[] obuffer;
        string outPath = inputFolderPath + @"\" + outputPathAndFile;
        ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream
        if (password != null && password != String.Empty)
            oZipStream.Password = password;
        oZipStream.SetLevel(9); // maximum compression
        ZipEntry oZipEntry;
        foreach (string Fil in ar) // for each file, generate a zipentry
        {
            oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
            oZipStream.PutNextEntry(oZipEntry);

            if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
            {
                ostream = File.OpenRead(Fil);
                obuffer = new byte[ostream.Length];
                ostream.Read(obuffer, 0, obuffer.Length);
                oZipStream.Write(obuffer, 0, obuffer.Length);
                ostream.Close();
            }
        }        
        oZipStream.Finish();
        oZipStream.Close();
    }

    private static ArrayList GenerateFileList(string Dir)
    {
        ArrayList fils = new ArrayList();
        bool Empty = true;
        foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
        {
            if (Path.GetExtension(file) == ".pdf" )//|| Path.GetExtension(file) == ".xml")
            {
                fils.Add(file);
            }
            Empty = false;
        }

        if (Empty)
        {
            if (Directory.GetDirectories(Dir).Length == 0)
            // if directory is completely empty, add it
            {
                fils.Add(Dir + @"/");
            }
        }

        //foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
        //    {
        //    foreach (object obj in GenerateFileList(dirs))
        //        {
        //        fils.Add(obj);
        //        }
        //    }
        return fils; // return file list
    }

    public static void UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
    {
        ZipInputStream s = new ZipInputStream(File.OpenRead(zipPathAndFile));
        if (password != null && password != String.Empty)
            s.Password = password;
        ZipEntry theEntry;
        string tmpEntry = String.Empty;
        while ((theEntry = s.GetNextEntry()) != null)
        {
            string directoryName = outputFolder;
            string fileName = Path.GetFileName(theEntry.Name);
            // create directory 
            if (directoryName != "")
            {
                Directory.CreateDirectory(directoryName);
            }
            if (fileName != String.Empty)
            {
                if (theEntry.Name.IndexOf(".ini") < 0)
                {
                    string fullPath = directoryName + "\\" + theEntry.Name;
                    fullPath = fullPath.Replace("\\ ", "\\");
                    string fullDirPath = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                    FileStream streamWriter = File.Create(fullPath);
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
        }
        s.Close();
        if (deleteZipFile)
            File.Delete(zipPathAndFile);
    }

    #endregion

    protected void grdvParticipantList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

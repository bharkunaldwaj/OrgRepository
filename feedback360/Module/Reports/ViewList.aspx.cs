using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Reporting.WebForms;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using ICSharpCode.SharpZipLib.Zip;

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

    Project_BAO projectBusinessAccessObject = new Project_BAO();
    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
    AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
    ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
    ReportManagement_BE reportManagementBusinessEntity = new ReportManagement_BE();
    AssignQstnParticipant_BAO assignQstnParticipantBusinessAccessObject = new AssignQstnParticipant_BAO();

    DataTable dataTableCompanyName;
    DataTable dataTableGroupList;
    DataTable dataTableSelfName;
    DataTable dataTableReportsID;

    string stringGroupList;
    string stringFrontPage;
    string stringReportIntroduction;
    string stringConclusionPage;
    string stringRadarChart;
    string stringDetailedQst;
    string stringCategoryQstlist;
    string stringCategoryBarChart;
    string stringFullProjectGrp;
    string stringSelfNameGrp;
    string stringProgrammeGrp;
    string stringReportName;
    string targetradarname = string.Empty;
    string targetradarPreviousScore = string.Empty;
    string targetradarBenchmark = string.Empty;
    string stringConHighLowRange;
    string stringReportType = string.Empty;
    string stringPreScoreVisibility = string.Empty;
    string stringStaticBarLabelVisibility = string.Empty;
    string stringBenchMarkGrpVisibility = string.Empty;
    string stringBenchMarkVisibility = string.Empty;
    string stringBenchConclusionPageVisibility = string.Empty;

    string stringTargetPersonID;
    string stringProjectID;
    string stringAccountID;
    string stringProgrammeID;
    string stringAdmin;
    int rptCandidateCount = 0;

    Category_BAO categoryBusinessAccessObject = new Category_BAO();
    Category_BE categoryBusinessEntity = new Category_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int reportCount = 0;
    string pageNo = "";

    string selfStatus = "";
    string filename = "";
    
    //string participantName;
    #endregion

    protected Label Label1;
    protected Label Label2;
    protected Label Label3;
    protected DropDownList ExplodedPointList;
    protected Label Label4;
    protected DropDownList HoleSizeList;

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




        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        try
        {
            System.GC.Collect();

            identity = this.Page.User.Identity as WADIdentity;
            //set the page size for grid.
            grdvParticipantList.PageSize = 50;
            ManagePaging();

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (txtGoto != null)
                txtGoto.Text = pageNo;                      


            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;
                //If user is super Admin then bind reminder grid with account id=1 else user account id.
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

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Bind account drop down by user account id.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";

                Project_BAO projectBusinessAccessObject = new Project_BAO();

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
                string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

                //If user group is Participant
                if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
                {
                    //Bind project by user account id.
                    ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ViewState["strAdmin"] = "N";
                    grdvParticipantList.AllowSorting = false;

                    AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
                    DataTable dataTableParticipantInformation = new DataTable();
                    //Get all participant in a project .
                    dataTableParticipantInformation = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                    if (dataTableParticipantInformation.Rows.Count>0)
                    ddlProject.SelectedValue = dataTableParticipantInformation.Rows[0]["ProjecctID"].ToString();

                    DataTable dataTableProgramme = new DataTable();
                    //Get all program in a project and program drop down list.
                    dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

                    if (dataTableProgramme.Rows.Count > 0)
                    {
                        //Bind program drop down list.
                        ddlProgramme.DataSource = dataTableProgramme;
                        ddlProgramme.DataTextField = "ProgrammeName";
                        ddlProgramme.DataValueField = "ProgrammeID";
                        ddlProgramme.DataBind();

                        if (dataTableParticipantInformation.Rows.Count>0)
                        ddlProgramme.SelectedValue = dataTableParticipantInformation.Rows[0]["ProgrammeID"].ToString();
                    }

                    //If user group is Participant then project andprogram drop down is disable.
                    ddlProject.Enabled = false;
                    ddlProgramme.Enabled = false;

                    //Clear object datasource for report grid and set parameters.
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
                else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))//If its group is Manager
                {
                    ViewState["strAdmin"] = "N"; //user is not Admin
                    grdvParticipantList.AllowSorting = false;

                    DataTable dataTableManagerProject = new DataTable();
                    //Get all project in a Manager and bind project dropdown.
                    dataTableManagerProject = projectBusinessAccessObject.GetManagerProject(identity.User.Email, Convert.ToInt32(identity.User.AccountID));

                    if (dataTableManagerProject.Rows.Count > 0)
                    {//bind project dropdown
                        ddlProject.DataSource = dataTableManagerProject;
                        ddlProject.DataValueField = "ProjectID";
                        ddlProject.DataTextField = "Title";
                        ddlProject.DataBind();

                        //ddlProject.SelectedValue = dtManagerProject.Rows[0]["ProjectID"].ToString();
                    }

                    DataTable dataTableManagerProgramme = new DataTable();
                    dataTableManagerProgramme = projectBusinessAccessObject.GetManagerProgramme(identity.User.Email, Convert.ToInt32(identity.User.AccountID));
                    //Clear object datasource for report grid and set parameters.
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
                    //Bind project by user account id.
                    ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ViewState["strAdmin"] = "Y";//user is  Admin
                    grdvParticipantList.AllowSorting = true;
                    //Clear object datasource for report grid and set parameters.
                    odsReport.SelectParameters.Clear();
                    odsReport.SelectParameters.Add("accountID", null);
                    odsReport.SelectParameters.Add("projectID", null);
                    odsReport.SelectParameters.Add("programmeID", null);
                    odsReport.SelectParameters.Add("admin", stringAdmin);
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
    /// <summary>
    /// Generate Radar chart
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void Radar(string strTargetPersonID, string strGroupList)
    {
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get Radar chart data for self assement.
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get Radar chart data for full project group.
        DataTable dataTableFullProjectData = reportManagementBusinessAccessObject.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            xValues[i] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dataTableFullProjectData.Rows.Count];
        double[] yValues1 = new double[dataTableFullProjectData.Rows.Count];
        for (int i = 0; i < dataTableFullProjectData.Rows.Count; i++)
        {
            xValues1[i] = dataTableFullProjectData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dataTableFullProjectData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullProjectData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullProjectData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullProjectData.Rows.Count > 0)
            Series2 = dataTableFullProjectData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullProjectData.Rows.Count > 0 || dataTableFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

    /// <summary>
    /// This shows the scores of the colleagues of the participant
    /// compare scores given for the participant with the average of all scores given for this project.
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarPreviousScore(string strTargetPersonID, string strGroupList)
    {
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
       //Get previous scores of the self assement.
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get previous scores of the colleagues of the participant 
        DataTable dataTableFullPreviousData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            xValues[i] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dataTableFullPreviousData.Rows.Count];
        double[] yValues1 = new double[dataTableFullPreviousData.Rows.Count];
        for (int i = 0; i < dataTableFullPreviousData.Rows.Count; i++)
        {
            xValues1[i] = dataTableFullPreviousData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dataTableFullPreviousData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullPreviousData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullPreviousData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullPreviousData.Rows.Count > 0)
            Series2 = dataTableFullPreviousData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullPreviousData.Rows.Count > 0 || dataTableFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
    }

    /// <summary>
    /// Radar chart data for self and full project group
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarCPL(string strTargetPersonID, string strGroupList)
    {
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get radar chart data for self assement
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get radar chart data for full project group
        DataTable dataTableFullProjectData = reportManagementBusinessAccessObject.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            if (i == dataTableSelfData.Rows.Count - 1)
            {
                xValues[0] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[0] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues[i + 1] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i + 1] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
        }

        string[] xValues1 = new string[dataTableFullProjectData.Rows.Count];
        double[] yValues1 = new double[dataTableFullProjectData.Rows.Count];
        for (int i = 0; i < dataTableFullProjectData.Rows.Count; i++)
        {
            if (i == dataTableFullProjectData.Rows.Count - 1)
            {
                xValues1[0] = dataTableFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[0] = Convert.ToDouble(dataTableFullProjectData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues1[i + 1] = dataTableFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[i + 1] = Convert.ToDouble(dataTableFullProjectData.Rows[i]["Average"].ToString());
            }
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullProjectData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullProjectData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullProjectData.Rows.Count > 0)
            Series2 = dataTableFullProjectData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullProjectData.Rows.Count > 0 || dataTableFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

    /// <summary>
    /// Get previous score for radar chart 
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarPreviousScoreCPL(string strTargetPersonID, string strGroupList)
    {
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get radar chart previous score data for self
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get radar chart previous score data 
        DataTable dataTableFullPreviousData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            if (i == dataTableSelfData.Rows.Count - 1)
            {
                xValues[0] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[0] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues[i + 1] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i + 1] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
        }

        string[] xValues1 = new string[dataTableFullPreviousData.Rows.Count];
        double[] yValues1 = new double[dataTableFullPreviousData.Rows.Count];
        for (int i = 0; i < dataTableFullPreviousData.Rows.Count; i++)
        {
            if (i == dataTableFullPreviousData.Rows.Count - 1)
            {
                xValues1[0] = dataTableFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[0] = Convert.ToDouble(dataTableFullPreviousData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues1[i + 1] = dataTableFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[i + 1] = Convert.ToDouble(dataTableFullPreviousData.Rows[i]["Average"].ToString());
            }
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullPreviousData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullPreviousData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullPreviousData.Rows.Count > 0)
            Series2 = dataTableFullPreviousData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullPreviousData.Rows.Count > 0 || dataTableFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
    }

    /// <summary>
    /// Get Radar chart benchmark data for self and full project group.
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    public void RadarBenchMark(string strTargetPersonID)
    {
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Radar chart benchmark data for self.
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "S");
        //Radar chart benchmark data for previous project.
        DataTable dataTableBenchMarkData = reportManagementBusinessAccessObject.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "P");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            xValues[i] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dataTableBenchMarkData.Rows.Count];
        double[] yValues1 = new double[dataTableBenchMarkData.Rows.Count];
        for (int i = 0; i < dataTableBenchMarkData.Rows.Count; i++)
        {
            xValues1[i] = dataTableBenchMarkData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dataTableBenchMarkData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableBenchMarkData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableBenchMarkData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableBenchMarkData.Rows.Count > 0)
            Series2 = dataTableBenchMarkData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(193, 255, 193); //(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarBenchmark = Server.MapPath("~\\UploadDocs\\") + "RadarChartBenchMark" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableBenchMarkData.Rows.Count > 0 || dataTableBenchMarkData.Rows.Count > 0)
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
    /// <summary>
    /// Fill grid with participant details .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        FillGridData();
        ManagePaging();
    }

    /// <summary>
    /// Reset control with default value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ResetControls();
    }
    #endregion   

    #region dropdown event
    /// <summary>
    /// Fill project an company details by account .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetControls();

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account conapmy details.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);

            //set company name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();


            if (ddlAccountCode.SelectedIndex > 0)
            {
                //Get all project in a Account.
                DataTable dataTableProjectList = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

                if (dataTableProjectList.Rows.Count > 0)
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                    //Bind project dropdown
                    ddlProject.DataSource = dataTableProjectList;
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

    /// <summary>
    /// Fill Program in a project .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        //Get all program in a project.
        dtProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            //Bind program drop down.
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

    /// <summary>
    /// save program value to viewstate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }
    #endregion

    #region ReportMethods
    /// <summary>
    /// Set details of Account ,Project and program of participant.
    /// </summary>
    /// <param name="targetid"></param>
    protected void GetDetailFromTargetPersonID(string targetid)
    {
        stringTargetPersonID = targetid;
        ViewState["strTargetPersonID"] = targetid;
        //Get all questionnaire in a participant.
        DataTable dataTableUserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(stringTargetPersonID));

        if (dataTableUserlist != null && dataTableUserlist.Rows.Count > 0)
        {
            int projectid = Convert.ToInt32(dataTableUserlist.Rows[0]["ProjectID"]);
            stringProjectID = dataTableUserlist.Rows[0]["ProjectID"].ToString();
            ViewState["strProjectID"] = dataTableUserlist.Rows[0]["ProjectID"].ToString();
            
            DataTable project = projectBusinessAccessObject.GetdataProjectByID(projectid);

            if (project != null && project.Rows.Count > 0)
            {
                stringAccountID = project.Rows[0]["AccountID"].ToString();
                ViewState["strAccountID"] = project.Rows[0]["AccountID"].ToString();
            }

            DataTable programme = reportManagementBusinessAccessObject.GetdataProjectByID(projectid);

            if (programme != null && programme.Rows.Count > 0)
            {
                stringProgrammeID = programme.Rows[0]["ProgrammeID"].ToString();
                ViewState["strProgrammeID"] = programme.Rows[0]["ProgrammeID"].ToString();
            }
        }
        else
        {   
            ViewState["strProjectID"] = Convert.ToString(ViewState["prjid"]);

            stringAccountID = Convert.ToString(ViewState["accid"]);
            ViewState["strAccountID"] = stringAccountID;

            int projectid = Convert.ToInt32(ViewState["prjid"]);

            DataTable programme = reportManagementBusinessAccessObject.GetdataProjectByID(projectid);

            if (programme != null && programme.Rows.Count > 0)
            {
                stringProgrammeID = programme.Rows[0]["ProgrammeID"].ToString();
                ViewState["strProgrammeID"] = programme.Rows[0]["ProgrammeID"].ToString();
            }
        }

        /*For Self Name by Using Target Person ID & Account ID*/
        dataTableSelfName = accountUserBusinessAccessObject.GetdtAccountUserByID(Convert.ToInt32(stringAccountID), Convert.ToInt32(stringTargetPersonID));

        if (dataTableSelfName != null && dataTableSelfName.Rows.Count > 0)
        {            
            stringReportName = dataTableSelfName.Rows[0]["FirstName"].ToString() + dataTableSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyHHmmss");
            //ViewState["strReportName"] = dtSelfName.Rows[0]["FirstName"].ToString() + dtSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss");
        }
    }
        
    /// <summary>
    /// Initilize parameter for report controls.
    /// </summary>
    /// <param name="projectid"></param>
    protected void ControlToParameter(string projectid)
    {
        if (projectid != null)
        {
            DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));

            if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
            {
                // This parameter will Decide: which type of Report will Call                
                if (dataTableReportsetting.Rows[0]["ReportType"].ToString() != string.Empty)
                    stringReportType = dataTableReportsetting.Rows[0]["ReportType"].ToString();

                if (dataTableReportsetting.Rows[0]["CoverPage"].ToString() != string.Empty)
                    stringFrontPage = dataTableReportsetting.Rows[0]["CoverPage"].ToString();

                if (dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString() != string.Empty)
                    stringReportIntroduction = dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString();

                if (dataTableReportsetting.Rows[0]["Conclusionpage"].ToString() != string.Empty)
                    stringConclusionPage = dataTableReportsetting.Rows[0]["Conclusionpage"].ToString();

                if (dataTableReportsetting.Rows[0]["RadarChart"].ToString() != string.Empty)
                    stringRadarChart = dataTableReportsetting.Rows[0]["RadarChart"].ToString();

                if (dataTableReportsetting.Rows[0]["QstTextResponses"].ToString() != string.Empty)
                    stringDetailedQst = dataTableReportsetting.Rows[0]["QstTextResponses"].ToString();

                if (dataTableReportsetting.Rows[0]["CatQstList"].ToString() != string.Empty)
                    stringCategoryQstlist = dataTableReportsetting.Rows[0]["CatQstList"].ToString();

                if (dataTableReportsetting.Rows[0]["CatDataChart"].ToString() != string.Empty)
                    stringCategoryBarChart = dataTableReportsetting.Rows[0]["CatDataChart"].ToString();

                if (dataTableReportsetting.Rows[0]["CandidateSelfStatus"].ToString() != string.Empty)
                    stringSelfNameGrp = dataTableReportsetting.Rows[0]["CandidateSelfStatus"].ToString();

                if (dataTableReportsetting.Rows[0]["FullProjectGrp"].ToString() != string.Empty)
                    stringFullProjectGrp = dataTableReportsetting.Rows[0]["FullProjectGrp"].ToString();

                if (dataTableReportsetting.Rows[0]["ProgrammeGrp"].ToString() != string.Empty)
                    stringProgrammeGrp = dataTableReportsetting.Rows[0]["ProgrammeGrp"].ToString();

                if (dataTableReportsetting.Rows[0]["ProjectRelationGrp"].ToString() != string.Empty)
                    stringGroupList = dataTableReportsetting.Rows[0]["ProjectRelationGrp"].ToString();

                if (dataTableReportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != string.Empty)
                    stringConHighLowRange = dataTableReportsetting.Rows[0]["ConclusionHighLowRange"].ToString();

                if (dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString() != string.Empty)
                    stringPreScoreVisibility = dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString();


                if (dataTableReportsetting.Rows[0]["BenchMarkGrpVisible"].ToString() != string.Empty)
                    stringBenchMarkGrpVisibility = dataTableReportsetting.Rows[0]["BenchMarkGrpVisible"].ToString();

                if (dataTableReportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() != string.Empty)
                    stringBenchMarkVisibility = dataTableReportsetting.Rows[0]["BenchMarkScoreVisible"].ToString();

                if (dataTableReportsetting.Rows[0]["BenchConclusionpage"].ToString() != string.Empty)
                    stringBenchConclusionPageVisibility = dataTableReportsetting.Rows[0]["BenchConclusionpage"].ToString();
                
            }
        }
    }

    /// <summary>
    /// Export pdf report.
    /// </summary>
    /// <param name="dirName"></param>
    protected void btnExport(string dirName)
    {
        try
        {
          //  Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
            //Set report server path.
            rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string root = string.Empty;
            //set report folder path.
            root = Server.MapPath("~") + "\\ReportGenerate\\";

            /* Function : For Filling Paramters From Controls */
            ControlToParameter(stringProjectID);

            if (ddlAccountCode.SelectedValue != string.Empty)
                stringStaticBarLabelVisibility = ddlAccountCode.SelectedItem.ToString();
            else
                stringStaticBarLabelVisibility = "";

            //If strReportType = 1 Then FeedbackReport will Call
            //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
            if (stringReportType == "1") //If strReportType = 1 Then FeedbackReport will Call
            {
                DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(stringProjectID));
                if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
                {
                    /*
                     * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                     * & Making Entry in Table with Radarchatname
                     * & Calling in RDL (RadarImage)
                     */
                    if (dataTableReportsetting.Rows[0]["RadarChart"].ToString() == "1")
                        Radar(stringTargetPersonID, stringGroupList);
                    else
                        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScore(stringTargetPersonID, stringGroupList);
                    else
                        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //BenchMark Radar Chart.
                    if (dataTableReportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() == "1")
                        RadarBenchMark(stringTargetPersonID);
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

                //Initilize report Parameter for pdf report.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", stringTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("RadarChartVisibility", stringRadarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", stringGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", stringDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", stringCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", stringCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", stringSelfNameGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", stringFullProjectGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarName", targetradarname));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", stringProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", stringConHighLowRange));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", stringPreScoreVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", stringStaticBarLabelVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchMarkGrpVisibility", stringBenchMarkGrpVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNameBenchmark", targetradarBenchmark));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchMarkVisibility", stringBenchMarkVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchConclusionVisibility", stringBenchConclusionPageVisibility));
                //set parameter to report viewer.
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (stringReportType == "2")//For report 2 FeedbackReportClient1 is called.
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", stringTargetPersonID));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));            
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", stringGroupList));
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

                //set parameter for report viewer.
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (stringReportType == "3")//For report 3 report FeedbackReportClient2 is called.
            {
                 //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
              //  rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //New Changes 
                //Changed by Amit Singh
                DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(stringProjectID));
                if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
                {
                   // if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                    /*
                    * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                    * & Making Entry in Table with Radarchatname
                    * & Calling in RDL (RadarImage)
                    */
                        RadarCPL(stringTargetPersonID, stringGroupList);
                   // else
                      //  targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScoreCPL(stringTargetPersonID, stringGroupList);
                    else
                        targetradarPreviousScore = "RadarChartNoImage";

                }

                //Set report path.
                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", stringTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", stringFullProjectGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", stringProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", stringConHighLowRange));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarName", targetradarname));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (stringReportType == "4") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
               // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //set report path.
                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/CurFeedbackReport";
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", stringTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", stringGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", stringDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", stringCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", stringCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", stringSelfNameGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", stringFullProjectGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", stringProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", stringConHighLowRange));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", stringStaticBarLabelVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", stringPreScoreVisibility));
                rview.ServerReport.SetParameters(paramList);
            }         

//            rview.Visible = false;
            //Send parameter to report server.
            byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //string PDF_path = root + dirName + "\\" + strReportName + ".pdf";
            string PDF_path = root + stringReportName + ".pdf";
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
          //  if(ex.Message=="")
            HandleException(ex);
        }
    }
    #endregion

    #region GridView
    /// <summary>
    /// Bind values to grid view rows
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                //Find gridview control.
                HiddenField hdnUserID = (HiddenField)e.Row.FindControl("hdnUserID");
                Label labelCandidateCount = (Label)e.Row.FindControl("lblCandidateCount");
                Label labelCompleted = (Label)e.Row.FindControl("lblCompleted");
                Label labelSelfAssessment = (Label)e.Row.FindControl("lblSelfAssessment");
                
                //Get report management data 
                DataTable dataTableParticipantInformation = new DataTable();

                dataTableParticipantInformation = assignQstnParticipantBusinessAccessObject.GetParticipantReportInfo(Convert.ToInt32(hdnUserID.Value));

                rptCandidateCount = 0;

                if (dataTableParticipantInformation.Rows.Count > 0)
                {
                    if (dataTableParticipantInformation.Rows[0]["SubmitCount"].ToString() == "")
                        dataTableParticipantInformation.Rows[0]["SubmitCount"] = "0";
                    // Set Number of colleague in a participant.
                    rptCandidateCount = Convert.ToInt32(dataTableParticipantInformation.Rows[0]["SubmitCount"].ToString());
                }
                else
                {
                    rptCandidateCount = 0;
                }

                //set Number of candidate completed survey.
                if (labelCandidateCount != null)
                    labelCandidateCount.Text = assignQstnParticipantBusinessAccessObject.GetCandidatesCount(Convert.ToInt32(hdnUserID.Value)).ToString();

                if (labelCompleted != null)
                    labelCompleted.Text = assignQstnParticipantBusinessAccessObject.GetSubmissionCount(Convert.ToInt32(hdnUserID.Value)).ToString();

                //If survey inclueds self assement the nset self assement value as yes or no.
                if (labelSelfAssessment != null)
                {
                    selfStatus=assignQstnParticipantBusinessAccessObject.GetSelfAssessment(Convert.ToInt32(hdnUserID.Value)).ToString();
                    if (selfStatus == "1")
                        labelSelfAssessment.Text = "Yes";
                    else
                        labelSelfAssessment.Text = "No";
                }
                //Get the report type 
                if ((Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "1") || (Request.QueryString["Type"] == null))
                {
                    ImageButton imagebuttonReport = (ImageButton)e.Row.FindControl("ibtnReport");
                    filename = assignQstnParticipantBusinessAccessObject.GetReportFileName(Convert.ToInt32(hdnUserID.Value));

                    if (filename != "")
                        imagebuttonReport.Visible = true;
                    else
                        imagebuttonReport.Visible = false;

                    ImageButton ibtnEdit = (ImageButton)e.Row.FindControl("imgEdit");
                    if (Convert.ToInt32(labelCompleted.Text) == rptCandidateCount)
                        ibtnEdit.Visible = false;
                    else
                    {
                        ibtnEdit.Visible = true;
                        imagebuttonReport.Visible = false;
                    }
                }
   
                //string fpath = "http://localhost:2258/feedback360/reportgenerate/DrupalCMS.pdf";
                //ibtnReport.OnClientClick = "javascript:window.open ('" + fpath + "', 'mywindow','status=1,toolbar=1,location=0,menubar=0,resizable=0,scrollbars=0,height=100,width=100');";

                dataTableParticipantInformation.Dispose();
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// No use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Add Event for download of pdfreport.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvParticipantList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            string fileName = assignQstnParticipantBusinessAccessObject.GetReportFileName(Convert.ToInt32(e.CommandArgument)).ToString();

            string participantid = Convert.ToString(e.CommandArgument);
            //Get participant Account details.
            GetDetailFromTargetPersonID(participantid);

            //Retrieve initilize values for participant.
            if (Convert.ToString(ViewState["strTargetPersonID"]) != string.Empty)
                stringTargetPersonID = Convert.ToString(ViewState["strTargetPersonID"]);
            if (Convert.ToString(ViewState["strAccountID"]) != string.Empty)
                stringAccountID = Convert.ToString(ViewState["strAccountID"]);
            if (Convert.ToString(ViewState["strProjectID"]) != string.Empty)
                stringProjectID = Convert.ToString(ViewState["strProjectID"]);
            //if (Convert.ToString(ViewState["strReportName"]) != string.Empty)
            //    strReportName = Convert.ToString(ViewState["strReportName"]);
            if (Convert.ToString(ViewState["strProgrammeID"]) != string.Empty)
                stringProgrammeID = Convert.ToString(ViewState["strProgrammeID"]);

            if (stringTargetPersonID != null && stringAccountID != null && stringProjectID != null && stringReportName != null)
            {
                btnExport(""); 
                try
                {
                    string root = Server.MapPath("~") + "\\ReportGenerate\\";
                    string openpdf = root + stringReportName + ".pdf";
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
                    Response.Redirect("download.aspx?filename=" + stringReportName);
                    

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
            string reportFileName = assignQstnParticipantBusinessAccessObject.GetReportFileName(Convert.ToInt32(e.CommandArgument)).ToString();

            if (reportFileName != "")
            {
                string root = Server.MapPath("~") + "\\ReportGenerate\\";
                string openpdf = root + reportFileName;
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

    /// <summary>
    /// No use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvParticipantList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    /// <summary>
    /// No use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvParticipantList_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    #endregion

    /// <summary>
    /// It is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {        
        try
        {
            string participantid = string.Empty;

            string root = Server.MapPath("~") + "\\ReportGenerate\\";
            string newDir = ddlAccountCode.SelectedItem.Text + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");

            DirectoryInfo directoryrInformation = new DirectoryInfo(root);
            directoryrInformation.CreateSubdirectory(newDir);
            
            foreach (GridViewRow row in grdvParticipantList.Rows)
            {
                CheckBox myCheckBox = (CheckBox)row.FindControl("myCheckBox");
                HiddenField hdnUserID = (HiddenField)row.FindControl("hdnUserID");

                if (myCheckBox != null && myCheckBox.Checked == true)
                {
                    participantid = hdnUserID.Value.ToString();
                    GetDetailFromTargetPersonID(participantid);

                    if (Convert.ToString(ViewState["strTargetPersonID"]) != string.Empty)
                        stringTargetPersonID = Convert.ToString(ViewState["strTargetPersonID"]);
                    if (Convert.ToString(ViewState["strAccountID"]) != string.Empty)
                        stringAccountID = Convert.ToString(ViewState["strAccountID"]);
                    if (Convert.ToString(ViewState["strProjectID"]) != string.Empty)
                        stringProjectID = Convert.ToString(ViewState["strProjectID"]);
                    if (Convert.ToString(ViewState["strProgrammeID"]) != string.Empty)
                        stringProgrammeID = Convert.ToString(ViewState["strProgrammeID"]);

                    if (stringTargetPersonID != null && stringAccountID != null && stringProjectID != null && stringReportName != null)
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
    /// <summary>
    /// No use.
    /// </summary>
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

    /// <summary>
    /// Save the view state of the page.
    /// </summary>
    /// <returns></returns>
    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, reportCount };
    }

    /// <summary>
    /// Reload the viewsate when view sate of the page expires.
    /// </summary>
    /// <param name="savedState"></param>
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

    /// <summary>
    /// Gridview Paging Next previous button event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton objlb = (LinkButton)sender;
        //Reset Page index to new index.
        grdvParticipantList.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvParticipantList.DataBind();

        ManagePaging();

    }

    /// <summary>
    /// Handel record search in gridview by page number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
        if (txtGoto.Text.Trim() != "")
        {
            pageNo = txtGoto.Text;
            plcPaging.Controls.Clear();
            //Reset Page index to new index.
            grdvParticipantList.PageIndex = Convert.ToInt32(txtGoto.Text.Trim()) - 1;
            grdvParticipantList.DataBind();
            //Handel pageing in Gridview.
            ManagePaging();

            txtGoto.Text = pageNo;
        }
    }
    #endregion    

    #region Grid Method
    /// <summary>
    /// Initilize object data source for grid.
    /// </summary>
    public void FillGridData()
    {
        odsReport.SelectParameters.Clear();
        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
        string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

        if (identity.User.GroupID == Convert.ToInt32(participantRoleId))//If user group is participant.
        {
            //Initilize object datasource for grid.
            odsReport.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            odsReport.SelectParameters.Add("projectID", ddlProject.SelectedValue);
            odsReport.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue);
            odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
            odsReport.Select();

            odsReport.FilterExpression = "UserID=" + identity.User.UserID.ToString();
            odsReport.FilterParameters.Clear();
            plcPaging.Controls.Clear();
        }
        else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))//If user group is Manager.
        {
            //Initilize object datasource for grid.
            odsReport.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            odsReport.SelectParameters.Add("projectID", ddlProject.SelectedValue);
            odsReport.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue);
            odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
            odsReport.Select();
        }
        else
        {
            //If user group is Admin ,Initilize object datasource for grid with stored values.
            odsReport.SelectParameters.Add("accountID", Convert.ToString(ViewState["accid"]));
            odsReport.SelectParameters.Add("projectID", Convert.ToString(ViewState["prjid"]));
            odsReport.SelectParameters.Add("programmeID", Convert.ToString(ViewState["prgid"]));
            odsReport.SelectParameters.Add("admin", Convert.ToString(ViewState["strAdmin"]));
            odsReport.Select();
        }

    }       

    /// <summary>
    /// Reset controls value to default.
    /// </summary>
    protected void ResetControls()
    {
        try
        {
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
            {
                //Clear object data source for report grid.
                odsReport.SelectParameters.Clear();
                //Reset object data source for report grid.
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
    /// <summary>
    /// Create a Zip file for download.
    /// </summary>
    /// <param name="inputFolderPath"></param>
    /// <param name="outputPathAndFile"></param>
    /// <param name="password"></param>
    public static void ZipFiles(string inputFolderPath, string outputPathAndFile, string password)
    {
        ArrayList arrayFileList = GenerateFileList(inputFolderPath); // generate file list
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

        foreach (string Fil in arrayFileList) // for each file, generate a zipentry
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

    /// <summary>
    /// Add pdf files to directory and return file list in a directory.
    /// </summary>
    /// <param name="Dir"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Unzip the  zip file
    /// </summary>
    /// <param name="zipPathAndFile"></param>
    /// <param name="outputFolder"></param>
    /// <param name="password"></param>
    /// <param name="deleteZipFile"></param>
    public static void UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
    {
        ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipPathAndFile));

        if (password != null && password != String.Empty)
            zipStream.Password = password;

        ZipEntry theEntry;
        string tmpEntry = String.Empty;

        while ((theEntry = zipStream.GetNextEntry()) != null)
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
                        size = zipStream.Read(data, 0, data.Length);
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

        zipStream.Close();

        if (deleteZipFile)
            File.Delete(zipPathAndFile);//Delete file
    }
    #endregion

    /// <summary>
    /// No use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvParticipantList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

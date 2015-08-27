using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using System.Collections;
using Admin_BAO;
using DatabaseAccessUtilities;
using System.Data.OleDb;

public partial class Survey_Module_Questionnaire_AddPreviousScore : CodeBehindBase
{
    Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            fillAccountCode();
        }
        //fillAnalysis();
    }

    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            CNameValueList lstcname = new CNameValueList();

            lstcname.Add("@Operation", "ADDPRVSCR");
            lstcname.Add("@AccountID", Convert.ToInt32(ddlAccountCode.SelectedValue));
            lstcname.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            lstcname.Add("@ProgrammeID", Convert.ToInt32(ddlProgrammeName.SelectedValue));
            lstcname.Add("@ProjectID", Convert.ToInt32(ddlProject.SelectedValue));
            //lstcname.Add("@ScoreTitle", txtName.Text);
            lstcname.Add("@TeamName", txtClientName0.Text);
            lstcname.Add("@Score1Title", txtScore1Title.Text);
            lstcname.Add("@Score2Title", txtScore2Title.Text);

            Common_BAO objCommon_BAO = new Common_BAO();
            int previousScoreId = objCommon_BAO.InsertAndUpdate("Survey_UspPreviousScore", lstcname);


            var lstpreviousScore = GetPreviousScoreList(previousScoreId);

            CNameValueList lstcnamesc = new CNameValueList();

            foreach (var previousScore in lstpreviousScore)
            {

                lstcnamesc.Add("@Operation", "ADDPRVSCRDET");
                lstcnamesc.Add("@CategoryID", previousScore.CategoryID);
                lstcnamesc.Add("@AnalysisType", previousScore.AnalysisType);
                lstcnamesc.Add("@PreviousScoreID", previousScore.PreviousScoreID);
                lstcnamesc.Add("@QuestionID", previousScore.QuestionID);
                lstcnamesc.Add("@Score1", previousScore.Score1);
                lstcnamesc.Add("@Score2", previousScore.Score2);

                objCommon_BAO.InsertAndUpdate("Survey_UspPreviousScore", lstcnamesc);
                lstcnamesc.Clear();
            }

           // Response.Redirect("~/Survey_Default.aspx", false);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "alert('Successfully added');window.location.href='AddPreviousScore.aspx';", true);
            //lbl_save_message.Text = "Successfully added.";
            

        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("~/Survey_Default.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillProject(ddlAccountCode.SelectedValue);
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCompany();
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillProgramme();
    }

    protected void ddlProgrammeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillAnalysis();

        DataTable dt = GetPreviousScoreSettings();

        if (dt != null && dt.Rows.Count > 0)
        {
            txtClientName0.Text = dt.Rows[0]["TeamName"].ToString();
            txtScore1Title.Text = dt.Rows[0]["Score1Title"].ToString();
            txtScore2Title.Text = dt.Rows[0]["Score2Title"].ToString();
        }
    }

    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {

    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);

    }

    private void fillCompany()
    {
        Survey_Company_BAO company_BAO = new Survey_Company_BAO();
        var dt = company_BAO.GetdtCompanyList(GetCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dt;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }

    private void fillAccountCode()
    {
        Account_BAO account_BAO = new Account_BAO();
        ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
        ddlAccountCode.DataValueField = "AccountID";
        ddlAccountCode.DataTextField = "Code";
        ddlAccountCode.DataBind();

    }

    private void fillProject(string accountId)
    {
        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.DataSource = project_BAO.GetdtProjectList(accountId);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

    }

    private void fillProgramme()
    {
        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();

        string accountId = GetConditionProgramme();

        ddlProgrammeName.Items.Clear();
        ddlProgrammeName.Items.Insert(0, new ListItem("Select", "0"));

        ddlProgrammeName.DataSource = programme_BAO.GetdtProgrammeListNew(accountId);
        ddlProgrammeName.DataValueField = "ProgrammeID";
        ddlProgrammeName.DataTextField = "ProgrammeName";
        ddlProgrammeName.DataBind();
    }

    private void fillAnalysis()
    {
        DataTable dtAnalysis1;
        DataTable dtAnalysis2;
        DataTable dtAnalysis3;

        int programmeID = Convert.ToInt32(ddlProgrammeName.SelectedValue);
        if (programmeID > 0)
        {
            dtAnalysis1 = programme_BAO.GetAnalysis1(programmeID);

            Repeater0.DataSource = dtAnalysis1;
            Repeater0.DataBind();
            Repeater0.Visible = true;

            dtAnalysis2 = programme_BAO.GetAnalysis2(programmeID);
            Repeater1.DataSource = dtAnalysis2;
            Repeater1.DataBind();
            Repeater1.Visible = true;

            dtAnalysis3 = programme_BAO.GetAnalysis3(programmeID);
            Repeater2.DataSource = dtAnalysis3;
            Repeater2.DataBind();
            Repeater2.Visible = true;


        }
        else
        {

            DataTable blankdt = new DataTable();
            Repeater0.DataSource = blankdt;
            Repeater0.DataBind();
            Repeater0.Visible = false;

            Repeater1.DataSource = blankdt;
            Repeater1.DataBind();
            Repeater1.Visible = false;

            Repeater2.DataSource = blankdt;
            Repeater2.DataBind();
            Repeater2.Visible = false;
        }
        UpdatePanel1.Update();
    }

    protected void Repeater0_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem rpItem = e.Item;

        //Label lblCatID = (Label)rpItem.FindControl("lblCategoryID");
        Repeater rptrQuestionPreviousScoresAn1 = (Repeater)rpItem.FindControl("rptrQuestionPreviousScoresAn1");

        Label lblCategoryID = (Label)rpItem.FindControl("lblCategoryID");

        int categoryId = 0;
        if (lblCategoryID != null && !string.IsNullOrEmpty(lblCategoryID.Text))
            categoryId = Convert.ToInt32(lblCategoryID.Text);

        DataTable dtOldData = GetOldPreviousScoreList(categoryId);

        if (rptrQuestionPreviousScoresAn1 != null)
        {
            fillQuestionnaire("1", rptrQuestionPreviousScoresAn1,dtOldData);
        }
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem rpItem = e.Item;

        //Label lblCatID = (Label)rpItem.FindControl("lblCategoryID");
        Repeater rptrQuestionPreviousScoresAn2 = (Repeater)rpItem.FindControl("rptrQuestionPreviousScoresAn2");

        Label lblCategoryID = (Label)rpItem.FindControl("lblCategoryID");

        int categoryId = 0;
        if (lblCategoryID != null && !string.IsNullOrEmpty(lblCategoryID.Text))
            categoryId = Convert.ToInt32(lblCategoryID.Text);

        DataTable dtOldData = GetOldPreviousScoreList(categoryId);

        if (rptrQuestionPreviousScoresAn2 != null)
        {
            fillQuestionnaire("2", rptrQuestionPreviousScoresAn2,dtOldData);

        }


    }

    protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem rpItem = e.Item;

        //Label lblCatID = (Label)rpItem.FindControl("lblCategoryID");
        Repeater rptrQuestionPreviousScoresAn3 = (Repeater)rpItem.FindControl("rptrQuestionPreviousScoresAn3");

        Label lblCategoryID = (Label)rpItem.FindControl("lblCategoryID");

        int categoryId = 0;
        if (lblCategoryID != null && !string.IsNullOrEmpty(lblCategoryID.Text))
            categoryId = Convert.ToInt32(lblCategoryID.Text);

        DataTable dtOldData = GetOldPreviousScoreList(categoryId);


        if (rptrQuestionPreviousScoresAn3 != null)
        {
            fillQuestionnaire("3", rptrQuestionPreviousScoresAn3,dtOldData);

        }

    }

    private void fillQuestionnaire(string analysisType, Repeater rptr, DataTable dtOldData)
    {
        DataTable dtTemplate = new DataTable();
        Common_BAO objCommon_BAO = new Common_BAO();

        CNameValueList objCnameList = new CNameValueList();
        objCnameList.Add("@Operation", "GETQUEST");
        objCnameList.Add("@ProjectID", Convert.ToInt32(ddlProject.SelectedValue));
        objCnameList.Add("@AnalysisType", analysisType);
        objCnameList.Add("@PreviousScoreID", DBNull.Value);

        dtTemplate = objCommon_BAO.GetDataTable("Survey_UspPreviousScore", objCnameList);

        if (dtOldData != null && dtOldData.Rows.Count > 0)
        {
            foreach (DataRow item in dtTemplate.Rows)
            {
                int questionid = 0;
                if (item["QuestionID"] != null && !string.IsNullOrEmpty(Convert.ToString(item["QuestionID"])))
                    questionid = Convert.ToInt32(item["QuestionID"]);
               
                DataRow[] result = dtOldData.Select("QuestionID = " + questionid);

                if (result.Length > 0)
                {
                    item["Score1"] = Convert.ToString(result[0]["Score1"]);
                    item["Score2"] = Convert.ToString(result[0]["Score2"]);
                }
                //else
                //{
                //    item["Score1"] = "0.00";
                //    item["Score2"] = "0.00";
                //}
            }
        }

        rptr.DataSource = dtTemplate;
        rptr.DataBind();
    }

    public string GetConditionProgramme()
    {
        string str = "";

        if (ddlAccountCode.SelectedIndex > 0)
            str = str + "" + ddlAccountCode.SelectedValue + " and ";
        else
            str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlProject.SelectedIndex > 0)
            str = str + "[Survey_Project].[ProjectID] = " + ddlProject.SelectedValue + " and ";

        if (ddlCompany.SelectedIndex > 0)
            str = str + "Survey_Analysis_Sheet.[CompanyID] = " + ddlCompany.SelectedValue + " and ";

        string param = str.Substring(0, str.Length - 4);

        return param;
    }

    public string GetCondition()
    {
        string str = "";

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    str = str + "" + ViewState["AccountID"] + " and ";
        //else
        //    str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlAccountCode.SelectedIndex > 0)
            str = str + "" + ddlAccountCode.SelectedValue + " and ";

        if (ddlProject.SelectedIndex > 0)
            str = str + "Survey_Project.[ProjectID] = " + ddlProject.SelectedValue + " and ";

        string param = str.Substring(0, str.Length - 4);

        return param;
    }

    private List<Survey_PrvScore_QstDetails_BE> GetPreviousScoreList(int previousScoreId)
    {

        List<Survey_PrvScore_QstDetails_BE> previousScoreDetails_BEList = new List<Survey_PrvScore_QstDetails_BE>();

        //for analysis-1 repeater
        foreach (RepeaterItem item in Repeater0.Items)
        {
            Repeater rptrQuestionPreviousScoresAn1 = (Repeater)item.FindControl("rptrQuestionPreviousScoresAn1");
            Label lblCategoryID = (Label)item.FindControl("lblCategoryID");

            int categoryId = 0;
            if (lblCategoryID != null && !string.IsNullOrEmpty(lblCategoryID.Text))
                categoryId = Convert.ToInt32(lblCategoryID.Text);

            foreach (RepeaterItem analysis in rptrQuestionPreviousScoresAn1.Items)
            {
                Label lblQuestionnaireID = (Label)analysis.FindControl("lblQuestionnaireID");
                TextBox txtScore1 = (TextBox)analysis.FindControl("txtScore1");
                TextBox txtScore2 = (TextBox)analysis.FindControl("txtScore2");

                Survey_PrvScore_QstDetails_BE previuosScoreDetails_BE = new Survey_PrvScore_QstDetails_BE();
                previuosScoreDetails_BE.AnalysisType = "1";
                previuosScoreDetails_BE.CategoryID = categoryId;
                previuosScoreDetails_BE.PreviousScoreID = previousScoreId;
                if (lblQuestionnaireID != null && !string.IsNullOrEmpty(lblQuestionnaireID.Text))
                    previuosScoreDetails_BE.QuestionID = Convert.ToInt32(lblQuestionnaireID.Text);
                previuosScoreDetails_BE.Score1 = string.IsNullOrEmpty(txtScore1.Text) ? 0 : Convert.ToDecimal(txtScore1.Text);
                previuosScoreDetails_BE.Score2 = string.IsNullOrEmpty(txtScore2.Text) ? 0 : Convert.ToDecimal(txtScore2.Text);
                previousScoreDetails_BEList.Add(previuosScoreDetails_BE);
            }
        }

        //for analysis-II repeater
        foreach (RepeaterItem item in Repeater1.Items)
        {
            Repeater rptrQuestionPreviousScoresAn2 = (Repeater)item.FindControl("rptrQuestionPreviousScoresAn2");
            Label lblCategoryID = (Label)item.FindControl("lblCategoryID");

            int categoryId = 0;
            if (lblCategoryID != null && !string.IsNullOrEmpty(lblCategoryID.Text))
                categoryId = Convert.ToInt32(lblCategoryID.Text);

            foreach (RepeaterItem analysis in rptrQuestionPreviousScoresAn2.Items)
            {
                Label lblQuestionnaireID = (Label)analysis.FindControl("lblQuestionnaireID");
                TextBox txtScore1 = (TextBox)analysis.FindControl("txtScore1");
                TextBox txtScore2 = (TextBox)analysis.FindControl("txtScore2");

                Survey_PrvScore_QstDetails_BE previuosScoreDetails_BE = new Survey_PrvScore_QstDetails_BE();
                previuosScoreDetails_BE.AnalysisType = "2";
                previuosScoreDetails_BE.CategoryID = categoryId;
                previuosScoreDetails_BE.PreviousScoreID = previousScoreId;
                if (lblQuestionnaireID != null && !string.IsNullOrEmpty(lblQuestionnaireID.Text))
                    previuosScoreDetails_BE.QuestionID = Convert.ToInt32(lblQuestionnaireID.Text);
                previuosScoreDetails_BE.Score1 = string.IsNullOrEmpty(txtScore1.Text) ? 0 : Convert.ToDecimal(txtScore1.Text);
                previuosScoreDetails_BE.Score2 = string.IsNullOrEmpty(txtScore2.Text) ? 0 : Convert.ToDecimal(txtScore2.Text);
                previousScoreDetails_BEList.Add(previuosScoreDetails_BE);
            }
        }

        //for analysis-III repeater
        foreach (RepeaterItem item in Repeater2.Items)
        {
            Repeater rptrQuestionPreviousScoresAn3 = (Repeater)item.FindControl("rptrQuestionPreviousScoresAn3");
            Label lblCategoryID = (Label)item.FindControl("lblCategoryID");

            int categoryId = 0;
            if (lblCategoryID != null && !string.IsNullOrEmpty(lblCategoryID.Text))
                categoryId = Convert.ToInt32(lblCategoryID.Text);

            foreach (RepeaterItem analysis in rptrQuestionPreviousScoresAn3.Items)
            {
                Label lblQuestionnaireID = (Label)analysis.FindControl("lblQuestionnaireID");
                TextBox txtScore1 = (TextBox)analysis.FindControl("txtScore1");
                TextBox txtScore2 = (TextBox)analysis.FindControl("txtScore2");

                Survey_PrvScore_QstDetails_BE previuosScoreDetails_BE = new Survey_PrvScore_QstDetails_BE();
                previuosScoreDetails_BE.AnalysisType = "3";
                previuosScoreDetails_BE.CategoryID = categoryId;
                previuosScoreDetails_BE.PreviousScoreID = previousScoreId;
                if (lblQuestionnaireID != null && !string.IsNullOrEmpty(lblQuestionnaireID.Text))
                    previuosScoreDetails_BE.QuestionID = Convert.ToInt32(lblQuestionnaireID.Text);
                previuosScoreDetails_BE.Score1 = string.IsNullOrEmpty(txtScore1.Text) ? 0 : Convert.ToDecimal(txtScore1.Text);
                previuosScoreDetails_BE.Score2 = string.IsNullOrEmpty(txtScore2.Text) ? 0 : Convert.ToDecimal(txtScore2.Text);
                previousScoreDetails_BEList.Add(previuosScoreDetails_BE);
            }
        }

        return previousScoreDetails_BEList;
    }

    protected void imbFindOld_Click(object sender, ImageClickEventArgs e)
    {
        fillAnalysis();

    }

    private DataTable GetOldPreviousScoreList(int categoryId)
    {
        DataTable dtTemplate = new DataTable();
        Common_BAO objCommon_BAO = new Common_BAO();

        CNameValueList objCnameList = new CNameValueList();
        objCnameList.Add("@Operation", "GETOLDPREV");
        objCnameList.Add("@AccountID", Convert.ToInt32(ddlAccountCode.SelectedValue));
        objCnameList.Add("@ProjectID", Convert.ToInt32(ddlProject.SelectedValue));
        objCnameList.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
        objCnameList.Add("@ProgrammeID", Convert.ToInt32(ddlProgrammeName.SelectedValue));
        objCnameList.Add("@TeamName", txtClientName0.Text.Trim());
        objCnameList.Add("@Score1Title", txtScore1Title.Text.Trim());
        objCnameList.Add("@Score2Title", txtScore2Title.Text.Trim());
        objCnameList.Add("@CategoryID", categoryId);


        dtTemplate = objCommon_BAO.GetDataTable("Survey_UspPreviousScore", objCnameList);

        return dtTemplate;

    }

    private DataTable GetPreviousScoreSettings()
    {
        DataTable dtTemplate = new DataTable();
        Common_BAO objCommon_BAO = new Common_BAO();

        CNameValueList objCnameList = new CNameValueList();
        objCnameList.Add("@Operation", "GETPREVSCORE");
        objCnameList.Add("@AccountID", Convert.ToInt32(ddlAccountCode.SelectedValue));
        objCnameList.Add("@ProjectID", Convert.ToInt32(ddlProject.SelectedValue));
        objCnameList.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
        objCnameList.Add("@ProgrammeID", Convert.ToInt32(ddlProgrammeName.SelectedValue));
    

        dtTemplate = objCommon_BAO.GetDataTable("Survey_UspPreviousScore", objCnameList);

        return dtTemplate;

    }

    protected void txtScore1Title_TextChanged(object sender, EventArgs e)
    {
        fillAnalysis();
    }

    protected void txtScore2Title_TextChanged(object sender, EventArgs e)
    {
        fillAnalysis();
    }

    protected void txtClientName0_TextChanged(object sender, EventArgs e)
    {
        fillAnalysis();
    }

    // --> 1.0.0.1.2 [Upload Previous Score from Excel File]
    protected void imgBtnUploadScoreExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (true)
            {
                if (fleUploadScoreExcel.HasFile)
                {
                    if (this.IsFileValid(this.fleUploadScoreExcel))
                    {
                        string fileName = string.Empty;
                        string file = string.Empty;

                        fileName = System.IO.Path.GetFileName(fleUploadScoreExcel.PostedFile.FileName);
                        file = GetUniqueFilename(fileName);

                        fileName = Server.MapPath("~") + "\\UploadDocs\\" + file;
                        fleUploadScoreExcel.SaveAs(fileName);

                        DataTable dtPreviousScore = GetExcelForPreviousScore(fileName);

                        if (dtPreviousScore != null && dtPreviousScore.Rows.Count > 0)
                        {
                            dtPreviousScore.TableName = "SurveyPreviousScore";
                            string xml = string.Empty;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using(TextWriter tw = new StreamWriter(ms))
                                {
                                    System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer(typeof(DataTable));
                                    xmlSer.Serialize(tw,dtPreviousScore);
                                    xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                                }
                            }

                            if (!string.IsNullOrEmpty(xml))
                            {
                                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                                xml = xml.Remove(xml.IndexOf("<xs:schema"), xml.IndexOf("<DocumentElement>") - xml.IndexOf("<xs:schema"));
                                xml = xml.Replace("diffgr:", "").Replace("msdata:","");
                                xml = xml.Replace("</diffgram>", "");

                                Common_BAO objCommon_BAO = new Common_BAO();
                                CNameValueList lstcnamesc = new CNameValueList();
                                try
                                {
                                    lstcnamesc.Add(new CNameValue("@AccountID", ddlAccountCode.SelectedValue));
                                    lstcnamesc.Add(new CNameValue("@ProjectID", ddlProject.SelectedValue));
                                    lstcnamesc.Add(new CNameValue("@CompanyID", ddlCompany.SelectedValue));
                                    lstcnamesc.Add(new CNameValue("@ProgramID", ddlProgrammeName.SelectedValue));
                                    lstcnamesc.Add(new CNameValue("@TeamName", txtClientName0.Text.Trim()));
                                    lstcnamesc.Add(new CNameValue("@Score1Title", txtScore1Title.Text.Trim()));
                                    lstcnamesc.Add(new CNameValue("@Score2Title", txtScore2Title.Text.Trim()));
                                    lstcnamesc.Add(new CNameValue("@PreviousScoreXML",xml));

                                    int retVal = objCommon_BAO.InsertAndUpdate("Survey_UspUploadPreviousScoreByXML", lstcnamesc);
                                    if (retVal > 0)
                                    {
                                        lbl_save_message.Text = "Score updated successfully.";

                                        fillAnalysis();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    HandleException(ex);
                                }
                                finally
                                {
                                    objCommon_BAO = null;
                                    lstcnamesc.Clear();
                                }
                            }
                        }
                        else
                        {
                            lbl_save_message.Text = "Please check your file data. "+ fileName;
                        }
                        File.Delete(fileName);
                    }
                    else
                    {
                        lbl_save_message.Text = "Invalid file type.";
                    }
                }
                else
                {
                    lbl_save_message.Text = "Please browse a file to upload.";
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private DataTable GetExcelForPreviousScore(string fileName)
    {
        Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
        Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
        int index = 0;
        object rowIndex = 2;

        DataTable dtExcel = new DataTable();
        dtExcel.Columns.Add("Analysis", typeof(string));
        dtExcel.Columns.Add("Category", typeof(string));
        dtExcel.Columns.Add("QuestionSequence", typeof(int));
        dtExcel.Columns.Add("P1", typeof(int));
        dtExcel.Columns.Add("P2", typeof(int));

        string projid = ddlProject.SelectedValue.ToString();
        DataRow row;
        try
        {
            while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
            {
                row = dtExcel.NewRow();

                row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                row[2] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);
                row[3] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2);
                row[4] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2);

                index++;
                rowIndex = 2 + index;
                dtExcel.Rows.Add(row);
            }
        }
        catch
        {
            dtExcel = null;
        }
        finally
        {
            app.Workbooks.Close();
        }

        return dtExcel;
    }

    protected bool IsFileValid(FileUpload uploadControl)
    {
        bool isFileOk = true;
        try
        {
            string FileExt = ".xls,.xlsx,.XLS,.XLSX";
            string[] AllowedExtensions = FileExt.Split(',');

            bool isExtensionError = false;

            int MaxSizeAllowed = 5 * 1048576;// Size Allow only in mb
            if (uploadControl.PostedFile != null)
            {
                bool isSizeError = false;
                // Validate for size less than MaxSizeAllowed...
                if (uploadControl.PostedFile.ContentLength > MaxSizeAllowed)
                {
                    isFileOk = false;
                    isSizeError = true;
                }
                else
                {
                    isFileOk = true;
                    isSizeError = false;
                }

                // If OK so far, validate the file extension...
                if (isFileOk)
                {
                    isFileOk = false;
                    isExtensionError = true;

                    // Get the file's extension...
                    string fileExtension = System.IO.Path.GetExtension(uploadControl.PostedFile.FileName).ToLower();

                    for (int i = 0; i < AllowedExtensions.Length; i++)
                    {
                        if (fileExtension.Trim() == AllowedExtensions[i].Trim())
                        {
                            isFileOk = true;
                            isExtensionError = false;
                            break;
                        }
                    }
                }

                if (isExtensionError)
                {
                    lbl_save_message.Text = "Invalid file type";
                }
                if (isSizeError)
                {
                    lbl_save_message.Text = "Maximum Size of the File exceeded";
                }
            }
            return isFileOk;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        return uniquefilename;
    }

    public System.Data.DataTable ReturnExcelDataTableMot(string FullFileNamePath)
    {
        DateTime dt3 = new DateTime();

        string SheetName = string.Empty;
        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
            int index = 0;
            object rowIndex = 2;

            DataTable dtExcel = new DataTable();

            dtExcel.Columns.Add("Analysis1", typeof(string));
            dtExcel.Columns.Add("Analysis2", typeof(string));
            dtExcel.Columns.Add("Analysis3", typeof(string));
            dtExcel.Columns.Add("Name", typeof(string));
            dtExcel.Columns.Add("EmailAddress", typeof(string));

            DataRow row;
            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    //rowIndex = 2 + index;
                    row = dtExcel.NewRow();
                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    string projid = ddlProject.SelectedValue.ToString();
                    DataTable dtAllProject = new DataTable();
                    object[] param1 = new object[3] { projid, "2", 'P' };

                    dtAllProject = cDataSrc.ExecuteDataSet("Survey_UspProjectSelect", param1, null).Tables[0];

                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);
                    row[3] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2);
                    row[4] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2);

                    index++;
                    rowIndex = 2 + index;
                    dtExcel.Rows.Add(row);
                }
            }
            catch
            {
                lbl_save_message.Text = "Please check your file data.";
                dtExcel = null;
            }
            app.Workbooks.Close();
            return dtExcel;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // 1.0.0.1.2 [Upload Previous Score from Excel File] <--
}

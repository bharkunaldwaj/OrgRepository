using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Admin_BAO;
using Questionnaire_BAO;
using Questionnaire_BE;
using DatabaseAccessUtilities;
using Miscellaneous;
using System.Configuration;
using Admin_BE;
//using CodeBehindBase;
public partial class Survey_Module_Register : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string uniqueId = Request.QueryString["LinkId"];
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(uniqueId))
            {
                Common_BAO objCommon_BAO = new Common_BAO();

                CNameValueList lstcname = new CNameValueList();
                lstcname.Add("@Operation", "GETLINK");
                lstcname.Add("@UniqueID", uniqueId);


                var dtExLink = objCommon_BAO.GetDataTable("Survey_UspExternalLink", lstcname);
                Session["DTEXLINK"] = dtExLink;
                if (dtExLink != null && dtExLink.Rows.Count > 0)
                {
                    Account_BAO account_BAO = new Account_BAO();
                    List<Account_BE> account_BEList = account_BAO.GetAccountByID(Convert.ToInt32(dtExLink.Rows[0]["AccountId"]));

                    if (account_BEList.Any() && !string.IsNullOrEmpty(account_BEList[0].CompanyLogo))
                    {
                        imgHeader.Visible = true;
                        imgHeader.ImageUrl = "~/UploadDocs/" + account_BEList[0].CompanyLogo;
                    }


                    //Set Programme logo
                   // DataTable dtCompany = new DataTable();
                    Survey_Company_BAO company_BAO = new Survey_Company_BAO();
                    List<Survey_Company_BE> dtCompany = company_BAO.GetCompanyByID(Convert.ToInt32(dtExLink.Rows[0]["CompanyID"]));
                    if (dtCompany != null && dtCompany.Count > 0)
                    {
                        Survey_Company_BE company = dtCompany.First();
                        if (company.QuestLogo != null && company.QuestLogo != "")
                        {
                            imgProjectLogo.Visible = true;
                            imgProjectLogo.ImageUrl = "~/UploadDocs/" + company.QuestLogo.ToString();
                        }
                        else
                        {
                            imgProjectLogo.Visible = false;
                        }
                    }


                    //Set Programme logo
                    
                        DataTable dtProgramme = new DataTable();
                        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
                        dtProgramme = programme_BAO.GetProgrammeByID(Convert.ToInt32(dtExLink.Rows[0]["ProgrammeId"]));
                        if (dtProgramme != null && dtProgramme.Rows.Count > 0)
                        {
                            if (dtProgramme.Rows[0]["Logo"].ToString() != "")
                            {
                                imgProjectLogo.Visible = true;
                                imgProjectLogo.ImageUrl = "~/UploadDocs/" + dtProgramme.Rows[0]["Logo"].ToString();
                            }
                            else
                            {
                                if (imgProjectLogo.ImageUrl == "")
                                    imgProjectLogo.Visible = false;
                            }
                        }

                    lblCompanyName.Text = Convert.ToString(dtExLink.Rows[0]["CompanyName"]);
                    lblQuestionName.Text = Convert.ToString(dtExLink.Rows[0]["QuestionName"]);
                    lblInst.Text = dtExLink.Rows[0]["Instructions"]!=null ?  Convert.ToString(dtExLink.Rows[0]["Instructions"]):"";
                    lblInst.Text = dtExLink.Rows[0]["Instructions"] != null ? Convert.ToString(dtExLink.Rows[0]["Instructions"]) : "";
                    lblAccount.Text = Convert.ToString(dtExLink.Rows[0]["AccountCode"]);
                    lblProject.Text = Convert.ToString(dtExLink.Rows[0]["ProjectName"]);
                    int progrmId = 0;
                    if (Int32.TryParse(Convert.ToString(dtExLink.Rows[0]["ProgrammeId"]), out progrmId))
                        fillAnalysis(progrmId);
                }


            }
        }

    }

    private void fillAnalysis(int ProgrammeID)
    {
        Common_BAO objCommon_BAO = new Common_BAO();
        CNameValueList lstcname1 = new CNameValueList();
        lstcname1.Add("@Operation", "GETANA");
        lstcname1.Add("@ProgrammeID", ProgrammeID);

        var dtAna = objCommon_BAO.GetDataTable("Survey_UspExternalLink", lstcname1);

        if (dtAna != null && dtAna.Rows.Count > 0)
        {
            DataView dvAna1 = dtAna.DefaultView;
            dvAna1.RowFilter = "Analysis_Type ='ANALYSIS- I'";

            //DataRow[] drAna1 = dtAna.Select("Analysis_Type ='ANALYSIS- I'");
            if (dvAna1 != null)
            {
                ddlAnalysis1.DataSource = dvAna1;
                ddlAnalysis1.DataValueField = "Analysis_Category_Id";
                ddlAnalysis1.DataTextField = "Category_Detail";
                ddlAnalysis1.DataBind();
            }

            // DataRow[] drAna2 = dtAna.Select("Analysis_Type ='ANALYSIS- II'");
            dvAna1.RowFilter = "Analysis_Type ='ANALYSIS- II'";
            if (dvAna1 != null && dvAna1.Count > 0)
            {
                ddlAnalysis2.DataSource = dvAna1;
                ddlAnalysis2.DataValueField = "Analysis_Category_Id";
                ddlAnalysis2.DataTextField = "Category_Detail";
                ddlAnalysis2.DataBind();
            }
            else
            {
                ddlAnalysis2.Visible = false;
                lblAnalysis2.Visible = false;
            }

            //DataRow[] drAna3 = dtAna.Select("Analysis_Type ='ANALYSIS- III'");
            dvAna1.RowFilter = "Analysis_Type ='ANALYSIS- III'";
            if (dvAna1 != null && dvAna1.Count>0)
            {
                ddlAnalysis3.DataSource = dvAna1;
                ddlAnalysis3.DataValueField = "Analysis_Category_Id";
                ddlAnalysis3.DataTextField = "Category_Detail";
                ddlAnalysis3.DataBind();
            }
            else
            {
                ddlAnalysis3.Visible = false;
                trAna3.Visible = false;
            }
        }
    }

    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (ddlAnalysis1.SelectedValue == "0" && ddlAnalysis2.SelectedValue == "0" && ddlAnalysis3.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key6", "alert(\"Please select at least one analysis.\");", true);
                return;
            }

            string uniqueId = Request.QueryString["LinkId"];
            Common_BAO objCommon_BAO = new Common_BAO();

            CNameValueList lstcname1 = new CNameValueList();
            lstcname1.Add("@Operation", "GETLINK");
            lstcname1.Add("@UniqueID", uniqueId);


            var dtExLink = objCommon_BAO.GetDataTable("Survey_UspExternalLink", lstcname1);
            if (dtExLink != null && dtExLink.Rows.Count > 0)
            {
                string questionnaireId = Convert.ToString(dtExLink.Rows[0]["QuestionnaireID"]);

                CNameValueList lstcname = new CNameValueList();
                lstcname.Add("@Operation", "UPPARTIC");
                lstcname.Add("@AccountID", Convert.ToInt32(dtExLink.Rows[0]["AccountID"]));
                lstcname.Add("@QuestionnaireID", Convert.ToInt32(dtExLink.Rows[0]["QuestionnaireID"]));
                lstcname.Add("@ProgrammeID", Convert.ToInt32(dtExLink.Rows[0]["ProgrammeID"]));
                lstcname.Add("@ProjectID", Convert.ToInt32(dtExLink.Rows[0]["ProjectID"]));
                lstcname.Add("@Analysis1", ddlAnalysis1.SelectedItem.Text);
                lstcname.Add("@Analysis2", ddlAnalysis2.SelectedItem.Text);
                lstcname.Add("@Analysis3", ddlAnalysis3.SelectedItem.Text);
                lstcname.Add("@CandidateName", txtName.Text);
                lstcname.Add("@CandidateEmail", txtEmailAddress.Text);
                lstcname.Add("@SubmitFlag", false);
                lstcname.Add("@EmailSendFlag", dtExLink.Rows[0]["SendEmailOnCompletion"] = true ? 1:0);

                Session["ParticpantEmail"] = txtEmailAddress.Text;
                Session["ParticpantName"] = txtName.Text;
                string candidateID = Convert.ToString(objCommon_BAO.InsertAndUpdate("Survey_UspExternalLink", lstcname));

                questionnaireId = PasswordGenerator.EnryptString(questionnaireId);
                candidateID = PasswordGenerator.EnryptString(candidateID);

                string urlPath = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();
                string redirectpath = urlPath + "Feedback.aspx?QID=" + questionnaireId + "&CID=" + candidateID;
                Response.Redirect(redirectpath,false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ddlAnalysis1.SelectedIndex = -1;
        ddlAnalysis2.SelectedIndex = -1;
        ddlAnalysis3.SelectedIndex = -1;
        txtEmailAddress.Text = "";
        txtName.Text = "";
    }
}

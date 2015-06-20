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
//using CodeBehindBase;
public partial class Survey_Module_Question_range : CodeBehindBase
{

    //Survey_Question_Range_BAO 
    Questionnaire_BAO.Programme_BAO programme_BAO = new Questionnaire_BAO.Programme_BAO();
    Questionnaire_BAO.Survey_Question_Range_BAO prog_bao = new Survey_Question_Range_BAO();

    // Questionnaire_BAO
    Project_BE project_BE = new Project_BE();
    public Programme_BE programme_range;
    WADIdentity identity;
    DataTable dtCompanyName;

    protected void Page_Load(object sender, EventArgs e)
    {

                if (Request.QueryString["Mode"] == "V")
        {
            imbSubmit.Visible = false;
            txtTo.Visible = false;
            Label3.Visible = false;
          hidespan.Visible=false;
            Session["Mode"] = "V";
            int r_ID = Convert.ToInt32(Request.QueryString["RangeId"]);
            Session["RangeID"] = r_ID;

            Survey_Question_Range_BAO sur_edit_range = new Survey_Question_Range_BAO();
            DataTable edit_range = sur_edit_range.get_range_detail(r_ID);
            txtName.Text = edit_range.Rows[0][1].ToString();
            txtTitle.Text = edit_range.Rows[0][2].ToString();
            txtTo.Text = edit_range.Rows[0][3].ToString();
            rptrCandidateList.DataSource = edit_range;
            rptrCandidateList.Visible = true;
          //  imbAssign.Visible = false;
            ImgBtn_Rset.Visible = false;

            //rptrCandidateList.DataMember = edit_range.Columns["Rating_Text"].ToString();
            //rptrCandidateList.
            rptrCandidateList.DataBind();
            // TextBox tb = rptrCandidateList.FindControl("Rating_TextBox") as TextBox;

            //foreach (tb t in rptrCandidateList)
            //{ 

            //}

            // tb.Enabled = false;
        }
        else
        {
            Session["Mode"] = "I";
            imbAssign.Visible = true;
            ImgBtn_Rset.Visible = true;
            imbBack.Visible = false;



            Label ll = (Label)this.Master.FindControl("Current_location");
            ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            if (!IsPostBack)
            {
                rptrCandidateList.Visible = false;
            }

            //try
            //{
            identity = this.Page.User.Identity as WADIdentity;
            ////////////  Account_BAO account_BAO = new Account_BAO();
            ///////////// ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ////////////// ddlAccountCode.DataValueField = "AccountID";
            /////////// //ddlAccountCode.DataTextField = "Code";
            ///////////////  ddlAccountCode.DataBind();

            if (!IsPostBack)
            {
                //    Project_BAO project_BAO = new Project_BAO();
                //    ddlproject.DataSource = project_BAO.GetAccProject(Convert.ToInt32(identity.User.AccountID));
                //    ddlproject.DataValueField = "ProjectID";
                //    ddlproject.DataTextField = "Title";
                //    ddlproject.DataBind();

                ////////////////////if (identity.User.GroupID == 1)
                ////////////////////{
                ////////////////////    //divAccount.Visible = true;
                ////////////////////    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                ////////////////////    ddlAccountCode_SelectedIndexChanged(sender, e);
                ////////////////////}
                //else
                //{
                //    divAccount.Visible = false;
                //}

                ViewState["ProjectID"] = "0";
                ViewState["Programme"] = "";


            }

            //HandleWriteLog("Start", new StackTrace(true));
            //////}
            //////catch (Exception ex)
            //////{
            ////// //   HandleException(ex);
            //////}



        }
    }
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            lblMessage.Text = "";
            Question_Range_BE QR_BE = new Question_Range_BE();
            QR_BE.RangeID =0;
            QR_BE.name = "";
            QR_BE.title = "";
            QR_BE.r_upto = 0;
            QR_BE.rating_text = "";

            TextBox rptr_score_ratings = null;
            string rangeList = "";



            int ff = 0;
            int ii = rptrCandidateList.Items.Count;




            foreach (RepeaterItem item in rptrCandidateList.Items)
            {
                rptr_score_ratings = (TextBox)item.FindControl("Rating_TextBox");


                if (rptr_score_ratings.Text == "")
                    rptr_score_ratings.Text = "sorry";


                rangeList = rangeList + rptr_score_ratings.Text + ",";
            }

            if (Session["Mode"] == "V")
            {
                QR_BE.RangeID = Convert.ToInt32(Session["RangeID"]);
            }
            else
            {
                QR_BE.RangeID = -1;
            }
            QR_BE.name = txtName.Text;
            QR_BE.title = txtTitle.Text;
            QR_BE.r_upto = Convert.ToInt32(txtTo.Text);
            QR_BE.rating_text = rangeList;
            
            if (Session["Mode"] == "I")
            {
                int result = prog_bao.insert_range(QR_BE,"I");
            }
            else
            {
                int result = prog_bao.insert_range(QR_BE,"V");
            
            }
            rptrCandidateList.Visible = false;
            txtName.Text = txtTitle.Text = txtTo.Text = "";
            // }
            //   if (ff == ii)
           // Response.Redirect(@"\feedback360\Survey_Module\Question_rangeList.aspx");
            Response.Redirect("Question_rangeList.aspx");




        }
    }
        //}
            //catch (Exception ex)
            //{
            //   // HandleException(ex);
            //}
        
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {

        int x = 0;
        x= chk_rangeName_Availability();
        if (x >= 1 && Session["Mode"] != "V")
        {
            Label4.Visible = true;
            Label4.Text = "This Range Name is already taken.";
        }
        else
        {
            Label4.Visible = false;
            rptrCandidateList.Visible = true;
            if (txtTo.Text.Trim() != "")
                BindCandidateList(Convert.ToInt32(txtTo.Text.Trim()));
        }




    }



    public int chk_rangeName_Availability()
    {
        int x = prog_bao.chk_rng_Availability(txtName.Text);
        return x;
    }


    private void BindCandidateList(int candidateCount)
    {
        try
        {
            DataTable dtCandidate = new DataTable();
            dtCandidate.Columns.Add("Rating_Text");

            for (int count = 0; count < candidateCount; count++)
                dtCandidate.Rows.Add("");

            rptrCandidateList.DataSource = dtCandidate;
            rptrCandidateList.DataBind();



        }
        catch (Exception ex)
        {
            //  HandleException(ex);
        }
    }
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Project_BAO project_BAO = new Project_BAO();
        ////////ddlproject.Items.Clear();
        ////////ddlproject.Items.Insert(0, new ListItem("Select", "0"));

        //if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        //{
        //    Account_BAO account_BAO = new Account_BAO();

        //    dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
        //    DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
        //    DataTable dtAccount = dtCompanyName.Clone();
        //    foreach (DataRow drAccount in resultsAccount)
        //        dtAccount.ImportRow(drAccount);

        //    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

        //    //odsProgramme.SelectParameters.Clear();
        //    //odsProgramme.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue);
        //    //odsProgramme.Select();

        //    //ManagePaging();
        //    ViewState["AccountID"] = ddlAccountCode.SelectedValue;

        //    ////////ddlproject.DataSource = project_BAO.GetAccProject(Convert.ToInt32(ddlAccountCode.SelectedValue));
        //    ////////ddlproject.DataValueField = "ProjectID";
        //    ////////ddlproject.DataTextField = "Title";
        //    ////////ddlproject.DataBind();
        //}
        //else
        //{
        //    lblcompanyname.Text = "";
        //    ViewState["AccountID"] = "0";
        //    //odsProgramme.SelectParameters.Clear();
        //    //odsProgramme.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
        //    //odsProgramme.Select();

        //    //ManagePaging();

        //    //////ddlproject.DataSource = project_BAO.GetAccProject(Convert.ToInt32(identity.User.AccountID));
        //    //////ddlproject.DataValueField = "ProjectID";
        //    //////ddlproject.DataTextField = "Title";
        //    //////ddlproject.DataBind();
        //}
    }



    protected void ImgBtn_Rset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtName.Text = txtTo.Text = txtTitle.Text = "";
            rptrCandidateList.DataSource = null;
            rptrCandidateList.DataBind();
        }
        catch (Exception ex)
        {

        }
    }
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Question_rangeList.aspx");
    }
}
  
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BAO;
using Questionnaire_BE;
//using CodeBehindBase;
public partial class Survey_Module_Question_range : CodeBehindBase
{
    //Global variables
    //Survey_Question_Range_BAO 
    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
    Survey_Question_Range_BAO questionRangeBusinessAccessObject = new Survey_Question_Range_BAO();

    // Questionnaire_BAO
    //Project_BE projectBusinesEntity = new Project_BE();
    public Programme_BE programmeRangeBusinesEntity;
    WADIdentity identity;
    DataTable dataTableCompanyName;

    protected void Page_Load(object sender, EventArgs e)
    {
        //If  query string Mode="V" Then Edit else View then hide show controls accordingly. 
        if (Request.QueryString["Mode"] == "V")
        {
            imbSubmit.Visible = false;
            txtTo.Visible = false;
            Label3.Visible = false;
            hidespan.Visible = false;
            Session["Mode"] = "V";
            //Get range id from query string.
            int rangeId = Convert.ToInt32(Request.QueryString["RangeId"]);
            Session["RangeID"] = rangeId;

            Survey_Question_Range_BAO editRangeBusinessAccessObject = new Survey_Question_Range_BAO();
            //Get all question range by eange id.
            DataTable dataTableRange = editRangeBusinessAccessObject.get_range_detail(rangeId);
            //Then bind controls fron database value.
            txtName.Text = dataTableRange.Rows[0][1].ToString();
            txtTitle.Text = dataTableRange.Rows[0][2].ToString();
            txtTo.Text = dataTableRange.Rows[0][3].ToString();
            rptrCandidateList.DataSource = dataTableRange;
            rptrCandidateList.Visible = true;
            //  imbAssign.Visible = false;
            ImgBtn_Rset.Visible = false;

            //rptrCandidateList.DataMember = edit_range.Columns["Rating_Text"].ToString();
            //rptrCandidateList.
            rptrCandidateList.DataBind();
            // TextBox tb = rptrCandidateList.FindControl("Rating_TextBox") as TextBox;
        }
        else
        {
            Session["Mode"] = "I";
            imbAssign.Visible = true;
            ImgBtn_Rset.Visible = true;
            imbBack.Visible = false;

            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

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

    /// <summary>
    /// Save the record in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate();

        if (Page.IsValid)
        {
            lblMessage.Text = "";
            Question_Range_BE questionRangeBusinesEntity = new Question_Range_BE();
            questionRangeBusinesEntity.RangeID = 0;
            questionRangeBusinesEntity.name = "";
            questionRangeBusinesEntity.title = "";
            questionRangeBusinesEntity.r_upto = 0;
            questionRangeBusinesEntity.rating_text = "";

            TextBox textBoxScoreRatings = null;
            string rangeList = "";

            int ff = 0;
            int ii = rptrCandidateList.Items.Count;

            foreach (RepeaterItem item in rptrCandidateList.Items)
            {
                textBoxScoreRatings = (TextBox)item.FindControl("Rating_TextBox");


                if (textBoxScoreRatings.Text == "")
                    textBoxScoreRatings.Text = "sorry";


                rangeList = rangeList + textBoxScoreRatings.Text + ",";
            }
            //If query string Mode="V" edit then range id from session.
            if (Session["Mode"] == "V")
            {
                questionRangeBusinesEntity.RangeID = Convert.ToInt32(Session["RangeID"]);
            }
            else
            {
                questionRangeBusinesEntity.RangeID = -1;//else default value as -1.
            }
            questionRangeBusinesEntity.name = txtName.Text;
            questionRangeBusinesEntity.title = txtTitle.Text;
            questionRangeBusinesEntity.r_upto = Convert.ToInt32(txtTo.Text);
            questionRangeBusinesEntity.rating_text = rangeList;

            //if Mode is I then Inser question Range else update.
            if (Session["Mode"] == "I")
            {
                int result = questionRangeBusinessAccessObject.insert_range(questionRangeBusinesEntity, "I");//Insert range.
            }
            else
            {
                int result = questionRangeBusinessAccessObject.insert_range(questionRangeBusinesEntity, "V");//UPdate Range.

            }

            rptrCandidateList.Visible = false;
            txtName.Text = txtTitle.Text = txtTo.Text = "";
            // }
            //   if (ff == ii)
            // Response.Redirect(@"\feedback360\Survey_Module\Question_rangeList.aspx");
            Response.Redirect("Question_rangeList.aspx");
        }
    }
      
    /// <summary>
    /// Bind range repeator.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        int x = 0;
        x= CheckRangeNameAvailability();

        if (x >= 1 && Session["Mode"] != "V")//Mode view
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

    /// <summary>
    /// Check that entered range name exiet or not.
    /// </summary>
    /// <returns></returns>
    public int CheckRangeNameAvailability()
    {
        int x = questionRangeBusinessAccessObject.chk_rng_Availability(txtName.Text);
        return x;
    }

    /// <summary>
    /// Bind range grid with default blank columns.
    /// </summary>
    /// <param name="candidateCount"></param>
    private void BindCandidateList(int candidateCount)
    {
        try
        {
            DataTable dataTableCandidate = new DataTable();
            dataTableCandidate.Columns.Add("Rating_Text");

            for (int count = 0; count < candidateCount; count++)
                dataTableCandidate.Rows.Add("");

            rptrCandidateList.DataSource = dataTableCandidate;
            rptrCandidateList.DataBind();
        }
        catch (Exception ex)
        {
            //  HandleException(ex);
        }
    }

    /// <summary>
    /// Its of No use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Reset controls value to default.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Redirect to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Question_rangeList.aspx");
    }
}
  
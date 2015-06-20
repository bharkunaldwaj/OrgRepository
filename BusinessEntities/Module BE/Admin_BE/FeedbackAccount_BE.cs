using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using feedbackFramework_BE;

namespace Admin_BE
{
    [Serializable]
    public class FeedbackAccount_BE:BE_Base
    {
        public int? AccountID { get; set; }
        public String Code { get; set; }
        public String LoginID { get; set; }
        public String Password { get; set; }
        public String OrganisationName { get; set; }
        public int? AccountTypeID { get; set; }
        public String Description { get; set; }
        public String EmailID { get; set; }
        public String Website { get; set; }
        public int? StatusID { get; set; }
        public String CompanyLogo { get; set; }
        public String CopyRightLine { get; set; }
        public String HeaderBGColor { get; set; }
        public String MenuBGColor { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }

        public FeedbackAccount_BE()
        {
            this.AccountID = null;
            this.Code = String.Empty;
            this.LoginID = String.Empty;
            this.Password = String.Empty;
            this.OrganisationName = String.Empty;
            this.AccountTypeID = null;
            this.Description = String.Empty;
            this.EmailID = String.Empty;
            this.Website = String.Empty;
            this.StatusID = null;
            this.CompanyLogo = String.Empty;
            this.CopyRightLine = String.Empty;
            this.HeaderBGColor = String.Empty;
            this.MenuBGColor = String.Empty;
            this.ModifyBy = null;
            this.ModifyDate = null;
            this.IsActive = null;
        } 
    }
}

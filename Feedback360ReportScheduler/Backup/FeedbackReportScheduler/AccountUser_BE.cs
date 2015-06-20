using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using feedbackFramework_BE;

namespace Admin_BE
{
    [Serializable]
    public class AccountUser_BE //: BE_Base
    {
        public int? UserID { get; set; }
        public String LoginID { get; set; }
        public String Password { get; set; }
        public int? GroupID { get; set; }
        public int? AccountID { get; set; }
        public int? StatusID { get; set; }
        public String Salutation { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EmailID { get; set; }
        public bool? Notification { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }
        public string Code { get; set; }

        public AccountUser_BE()
        {
            this.UserID = null ;
            this.LoginID = String.Empty ;
            this.Password = String.Empty ;
            this.GroupID = null ;
            this.AccountID = null ;
            this.StatusID = null ;
            this.Salutation = String.Empty ;
            this.FirstName = String.Empty ;
            this.LastName = String.Empty ;
            this.EmailID = String.Empty ;
            this.Notification = null ;
            this.ModifyBy = null ;
            this.ModifyDate = null ;
            this.IsActive = null ;
            this.Code = null;
        } 
    }
}

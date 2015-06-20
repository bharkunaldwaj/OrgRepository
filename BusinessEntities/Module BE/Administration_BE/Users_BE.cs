/*  
* PURPOSE: This is the Business Entity for User
* AUTHOR: Ashish Dhar
* Date Of Creation: 29/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Administration_BE {
    [Serializable]
    public class User_BE : feedbackFramework_BE.BE_Base
    {
        #region "Public Properties"

        public int? UserID { get; set; }
        public int? GroupID { get; set; }
        public String LoginID { get; set; }
        public String UserCode { get; set; }
        public String Password { get; set; }
        public String FName { get; set; }
        public String MName { get; set; }
        public String LName { get; set; }
        public String Email { get; set; }
        public bool? IsActive { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public int? CountryID { get; set; }
        public String Zip { get; set; }
        public String TelNumber { get; set; }
        public String FaxNumber { get; set; }
        public String Website { get; set; }
        public String Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsConfirmed { get; set; }
        public String Type { get; set; }
        public String BPNumber { get; set; }

        public bool? IsOnline { get; set; }
        public DateTime? LastActionTime { get; set; }

        public Group_BE PKGroup_BE { get; set; }
        public Country_BE PKCountry_BE { get; set; }

        public int? AccountID { get; set; }
        public String AccountCode { get; set; }
        public String AccountName { get; set; }
        public String CompanyLogo { get; set; }
        public String HeaderBGColor { get; set; }
        public String MenuBGColor { get; set; }
        public String CopyRightLine { get; set; }
        public String SessionData { get; set; }

        #endregion

        #region Public Constructor

        public User_BE()
        {
            this.UserID = null;
            this.GroupID = null;
            this.LoginID = null;
            this.UserCode = null;
            this.Password = null;
            this.FName = null;
            this.MName = null;
            this.LName = null;
            this.Email = null;
            this.IsActive = null;
            this.Address1 = null;
            this.Address2 = null;
            this.City = null;
            this.State = null;
            this.CountryID = null;
            this.Zip = null;
            this.TelNumber = null;
            this.FaxNumber = null;
            this.Website = null;
            this.Note = null;
            this.CreatedDate = null;
            this.ModifiedDate = null;
            this.IsConfirmed = null;
            this.Type = null;
            this.BPNumber = null;

            this.IsOnline = null;
            this.LastActionTime = null;

            this.PKGroup_BE = new Group_BE();
            this.PKCountry_BE = new Country_BE();

            this.AccountID = null;
            this.AccountCode = null;
            this.AccountName = null;
            this.CompanyLogo = null;
            this.HeaderBGColor = null;
            this.MenuBGColor = null;
            this.CopyRightLine = null;
        }

        #endregion
    }
}

/*  
* PURPOSE: This is the Business Entity for Contact
* AUTHOR: Ashish Dhar
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedbackFramework_BE;

namespace Administration_BE {
    [Serializable]
    public class Contact_BE : feedbackFramework_BE.BE_Base
    {
        #region "Public Properties"

        public int? ContactID { get; set; }
        public int? UserID { get; set; }
        public String BPNumber { get; set; }
        public String SAPBPShipTo { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public int? CountryID { get; set; }
        public String Zip { get; set; }
        public String TelNumber { get; set; }
        public String FaxNumber { get; set; }
        public bool? IsActive { get; set; }
        public int? ContactTypeID { get; set; }
        public bool? IsDefault { get; set; }

        public User_BE PKUser_BE { get; set; }
        public Country_BE PKCountry_BE { get; set; }

        //Property to maintain status of the record
        public enum statusType { New, Update, Delete }
        public statusType Status { get; set; }
        public int? SNo { get; set; }

        #endregion

        #region "Public Constructor"

        public Contact_BE() {
            this.ContactID = null;
            this.UserID = null;
            this.BPNumber = null;
            this.SAPBPShipTo = null;
            this.Name = null;
            this.Email = null;
            this.Address1 = null;
            this.Address2 = null;
            this.City = null;
            this.State = null;
            this.CountryID = null;
            this.Zip = null;
            this.TelNumber = null;
            this.FaxNumber = null;
            this.IsActive = null;
            this.ContactTypeID = null;
            this.IsDefault = null;
            
            this.PKUser_BE = new User_BE();
            this.PKCountry_BE = new Country_BE();

        #endregion
        }
    }
}

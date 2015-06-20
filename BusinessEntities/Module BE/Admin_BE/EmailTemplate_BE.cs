#region Creation
/*  
* PURPOSE: This is the Business Entity for Contact
* AUTHOR: Ashish Dhar
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

#region Module

namespace Admin_BE
{
    [Serializable]
    public class EmailTemplate_BE : feedbackFramework_BE.BE_Base
    {
        #region Public Properties
        public int? EmailTemplateID { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Subject { get; set; }
        public String EmailText { get; set; }
        public String EmailImage { get; set; }
        public int? AccountID { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }
        #endregion

        #region Public Constructor
        public EmailTemplate_BE()
        {
            this.EmailTemplateID = null;
            this.Title = String.Empty;
            this.Description = String.Empty;
            this.Subject = String.Empty;
            this.EmailText = String.Empty;
            this.EmailImage = String.Empty;
            this.ModifyBy = null;
            this.AccountID = null;
            this.ModifyDate = null;
            this.IsActive = null;
        }
        #endregion
    }









    [Serializable]
    public class Survey_EmailTemplate_BE : feedbackFramework_BE.Survey_BE_Base
    {
        #region Public Properties
        public int? EmailTemplateID { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Subject { get; set; }
        public String EmailText { get; set; }
        public String EmailImage { get; set; }
        public int? AccountID { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }
        #endregion

        #region Public Constructor
        public Survey_EmailTemplate_BE()
        {
            this.EmailTemplateID = null;
            this.Title = String.Empty;
            this.Description = String.Empty;
            this.Subject = String.Empty;
            this.EmailText = String.Empty;
            this.EmailImage = String.Empty;
            this.ModifyBy = null;
            this.AccountID = null;
            this.ModifyDate = null;
            this.IsActive = null;
        }
        #endregion
    }


}
#endregion
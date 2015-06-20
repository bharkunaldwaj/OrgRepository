/*  
* PURPOSE: This is the Business Entity for Group
* AUTHOR: Rajesh Kumar
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
    public class Group_BE : feedbackFramework_BE.BE_Base
    {

        #region "Public Properties"

        public int? GroupID { get; set; }
        public String GroupName { get; set; }
        public String Description { get; set; }
        public String WelcomeText { get; set; }
        public String NewsText { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region "Public Constructor"

        public Group_BE()
        {
            GroupID = null;
            GroupName = null;
            Description = null;
            WelcomeText = null;
            NewsText = null;
            IsActive = null;
            CreatedDate = null;
            ModifiedDate = null;
        }

        #endregion
    }


    [Serializable]
    public class Survey_Group_BE : feedbackFramework_BE.Survey_BE_Base
    {

        #region "Public Properties"

        public int? GroupID { get; set; }
        public String GroupName { get; set; }
        public String Description { get; set; }
        public String WelcomeText { get; set; }
        public String NewsText { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region "Public Constructor"

        public Survey_Group_BE()
        {
            GroupID = null;
            GroupName = null;
            Description = null;
            WelcomeText = null;
            NewsText = null;
            IsActive = null;
            CreatedDate = null;
            ModifiedDate = null;
        }

        #endregion
    }



}

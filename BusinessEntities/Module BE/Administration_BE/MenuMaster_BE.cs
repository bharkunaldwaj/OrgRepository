/*    
* PURPOSE: This is the Business Entity for MenuMaster
* AUTHOR: Rajesh Kumar
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

namespace Administration_BE {
    [Serializable]
    public class MenuMaster_BE : feedbackFramework_BE.BE_Base
    {

        #region "Public Properties"

        public int? MenuID { get; set; }
        public String Name { get; set; }
        public String Page { get; set; }
        public String LinkedPage { get; set; }
        public int? ParentID { get; set; }
        public String Visibility { get; set; }
        public bool? IsActive { get; set; }
        public int? SortOrder { get; set; }
        public String ADEVFlag { get; set; }
        
        #endregion

        #region "Public Constructor"

        public MenuMaster_BE()
        {
            MenuID = null;
            Name = null;
            Page = null;
            LinkedPage = null;
            ParentID = null;
            Visibility = null;
            IsActive = null;
            SortOrder = null;
            ADEVFlag = null;
        }

        #endregion
    }







    [Serializable]
    public class Survey_MenuMaster_BE : feedbackFramework_BE.Survey_BE_Base
    {

        #region "Public Properties"

        public int? MenuID { get; set; }
        public String Name { get; set; }
        public String Page { get; set; }
        public String LinkedPage { get; set; }
        public int? ParentID { get; set; }
        public String Visibility { get; set; }
        public bool? IsActive { get; set; }
        public int? SortOrder { get; set; }
        public String ADEVFlag { get; set; }

        #endregion

        #region "Public Constructor"

        public Survey_MenuMaster_BE()
        {
            MenuID = null;
            Name = null;
            Page = null;
            LinkedPage = null;
            ParentID = null;
            Visibility = null;
            IsActive = null;
            SortOrder = null;
            ADEVFlag = null;
        }

        #endregion
    }

















}


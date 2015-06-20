/*  
* PURPOSE: This is the Business Entity for GroupRight
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
    public class GroupRight_BE : feedbackFramework_BE.BE_Base
    {
       
        private Group_BE fkgroup_BE = new Group_BE();
        private MenuMaster_BE fkmenuMaster_BE = new MenuMaster_BE();
        
        #region "Public Properties"

        public int? GroupRightID { get; set; }
        public int? GroupID { get; set; }
        public int? MenuID { get; set; }
        public String AccessRights { get; set; }
        public String ADEVFlag { get; set; }

        public int? ParentId { get; set; }


        public Group_BE FKGroup_BE
        {
            get { return fkgroup_BE; }
            set { fkgroup_BE = value; }
        }

        public MenuMaster_BE FKMenuMaster_BE
        {
            get { return fkmenuMaster_BE; }
            set { fkmenuMaster_BE = value; }
        }

        #endregion
        
        //public Group_BE FKGroup_BE { get; set; }
        //public MenuMaster_BE FKMenuMaster_BE { get; set; }

        #region "Public Constructor"

        public GroupRight_BE()
        {
            GroupID = null;
            MenuID = null;
            AccessRights = null;
            ADEVFlag = null;
            ParentId = null;

            //FKGroup_BE = null;
            //FKMenuMaster_BE = null;
        }

        #endregion
    }


    [Serializable]
    public class Survey_GroupRight_BE : feedbackFramework_BE.Survey_BE_Base
    {

        private Survey_Group_BE fkgroup_BE = new Survey_Group_BE();
        private Survey_MenuMaster_BE fkmenuMaster_BE = new Survey_MenuMaster_BE();

        #region "Public Properties"

        public int? GroupRightID { get; set; }
        public int? GroupID { get; set; }
        public int? MenuID { get; set; }
        public String AccessRights { get; set; }
        public String ADEVFlag { get; set; }
        public String ParentId { get; set; }

        public Survey_Group_BE FKGroup_BE
        {
            get { return fkgroup_BE; }
            set { fkgroup_BE = value; }
        }

        public Survey_MenuMaster_BE FKMenuMaster_BE
        {
            get { return fkmenuMaster_BE; }
            set { fkmenuMaster_BE = value; }
        }

        #endregion

        //public Group_BE FKGroup_BE { get; set; }
        //public MenuMaster_BE FKMenuMaster_BE { get; set; }

        #region "Public Constructor"

        public Survey_GroupRight_BE()
        {
            GroupID = null;
            MenuID = null;
            AccessRights = null;

            ADEVFlag = null;
            ParentId = null;


            //FKGroup_BE = null;
            //FKMenuMaster_BE = null;
        }

        #endregion
    }

}

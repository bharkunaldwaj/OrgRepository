/*  
* PURPOSE: This is the Business Entity for FieldRight
* AUTHOR: 
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
    public class FieldRight_BE : feedbackFramework_BE.BE_Base
    {
        #region "Public Properties"

        public int? FRID { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
        public int? SortOrder { get; set; }
        public int? IsActive { get; set; }
        public String MenuID { get; set; }

        #endregion

        #region "Public Constructor"

        public FieldRight_BE()
        {
            this.FRID = null;
            this.Name = null;
            this.Value = null;
            this.SortOrder = null;
            this.IsActive = null;
            this.MenuID = null;
        }

        #endregion
    }












    [Serializable]
    public class Survey_FieldRight_BE : feedbackFramework_BE.BE_Base
    {
        #region "Public Properties"

        public int? FRID { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
        public int? SortOrder { get; set; }
        public int? IsActive { get; set; }
        public String MenuID { get; set; }

        #endregion

        #region "Public Constructor"

        public Survey_FieldRight_BE()
        {
            this.FRID = null;
            this.Name = null;
            this.Value = null;
            this.SortOrder = null;
            this.IsActive = null;
            this.MenuID = null;
        }

        #endregion
    }










}

/*  
* PURPOSE: This is the Base Class for all Business Entity classes
* AUTHOR: Manish Mathur
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

namespace feedbackFramework_BE {
    [Serializable]
    public class BE_Base {
        #region "Public Properties"

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public String SortOrder { get; set; }

        #endregion


        #region Public Constructor

        public BE_Base() {
            this.PageIndex = null;
            this.PageSize = null;
            this.SortOrder = string.Empty;
        }

        #endregion
    }







    [Serializable]
    public class Survey_BE_Base
    {
        #region "Public Properties"

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public String SortOrder { get; set; }

        #endregion


        #region Public Constructor

        public Survey_BE_Base()
        {
            this.PageIndex = null;
            this.PageSize = null;
            this.SortOrder = string.Empty;
        }

        #endregion
    }



    }

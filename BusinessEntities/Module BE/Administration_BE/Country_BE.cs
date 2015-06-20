/*  
* PURPOSE: This is the Business Entity for Country
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

namespace Administration_BE {
    [Serializable]
    public class Country_BE : feedbackFramework_BE.BE_Base
    {
        #region "Public Properties"

        public int? CountryID { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }

        #endregion

        #region Public Constructor

        public Country_BE() {
            this.CountryID = null;
            this.Code = null;
            this.Name = null;
        }

        #endregion
    }
}

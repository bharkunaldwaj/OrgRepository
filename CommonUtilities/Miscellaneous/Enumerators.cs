/*
* PURPOSE: Enumerator class to assign enumerated values to all layers
* AUTHOR:  Manish Mathur
* Date Of Creation: <31/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miscellaneous
{
    public static class Enumerators
    {
        #region Public Methods

        /// <summary>
        /// Function to return enumerated value of a layer for logging
        /// </summary>
        public enum LogLevel
        {
            UI = 1,
            BAO = 2,
            DAO = 3,
            DAU = 4
        }
        #endregion

    }
}

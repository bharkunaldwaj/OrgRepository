/*   
* PURPOSE: This is the Business Access Object for Menu Master Entity
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
using System.Diagnostics;

using DAF_BAO;

using Administration_BE;
using Administration_DAO;

namespace Administration_BAO {
    public class MenuMaster_BAO : Base_BAO {

        #region "Business Logic for Menu Master BAO"

        /// <summary>
        /// This Method returns the Menus
        /// </summary>
        /// <param name="p_menuMaster_BE"></param>
        /// <returns></returns>
        public List<MenuMaster_BE> GetMenuMaster(MenuMaster_BE p_menuMaster_BE) {
            List<MenuMaster_BE> MenuMaster_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));
                MenuMaster_DAO MenuMaster_DAO = new MenuMaster_DAO();

                MenuMaster_DAO.GetMenuMaster(p_menuMaster_BE);

                MenuMaster_BEList = MenuMaster_DAO.MenuMaster_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return MenuMaster_BEList;
        }

        /// <summary>
        /// This Method returns the Parent Menu
        /// </summary>
        /// <param name="p_menuMaster_BE"></param>
        public List<MenuMaster_BE> GetMenuParent(List<MenuMaster_BE> p_menuMaster_BE) {
            MenuMaster_DAO menuMaster_DAO = new MenuMaster_DAO();
            try {
                HandleWriteLog("Start", new StackTrace(true));
                menuMaster_DAO = new MenuMaster_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return menuMaster_DAO.GetMenuParent(p_menuMaster_BE);
        }

        #endregion
    }


























    public class Survey_MenuMaster_BAO : Base_BAO
    {

        #region "Business Logic for Menu Master BAO"

        /// <summary>
        /// This Method returns the Menus
        /// </summary>
        /// <param name="p_menuMaster_BE"></param>
        /// <returns></returns>
        public List<Survey_MenuMaster_BE> GetMenuMaster(Survey_MenuMaster_BE p_menuMaster_BE)
        {
            List<Survey_MenuMaster_BE> MenuMaster_BEList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Survey_MenuMaster_DAO MenuMaster_DAO = new Survey_MenuMaster_DAO();

                MenuMaster_DAO.GetMenuMaster(p_menuMaster_BE);

                MenuMaster_BEList = MenuMaster_DAO.MenuMaster_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return MenuMaster_BEList;
        }

        /// <summary>
        /// This Method returns the Parent Menu
        /// </summary>
        /// <param name="p_menuMaster_BE"></param>
        public List<Survey_MenuMaster_BE> GetMenuParent(List<Survey_MenuMaster_BE> p_menuMaster_BE)
        {
            Survey_MenuMaster_DAO menuMaster_DAO = new Survey_MenuMaster_DAO();
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                menuMaster_DAO = new Survey_MenuMaster_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return menuMaster_DAO.GetMenuParent(p_menuMaster_BE);
        }

        #endregion
    }












}

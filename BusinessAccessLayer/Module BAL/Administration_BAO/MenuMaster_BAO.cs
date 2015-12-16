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
using System.Diagnostics;

using DAF_BAO;

using Administration_BE;
using Administration_DAO;

namespace Administration_BAO
{
    public class MenuMaster_BAO : Base_BAO
    {

        #region "Business Logic for Menu Master BAO"

        /// <summary>
        /// This Method returns the Menus
        /// </summary>
        /// <param name="menuMasterBusinessEntity"></param>
        /// <returns></returns>
        public List<MenuMaster_BE> GetMenuMaster(MenuMaster_BE menuMasterBusinessEntity)
        {
            List<MenuMaster_BE> MenuMasterBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                MenuMaster_DAO MenuMasterDataAccessObject = new MenuMaster_DAO();

                MenuMasterDataAccessObject.GetMenuMaster(menuMasterBusinessEntity);

                MenuMasterBusinessEntityList = MenuMasterDataAccessObject.MenuMasterBusinessEntityList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return MenuMasterBusinessEntityList;
        }

        /// <summary>
        /// This Method returns the Parent Menu
        /// </summary>
        /// <param name="menuMasterBusinessEntity"></param>
        public List<MenuMaster_BE> GetMenuParent(List<MenuMaster_BE> menuMasterBusinessEntity)
        {
            MenuMaster_DAO menuMasterDataAccessObject = new MenuMaster_DAO();
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                menuMasterDataAccessObject = new MenuMaster_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return menuMasterDataAccessObject.GetMenuParent(menuMasterBusinessEntity);
        }

        #endregion
    }

    public class Survey_MenuMaster_BAO : Base_BAO
    {
        #region "Business Logic for Menu Master BAO"

        /// <summary>
        /// This Method returns the Menus
        /// </summary>
        /// <param name="menuMasterBusinessEntity"></param>
        /// <returns></returns>
        public List<Survey_MenuMaster_BE> GetMenuMaster(Survey_MenuMaster_BE menuMasterBusinessEntity)
        {
            List<Survey_MenuMaster_BE> MenuMasterBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Survey_MenuMaster_DAO MenuMasterDataAccessObject = new Survey_MenuMaster_DAO();

                MenuMasterDataAccessObject.GetMenuMaster(menuMasterBusinessEntity);

                MenuMasterBusinessEntityList = MenuMasterDataAccessObject.MenuMasterBusinessEntityList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return MenuMasterBusinessEntityList;
        }

        /// <summary>
        /// This Method returns the Parent Menu
        /// </summary>
        /// <param name="menuMasterBusinessEntity"></param>
        public List<Survey_MenuMaster_BE> GetMenuParent(List<Survey_MenuMaster_BE> menuMasterBusinessEntity)
        {
            Survey_MenuMaster_DAO menuMasterDataAccessObject = new Survey_MenuMaster_DAO();
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                menuMasterDataAccessObject = new Survey_MenuMaster_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return menuMasterDataAccessObject.GetMenuParent(menuMasterBusinessEntity);
        }

        #endregion
    }
}

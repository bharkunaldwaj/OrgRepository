using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Survey_Company_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Properties"

        public List<Survey_Company_BE> company_BEList { get; set; }

        #endregion

        public List<Survey_Company_BE> GetCompanyByID(int companyID)
        {
            List<Survey_Company_BE> company_BEList = new List<Survey_Company_BE>();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();

                CNameValueList cNameValueList = null;
                cNameValueList = new CNameValueList();
                cNameValueList.Add("@Operation", "GETCOMP");
                cNameValueList.Add("@CompanyID", companyID);

                dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspCompanyManagement", cNameValueList, null).Tables[0];

                CopyDataTableValueToList<Survey_Company_BE>(dtAlluser, ref company_BEList);

                HandleWriteLogDAU("Survey_UspCompanyManagement", cNameValueList, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return company_BEList;
        }


        /// <summary>
        /// Add Company
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int AddCompany(Survey_Company_BE company_BE)
        {
            int returnValue = 0;
            try
            {

                //HandleWriteLog("Start", new StackTrace(true));

                CNameValueList cNameValueList = null;
                cNameValueList = new CNameValueList();
                if (company_BE.CompanyID > 0)
                {
                    cNameValueList.Add("@Operation", "U");
                    cNameValueList.Add("@CompanyID", company_BE.CompanyID);
                }
                else
                    cNameValueList.Add("@Operation", "I");
                cNameValueList.Add("@ProjectID", company_BE.ProjectID);
                cNameValueList.Add("@StatusID", company_BE.StatusID);
                cNameValueList.Add("@Title", company_BE.Title);
                cNameValueList.Add("@Description", company_BE.Description);
                cNameValueList.Add("@AccountID", company_BE.AccountID);
                cNameValueList.Add("@ManagerID", company_BE.ManagerID);
                cNameValueList.Add("@CompanyName", company_BE.CompanyName);
                cNameValueList.Add("@QuestLogo", company_BE.QuestLogo);
                cNameValueList.Add("@ReportLogo", company_BE.ReportLogo);
                cNameValueList.Add("@EmailTMPLStart", company_BE.EmailTMPLStart);
                cNameValueList.Add("@EmailTMPLReminder1", company_BE.EmailTMPLReminder1);
                cNameValueList.Add("@EmailTMPLReminder2", company_BE.EmailTMPLReminder2);
                cNameValueList.Add("@EmailTMPLReminder3", company_BE.EmailTMPLReminder3);
                cNameValueList.Add("@FaqText", company_BE.FaqText);
                cNameValueList.Add("@ModifyBy", company_BE.ModifyBy);
                cNameValueList.Add("@ModifyDate", company_BE.ModifyDate);
                cNameValueList.Add("@IsActive", company_BE.IsActive);
                cNameValueList.Add("@Finish_EmailID", company_BE.Finish_EmailID);
                cNameValueList.Add("@Finish_EmailID_Chkbox", company_BE.Finish_EmailID_Chkbox);
                cNameValueList.Add("@EmailFinishEmailTemplate", company_BE.EmailFinishEmailTemplate);

                returnValue = Common_DAO.ExecutScalarNew("Survey_UspCompanyManagement", cNameValueList);

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Add Company
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int DeleteCompany(Survey_Company_BE company_BE)
        {
            int returnValue = 0;
            try
            {
                CNameValueList cNameValueList = null;
                cNameValueList = new CNameValueList();
               
                    cNameValueList.Add("@Operation", "D");
                    cNameValueList.Add("@CompanyID", company_BE.CompanyID);


                    returnValue = Common_DAO.ExecutScalarNew("Survey_UspCompanyManagement", cNameValueList);

                cDataSrc = null;

            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }


        public DataTable GetdtCompanyList(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            try
            {
                string sql = "Select Survey_Company.[ProjectID]" +
                        ",Survey_Company.CompanyID" +
                        ",Survey_Company.CompanyName" +
                        ",Survey_Company.StatusID" +
                        ",Survey_Company.AccountID AS AccountID" +
                        ",Survey_Company.[Title]" +
                        ",Survey_Company.Description" +
                        ",Survey_Project.Title AS ProjectName" +
                        ",Survey_Company.[ManagerID]" +
                        ",Survey_Company.[QuestLogo]" +
                        ",Survey_Company.[EmailTMPLStart]" +
                        ",Survey_Company.[EmailTMPLReminder1]" +
                        ",Survey_Company.[EmailTMPLReminder2]" +
                        ",Survey_Company.[EmailTMPLReminder3]" +

                        ",Survey_Company.[FaqText]" +
                        ",Survey_Company.ModifyBy" +
                        ",Survey_Company.ModifyDate" +
                        ",Survey_Company.IsActive" +
                        ",Survey_Company.Finish_EmailID" +
                        ",Survey_Company.Finish_EmailID_Chkbox" +
                        ",[User].UserID " +
                        ",[User].FirstName as firstname" +
                        ",[User].LastName  as lastname" +
                        ", (firstname + ' ' + lastname) as finalname" +
                        ",Survey_MSTProjectStatus.Name as ProjectStatus" +
                        ",[Account].[Code] as Code" +
                        " FROM   [Survey_Company] Inner Join [User] on dbo.Survey_Company.ManagerID = [User].UserID" +
                        " Inner Join Survey_MSTProjectStatus on Survey_Company.StatusID = Survey_MSTProjectStatus.PRJStatusID" +
                        " INNER JOIN dbo.Account ON Survey_Company.AccountID = dbo.Account.AccountID" +
                        " left JOIN dbo.Survey_Project ON Survey_Company.ProjectID = dbo.Survey_Project.ProjectID" +
                        " Where  Survey_Company.[AccountID] = " + accountID +
                        " order by dbo.Account.[Code] ,Survey_Company.[ProjectID] desc";

                dtAllProject = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }


        /// <summary>
        /// Copy Data Table Value To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="lstDestination"></param>
        public static void CopyDataTableValueToList<T>(DataTable source, ref List<T> lstDestination)
        {
            if (source != null)
            {
                foreach (DataRow objDataRow in source.Rows)
                {
                    T objDestination = Activator.CreateInstance<T>();
                    foreach (DataColumn objDataColumn in source.Columns)
                    {
                        PropertyInfo sourcePI = objDestination.GetType().GetProperty(objDataColumn.ColumnName);
                        if (sourcePI != null)
                            sourcePI.SetValue(objDestination, (objDataRow[objDataColumn] != DBNull.Value ? objDataRow[objDataColumn] : null), null);
                    }

                    lstDestination.Add(objDestination);
                }
            }
        }


       
    }
}

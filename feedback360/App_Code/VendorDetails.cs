/*  
* PURPOSE: This is the Web Service class for fetching Vendor First Names
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for VendorDetails
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ToolboxItem(false)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]

public class VendorDetails : System.Web.Services.WebService
{

    public VendorDetails()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    /// <summary>
    /// Web Method returning list of Vendor First Name when matching criteria is passed
    /// </summary>
    /// <param name="p_prefixText"></param>
    /// <returns></returns>

    [WebMethod(true)]
    public string[] GetVendorDetails(string p_prefixText)
    {
        List<String> result = new List<string>();


        string connectionString =
        "Data Source=172.29.9.117;Initial Catalog=ShamrockIFC;Persist Security Info=True;User ID=sa;Password=spice";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand(string.Format("Select * from [User] where Type='Vendor' and IsActive=1 and FName Like '{0}%' ", p_prefixText), conn);

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(AutoCompleteItem(Convert.ToString(reader["FName"]), Convert.ToInt32(reader["UserID"])));
                }
                //reader.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        return result.ToArray();

    }

    private string AutoCompleteItem(string p_value, int p_id)
    {
        return string.Format("{{\"First\":\"{0}\",\"Second\":\"{1}\"}}", p_value, p_id);
    }

}


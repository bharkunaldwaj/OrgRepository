using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Utilities
/// </summary>
public static class Utilities
{	

    /// <summary>
    /// Create and encrypte query string
    /// </summary>
    /// <Author>Rudra Prakash Mishra</Author>
    /// <Date>02/06/2014</Date>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string CreateEncryptedQueryString(string userName, string password, string accountCode)
    {
        string strQS = "username=" + userName + ",password=" + password + ",accountcode=" + accountCode;
        return HttpUtility.UrlEncode(EnCryptDecrypt.CryptorEngine.Encrypt(strQS, true));
    }
}
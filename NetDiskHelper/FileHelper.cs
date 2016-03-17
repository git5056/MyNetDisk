using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetDiskHelper
{
    public class FileHelper
    {
        #region public methods

        /// <summary>
        /// GetMD5HashFromFile
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }  

        static public bool Compare(string loacalPath1, string localPath2)
        {
            return CompareWithMD5(GetMD5HashFromFile(loacalPath1), localPath2);
        }

        static public bool CompareWithMD5(string md5, string loacalPath)
        {
            return CompareMD5(md5, GetMD5HashFromFile(loacalPath));
        }

        #endregion

        #region   private methods

        static private bool CompareMD5(string md51, string md52)
        {
            return md51 == null || md52 == null || md51.Trim() == "" || md52.Trim() == "" ? false : md51 == md52;
        }

        #endregion
    }
}

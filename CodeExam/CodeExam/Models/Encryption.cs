using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CodeExam.Models
{
    public class Encryption
    {
        public static string Encrypt(string encript)
        {
            SHA1CryptoServiceProvider sh = new SHA1CryptoServiceProvider();
            try
            {
                sh.ComputeHash(ASCIIEncoding.ASCII.GetBytes(encript));
                byte[] re = sh.Hash;
                StringBuilder sb = new StringBuilder();
                foreach (byte item in re)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
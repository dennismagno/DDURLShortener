using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DDShortener.Models
{
    public static class Func
    {
        /// <summary>
        /// Check if long URL is a valid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        /// <summary>
        /// Generate random 6 alphanumeric string
        /// </summary>
        /// <returns></returns>
        public static string CreateShortString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}
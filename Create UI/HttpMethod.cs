using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Create_UI
{
    public class HttpMethod
    {
        public static string HttpGet(string URL)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            //req.Proxy = new WebProxy(ProxyString, true); //true means no proxy
            req.Method = "GET";
            //req.ContentType = "text/html;charset=UTF-8";
            WebResponse resp = null;

            req.Timeout = 2000;

            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                req.Abort();
                req = null;
                return "timeout";
            }
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            string sReturn = sr.ReadToEnd().Trim();
            resp.Close(); sr.Close();
            return sReturn;
        }
        public static string HttpPost(string URL, string Parameters)
        {
            byte[] bytes = Encoding.Default.GetBytes(Parameters);

            ServicePointManager.DefaultConnectionLimit = 200;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            //req.Proxy = new WebProxy(ProxyString, true);
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.8) Gecko/20051111 Firefox/1.5";
            req.Accept = "text/xml,application/xml,application/xhtml+xml,text/html";
            req.KeepAlive = false;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            req.ContentLength = bytes.Length;

            req.Timeout = 3000;

            Stream os = null;
            try
            {
                os = req.GetRequestStream();
            }
            catch
            {
                req.Abort();
                req = null;
                return "timeout";
            }
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp == null) return null;
                StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string sReturn = sr.ReadToEnd().Trim();
                req.Abort();
                req = null;
                resp.Close();
                resp = null;
                sr.Close();

                return sReturn;
            }
            catch
            {
                req.Abort();
                req = null;
                return "timeout";
            }
        }
    }
}

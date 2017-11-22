using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace VideoFace.Common.Util
{
    /// <summary>
    ///     有关HTTP请求的辅助类
    /// </summary>
    public class HttpWebReqUtility
    {
        private static readonly string DefaultUserAgent =
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        /// <summary>
        ///     创建GET方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
        /// <returns></returns>
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent,
            CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        ///     创建POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters,
            int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            return CreatePostHttpResponse(url, parameters, timeout, userAgent, null, null, requestEncoding, cookies);
        }

        /// <summary>
        ///     创建POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="acceptType">请求内容方式</param>
        /// <param name="contentType">请求内容的类型</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters,
            int? timeout, string userAgent,string acceptType,string contentType, Encoding requestEncoding, CookieCollection cookies)
        {
            //如果需要POST数据
            if (!(parameters == null || parameters.Count == 0))
            {
                var buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                var strParams= buffer.ToString();
                return CreatePostHttpResponse(url, strParams, timeout, userAgent, acceptType, contentType, requestEncoding, cookies);
            }
            return CreatePostHttpResponse(url, string.Empty, timeout, userAgent, acceptType, contentType, requestEncoding, cookies);
        }

        /// <summary>
        ///     创建POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="parameterVal">随同请求POST的参数名称及参数值</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="acceptType">请求内容方式</param>
        /// <param name="contentType">请求内容的类型</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, string parameterVal,
            int? timeout, string userAgent, string acceptType, string contentType, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    CheckValidationResult;
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";

            if (!string.IsNullOrEmpty(acceptType))
            {
                request.Accept = acceptType;
            }
            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据
            if (!string.IsNullOrEmpty(parameterVal))
            {
                byte[] data = requestEncoding.GetBytes(parameterVal);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true; //总是接受
        }
    }

    public static class UtilityTest
    {
        /// <summary>
        ///     POST数据到HTTPS站点
        /// </summary>
        public static void Test1()
        {
            string loginUrl = "https://passport.baidu.com/?login";
            string userName = "userName";
            string password = "password";
            string tagUrl = "http://cang.baidu.com/" + userName + "/tags";
            Encoding encoding = Encoding.GetEncoding("gb2312");

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tpl", "fa");
            parameters.Add("tpl_reg", "fa");
            parameters.Add("u", tagUrl);
            parameters.Add("psp_tt", "0");
            parameters.Add("username", userName);
            parameters.Add("password", password);
            parameters.Add("mem_pass", "1");
            HttpWebResponse response = HttpWebReqUtility.CreatePostHttpResponse(loginUrl, parameters, null, null,
                encoding, null);
            string cookieString = response.Headers["Set-Cookie"];

            // response.ResponseUri
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream != null)
            {
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                Console.WriteLine(retString);
            }
            Console.WriteLine("null");
        }

        /// <summary>
        ///     发送GET请求到HTTP站点
        /// </summary>
        public static void Test2()
        {
            string userName = "userName";
            string tagUrl = "http://cang.baidu.com/" + userName + "/tags";
            var cookies = new CookieCollection();
            //如何从response.Headers["Set-Cookie"];中获取并设置CookieCollection的代码略  
            HttpWebResponse response = HttpWebReqUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream != null)
            {
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                Console.WriteLine(retString);
            }
            Console.WriteLine("null");
        }

        /// <summary>
        ///     发送POST请求到HTTP站点
        /// </summary>
        public static void Test3()
        {
            string loginUrl = "http://home.51cto.com/index.php?s=/Index/doLogin";
            string userName = "userName";
            string password = "password";

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("email", userName);
            parameters.Add("passwd", password);

            HttpWebResponse response = HttpWebReqUtility.CreatePostHttpResponse(loginUrl, parameters, null, null,
                Encoding.UTF8, null);

            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream != null)
            {
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                Console.WriteLine(retString);
            }

            Console.WriteLine("null");
        }
    }
}
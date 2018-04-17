using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class HttpHelper
    {
        /// <summary>
        /// 向服务端请求数据
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int PostData(byte[] bs, out string retStr, string url)
        {
            int ret = -1;
            ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//保证证书不错误
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            //req.Method = "GET";
            req.Method = "POST";
            req.Timeout = 120000;
            req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentType = "application/octet-stream";
            req.ContentLength = bs.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader stream = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    StringBuilder strB = new StringBuilder(stream.ReadToEnd());
                    stream.Dispose();
                    stream.Close();
                    ret = int.Parse(strB[strB.Length - 1].ToString());
                    strB = strB.Remove(strB.Length - 1, 1);
                    retStr = strB.ToString();
                    strB.Clear();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 向服务端请求数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int PostData(string data, out string retStr, string url)
        {
            int ret = -1;
            ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//保证证书不错误
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            req.Method = "POST";
            req.Timeout = 120000;
            //对发送的数据不使用缓存
            //req.AllowWriteStreamBuffering = false;
            req.ContentType = "application/x-www-form-urlencoded";
            using (StreamWriter writer = new StreamWriter(req.GetRequestStream(), Encoding.UTF8))//请求向服务器写入数据的流
            {
                string writeData = "";
                int writingLen = 500;
                int writedLen = 0;
                while ((writedLen + writingLen) < data.Length)
                {
                    writeData = data.Substring(writedLen, writingLen);
                    writedLen += writingLen;
                    writer.Write(writeData);
                    //System.Threading.Thread.Sleep(1000);
                }
                writeData = data.Substring(writedLen);//传剩下的
                writer.Write(writeData);
            }
            using (WebResponse wr = req.GetResponse())
            {
                //在这里对接收到的页面内容进行处理
                StreamReader stream = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                StringBuilder strB = new StringBuilder(stream.ReadToEnd());
                stream.Dispose();
                stream.Close();
                ret = int.Parse(strB[strB.Length - 1].ToString());
                strB = strB.Remove(strB.Length - 1, 1);
                retStr = strB.ToString();
                strB.Clear();
                return ret;
            }
        }
        /// <summary>
        /// 向服务端请求数据
        /// 发送字节数组，获取base64字符串并转成正常字符串out
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="retStr"></param>
        /// <param name="url"></param>
        /// <returns>服务端接口返回值</returns>
        public static int PostData_base64(byte[] bs, out string retStr, string url)
        {
            int ret = -1;
            ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//保证证书不错误
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 120000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader stream = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    StringBuilder strB = new StringBuilder(stream.ReadToEnd());
                    stream.Dispose();
                    stream.Close();
                    ret = int.Parse(strB[strB.Length - 1].ToString());
                    strB = strB.Remove(strB.Length - 1, 1);
                    byte[] bs64 = Convert.FromBase64String(strB.ToString());
                    strB.Clear();
                    retStr = Encoding.UTF8.GetString(bs64);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 向服务端请求数据
        /// 发送字节数组，获取base64字符串并转成字节数组out
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="buffer"></param>
        /// <param name="url"></param>
        /// <returns>服务端接口返回值</returns>
        public static int PostData_base64(byte[] bs, out byte[] buffer, string url)
        {
            int ret = -1;
            ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//保证证书不错误
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 120000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader stream = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    StringBuilder strB = new StringBuilder(stream.ReadToEnd());
                    stream.Dispose();
                    stream.Close();
                    ret = int.Parse(strB[strB.Length - 1].ToString());
                    strB = strB.Remove(strB.Length - 1, 1);
                    buffer = Convert.FromBase64String(strB.ToString());
                    strB.Clear();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static bool CheckValidationResult(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using Yeanzhi.Framework.Utility;

/// <summary>
///WeiXinLogin 的摘要说明
/// </summary>
public class WeiXinLogin
{
   
    /// <summary>
    /// MD5　32位加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static string GetMd5Str32(string str)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        // Convert the input string to a byte array and compute the hash.  
        char[] temp = str.ToCharArray();
        byte[] buf = new byte[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            buf[i] = (byte)temp[i];
        }
        byte[] data = md5Hasher.ComputeHash(buf);
        // Create a new Stringbuilder to collect the bytes  
        // and create a string.  
        StringBuilder sBuilder = new StringBuilder();
        // Loop through each byte of the hashed data   
        // and format each one as a hexadecimal string.  
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        // Return the hexadecimal string.  
        return sBuilder.ToString();
    }

    public static bool ExecLogin(string name,string pass)
    {
        bool result = false;
        string password = GetMd5Str32(pass).ToUpper(); 
        string padata = "username=" + name + "&pwd=" + password + "&imgcode=&f=json";
        string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN ";//请求登录的URL
        try
        {
            CookieContainer cc = new CookieContainer();//接收缓存
            byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
            webRequest2.Referer = "https://mp.weixin.qq.com/cgi-bin/loginpage?lang=zh_CN&t=wxm2-login";
            webRequest2.CookieContainer = cc;                                      //保存cookie  
            webRequest2.Method = "POST";                                          //请求方式是POST
            webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
            webRequest2.ContentLength = byteArray.Length;
          
            Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
            // Send the data.
            newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
            newStream.Close();
            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
            StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);
            string text2 = sr2.ReadToEnd();

            //此处用到了newtonsoft来序列化
            WeiXinRetInfo retinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiXinRetInfo>(text2);
            string token = string.Empty;
            if (retinfo.base_resp.ret==0)
            {
                token = retinfo.redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令牌
                Caching.Set("LoginCookie", cc,300);
                Caching.Set("CreateDate", DateTime.Now, 300);
                Caching.Set("Token", token,300);
                LoginInfo.LoginCookie = cc;
                LoginInfo.CreateDate = DateTime.Now;
                LoginInfo.Token = token;
                result = true;
            }
        }
        catch (Exception ex)
        {
          
            throw new Exception(ex.StackTrace);
        }
        return result;
    }

    public static class LoginInfo
    {
        /// <summary>
        /// 登录后得到的令牌
        /// </summary>        
        public static string Token { get; set; }
        /// <summary>
        /// 登录后得到的cookie
        /// </summary>
        public static CookieContainer LoginCookie { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public static DateTime CreateDate { get; set; }

    }
}
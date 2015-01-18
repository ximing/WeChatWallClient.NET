using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
/// <summary>
///WeiXin 的摘要说明
/// </summary>
public class WeiXin
{

    public static bool SendMessage(string Message, string fakeid)
    {
        bool result = false;
        CookieContainer cookie = null;
        string token = null;
        cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
        token =  WeiXinLogin.LoginInfo.Token;//取得token

        string strMsg = System.Web.HttpUtility.UrlEncode(Message);  //对传递过来的信息进行url编码
        string padate = "type=1&content=" + strMsg + "&error=false&tofakeid=" + fakeid + "&token=" + token + "&ajax=1";
        string url = "https://mp.weixin.qq.com/cgi-bin/singlesend?t=ajax-response&lang=zh_CN";

        byte[] byteArray = Encoding.UTF8.GetBytes(padate); // 转化

        HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);

        webRequest2.CookieContainer = cookie; //登录时得到的缓存

        webRequest2.Referer = "https://mp.weixin.qq.com/cgi-bin/singlemsgpage?token=" + token + "&fromfakeid=" + fakeid + "&msgid=&source=&count=20&t=wxm-singlechat&lang=zh_CN";

        webRequest2.Method = "POST";

        webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";

        webRequest2.ContentType = "application/x-www-form-urlencoded";

        webRequest2.ContentLength = byteArray.Length;

        Stream newStream = webRequest2.GetRequestStream();

        // Send the data.            
        newStream.Write(byteArray, 0, byteArray.Length);    //写入参数    

        newStream.Close();

        HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

        StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

        string text2 = sr2.ReadToEnd();
        if (text2.Contains("ok"))
        {
            result = true;
        }
        return result;
    }

    public static bool SendImageTextMessage(string appMsgId, string fakeid)
    {
        bool result = false;
        CookieContainer cookie = null;
        string token = null;
        cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
        token = WeiXinLogin.LoginInfo.Token;//取得token

       // string strMsg = System.Web.HttpUtility.UrlEncode(Message);  //对传递过来的信息进行url编码
        string padate = "type=10&fid=" + appMsgId + "&appmsgid=" + appMsgId + "&error=false&tofakeid=" + fakeid + "&token=" + token + "&ajax=1&content=&imagecode=";
        string url = "https://mp.weixin.qq.com/cgi-bin/singlesend?t=ajax-response&lang=zh_CN";

        byte[] byteArray = Encoding.UTF8.GetBytes(padate); // 转化

        HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);

        webRequest2.CookieContainer = cookie; //登录时得到的缓存

        webRequest2.Referer = "https://mp.weixin.qq.com/cgi-bin/singlemsgpage?token=" + token + "&fromfakeid=" + fakeid + "&msgid=&source=&count=20&t=wxm-singlechat&lang=zh_CN";

        webRequest2.Method = "POST";

        webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";

        webRequest2.ContentType = "application/x-www-form-urlencoded";

        webRequest2.ContentLength = byteArray.Length;

        Stream newStream = webRequest2.GetRequestStream();

        // Send the data.            
        newStream.Write(byteArray, 0, byteArray.Length);    //写入参数    

        newStream.Close();

        HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

        StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

        string text2 = sr2.ReadToEnd();
        if (text2.Contains("ok"))
        {
            result = true;
        }
        return result;
    }
   
  
}
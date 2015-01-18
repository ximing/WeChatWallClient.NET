using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WindowFormWeixin.Util
{
   public class WeiXinTool
    {
         public static List<WeiXinUserInfoModel> SubscribeMP()
        {

            try
            {
                CookieContainer cookie = null;
                string token = null;


                cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
                token = WeiXinLogin.LoginInfo.Token;//取得token

                /*获取用户信息的url，这里有几个参数给大家讲一下，1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数

                3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id，当然这也是我的猜想不一定正确*/
                string Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?tuser/index&token=" + token + "&lang=zh_CN&pagesize=100000000&pageidx=0&type=0&groupid=0";
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(Url);
                webRequest2.CookieContainer = cookie;
                webRequest2.ContentType = "text/html; charset=gb2312";
                webRequest2.Method = "GET";
                webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
                webRequest2.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();


                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);
                string text2 = sr2.ReadToEnd();
                MatchCollection mc;
                string textresult = GetValue(text2, "contacts", "}).contacts").Replace("friendsList:({\\\"contacts\\\":", "").Replace(" ", "");
                //string reg = "<script id=\\\"json-friendList\\\" type=\\\"json/text\\\">[^<]*</script>";  
                ////由于此方法获取过来的信息是一个html网页所以此处使用了正则表达式，注意：（此正则表达式只是获取了fakeid的信息如果想获得一些其他的信息修改此处的正则表达式就可以了。）
                //Regex r = new Regex(reg); //定义一个Regex对象实例
                //string textresult1 = text2.Replace("\r\n", "").Replace("\n","").Replace("\\\"","\"").Trim();
                //mc = r.Matches(textresult);
                //Int32 friendSum = mc.Count;          //好友总数
               List<WeiXinUserInfoModel> wxlist=  JsonConvert.DeserializeObject<List<WeiXinUserInfoModel>>(textresult);

            
                //List<WeiXinUserInfoModel> fackID1 = new List<WeiXinUserInfoModel>();

                //for (int i = 0; i < friendSum; i++)
                //{
                //    WeiXinUserInfoModel wxuim = new WeiXinUserInfoModel();
                //    wxuim.fakeId= mc[i].Value.Split(new char[] { ':' })[1].Replace("\"", "").Trim();
                //    wxuim.nickName = mc[i].Value.Split(new char[] { ':' })[2].Replace("\"", "").Trim();
                //    wxuim.remarkName = mc[i].Value.Split(new char[] { ':' })[3].Replace("\"", "").Trim();
                //    wxuim.groupId = mc[i].Value.Split(new char[] { ':' })[4].Replace("\"", "").Trim();
                //    fackID1.Add(wxuim);
                //}

                return wxlist;



            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

         /// <summary>
         /// 获得字符串中开始和结束字符串中间得值
         /// </summary>
         /// <param name="str">字符串</param>
         /// <param name="s">开始</param>
         /// <param name="e">结束</param>
         /// <returns></returns> 
         public static string GetValue(string str, string s, string e)
         {
             int startindex = str.IndexOf(s)+10;
             int endindex = str.IndexOf(e);
             string strresult = str.Substring(startindex, endindex - startindex);
             return strresult;
         }


 
    }
}

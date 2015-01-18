using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeChatWallClient.NET.Hubs;
using WeChatWallClient.NET.Models;
using Yeanzhi.Framework.Utility;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace WeChatWallClient.NET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Client()
        {
            return View();
        }
        public ActionResult Index()
        {
            if (WeiXinLogin.ExecLogin("email", "password"))
            {
                return RedirectToAction("GetInfo");
            }
            return View();
        }

        /// <summary>
        /// 从微信平台获取信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetInfo()
        {
            MongoUtil mongo = new MongoUtil();
            var colloction = mongo.getCollection("message");
            var res = colloction.FindAll().ToList();
            foreach (var item in res)
            {
                if (!IsExitImage(item.fakeid))
                {
                    DownImage(item.fakeid);
                }
            }
            return View(res);
        }

        /// <summary>
        /// 上墙
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sq(string id)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<WeChatHub>();
            if (hubContext != null)
            {
                ObjectId objid = new ObjectId(id);
                MongoUtil mongo = new MongoUtil();
                var colloction = mongo.getCollection("message");
                var msg = colloction.AsQueryable<Msg>().FirstOrDefault(a => a.Id == objid);
                if (msg!=null)
                {
                    hubContext.Clients.All.addData(msg);
                }             
            }
            return RedirectToAction("GetInfo");
        }

        private bool IsExitImage(string fakeid)
        {
            string rootpath = Server.MapPath("~/corvers/");
            string path = Path.Combine(rootpath, fakeid + ".jpg");
            System.IO.FileInfo fileInfo = null;
            try
            {
                fileInfo = new System.IO.FileInfo(path);
            }
            catch (Exception e)
            {
                return false;
            }
            // 如果文件存在
            if (fileInfo != null && fileInfo.Exists)
            {
                System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
                if (fileInfo.Length > 0)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        private string DownImage(string fakeid)
        {
            try
            {
                CookieContainer cookie = null;
                string token = null;
                cookie = (CookieContainer)Caching.Get("LoginCookie");
                token = (string)Caching.Get("Token");
                string Url = string.Format("https://mp.weixin.qq.com/misc/getheadimg?token={0}&fakeid={1}", token, fakeid);
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(Url);
                webRequest2.CookieContainer = cookie;
                webRequest2.ContentType = "image/jpeg";
                webRequest2.Method = "GET";
                webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
                var str2 = response2.GetResponseStream();
                string rootpath = Server.MapPath("~/corvers/");
                if (Directory.Exists(rootpath) == false)
                {
                    Directory.CreateDirectory(rootpath);
                }
                Stream reader = response2.GetResponseStream();
                FileStream writer = new FileStream(Path.Combine(rootpath, fakeid + ".jpg"), FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[512];
                int c = 0; //实际读取的字节数
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                writer.Flush();
                str2.Close();
                writer.Close();
                writer.Dispose();
                reader.Close();
                reader.Dispose();
                response2.Close();
                return Path.Combine(rootpath, fakeid + ".jpg");
            }
            catch (Exception ex)
            {
            }
            return "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///WeiXinRetInfo 的摘要说明
/// </summary>
public class WeiXinRetInfo
{
    public base_resp base_resp { get; set; }
    public string redirect_url { get; set; }
    public string ShowVerifyCode { get; set; }
}
public class base_resp
{
    public int ret { get; set; }
    public string err_msg { get; set; }
}
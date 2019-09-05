using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.UserSetting;
using ZNV.Timesheet.Web.Common;

namespace ZNV.Timesheet
{
    public class TimeSheetAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            if (!isLogin(filterContext))
            {
                if (Debugger.IsAttached)
                {//如果是调试阶段，则跳过sso登录，节省时间
                    filterContext.HttpContext.Session["OAUserName"] = "0049002415";
                    CommonHelper.CurrentUser = "0049002415";
                    return;
                }
                string casServerUrlPrefix = ConfigurationManager.AppSettings["casServerUrlPrefix"];

                //SSO的业务逻辑
                if (request.Params["ticket"] == null || request.Params["ticket"].Trim() == "")
                {
                    // 如果Request.Url的值带参数，需要对这个url先进过编码
                    // 跳转url的值，例如：http://sso.liyong.com:8280/sso/login?service=http://localhost/test.aspx,其中http://localhost是.net系统的地址
                    filterContext.Result = new EmptyResult();//解决 服务器无法在发送 HTTP 标头之后修改 cookie此类问题
                    response.Redirect(casServerUrlPrefix + "login?service=" + request.Url);
                    response.End();
                    return;
                }
                else
                {
                    WebClient web = new WebClient();
                    try
                    {
                        String service = request.Url.ToString();
                        int ticketIndex = 0;
                        ticketIndex = service.IndexOf("?ticket");
                        if (ticketIndex != -1)
                            service = service.Substring(0, ticketIndex);
                        // url的值，例如：http://sso.liyong.com:8280/sso/validate?ticket=ST-3-QrZ3WmuPOX94L4og7vbQ-cas&service=http://localhost/test.aspx
                        string url = casServerUrlPrefix + "validate?ticket=" + request.Params["ticket"] + "&service=" + service;
                        string strResponse = web.DownloadString(url);
                        StringReader reader = new StringReader(strResponse);
                        string isOk = reader.ReadLine();
                        if (isOk.Trim() == "yes")
                        {
                            string name = reader.ReadLine();
                            filterContext.HttpContext.Session["OAUserName"] = name;
                            CommonHelper.CurrentUser = name;
                        }
                        else
                        {
                            response.Write("Ticket验证失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Write("SSO出错" + ex.ToString());
                    }
                }
            }
        }

        private bool isLogin(AuthorizationContext filterContext)
        {
            //根据业务系统的验证逻辑，判断用户是否已经登录，已经登录则直接跳转到相应的页面
            if (filterContext.HttpContext.Session["OAUserName"] == null || filterContext.HttpContext.Session["OAUserName"].ToString().Trim() == "")
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(CommonHelper.CurrentUser))
                {
                    CommonHelper.CurrentUser = filterContext.HttpContext.Session["OAUserName"].ToString().Trim();
                }
                return true;
            }
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using ZNV.Timesheet.ConfigurationManagement;
using ZNV.Timesheet.Web.App_Start;

namespace ZNV.Timesheet.Web.Controllers
{
    [TimesheetAuthorize(ModuleCode = "00010007")]
    public class ConfigurationManagementController : Controller
    {
        private readonly IConfigurationAppService _configurationAppService;
        public ConfigurationManagementController(IConfigurationAppService configurationAppService)
        {
            _configurationAppService = configurationAppService;
        }

        public ActionResult Index()
        {
            List<Configuration> configList = _configurationAppService.GetConfigurationList();
            ViewBag.ConfigurationList = configList;
            return View();
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            var list = _configurationAppService.GetConfigurationList().Where(item => !item.ParentId.HasValue).ToList();
            list.ForEach(item => {
                item.ConfigText = item.ConfigValue + "-" + item.ConfigText;
            });
            ViewBag.Modules = new SelectList(list, "Id", "ConfigText");
            if (id == 0)
            {
                return View(new Configuration());
            }
            else
            {
                var role = _configurationAppService.GetConfiguration(id);
                return View(role);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Configuration config)
        {
            if (config.Id == 0)
            {
                config.Creator = Common.CommonHelper.CurrentUser;
                config.LastModifier = config.Creator;
                config.LastModifyTime = DateTime.Now;
                _configurationAppService.AddConfiguration(config);
                return Json(new { success = true, message = "新增配置信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                config.LastModifier = Common.CommonHelper.CurrentUser;
                config.LastModifyTime = DateTime.Now;
                _configurationAppService.UpdateConfiguration(config);
                return Json(new { success = true, message = "更新配置信息成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _configurationAppService.DeleteConfiguration(id);
            return Json(new { success = true, message = "删除配置信息成功!" }, JsonRequestBehavior.AllowGet);
        }
    }
}
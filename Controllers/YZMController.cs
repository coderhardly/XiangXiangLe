using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCommon;

namespace XiangXiangLeWeb.Controllers
{
    public class YZMController : Controller
    {
        // GET: YZM
        public FileContentResult GetYZM()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 输入验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Check(string input)
        {
            string code = Session["ValidateCode"].ToString();
            if (input.Trim() == code.ToLower().Trim())
            {
                return "true";
            }
            return "false";
        }
    }
}
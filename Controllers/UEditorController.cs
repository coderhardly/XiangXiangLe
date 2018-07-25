using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangXiangLeWeb.Controllers
{
    public class UEditorController : Controller
    {
        // GET: UEditor
        IBll.IRemarkBll remarkBll = new Bll.RemarkBll();
        IBll.IUserInfoBll userInfoBll = new Bll.UserInfoService();
       
        public ActionResult Index()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection fc)

        {

            var content = fc["editor"];
            string contents = content.Replace("<p>", "");
            string contesnt1 = contents.Replace("</p>", "");
            int pid = Convert.ToInt32(Request["PId"]);
            int uid = Convert.ToInt32(Session["userId"]);
            //涉及外键时，先查询后插入
           Model.Users users= userInfoBll.LoadEntity(m => m.userId == uid).FirstOrDefault();
            int userId = users.userId;
            Model.Remark remark = new Model.Remark()
            {
                remarkId = Convert.ToInt32(XCommon.CreateId.CreateNum()),
                remTime = DateTime.Now.ToString("yyyy-MM-dd-hh"),
                userId1 =userId,
                remTxt=contesnt1,
                productId=pid,
            };
            if (remarkBll.addEntity(remark))
            {
                return RedirectToAction("Details", "Product", new { id = pid });
            }
            else
            {
                return Content("<script>alert('出错了')</script>");
            }
           
        }
    }
}
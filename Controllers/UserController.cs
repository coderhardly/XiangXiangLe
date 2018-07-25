using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.Json;
//using Avatar.Helper;
using System.IO;

namespace XiangXiangLeWeb.Controllers
{
    public class UserController : Controller {
        IBll.IUserInfoBll userInfoBll = new Bll.UserInfoService();
        static string urlPath = string.Empty;
        public UserController()
        {
            var applicationPath = VirtualPathUtility.ToAbsolute("~") == "/" ? "" : VirtualPathUtility.ToAbsolute("~");
            urlPath = applicationPath + "/Upload";
        }

        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["userId"]);
            Model.Users user = userInfoBll.LoadEntity(m => m.userId == id).FirstOrDefault();
            return View(user);
        }

        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = Request.Files["MenuIcon"];
            if (file == null)
            {
                return Content("no:上传文件不能为空!");
            }
            else
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(fileName);
                if (fileExt == ".jpg")
                {
                    string dir = "/FileUploadImage/";
                    Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                    string newfileName = Guid.NewGuid().ToString();
                    string fullDir = dir + newfileName + fileExt;
                    file.SaveAs(Request.MapPath(fullDir));
                    //把更改的图片路径存入数据库
                    int id = Convert.ToInt32(Session["userId"]);
                    Model.Users user = userInfoBll.LoadEntity(m => m.userId == id).FirstOrDefault();
                    TempData["Url"] = fullDir;
                    if (userInfoBll.UpdateImage(user, fullDir))
                    {
                        return Content("ok:" + fullDir);
                    }
                    else
                    {
                        return Content("no:上传文件格式错误!!");
                    }

                    //return Content("ok:" + fullDir);

                }
                else
                {
                    return Content("no:上传文件格式错误!!");
                }
            }
        }
        public ActionResult UpdateUserInfo()
        {
            string name = Request["Name"];
            string mail = Request["Mail"];
            string tel = Request["Tel"];
            int id = Convert.ToInt32(Session["userId"]);

            //构建一个修改对象，必须的字段必须赋默认值，不然会报错
            Model.Users users = new Model.Users()
            {
                userId = id,
                roles = "会员",
                upassword = "123456",
                umail = mail,
                telphone = tel,
                uName = name,

            };

            //传入要修改的字段集合
            List<string> filed = new List<string>()
           {
             "umail","uName","telphone"
           };

            if (userInfoBll.UpdateEntityFields(users, filed))
            {
                return Json(new { flag = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ManageUser()
        {
            var list = userInfoBll.LoadEntity(m => m.roles == "会员");
            return View(list);
        }
        public ActionResult Forbid(int Id)
        {

            Model.Users users = new Model.Users()
            {
                userId = Id,
                roles = "会员",
                upassword = "123456",
                uName = "张三",
                @lock="封禁",
                telphone="15797813182"
                
                
            };
            List<string> list = new List<string>()
            {
                "lock"
            };
            if (userInfoBll.UpdateEntityFields(users, list))
            {
                return RedirectToAction("Index", "Manage", "");
            }
            else
            {
                return Content("<script>alert('出错啦')</script>");
            }
        }
            

    }

}

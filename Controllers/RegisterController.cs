using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using IBll;
using Bll;

namespace XiangXiangLeWeb.Controllers
{
    public class RegisterController : Controller
    {
       IBll.IUserInfoBll userbll = new UserInfoService();
        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Users users)
        {
            users.telphone = "1111111";
            users.userId = Convert.ToInt32(users.userId);
            users.createTime = DateTime.Now;
            users.roles = "会员";
            users.uName = "张三";
            users.umail = null;
            users.usex = "男";
            users.uphoto = "userDefault.jpg";
            users.@lock = "正常";
           
            string validatecode = Request["txtverifcode"];
            var checkmember = userbll.LoadEntity(u => u.userId == users.userId).FirstOrDefault();
            string code = Session["ValidateCode"].ToString();
            if (validatecode!=code)
            {
                return Content("<script>alert('验证码错误')</script>");
            }
            else
            {
               
                if(checkmember!=null)
                {
                    return JavaScript("alert('该用户已存在')");
                }
                else
                {
                    
                        userbll.addEntity(users);
                    return Redirect("/Register/Login");
                
                    
                 
             
                }
            }
           
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users users)
        {
            string validatecode = Request["txtverifcode"];
            //int m = users.userId;
            var checkmember = userbll.LoadEntity(u => u.userId == users.userId).FirstOrDefault();
            string code = Session["ValidateCode"].ToString();
           int a= string.Compare(code, validatecode, true);
            if (validatecode != code)
            {
                return Content("<script>alert('验证码错误')</script>");
            }
            else
            {
               
                
                    if (checkmember != null)
                    {
                        if (checkmember.upassword == users.upassword)
                        {
                            Session["userId"] = checkmember.userId;
                            Session["uname"] = checkmember.uName;
                            Session["uphoto"] = checkmember.uphoto;
                            if (checkmember.roles == "会员")
                            {
                            return Redirect("/Home/Index");
                            }
                            else
                            {
                            return Redirect("/Manage/Index");
                             }
                          
                        }
                        else
                        {
                            return JavaScript("alert('密码错误')");
                        }
                    }
                    else
                    {
                        return JavaScript("alert('该用户不存在')");
                    }
             
               
            }
           
        }

    }
}
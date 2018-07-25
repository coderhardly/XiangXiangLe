using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangXiangLeWeb.Controllers
{
    public class ProductController : Controller
    {
        Bll.ProductBll ProductBll = new Bll.ProductBll();
        IBll.IRemarkBll remarkBll = new Bll.RemarkBll();
        IBll.IUserInfoBll userInfoBll = new Bll.UserInfoService();
        IBll.IcartsBll cartsBll = new Bll.CartsBll();
        string ImagePath;
        private const int PageSize = 2;
        private int counts;
        // GET: Product
        public ActionResult Details(int id)
        {
            var pDetail = ProductBll.LoadEntity(m => m.productId == id).FirstOrDefault();
            int uid = Convert.ToInt32(Session["userId"]);
            var list = remarkBll.LoadPageEntity(1, 5,out int total, m => m.productId == id, m => m.remTime, false);
            //var cart = cartsBll.LoadEntity(m => m.ofUser == uid).First();
            //Session["count"] = cart.pcount;

            Model.ViewModel.ProductRemarkView productRemark = new Model.ViewModel.ProductRemarkView
            {
              products=pDetail,
               remarks = list,
              
            };
            return View(productRemark);
        }
        public ActionResult ProductList()
        {
            var list = ProductBll.LoadEntity(m => true);
            return View(list);
;       }
        public ActionResult AddProduct(FormCollection fc)
        {
            int id =Convert.ToInt32( XCommon.CreateId.CreateNum());
            int price = Convert.ToInt32(fc["pPrice"]);
            int pcate = Convert.ToInt32(fc["pCategoryId"]);
            Model.Products product = new Model.Products()
            {
                productId = id,
                price = price,
                pcategoryId = pcate,
                pname = fc["pName"].Trim(),
                origin = fc["pOrigin"].Trim(),
                pUrl = fc["photo"].Trim(),
                Descript=fc["pDes"].Trim(),
                Phot=0,
            };
            if (ProductBll.addEntity(product))
            {

                return Json(new { flag = true, JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { flag = false, JsonRequestBehavior.AllowGet });
            }
        }
        public ActionResult UpdateProduct(FormCollection fc)
        { int id =Convert.ToInt32(fc["pId"]);
            int price = Convert.ToInt32(fc["pPrice"]);
            int pcate = Convert.ToInt32(fc["pCategoryId"]);
            Model.Products up = new Model.Products()
            {
                productId = id,
                price = price,
                pcategoryId = pcate,
                pname = fc["pName"].Trim(),
                origin=fc["pOrigin"].Trim(),
            };
            List<string> s = new List<string>()
            {
                "price","pcategoryId","pname","origin"
            };
            if (ProductBll.UpdateEntityFields(up, s))
            {
                return Json(new { flag = true, JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { flag = false, JsonRequestBehavior.AllowGet });
            }
           
        }
        public ActionResult Delete(int id)
        {
            var model = ProductBll.LoadEntity(m => m.productId == id).First();
            if (ProductBll.deleteEntity(model))
            {
                return RedirectToAction("ProductList");
            }
            else
            {
                return Content("<script>alert('出错啦')</script>");
            }
           
        }
        public ActionResult FileUpload(FormCollection fc)
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
                    ImagePath = fullDir;
                    //if (userInfoBll.UpdateImage(user, fullDir))
                    //{
                        return Content("ok:" + fullDir);
                    //}
                    //else
                    //{
                    //    return Content("no:上传文件格式错误!!");
                    //}

                    //return Content("ok:" + fullDir);

                }
                else
                {
                    return Content("no:上传文件格式错误!!");
                }
            }
        }
    

    }
}
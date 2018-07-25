using Bll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace XiangXiangLeWeb.Controllers
{
    public class CartsController : Controller
    {
        CartsBll CartsBll = new CartsBll();
    
        // GET: Carts
        public ActionResult CartsIndex()
        {
            int cid = Convert.ToInt32(Session["userId"]);
            var list = CartsBll.GetIndex(cid);
            //list = list.Where(m => m.Carts.cartId == cid).ToList();
            return View(list);
        }
        public ActionResult AddProduct(int pNum,int pId,int?ofUser)
        {
            ofUser= Convert.ToInt32(Session["userId"]);
            if(CartsBll.AddProduct(pNum,pId,ofUser))
            {
                 return Json(new {flag=true}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {flag=false}, JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult GetCount()
        {
            int cid = Convert.ToInt32(Session["userId"]);
            int count = CartsBll.GetCount(cid);
            return Json( new { number=count}, JsonRequestBehavior.AllowGet);
        }
     
        public ActionResult DeleteProduct(int pId)
        {
           if( CartsBll.DeleteProduct(pId))
            {
                return RedirectToAction("CartsIndex");
            }
            else
            {
                return JavaScript("alert(失败)");
            }
           
        }
        public ActionResult EmptyCarts()
        {
            int ofuer = Convert.ToInt32(Session["userId"]);
            if (CartsBll.EmptyCarts(ofuer))
            {
                return Json(new { flag = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateCount(int pId,int pNum)
        {
            if (CartsBll.UpdateCount(pId, pNum))
            {
                return Json(new { flag = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateOrder()
        {
            int cid = Convert.ToInt32(Session["userId"]);
            var list = CartsBll.GetIndex(cid);
            //list = list.Where(m => m.Carts.cartId == cid).ToList();
            return View(list);
         
        }
        
    }
}
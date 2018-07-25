using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bll;
using IBll;

namespace XiangXiangLeWeb.Controllers
{
    public class HomeController : Controller
    {

        IBll.IUserInfoBll userInfoBll = new Bll.UserInfoService();
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Index()
        {
            IProductBll productBll = new Bll.ProductBll();
            
            var productList1 = productBll.LoadEntity(m => m.pcategoryId ==1).ToList();
            var productList2 = productBll.LoadEntity(m => m.pcategoryId ==2).ToList();
            var hotProduct = productBll.LoadPageEntity(1, 6, out int total,m=>true, m => m.Phot, false);
            var lastProduct= productBll.LoadPageEntity(1, 6, out int totals, m => true, m => m.productId,true);
            var category1 = productBll.DoubleSelet().Where(s => s.CategotyId == 1).FirstOrDefault();
            //var category3 = productBll.DoubleSelet().Where(s => s.CategotyId ==2).FirstOrDefault();
            Model.ViewModel.IndexView index = new Model.ViewModel.IndexView()
            {
                Product1 = productList1,
                Product2 = productList2,
                Product3=hotProduct,
                Product4=lastProduct,
                Categories1 = category1,
                //Categories3=category3

        };
           
            return View(index);
        }
       
    }
}
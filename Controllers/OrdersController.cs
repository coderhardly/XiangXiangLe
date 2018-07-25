using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using XCommon;

namespace XiangXiangLeWeb.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        IBll.IOrdersBll ordersBll = new Bll.OrderBll();
        IBll.IOrderDetailBll orderDetailBll = new Bll.OrderDetailBll();
        IBll.IcartsBll cartsBll = new Bll.CartsBll();
        int orderNum = Convert.ToInt32(CreateId.CreateNum());

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CompleteOrder()
        {
            string cusTel = Request["CusTel"];
            string cusName = Request["CusName"];
            string cusAdress = Request["CusAdress"];
            int totalPrice = Convert.ToInt32(Request["TotalPrice"]);
            Model.Orders orders = new Model.Orders()
            {
                orderId = orderNum,
                orderAccount =totalPrice,
                userId = Convert.ToInt32(Session["userId"]),
                orderState = "未完成",
                adress = cusAdress,
                CreateTime = DateTime.Now.ToLocalTime(),
                userPhone = cusTel
            };
            if (ordersBll.addEntity(orders))
            {
                return Json(new { flag1 = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag1 = false }, JsonRequestBehavior.AllowGet);
            }
        }
        bool flag;
        public ActionResult CompleteOrder1(List<Model.ViewModel.CartsView> list)
        {

            //string result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            //List<Model.ViewModel.CartsView> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.ViewModel.CartsView>>(result);

                int _userId = Convert.ToInt32(Session["userId"]);
                Model.Orders orders = ordersBll.LoadEntity(m => m.userId == _userId).FirstOrDefault();
              
                foreach (var item in list)
                {

                    Model.OrderDetail orderDetail = new Model.OrderDetail()
                    {
                    OrderDetail_Id = Convert.ToInt32(CreateId.CreateNum()),
                    order_no = orders.orderId,
                        Pid = item.ProductId,
                        pname = item.Pname,
                        pnum = item.Pcount,
                        p_price = item.Price,
                        subtotal = item.Pcount * item.Price
                    };
                    if (orderDetailBll.addEntity(orderDetail))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                   
                }
            cartsBll.EmptyCarts(_userId);

            return Json(new { flag1 =flag}, JsonRequestBehavior.AllowGet);





        }
        public ActionResult SelectOrder()
        {
            int _userId = Convert.ToInt32(Session["userId"]);
            var list = ordersBll.LoadEntity(m => m.userId == _userId&&m.orderState=="未完成").ToList();
            return View(list);
        }
        public ActionResult OrderDetail(int Id)
        {
          
            var list = orderDetailBll.LoadEntity(m => m.order_no == Id);
            return View(list);
        }

    }
}
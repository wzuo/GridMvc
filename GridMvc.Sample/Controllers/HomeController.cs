﻿using System.Linq;
using System.Web.Mvc;
using GridMvc.Sample.Models;
using GridMvc.Sample.Models.Grids;

namespace GridMvc.Sample.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            var repository = new OrdersRepository();
            ViewBag.ActiveMenuTitle = "Demo";
            ViewBag.ColumnTitle = "qweqwe";
            return View(new OrdersGrid(repository.GetAll()));
        }

        public ActionResult About()
        {
            ViewBag.ActiveMenuTitle = "About";
            return View();
        }

        [HttpPost]
        public JsonResult GetOrder(int id)
        {
            var repository = new OrdersRepository();
            Order order = repository.GetById(id);
            if (order == null)
                return Json(new { Status = 0, Message = "Not found" });

            return Json(new { Status = 1, Message = "Ok", Content = RenderPartialViewToString("_OrderInfo", order) });
        }

        [HttpPost]
        public JsonResult GetCustomersNames()
        {
            var repository = new CustomersRepository();
            var allItems = repository.GetAll().Select(c => c.CompanyName);
            return Json(new { Items = allItems });
        }

        [HttpGet]
        public ActionResult AjaxPaging()
        {
            var repository = new OrdersRepository();
            ViewBag.ActiveMenuTitle = "AjaxPaging";

            return View("Index", new OrdersAjaxPagingGrid(repository.GetAll(), 1, false));
        }

        public JsonResult GetOrdersGridRows(int page)
        {
            var repository = new OrdersRepository();
            var grid = new OrdersAjaxPagingGrid(repository.GetAll(), page, true);

            return Json(new
            {
                Html = RenderPartialViewToString("_OrdersGrid", grid),
                HasItems = grid.DisplayingItemsCount >= grid.Pager.PageSize
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
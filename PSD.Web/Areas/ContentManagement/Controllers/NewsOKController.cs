using PSD.Web.Areas.ContentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Controllers
{
    public class NewsOKController : PSD.Web.Controllers._BaseWebController
    {
        public NewsOKController():
            base("NewsController")
        {

        }
        // GET: ContentManagement/NewsOK
        public ActionResult Index()
        {
            return View();
        }

        // GET: ContentManagement/NewsOK/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContentManagement/NewsOK/Create
        public ActionResult Create()
        {
            NewsViewModel model = new NewsViewModel();
            return View(model);
        }

        // POST: ContentManagement/NewsOK/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ContentManagement/NewsOK/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContentManagement/NewsOK/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ContentManagement/NewsOK/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContentManagement/NewsOK/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

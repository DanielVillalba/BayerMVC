using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;
using PSD.Web.Areas.Content.Models;
using PSD.Web.Helpers;

namespace PSD.Web.Areas.Content.Controllers
{
    public class NewsController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.NewsController controller;

        public NewsController()
            : base("NewsController")
        {
            controller = new Controller.Content.NewsController(Configurations);
        }
        public ActionResult Index()
        {
            List<News> newsList = new List<News>();
            if (Identity.CurrentUser.IsInRole(Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation))
            {
                newsList = controller.RetrieveAll()
                                     .Where(x => x.IsPublished)
                                     .OrderByDescending(x => x.Id)
                                     .ToList();
            }
            else if (Identity.CurrentUser.IsInRole(Controller.UserRole.CustomerDistributorOperation + "," + Controller.UserRole.CustomerDistributorView))
            {
                newsList = controller.RetrieveAll()
                                     .Where(x => x.IsPublished && x.IsDistributorVisible)
                                     .OrderByDescending(x => x.Id)
                                     .ToList();
            }
            else if (Identity.CurrentUser.IsInRole(Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView))
            {
                // checking subdistributor type, if farmer or else
                if(string.Equals(Identity.CurrentUser.UserType, "Agricultor"))
                    newsList = controller.RetrieveAll()
                                         .Where(x => x.IsPublished && x.IsFarmerVisible)
                                         .OrderByDescending(x => x.Id)
                                         .ToList();
                else
                    newsList = controller.RetrieveAll()
                                         .Where(x => x.IsPublished && x.IsSubdistributorVisible)
                                         .OrderByDescending(x => x.Id)
                                         .ToList();
            }
            else if (!Identity.CurrentUser.IsLogged)
            {
                newsList = controller.RetrieveAll()
                                     .Where(x => x.IsPublished && x.IsNonAuthenticatedVisible)
                                     .OrderByDescending(x => x.Id)
                                     .ToList();
            }

            return View(newsList);
        }

        public ActionResult Detail(string titleId = "")
        {
            List<News> newsFeed = new List<News>();
            if (Identity.CurrentUser.IsInRole(Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation))
            {
                newsFeed = controller.RetrieveAll()
                                     .Where(x => x.IsPublished)
                                     .ToList();
            }
            else if (Identity.CurrentUser.IsInRole(Controller.UserRole.CustomerDistributorOperation + "," + Controller.UserRole.CustomerDistributorView))
            {
                newsFeed = controller.RetrieveAll()
                                     .Where(x => x.IsPublished && x.IsDistributorVisible)
                                     .ToList();
            }
            else if (Identity.CurrentUser.IsInRole(Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView))
            {
                // checking subdistributor type, if farmer or else
                if (string.Equals(Identity.CurrentUser.UserType, "Agricultor"))
                    newsFeed = controller.RetrieveAll()
                                         .Where(x => x.IsPublished && x.IsFarmerVisible)
                                         .ToList();
                else
                    newsFeed = controller.RetrieveAll()
                                         .Where(x => x.IsPublished && x.IsSubdistributorVisible)
                                         .ToList();
            }
            else if (!Identity.CurrentUser.IsLogged)
            {
                newsFeed = controller.RetrieveAll()
                                     .Where(x => x.IsPublished && x.IsNonAuthenticatedVisible)
                                     .ToList();
            }
            //var newsFeed = controller.RetrieveAll().Where(x => x.IsPublished).OrderByDescending(x => x.PublishDate.Value).ToList();
            News newsDetail = newsFeed.Where(x => x.URLId.Equals(titleId)).FirstOrDefault();

            var previous = Utils.GetPrevious<News>(newsFeed, newsDetail);
            var next = Utils.GetNext<News>(newsFeed, newsDetail);

            DetailViewModel model = new  DetailViewModel()
            {
                DetailNews = newsDetail,
                PreviousNews = previous.Id != newsDetail.Id ? previous : null,  //providing different news only
                NextNews = next.Id != newsDetail.Id && next.Id != previous.Id ? next : null  //providing different news only  
            };
            return View(model);
        }
    }
}
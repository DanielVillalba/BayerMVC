using Newtonsoft.Json;
using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.DistributorFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.PipelineAndFilteringTest.Controllers
{
    public class DistributorController : Web.Controllers._BaseWebController
    {
        private Controller.DistributorController controller;
        public DistributorController()
        : base("DistributorController")
        {
            controller = new Controller.DistributorController(Configurations);
        }

        public ActionResult FilterByZone()
        {
            // get the Distributor list
            var distributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            DistributorFilteringPipeline distributorPipeline = new DistributorFilteringPipeline();

            // Register the filters to be executed
            distributorPipeline.Register(new ZoneFilter("40R"));

            // Start pipeline processing
            var filteredDistributors = distributorPipeline.Process(distributorList);

            // At this point fetch data from DB
            var listedDistributors = filteredDistributors.ToList();


            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedDistributors.Select(x => new Distributor
            {
                Id = x.Id,
                AddressId = x.AddressId,
                BusinessName = x.BusinessName,
                CommercialName = x.CommercialName,
                IdB = x.IdB,
                WebSite = x.WebSite,
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByZones()
        {
            // get the Distributor list
            var distributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            DistributorFilteringPipeline distributorPipeline = new DistributorFilteringPipeline();

            // Register the filters to be executed
            List<string> zones = new List<string> { "40R" };
            distributorPipeline.Register(new ZoneListFilter(null));

            // Start pipeline processing
            var filteredDistributors = distributorPipeline.Process(distributorList);

            // At this point fetch data from DB
            var listedDistributors = filteredDistributors.ToList();


            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedDistributors.Select(x => new Distributor
            {
                Id = x.Id,
                AddressId = x.AddressId,
                BusinessName = x.BusinessName,
                CommercialName = x.CommercialName,
                IdB = x.IdB,
                WebSite = x.WebSite,
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByState()
        {
            // get the Distributor list
            var distributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            DistributorFilteringPipeline distributorPipeline = new DistributorFilteringPipeline();

            // Register the filters to be executed
            distributorPipeline.Register(new StateFilter(1));

            // Start pipeline processing
            var filteredDistributors = distributorPipeline.Process(distributorList);

            // At this point fetch data from DB
            var listedDistributors = filteredDistributors.ToList();


            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedDistributors.Select(x => new Distributor
            {
                Id = x.Id,
                AddressId = x.AddressId,
                BusinessName = x.BusinessName,
                CommercialName = x.CommercialName,
                IdB = x.IdB,
                WebSite = x.WebSite,
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }
    }
}
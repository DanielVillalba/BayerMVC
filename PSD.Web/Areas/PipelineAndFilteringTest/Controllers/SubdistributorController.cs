using Newtonsoft.Json;
using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.PipelineAndFilteringTest.Controllers
{
    public class SubdistributorController : Web.Controllers._BaseWebController
    {
        private Controller.SubdistributorController controller;
        public SubdistributorController()
            : base("SubdistributorController")
        {
            controller = new Controller.SubdistributorController(Configurations);
        }

        public ActionResult FilterByZone()
        {
            // get the Distributor list
            var subdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            SubdistributorFilteringPipeline subdistributorPipeline = new SubdistributorFilteringPipeline();

            // Register the filters to be executed
            subdistributorPipeline.Register(new ZoneFilter("40R"));

            // Start pipeline processing
            var filteredSubdistributors = subdistributorPipeline.Process(subdistributorList);

            // At this point fetch data from DB
            var listedSubdistributors = filteredSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 

            var result = listedSubdistributors.Select(x => new Subdistributor
            {
                Id = x.Id,
                BNAddressId = x.BNAddressId,
                BNLegalRepresentative = x.BNLegalRepresentative,
                BusinessName = x.BusinessName,
                IdB = x.IdB,
                RTVCreator_BayerEmployeeId = x.RTVCreator_BayerEmployeeId,
                Type = x.Type,
                WebSite = x.WebSite
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByZones()
        {
            // get the Distributor list
            var subdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            SubdistributorFilteringPipeline subdistributorPipeline = new SubdistributorFilteringPipeline();

            // Register the filters to be executed
            List<string> zones = new List<string> { "40R" };
            subdistributorPipeline.Register(new ZoneListFilter(zones));

            // Start pipeline processing
            var filteredSubdistributors = subdistributorPipeline.Process(subdistributorList);

            // At this point fetch data from DB
            var listedSubdistributors = filteredSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 

            var result = listedSubdistributors.Select(x => new Subdistributor
            {
                Id = x.Id,
                BNAddressId = x.BNAddressId,
                BNLegalRepresentative = x.BNLegalRepresentative,
                BusinessName = x.BusinessName,
                IdB = x.IdB,
                RTVCreator_BayerEmployeeId = x.RTVCreator_BayerEmployeeId,
                Type = x.Type,
                WebSite = x.WebSite
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByState()
        {
            // get the Distributor list
            var subdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            SubdistributorFilteringPipeline subdistributorPipeline = new SubdistributorFilteringPipeline();

            // Register the filters to be executed
            subdistributorPipeline.Register(new StateFilter(9));

            // Start pipeline processing
            var filteredSubdistributors = subdistributorPipeline.Process(subdistributorList);

            // At this point fetch data from DB
            var listedSubdistributors = filteredSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 

            var result = listedSubdistributors.Select(x => new Subdistributor
            {
                Id = x.Id,
                BNAddressId = x.BNAddressId,
                BNLegalRepresentative = x.BNLegalRepresentative,
                BusinessName = x.BusinessName,
                IdB = x.IdB,
                RTVCreator_BayerEmployeeId = x.RTVCreator_BayerEmployeeId,
                Type = x.Type,
                WebSite = x.WebSite
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByRelatedByActiveContractDistributorId()
        {
            // get the Distributor list
            var subdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            SubdistributorFilteringPipeline subdistributorPipeline = new SubdistributorFilteringPipeline();

            // Register the filters to be executed
            subdistributorPipeline.Register(new RelatedByActiveContractDistributorIdFilter(1));

            // Start pipeline processing
            var filteredSubdistributors = subdistributorPipeline.Process(subdistributorList);

            // At this point fetch data from DB
            var listedSubdistributors = filteredSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 

            var result = listedSubdistributors.Select(x => new Subdistributor
            {
                Id = x.Id,
                BNAddressId = x.BNAddressId,
                BNLegalRepresentative = x.BNLegalRepresentative,
                BusinessName = x.BusinessName,
                IdB = x.IdB,
                RTVCreator_BayerEmployeeId = x.RTVCreator_BayerEmployeeId,
                Type = x.Type,
                WebSite = x.WebSite
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }
    }
}
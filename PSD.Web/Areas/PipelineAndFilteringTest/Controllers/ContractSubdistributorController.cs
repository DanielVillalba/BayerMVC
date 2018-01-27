using Newtonsoft.Json;
using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.PipelineAndFilteringTest.Controllers
{
    public class ContractSubdistributorController : Web.Controllers._BaseWebController
    {
        private Controller.Contracts.ContractsSubdistributorController controller;
        public ContractSubdistributorController()
            : base("ContractSubdistributorController")
        {
            controller = new Controller.Contracts.ContractsSubdistributorController(Configurations);
        }

        public ActionResult FilterByRegisteredZoneName()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            contractSubdistributorPipelline.Register(new RegisteredZoneNameFilter("40R"));  //40R

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByRegisteredZoneNameList()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            List<string> zoneNames = new List<string> { "40R", "10R" };
            contractSubdistributorPipelline.Register(new RegisteredZoneNameListFilter(zoneNames));  //40R

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterBySubdistributorId()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            contractSubdistributorPipelline.Register(new SubdistributorIdFilter(1));

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterBySubdistributorIdList()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            List<int> subdistributorIds = new List<int> { 1, 3, 5 };
            contractSubdistributorPipelline.Register(new SubdistributorIdListFilter(subdistributorIds));

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByContractStatusId()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            contractSubdistributorPipelline.Register(new StatusIdFilter(4));

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByContractStatusIdList()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            List<int> statusIds = new List<int> { 4, 6, 1 };
            contractSubdistributorPipelline.Register(new StatusIdListFilter(statusIds));

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByYear()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            contractSubdistributorPipelline.Register(new YearFilter(2017));

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByStateId()
        {
            // get the ContractDistributor list
            var contractSubdistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractSubdistributorFilteringPipeline contractSubdistributorPipelline = new ContractSubdistributorFilteringPipeline();

            // Register the filters to be executed
            contractSubdistributorPipelline.Register(new StateFilter(9));

            // Start pipeline processing
            var filteredcontractSubdistributors = contractSubdistributorPipelline.Process(contractSubdistributorList);

            // At this point fetch data from DB
            var listedContractSubdistributors = filteredcontractSubdistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractSubdistributors.Select(x => new ContractSubdistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractSubdistributorStatusId = x.ContractSubdistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

    }

}
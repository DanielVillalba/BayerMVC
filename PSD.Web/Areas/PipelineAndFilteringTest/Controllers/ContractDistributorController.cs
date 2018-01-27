using Newtonsoft.Json;
using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.PipelineAndFilteringTest.Controllers
{
    public class ContractDistributorController : Web.Controllers._BaseWebController
    {
        private Controller.Contracts.ContractsDistributorController controller;
        public ContractDistributorController()
            : base("ContractDistributorController")
        {
            controller = new Controller.Contracts.ContractsDistributorController(Configurations);
        }

        public ActionResult FilterByRegisteredZoneName()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            contractDistributorPipelline.Register(new RegisteredZoneNameFilter("40R"));  //40R

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByRegisteredZoneNameList()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            List<string> registeredZones = new List<string>{ "20R", "40R" };
            contractDistributorPipelline.Register(new RegisteredZoneNameListFilter(registeredZones));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByDistributorId()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            contractDistributorPipelline.Register(new DistributorIdFilter(1));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByDistributorIdList()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            List<int> distributorIds = new List<int> {1, 2, 3};
            contractDistributorPipelline.Register(new DistributorIdListFilter(distributorIds));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByContractStatusId()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            contractDistributorPipelline.Register(new StatusIdFilter(3));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByContractStatusIdList()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            List<int> statusIds = new List<int> { 3, 5, 2};
            contractDistributorPipelline.Register(new StatusIdListFilter(statusIds));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByYear()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            contractDistributorPipelline.Register(new YearFilter(2017));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

        public ActionResult FilterByStateId()
        {
            // get the ContractDistributor list
            var contractDistributorList = controller.RetrieveAllToFilter();

            // Construct the pipeline
            ContractDistributorFilteringPipeline contractDistributorPipelline = new ContractDistributorFilteringPipeline();

            // Register the filters to be executed
            contractDistributorPipelline.Register(new StateFilter(9));

            // Start pipeline processing
            var filteredcontractDistributors = contractDistributorPipelline.Process(contractDistributorList);

            // At this point fetch data from DB
            var listedContractDistributors = filteredcontractDistributors.ToList();

            // just sending the result for testing purposes, cannot serialize the entire object because of the circular references present from EF. 
            // (this is the same behavior with previous approach)

            var result = listedContractDistributors.Select(x => new ContractDistributor
            {
                Id = x.Id,
                IdB = x.IdB,
                ContractDistributorStatusId = x.ContractDistributorStatusId,
                RegisteredZoneName = x.RegisteredZoneName,
                RegisteredRegionName = x.RegisteredRegionName
            });

            string JsonResult = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return Content(JsonResult);
        }

    }
}
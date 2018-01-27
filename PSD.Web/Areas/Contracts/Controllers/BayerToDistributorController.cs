using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Contracts.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView)]
    public class BayerToDistributorController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Contracts.ContractsDistributorController controller;
        public BayerToDistributorController()
            : base("BayerToDistributorController")
        {
            controller = new Controller.Contracts.ContractsDistributorController(Configurations);
        }


        public ActionResult Index()
        {
            List<ContractDistributor> items = controller.FilteredItems();
            if (controller.ResultManager.IsCorrect)
            {
                return View(items);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int Id = -1)
        {
            //sys validations
            if (Id == -1)
            {
                NotifyUser(errorDefault, controllerTraceId + "Detail.111 No se recibió el parámetro Id");
                return RedirectToAction("Index");
            }

            ContractDistributor item = controller.Retrieve(Id);
            if (item != null && controller.ResultManager.IsCorrect)
            {
                return View(item);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Create(int Id = -1)
        {
            //sys validations
            if(Id == -1)
            {
                NotifyUser(errorDefault, controllerTraceId + "Create.111 No se recibió el parámetro id del Distribuidor");
                return RedirectToAction("Index");
            }

            ContractDistributor item = ContractDistributor.NewEmpty();
            PSD.Controller.DistributorController distributorController = new Controller.DistributorController(Configurations);
            item.Distributor = distributorController.RetrieveDistributor(Id);
            item.DistributorId = Id;
            item.RegisteredZoneName = item.Distributor.Address.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name;
            item.RegisteredRegionName = item.Distributor.Address.AddressColony.AddressPostalCode.AddressMunicipality.Zone.RegionName;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Create(ContractDistributor model)
        {                
            if (controller.Create(model, true) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contracto creado correctamente");
                return RedirectToAction("Index");
            }

            PSD.Controller.DistributorController distributorController = new PSD.Controller.DistributorController(Configurations);
            model.Distributor = distributorController.RetrieveDistributor(model.DistributorId);
            
            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Edit(int Id)
        {
            ContractDistributor itemToEdit = controller.Retrieve(Id);
            if (controller.ResultManager.IsCorrect)
            {
                return View(itemToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Edit(ContractDistributor model, string postAction = "")
        {
            if (model == null)
            {
                return RedirectToError(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
            }

            bool sendToDistributorReview = false;
            switch (postAction)
            {
                case "updateOnly": sendToDistributorReview = false; break;
                case "updateAndSendToDistributor": sendToDistributorReview = true; break;
            }
            if (controller.Update(model, sendToDistributorReview) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "Edit.831 Contrato '[" + model.Id + "]:" + model.IdB + "' actualizado");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Approve(int Id)
        {
            ContractDistributor item = controller.Retrieve(Id);
            if (!controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            if(item.AmountGoalTotal < item.AmountGoalTotalPre)//amount indicated by distributor is less than the indicated by bayer?
            {//yes
                //validate approver is GRV, RTV cannot approve on this case
                if(!Identity.CurrentUser.IsInRole(PSD.Controller.UserRole.AppAdmin + "," +  PSD.Controller.UserRole.EmployeeManagerOperation))
                {
                    NotifyUser(messageError: "Los montos indicados por el Distribuidor son menores, se necesita aprobación de GRV");
                    return RedirectToAction("Detail", new { Id = Id });
                }
            }

            if (controller.Approve(item) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "Approve.831 Contrato '[" + item.Id + "]:" + item.IdB + "' actualizado");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Detail", new { Id = Id });
        }
        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult RequestApprovalDistributor(int Id)
        {
            if (controller.SendToDistributor(Id) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Detail", new { Id = Id });
        }
        [Authorization(allowedRoles: Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult RequestApprovalGRV(int Id)
        {
            if (controller.RequestApprovalGRV(Id) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Detail", new { Id = Id });
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Delete(int Id)
        {
            if (controller.Delete(Id) || controller.ResultManager.IsCorrect)
                NotifyUser(messageOk: "Convenio con distribuidor eliminado correctamente");
            else
                NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
    }
}
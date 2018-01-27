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
    public class ToSubdistributorController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Contracts.ContractsSubdistributorController controller;
        public ToSubdistributorController()
            : base("ToSubdistributorController")
        {
            controller = new Controller.Contracts.ContractsSubdistributorController(Configurations);
        }


        public ActionResult Index()
        {
            List<ContractSubdistributor> items = controller.FilteredItems();
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
            if (Id == -1 || Id == 0)
            {
                NotifyUser(errorDefault, controllerTraceId + "Detail.111 No se recibió el parámetro Id");
                return RedirectToAction("Index");
            }

            ContractSubdistributor item = controller.Retrieve(Id);
            if (item != null && controller.ResultManager.IsCorrect)
            {
                return View(item);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");

        }
        [Authorization(allowedRoles: Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Create(int Id = -1)
        {
            if(Id == -1)
            {
                NotifyUser(errorDefault, controllerTraceId + ".Create.111 No se recibio el parámetro id");
                return RedirectToAction("Index");
            }
            
            PSD.Controller.DistributorController distributorController = new Controller.DistributorController(Configurations);
            ViewBag.AvailableDistributors = GetDistributors(distributorController.GetSubdistributorZones(Id));

            ContractSubdistributor item = controller.InitializeNew(Id);
            
            return View(item);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(allowedRoles: Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Create(ContractSubdistributor model, List<int> selectedDistributors = null)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: "No se recibió el modelo");
            }
            else
            {
                if (selectedDistributors == null)
                {
                    NotifyUser(messageError: "Se debe seleccionar al menos un distribuidor", messageDebug: "No se recibió el parámetro 'selectedDistributors'");
                }
                else
                {
                    //fill in selected distributors in model
                    model.DistributorPurchases = new List<DistributorPurchasesXContractSubdistributor>();
                    foreach (int item in selectedDistributors)
                    {
                        model.DistributorPurchases.Add(new DistributorPurchasesXContractSubdistributor() { ContractSubdistributorId = model.Id, DistributorId = item });
                    }
                    
                    if (controller.Create(model, true) || controller.ResultManager.IsCorrect)
                    {
                        NotifyUser(resultManager: controller.ResultManager);
                        return RedirectToAction("Index");
                    }
                }
            }

            PSD.Controller.DistributorController distributorController = new Controller.DistributorController(Configurations);
            ViewBag.AvailableDistributors = GetDistributors(distributorController.GetSubdistributorZones(model.Id));

            PSD.Controller.SubdistributorController subdistributorController = new Controller.SubdistributorController(Configurations);
            model.Subdistributor = subdistributorController.RetrieveSubdistributor(model.SubdistributorId);
            model.SubdistributorId = model.SubdistributorId;

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
        /*
        public ActionResult Edit(int Id)
        {
            ContractSubdistributor itemToEdit = controller.Retrieve(Id);
            if (controller.ResultManager.IsCorrect)
            {
                PSD.Controller.DistributorController distributorController = new Controller.DistributorController(Configurations);
                List<SelectListItem> auxAvailableDistributors = GetDistributors(distributorController.GetSubdistributorZone(Id));
                foreach (SelectListItem itemAvailable in auxAvailableDistributors)
                {
                    foreach (DistributorPurchasesXContractSubdistributor item in itemToEdit.DistributorPurchases)
                    {
                        if(itemAvailable.Value == item.DistributorId.ToString())
                        {
                            itemAvailable.Selected = true;
                            break;
                        }
                    }
                }
                ViewBag.AvailableDistributors = auxAvailableDistributors;
                
                return View(itemToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");

        }
        */

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Delete(int Id)
        {
            if (controller.Delete(Id) || controller.ResultManager.IsCorrect)
                NotifyUser(messageOk: "Convenio con subdistribuidor eliminado correctamente");
            else
                NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        /*
        [Authorization(allowedRoles: Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Approve(int Id)
        {
            ContractSubdistributor item = controller.Retrieve(Id);
            if (!controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            if (item.AmountGoalTotal < item.AmountGoalTotalPre)//amount indicated by distributor is less than the indicated by bayer?
            {//yes
                //validate approver is GRV, RTV cannot approve on this case
                if (!Identity.CurrentUser.IsInRole(PSD.Controller.UserRole.AppAdmin + "," + PSD.Controller.UserRole.EmployeeManagerOperation))
                {
                    NotifyUser(messageError: "Los montos indicados por el Subdistribuidor son menores, se necesita aprobación de GRV");
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
        */
    }
}
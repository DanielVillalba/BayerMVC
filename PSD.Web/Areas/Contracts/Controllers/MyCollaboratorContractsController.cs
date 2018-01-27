using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;
using PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters;

namespace PSD.Web.Areas.Contracts.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.CustomerDistributorOperation + "," + Controller.UserRole.CustomerDistributorView)]
    public class MyCollaboratorContractsController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Contracts.MyCollaboratorContractsController controller;
        public MyCollaboratorContractsController()
            : base("MyCollaboratorContractsController")
        {
            controller = new Controller.Contracts.MyCollaboratorContractsController(Configurations);
        }

        public ActionResult Index()
        {
            // applying filtering pipeline
            List<Subdistributor> items = null;

            items = controller.RetrieveAll();

            if (!controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Index.311: Error while retrieving ContractSubdistributor list from DB.");
                return RedirectToHome();
            }

            return View(items);
        }

        public ActionResult Detail(int Id = -1)
        {
            //sys validations
            if (Id == -1)
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

        public ActionResult RegisterPurchase(int Id)
        {
            if(!Configurations.BenefitProgram_Coupon_IsOpen || (!Configurations.BenefitProgram_Coupon_S1_IsOpen && !Configurations.BenefitProgram_Coupon_S2_IsOpen))
            {
                NotifyUser(messageError: "Ha concluido la temporada de captura de compras");
                return RedirectToAction("Detail", new { Id = Id });
            }

            ViewBag.CanRegisterS1 = Configurations.BenefitProgram_Coupon_S1_IsOpen;
            ViewBag.CanRegisterS2 = Configurations.BenefitProgram_Coupon_S2_IsOpen;

            ContractSubdistributor auxContractSubdistributor = controller.Retrieve(Id);
            return View(auxContractSubdistributor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterPurchase(ContractSubdistributor model, int selectedMonth = -1, decimal purchaseAmount = 0)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: "No se recibió el modelo");
            }
            else
            {
                if (selectedMonth == -1)
                {
                    NotifyUser(messageError: "Se debe seleccionar el mes", messageDebug: "No se recibió el parámetro 'selectedDistributors'");
                }
                else
                {
                    if (purchaseAmount == 0)
                    {
                        NotifyUser(messageError: "El monto no puede ser cero", messageDebug: "No se recibió el parámetro 'selectedDistributors'");
                    }
                    else
                    {
                        if (controller.RegisterPurchase(model, selectedMonth, purchaseAmount) && controller.ResultManager.IsCorrect)
                        {
                            NotifyUser(messageOk: "La compra ha sido registrada");
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            PSD.Controller.DistributorController distributorController = new Controller.DistributorController(Configurations);
            ViewBag.AvailableDistributors = GetDistributors(distributorController.GetSubdistributorZones(model.Id));

            PSD.Controller.SubdistributorController subdistributorController = new Controller.SubdistributorController(Configurations);
            model.Subdistributor = subdistributorController.RetrieveSubdistributor(model.SubdistributorId);
            model.SubdistributorId = model.SubdistributorId;

            ViewBag.CanRegisterS1 = Configurations.BenefitProgram_Coupon_S1_IsOpen;
            ViewBag.CanRegisterS2 = Configurations.BenefitProgram_Coupon_S1_IsOpen;

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
        public ActionResult Edit(int Id = -1)
        {
            //sys validations
            if (Id == -1)
            {
                NotifyUser(errorDefault, controllerTraceId + "Detail.111 No se recibió el parámetro Id");
                return RedirectToAction("Index");
            }

            if (!Configurations.BenefitProgram_Coupon_IsOpen || (!Configurations.BenefitProgram_Coupon_S1_IsOpen && !Configurations.BenefitProgram_Coupon_S2_IsOpen))
            {
                NotifyUser(messageError: "Ha concluido la temporada de captura de compras");
                return RedirectToAction("Detail", new { Id = Id });
            }

            ViewBag.CanRegisterS1 = Configurations.BenefitProgram_Coupon_S1_IsOpen;
            ViewBag.CanRegisterS2 = Configurations.BenefitProgram_Coupon_S2_IsOpen;
                        
            ContractSubdistributor item = controller.Retrieve(Id);
            if (item != null && controller.ResultManager.IsCorrect)
            {
                foreach (DistributorPurchasesXContractSubdistributor purchaseItem in item.DistributorPurchases)
                {
                    if (purchaseItem.DistributorId == CurrentUser.ParentId)
                    {
                        return View(purchaseItem);
                    }
                }
                NotifyUser(messageError: errorDefault, messageDebug: "No se encontro un registro de DistributorPurchasesXContractSubdistributor para el usuario logueado con parentId '" + CurrentUser.ParentId + "'");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistributorPurchasesXContractSubdistributor model)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: "No se recibió el modelo");
            }
            else
            {
                if (controller.Edit(model) && controller.ResultManager.IsCorrect)
                {
                    NotifyUser(messageOk: "Los montos han sido actualizados");
                    return RedirectToAction("Index");
                }
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
    }
}
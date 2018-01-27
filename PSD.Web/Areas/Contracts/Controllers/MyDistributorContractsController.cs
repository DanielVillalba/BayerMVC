using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Contracts.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.CustomerDistributorOperation + "," + Controller.UserRole.CustomerDistributorView)]
    public class MyDistributorContractsController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Contracts.MyDistributorContractsController controller;
        public MyDistributorContractsController()
            : base("MyDistributorContractsController")
        {
            controller = new Controller.Contracts.MyDistributorContractsController(Configurations);
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
        public ActionResult Edit(int Id = -1)
        {
            //sys validations
            if (Id == -1)
            {
                NotifyUser(errorDefault, controllerTraceId + "Edit.111 No se recibió el parámetro Id");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractDistributor model)
        {
            //sys validations
            if (model == null)
            {
                NotifyUser(errorDefault, controllerTraceId + "Edit.111 No se recibió el modelo");
                return RedirectToAction("Index");
            }

            if (controller.UpdateAndApprove(model) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "Approve.831 Contrato '[" + model.Id + "]:" + model.IdB + "' actualizado");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Detail", new { Id = model.Id });
        }
        public ActionResult Approve(int Id = -1)
        {
            //sys validations
            if (Id == -1)
            {
                NotifyUser(errorDefault, controllerTraceId + "Approve.111 No se recibió el parámetro Id");
                return RedirectToAction("Index");
            }

            if (controller.Approve(Id) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "Approve.831 Contrato '" + Id + "' actualizado");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Detail", new { Id = Id });
        }
    }
}
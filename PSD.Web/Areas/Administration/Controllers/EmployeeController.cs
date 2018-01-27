using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Administration.Controllers
{
    [Authorization(allowedRoles: "sysadmin,appadmin")]
    public class EmployeeController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.EmployeeController controller;

        public EmployeeController()
            : base ("EmployeeController")
        {
            controller = new Controller.EmployeeController(Configurations);///TODO: check when at lifetime of page to set hosturl, at constructor time the Request Object is null
        }

        public ActionResult Index()
        {
            bool vf = false;
            string result = string.Empty;
            IEnumerable<BayerEmployee> auxEmployees = null;
            try
            {
                auxEmployees = controller.RetrieveEmployeeList();
                result = "";
                vf = true;
            }
            catch (Exception ex)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Index.311: Error while retrieving employee list from DB.", ex: ex);
            }

            if (vf) { }

            return View(auxEmployees);
        }

        public ActionResult EmployeeDetail(int Id = -1, string selectedTab = "")
        {
            if (Id == -1)
            {
                NotifyUser(message: "Seleccione primero el empleado a consultar haciendo click en su Id", messageDebug: controllerTraceId + "EmployeeDetail.111: no Id was received");
                return RedirectToAction("Index");
            }

            bool vf = false;
            string result = string.Empty;
            BayerEmployee auxEmployee = null;
            try
            {
                auxEmployee = controller.RetrieveEmployee(Id);
                result = "";
                vf = true;
            }
            catch (Exception ex)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "EmployeeDetail.311: Error while retrieving item '" + Id + "' from DB. Exception:", ex: ex);
            }

            if (vf)
            {
                if (auxEmployee == null)
                {
                    vf = false;
                    NotifyUser(messageError: "No se encontro el empleado", messageDebug: controllerTraceId + "EmployeeDetail.511: Error while retrieving item '" + Id + "', item id not found in DB");
                }
            }

            //ViewBag.SelectedTab = selectedTab;
            ViewBag.EmployeeZones = controller.RetrieveEmployeeZones(auxEmployee.MunicipalitiesXEmployee);
            return View(auxEmployee);
        }

        public ActionResult EmployeeCreate()
        {
            ViewBag.EployeeRoles = PSD.Controller._BaseController.CatUserRolesBayer();
            return View(new BayerEmployee());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeCreate(BayerEmployee model, List<int?> selectedZone)
        {
            if (controller.CreateBayerEmployee(model, selectedZone) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.831 employee created", resultDetails: controller.ErrorManager.ToStringList());
                return RedirectToAction("Index");
            }
            NotifyUser(messageError: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.811", resultDetails: controller.ErrorManager.ToStringList());
            return View(model);
        }
        public ActionResult Edit(int Id = -1)
        {
            if (Id == -1)
            {
                NotifyUser(message: "Seleccione primero el usuario Bayer a editar haciendo click en su Id", messageDebug: controllerTraceId + "Edit.111: no Id was received");
                return RedirectToAction("Index");
            }

            BayerEmployee auxEmployee = null;
            auxEmployee = controller.RetrieveEmployee(Id);

            if (auxEmployee == null || !controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }
            
            ViewBag.EmployeeZoneIds = EmployeeMunicipalitiesToZonesSelectList(auxEmployee.MunicipalitiesXEmployee);
            
            return View(auxEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BayerEmployee model, List<int?> selectedZones)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Edit.511 User object was not received");
                return RedirectToError();
            }

            if (controller.UpdateBayerEmployee(model, selectedZones) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            //if we reached this point, there was an error. Notify error before using controller again (the error message will be lost if we don't notify before)
            NotifyUser(resultManager: controller.ResultManager);
            
            //retrieve again user details (model doesn't contain all ada anymore, for example: roles object was lost)
            BayerEmployee auxEmployee = null;
            auxEmployee = controller.RetrieveEmployee(model.Id);
            if (auxEmployee == null || !controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeZoneIds = EmployeeMunicipalitiesToZonesSelectList(auxEmployee.MunicipalitiesXEmployee); 
            return View(auxEmployee);
        }
        public ActionResult SendEmailInvitation(int Id = -1)
        {
            if(Id == -1)
            {
                NotifyUser(errorDefault, messageDebug: controllerTraceId + "SendEmailInvitation.111 No userNick was received");
                return RedirectToAction("Index");
            }
            if(controller.SendUserInvitationEmail(Id) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Se ha enviado un correo al usuario");
            }
            else
            {
                NotifyUser(errorDefault, messageDebug: controllerTraceId + "SendEmailInvitation.311 Error while sending email. " + controller.ErrorManager.ToDescriptionString());
            }
            return RedirectToAction("EmployeeDetail", new { Id = Id });
        }

        public ActionResult Delete(int Id)
        {
            if (controller.Delete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Usuario Bayer eliminado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        public ActionResult UserActivate(int Id)
        {
            if (controller.ChangeUserStatus(Id))
            {
                NotifyUser(messageOk: "El usuario ha sido activado exitosamente.");
                return RedirectToAction("EmployeeDetail", new { Id = Id });
            }
            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("EmployeeDetail", new { Id = Id });
        }


        public ActionResult UserDeactivate(int Id)
        {
            if (controller.ChangeUserStatus(Id))
            {
                NotifyUser(messageOk: "El usuario ha sido desactivado exitosamente.");
                return RedirectToAction("EmployeeDetail", new { Id = Id });
            }
            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("EmployeeDetail", new { Id = Id });
        }
    }
}
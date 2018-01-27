using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;
using Newtonsoft.Json;
using PSD.Web.Areas.Administration.Models.Catalog;

namespace PSD.Web.Areas.Administration.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
    public class CatalogController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.CatalogController controller;
        public CatalogController()
            : base("CatalogController")
        {
            controller = new PSD.Controller.CatalogController(Configurations);
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Crops
        public ActionResult CropIndex()
        {
            List<Cat_Crop> crops = controller.CropRetrieveAll();
            if(controller.ResultManager.IsCorrect)
            {
                return View(crops);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
        public ActionResult CropAdd()
        {
            return View(new Cat_Crop());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CropAdd(Cat_Crop item)
        {
            if(controller.CropAdd(item) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Cultivo agregado correctamente");
                return RedirectToAction("CropIndex");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(item);
        }
        public ActionResult CropEdit(int Id)
        {
            Cat_Crop cropToEdit = controller.CropRetrieve(Id);
            if (controller.ResultManager.IsCorrect)
            {
                return View(cropToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CropEdit(Cat_Crop model)
        {
            if (controller.CropUpdate(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Cultivo editado correctamente");
                return RedirectToAction("CropIndex");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
        public ActionResult CropDelete(int Id)
        {
            if (controller.CropDelete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Cultivo eliminado correctamente");
                return RedirectToAction("CropIndex");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CropIndex");
        }

        #endregion

        #region Roles

        public ActionResult RoleIndex()
        {
            List<Cat_UserRole> roles = controller.RoleRetrieveAll();
            if (controller.ResultManager.IsCorrect)
            {
                return View(roles);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        public ActionResult RoleEdit(int Id)
        {
            Cat_UserRole roleToEdit = controller.RoleRetrieve(Id);
            if (controller.ResultManager.IsCorrect)
            {
                return View(roleToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleEdit(Cat_UserRole model)
        {
            if (controller.RoleUpdate(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Role editado correctamente");
                return RedirectToAction("RoleIndex");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        #endregion

        #region Zones

        public ActionResult ZoneIndex()
        {
            List<Cat_Zone> zones = controller.ZoneRetrieveAll();
            if (controller.ResultManager.IsCorrect)
            {
                return View(zones);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        public ActionResult ZoneAdd()
        {
            ViewBag.AddressStates = AddressStates();

            return View(new Cat_Zone());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ZoneAdd(string data)
        {
            string jsonResult;
            var m = JsonConvert.DeserializeObject<Cat_Zone>(data);
            if (controller.ZoneAdd(m) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Zona agregada correctamente");
                jsonResult = "ZoneAdd_OK";
            }
            else
            {
                NotifyUser(resultManager: controller.ResultManager);
                jsonResult = controller.ResultManager.ToDescriptionString();
            }

            return Json(jsonResult);
        }

        public ActionResult ZoneEdit(int Id)
        {
            ViewBag.AddressStates = AddressStates();

            Cat_Zone zoneToEdit = controller.ZoneRetrieve(Id);
            if (controller.ResultManager.IsCorrect)
            {
                return View(zoneToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ZoneEdit(string data)
        {
            string jsonResult;
            var m = JsonConvert.DeserializeObject<Cat_Zone>(data);
            if (controller.ZoneUpdate(m) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Zona editada correctamente");
                jsonResult = "ZoneEdit_OK";
            }
            else
            {
                NotifyUser(resultManager: controller.ResultManager);
                jsonResult = controller.ResultManager.ToDescriptionString();
            }

            return Json(jsonResult);
        }

        public ActionResult ZoneDelete(int Id)
        {
            if (controller.ZoneDelete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Zona eliminada correctamente");
                return RedirectToAction("ZoneIndex");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("ZoneIndex");
        }

        [HttpPost]
        public JsonResult GetMunicipalityData(string selectedStateId)
        {
            int parsedSelectedStateId;
            if (Int32.TryParse(selectedStateId, out parsedSelectedStateId))
            {
                var distributorController = new PSD.Controller.DistributorController(Configurations);
                var result = distributorController.GetMunicipalitiesByState(parsedSelectedStateId)
                                                  .Select(x=> new MunicipalityDataViewModel { MunicipalityId = x.Id, MunicipalityName = x.Name});
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error when tried to retrieve municipality", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region catalog n...
        #endregion

    }
}
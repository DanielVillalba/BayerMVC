using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;
using Newtonsoft.Json;
using PSD.Web.Areas.Administration.Models.Distributor;

namespace PSD.Web.Areas.Administration.Controllers
{
    
    public class DistributorController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.DistributorController controller;

        public DistributorController()
            : base("DistributorController")
        {
            controller = new Controller.DistributorController(Configurations);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView)]
        public ActionResult Index()
        {
            bool vf = false;
            string result = string.Empty;
            IEnumerable<Distributor> auxDistributors = null;
            try
            {
                auxDistributors = controller.FilteredItems();
                result = "";
                vf = true;
            }
            catch (Exception ex)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Index.311: Error while retrieving Distributor list from DB.", ex: ex);
            }

            if (vf) { }

            return View(auxDistributors);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView)]
        public ActionResult DistributorDetail(int Id = -1, string selectedTab = "")
        {
            if (Id == -1)
            {
                NotifyUser(message: "Seleccione primero el distribuidor a consultar haciendo click en su Id", messageDebug: controllerTraceId + "EmployeeDetail.111: no Id was received");
                return RedirectToAction("Index");
            }

            bool vf = false;
            string result = string.Empty;
            Distributor auxDistributor = null;
            try
            {
                auxDistributor = controller.RetrieveDistributor(Id);
                result = "";
                vf = true;
            }
            catch (Exception ex)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "EmployeeDetail.311: Error while retrieving item '" + Id + "' from DB. Exception:", ex: ex);
            }

            if (vf)
            {
                if (auxDistributor == null)
                {
                    vf = false;
                    NotifyUser(messageError: "No se encontro el distribuidor", messageDebug: controllerTraceId + "EmployeeDetail.511: Error while retrieving item '" + Id + "', item id not found in DB");
                }
            }

            //ViewBag.SelectedTab = selectedTab;
            return View(auxDistributor);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
        public ActionResult DistributorCreate()
        {
            ViewBag.AddressStates = AddressStates();
            DistributorEmployee newEmployee = DistributorEmployee.NewEmpty();
            TempData["CropsXMunicipality"] = null; 
            return View(newEmployee);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DistributorCreate(DistributorEmployee model, string postAction = "", int selectedAddressState = -1, List<int> selectedMunicipalities = null, List<int> selectedCrops = null)
        {
            if (model == null)
            {
                return RedirectToError(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
            }

            if (TempData["CropsXMunicipality"] != null)
            {

                //copy the info from TempData into an aux object (cause we cannot use the same object from TempData, it would cause to have two UnitOfWork objects (one from the TempDate and one from the controller object), causing an error on the controller
                ICollection<DistributorCropsXMunicipality> auxCropsXMunicipality = (ICollection<DistributorCropsXMunicipality>)TempData["CropsXMunicipality"];
                foreach(DistributorCropsXMunicipality item in auxCropsXMunicipality)
                {
                    model.Distributor.CropsXMunicipality.Add(new DistributorCropsXMunicipality()
                    {
                        AddressMunicipalityAddressStateId = item.AddressMunicipalityAddressStateId,
                        AddressMunicipalityId = item.AddressMunicipalityId,
                        Cat_CropId = item.Cat_CropId,
                        //Crop = item.Crop,
                        //Municipality = item.Municipality
                    });
                }
                auxCropsXMunicipality = null;
                TempData["CropsXMunicipality"] = model.Distributor.CropsXMunicipality;
            }

            List<AddressMunicipality> auxMunicipalities;

            switch (postAction)
            {
                case "getMunicipalities": //get municipalities for the selected addressState
                    auxMunicipalities = controller.GetMunicipalitiesByState(selectedAddressState);
                    //ViewBag.SelectedPostalCode = auxPostalCode;
                    ViewBag.AvailableMunicipalities = auxMunicipalities;
                    ViewBag.AddressStates = AddressStates(selectedAddressState);
                    return View(model);
                case "addInfluence": //add "area de influencia" to the list
                    auxMunicipalities = controller.GetMunicipalitiesByState(selectedAddressState);
                    foreach (int itemMunicipalityId in selectedMunicipalities)
                    {
                        foreach (int itemCropId in selectedCrops)
                        {
                            AddressMunicipality municipalityName = new AddressMunicipality();
                            foreach (AddressMunicipality aMName in auxMunicipalities)
                            {
                                if (aMName.Id == itemMunicipalityId)
                                {
                                    municipalityName = aMName;
                                    break;
                                }
                            }

                            model.Distributor.CropsXMunicipality.Add(new DistributorCropsXMunicipality()
                                {
                                    AddressMunicipalityAddressStateId = selectedAddressState
                                    ,
                                    Crop = controller.GetCropById(itemCropId)
                                    ,
                                    AddressMunicipalityId = itemMunicipalityId
                                    ,
                                    Municipality = municipalityName
                                    ,
                                    Cat_CropId = itemCropId
                                    ,
                                    DistributorId = model.DistributorId
                                });
                            TempData["CropsXMunicipality"] = model.Distributor.CropsXMunicipality;
                        }
                    }
                    ViewBag.AddressStates = AddressStates();
                    return View(model);
                case "createDistributor":
                    if (controller.CreateDistributor(model) && controller.ResultManager.IsCorrect)
                    {
                        NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.831 employee created", resultDetails: controller.ErrorManager.ToStringList());
                        return RedirectToAction("Index");
                    }
                    break;
                default:
                    break;
            }
            ViewBag.AddressStates = AddressStates();
            NotifyUser(messageError: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.811", resultDetails: controller.ErrorManager.ToStringList());
            return View(model);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
        public ActionResult Edit(int Id)
        {
            ViewBag.AddressStates = AddressStates();
            PSD.Controller.AccountController accountController = new PSD.Controller.AccountController(Configurations);
            DistributorEmployee employee = accountController.GetDistributorEmployee(Id);
            if (employee.Distributor.CropsXMunicipality != null)
            {
                TempData["CropsXMunicipality"] = employee.Distributor.CropsXMunicipality;
            }
            return View(employee);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistributorEmployee model, string postAction = "", int selectedAddressState = -1, List<int> selectedMunicipalities = null, List<int> selectedCrops = null)
        {
            if (model == null)
            {
                return RedirectToError(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
            }

            if (TempData["CropsXMunicipality"] != null)
            {
                model.Distributor.CropsXMunicipality = (ICollection<DistributorCropsXMunicipality>)TempData["CropsXMunicipality"];
                TempData["CropsXMunicipality"] = model.Distributor.CropsXMunicipality;
            }

            List<AddressMunicipality> auxMunicipalities;

            switch (postAction)
            {
                case "getMunicipalities": //get municipalities for the selected addressState
                    auxMunicipalities = controller.GetMunicipalitiesByState(selectedAddressState);
                    //ViewBag.SelectedPostalCode = auxPostalCode;
                    ViewBag.AvailableMunicipalities = auxMunicipalities;
                    ViewBag.AddressStates = AddressStates(selectedAddressState);
                    model.Distributor.Address = controller.Repository.Addresses.Get((int)model.Distributor.AddressId);
                    return View(model);
                case "addInfluence": //add "area de influencia" to the list
                    auxMunicipalities = controller.GetMunicipalitiesByState(selectedAddressState);
                    foreach (int itemMunicipalityId in selectedMunicipalities)
                    {
                        foreach (int itemCropId in selectedCrops)
                        {
                            AddressMunicipality municipalityName = new AddressMunicipality();
                            foreach (AddressMunicipality aMName in auxMunicipalities)
                            {
                                if (aMName.Id == itemMunicipalityId)
                                {
                                    municipalityName = aMName;
                                    break;
                                }
                            }

                            model.Distributor.CropsXMunicipality.Add(new DistributorCropsXMunicipality()
                            {
                                AddressMunicipalityAddressStateId = selectedAddressState
                                ,
                                Crop = controller.GetCropById(itemCropId)
                                ,
                                AddressMunicipalityId = itemMunicipalityId
                                ,
                                Municipality = municipalityName
                                ,
                                Cat_CropId = itemCropId
                                ,
                                DistributorId = model.DistributorId
                            });
                            TempData["CropsXMunicipality"] = model.Distributor.CropsXMunicipality;
                        }
                    }
                    ViewBag.AddressStates = AddressStates();
                    model.Distributor.Address = controller.Repository.Addresses.Get((int)model.Distributor.AddressId);
                    return View(model);
                case "updateDistributor":
                    PSD.Controller.AccountController accountController = new PSD.Controller.AccountController(Configurations);
                    if (accountController.UpdateDistributorEmployee(model) && accountController.ResultManager.IsCorrect)
                    {
                        NotifyUser(messageOk: accountController.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.831 distributor updated", resultDetails: controller.ErrorManager.ToStringList());
                        return RedirectToAction("Index");
                    }
                    break;
                default:
                    break;
            }
            ViewBag.AddressStates = AddressStates();
            model.Distributor.Address = controller.Repository.Addresses.Get((int)model.Distributor.AddressId);
            NotifyUser(messageError: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.811", resultDetails: controller.ErrorManager.ToStringList());
            return View(model);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
        public ActionResult DistributorDelete(int id)
        {
            if (controller.DeleteDistributor(id) || controller.ResultManager.IsCorrect)
                NotifyUser(messageOk: "Distribuidor eliminado correctamente");
            else
                NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        private ActionResult SendEmailInvitation(int Id = -1)
        {
            if (Id == -1)
            {
                NotifyUser(errorDefault, messageDebug: controllerTraceId + "ResendEmailInvitation.111 No userNick was received");
                return RedirectToAction("Index");
            }
            if (!controller.SendUserInvitationEmail(Id) || !controller.ResultManager.IsCorrect)
            {
                NotifyUser(errorDefault, messageDebug: controllerTraceId + "ResendEmailInvitation.311 Error while sending email. " + controller.ErrorManager.ToDescriptionString());
            }
            return RedirectToAction("DistributorDetail", new { Id = Id });
        }

        #region Branches

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchDetail(int Id = -1)
        {
            DistributorBranch distributorBranchToEdit = controller.DistributorBranchRetrieve(Id);

            var postalCode = GetPostalCodeData((int)distributorBranchToEdit.Address.AddressPostalCodeId);
            ViewBag.PostalCode = postalCode.Name;

            if (controller.ResultManager.IsCorrect)
            {
                return View(distributorBranchToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);

            return RedirectToAction("BranchIndex", new { id = distributorBranchToEdit.DistributorId });
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchIndex(int id)
        {
            Distributor distributor = controller.RetrieveDistributor(id);
            ViewBag.DistributorName = distributor.SelectItemName;
            ViewBag.DistributorId = distributor.Id;

            List<DistributorBranch> branches = controller.GetDistributorBranchesByDistributorId(id).ToList();

            TempData["distributorId"] = id;

            if (controller.ResultManager.IsCorrect)
            {
                return View(branches);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Detail", new { Id = id }); ;
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchAdd(int id)
        {
            Distributor distributor = controller.RetrieveDistributor(id);
            ViewBag.DistributorName = distributor.SelectItemName;
            ViewBag.DistributorId = id;
            return View(new DistributorBranch());
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchAdd(DistributorBranch item)
        {
            if (controller.DistributorBranchAdd(item) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Sucursal agregada correctamente");

                var distributorId = (int)TempData["distributorId"];

                return RedirectToAction("BranchIndex", new { id = item.DistributorId });
            }

            ViewBag.DistributorId = (int)TempData["distributorId"];
            TempData["distributorId"] = ViewBag.DistributorId;  // save it again on TempData, if the action is succesfully or failed, this data is required
            NotifyUser(resultManager: controller.ResultManager);
            return View(item);
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchEdit(int Id)
        {
            DistributorBranch distributorBranchToEdit = controller.DistributorBranchRetrieve(Id);
            Distributor distributor = controller.RetrieveDistributor(distributorBranchToEdit.DistributorId);
            ViewBag.DistributorName = distributor.SelectItemName;

            var postalCode = GetPostalCodeData((int)distributorBranchToEdit.Address.AddressPostalCodeId);
            ViewBag.PostalCode = postalCode.Name;
            ViewBag.DistributorId = distributorBranchToEdit.DistributorId;

            if (controller.ResultManager.IsCorrect)
            {
                return View(distributorBranchToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);

            var distributorId = (int)TempData["distributorId"];

            return RedirectToAction("BranchIndex", new { id = distributorId });
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchEdit(DistributorBranch model)
        {
            if (controller.DistributorBranchUpdate(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Sucursal editada correctamente");

                var distributorId = (int)TempData["distributorId"];

                return RedirectToAction("BranchIndex", new { id = distributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchDelete(int Id)
        {
            var distributorId = (int)TempData["distributorId"];

            if (controller.DistributorBranchDelete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Sucursal eliminado correctamente");
                return RedirectToAction("BranchIndex", new { id = distributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("BranchIndex", new { id = distributorId });
        }

        [Authorization(allowedRoles: Controller.UserRole.All)]
        [HttpPost]
        public JsonResult GetPostalCodeData(string postalCode)
        {
            PostalCodeViewModel model = new PostalCodeViewModel();
            var postalCodeData = controller.GetPostalCode(postalCode);
            if (postalCodeData == null)
            {
                return Json(model.Error = "ZIP code data Not Found", JsonRequestBehavior.AllowGet);
            }
            model.Address.AddressPostalCodeId = postalCodeData.Id;
            model.Address.AddressStateId = postalCodeData.AddressStateId;
            model.Address.AddressMunicipalityId = postalCodeData.AddressMunicipalityId;
            model.AvailableColonies = controller.GetAvailableColoniesByPostalCode(postalCodeData.Id).Select(x=>new AvailableColoniesViewModel{ Id = x.Id, Name =x.Name }).ToList();
            model.MunicipalityName = GetMunicipalityName(postalCodeData.AddressMunicipalityId);
            model.StateName = GetStateName(postalCodeData.AddressStateId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Branch Contacts
        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchContactIndex(int id)
        {
            DistributorBranch distributorBranch = controller.DistributorBranchRetrieve(id);
            ViewBag.DistributorBranchName = distributorBranch.Name;
            ViewBag.Id = distributorBranch.Id;


            List<DistributorBranchContact> contacts = controller.BranchContactsRetrieveAll(id);

            TempData["branchId"] = id;

            if (controller.ResultManager.IsCorrect)
            {
                return View(contacts);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("BranchIndex", new { Id = id });
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchContactAdd(int id)
        {
            ViewBag.BranchId = id;
            return View(new DistributorBranchContact());
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchContactAdd(DistributorBranchContact item)
        {
            if (controller.BranchContactAdd(item) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto de sucursal agregado correctamente");
                return RedirectToAction("BranchContactIndex", new { id = item.DistributorBranchId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(item);
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchContactEdit(int Id)
        {
            DistributorBranchContact contactToEdit = controller.BranchContactRetrieve(Id);
            int distributorBranchId = contactToEdit.DistributorBranchId;

            if (controller.ResultManager.IsCorrect)
            {
                ViewBag.DistributorBranchid = distributorBranchId;
                return View(contactToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("BranchContactIndex", new { id = distributorBranchId });
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchContactEdit(DistributorBranchContact model)
        {
            if (controller.BranchContactUpdate(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto editado correctamente");
                return RedirectToAction("BranchContactIndex", new { id = model.DistributorBranchId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
        public ActionResult BranchContactDelete(int Id)
        {
            DistributorBranchContact contactToDelete = controller.BranchContactRetrieve(Id);
            int distributorBranchId = contactToDelete.DistributorBranchId;

            if (controller.BranchContactDelete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto eliminado correctamente");
                return RedirectToAction("BranchContactIndex", new { id = distributorBranchId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("BranchContactIndex", new { id = distributorBranchId });
        }

        #endregion

        #region Contacts

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation,customer-distributor_operation,customer-distributor_view")]
        public ActionResult ContactIndex(int id)
        {
            Distributor distributor = controller.RetrieveDistributor(id);
            ViewBag.DistributorName = distributor.SelectItemName;
            ViewBag.Id = id;

            List<DistributorContact> contacts = controller.ContactsRetrieveAll(id);

            TempData["distributorId"] = id;

            if (controller.ResultManager.IsCorrect)
            {
                return View(contacts);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation,customer-distributor_operation,customer-distributor_view")]
        public ActionResult ContactAdd(int id)
        {
            Distributor distributor = controller.RetrieveDistributor(id);
            ViewBag.DistributorName = distributor.SelectItemName;
            ViewBag.DistributorId = id;
            return View(new DistributorContact());
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation,customer-distributor_operation,customer-distributor_view")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactAdd(DistributorContact item)
        {
            if (controller.ContactAdd(item) || controller.ResultManager.IsCorrect)
            {
                var distributorId = (int)TempData["distributorId"];

                return RedirectToAction("ContactIndex", new { id = item.DistributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(item);
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation,customer-distributor_operation,customer-distributor_view")]
        public ActionResult ContactEdit(int Id)
        {
            DistributorContact contactToEdit = controller.ContactRetrieve(Id);
            Distributor distributor = controller.RetrieveDistributor(contactToEdit.DistributorId);
            ViewBag.DistributorName = distributor.SelectItemName;
            int distributorId = contactToEdit.DistributorId;

            if (controller.ResultManager.IsCorrect)
            {
                return View(contactToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("ContactIndex", new { id = distributorId });
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation,customer-distributor_operation,customer-distributor_view")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactEdit(DistributorContact model)
        {
            if (controller.ContactUpdate(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto editado correctamente");
                return RedirectToAction("ContactIndex", new { id = model.DistributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation,customer-distributor_operation,customer-distributor_view")]
        public ActionResult ContactDelete(int Id)
        {
            DistributorContact contactToDelete = controller.ContactRetrieve(Id);
            int distributorId = contactToDelete.DistributorId;

            if (controller.ContactDelete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto eliminado correctamente");
                return RedirectToAction("ContactIndex", new { id = distributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("ContactIndex", new { id = distributorId });
        }

        #endregion
    }
}
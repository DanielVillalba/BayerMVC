using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Administration.Controllers
{
    
    public class SubdistributorController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.SubdistributorController controller;
        public SubdistributorController()
            : base("SubdistributorController")
        {
            controller = new Controller.SubdistributorController(Configurations);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerDistributorOperation + "," + Controller.UserRole.CustomerDistributorView)]
        public ActionResult Index()
        {
            IEnumerable<Subdistributor> subdistributorFilteredList = controller.FilteredItems();
            if (controller.ResultManager.IsCorrect)
            {
                return View(subdistributorFilteredList);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerDistributorOperation + "," + Controller.UserRole.CustomerDistributorView)]
        public ActionResult Detail(int Id)
        {
            if (Id == -1)
            {
                NotifyUser(message: "Seleccione primero al subdistribuidor a consultar haciendo click en su Id", messageDebug: controllerTraceId + "Detail.111: no Id was received");
                return RedirectToAction("Index");
            }

            Subdistributor auxModel = controller.RetrieveSubdistributor(Id);
            if (!controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                RedirectToAction("Index");
            }
            else
            {
                return View(auxModel);
            }
            return RedirectToError(errorDefault);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Create()
        {
            ViewBag.AddressStates = AddressStates();
            TempData["CropsXMunicipality"] = null;
            return View(Subdistributor.NewEmpty());
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subdistributor model, string selectedType = "", string selectedCommercialNames = "", string postAction = "", int selectedAddressState = -1, List<int> selectedMunicipalities = null, List<int> selectedCrops = null)
        {
            if (model == null)
            {
                return RedirectToError(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
            }

            if (TempData["CropsXMunicipality"] != null)
            {
                //copy the info from TempData into an aux object (cause we cannot use the same object from TempData, it would cause to have two UnitOfWork objects (one from the TempDate and one from the controller object), causing an error on the controller ('An entity object cannot be referenced by multiple instances of IEntityChangeTracker')
                ICollection<SubdistributorCropsXMunicipality> auxCropsXMunicipality = (ICollection<SubdistributorCropsXMunicipality>)TempData["CropsXMunicipality"];
                foreach (SubdistributorCropsXMunicipality item in auxCropsXMunicipality)
                {
                    model.CropsXMunicipality.Add(new SubdistributorCropsXMunicipality()
                    {
                        AddressMunicipalityAddressStateId = item.AddressMunicipalityAddressStateId,
                        AddressMunicipalityId = item.AddressMunicipalityId,
                        Cat_CropId = item.Cat_CropId,
                        //Crop = item.Crop,
                        //Municipality = item.Municipality
                    });
                }
                auxCropsXMunicipality = null;

                TempData["CropsXMunicipality"] = model.CropsXMunicipality;
            }

            List<AddressMunicipality> auxMunicipalities;
            PSD.Controller.DistributorController distributorController = null;
            switch (postAction)
            {
                case "getMunicipalities": //get municipalities for the selected addressState
                    distributorController = new PSD.Controller.DistributorController(Configurations);
                    auxMunicipalities = distributorController.GetMunicipalitiesByState(selectedAddressState);
                    //ViewBag.SelectedPostalCode = auxPostalCode;
                    ViewBag.AvailableMunicipalities = auxMunicipalities;
                    ViewBag.AddressStates = AddressStates(selectedAddressState);
                    return View(model);
                case "addInfluence": //add "area de influencia" to the list
                    distributorController = new PSD.Controller.DistributorController(Configurations);
                    auxMunicipalities = distributorController.GetMunicipalitiesByState(selectedAddressState);
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

                            model.CropsXMunicipality.Add(new SubdistributorCropsXMunicipality()
                            {
                                AddressMunicipalityAddressStateId = selectedAddressState
                                ,
                                Crop = distributorController.GetCropById(itemCropId)
                                ,
                                AddressMunicipalityId = itemMunicipalityId
                                ,
                                Municipality = municipalityName
                                ,
                                Cat_CropId = itemCropId
                                ,
                                SubdistributorId = model.Id
                            });
                        }
                    }
                    TempData["CropsXMunicipality"] = model.CropsXMunicipality;
                    ViewBag.AddressStates = AddressStates();
                    return View(model);
                case "createDistributor":
                    model.Type = selectedType;

                    List<Model.SubdistributorCommercialName> businesses = new List<SubdistributorCommercialName>();
                    bool isFirstCommercialName = true;
                    foreach (string item in selectedCommercialNames.Split('/'))
                    {
                        businesses.Add(new SubdistributorCommercialName() { Name = item, SubdistributorId = model.Id, IsMain = isFirstCommercialName });
                        isFirstCommercialName = false;
                    }
                    model.CommercialNames = businesses;
                    

                    if (controller.CreateSubdistributor(model) && controller.ResultManager.IsCorrect)
                    {
                        NotifyUser(resultManager: controller.ResultManager);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        NotifyUser(resultManager: controller.ResultManager);
                    }
                    break;
                default:
                    break;
            }
            ViewBag.AddressStates = AddressStates();
            return View(model);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult Edit(int Id)
        {
            Subdistributor model = controller.RetrieveSubdistributor(Id);
            if (controller.ResultManager.IsCorrect)
            {
                //copy the info from TempData into an aux object (cause we cannot use the same object from TempData, it would cause to have two UnitOfWork objects (one from the TempDate and one from the controller object), causing an error on the controller
                ICollection<SubdistributorCropsXMunicipality> auxCropsXMunicipality = new List<SubdistributorCropsXMunicipality>();
                foreach (SubdistributorCropsXMunicipality item in model.CropsXMunicipality)
                {
                    auxCropsXMunicipality.Add(new SubdistributorCropsXMunicipality()
                    {
                        Id = item.Id,
                        AddressMunicipalityAddressStateId = item.AddressMunicipalityAddressStateId,
                        Crop = item.Crop,
                        AddressMunicipalityId = item.AddressMunicipalityId,
                        Municipality = item.Municipality,
                        Cat_CropId = item.Cat_CropId,
                        SubdistributorId = model.Id
                    });
                }

                TempData["CropsXMunicipality"] = auxCropsXMunicipality;
                ViewBag.AddressStates = AddressStates();
                return View(model);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subdistributor model, string selectedType = "", string selectedCommercialNames = "", string postAction = "", int selectedAddressState = -1, List<int> selectedMunicipalities = null, List<int> selectedCrops = null)
        {
            if (model == null)
            {
                return RedirectToError(messageDebug: controllerTraceId + "Edit[Post].111 No model was received");
            }

            if (TempData["CropsXMunicipality"] != null)
            {
                //copy the info from TempData into an aux object (cause we cannot use the same object from TempData, it would cause to have two UnitOfWork objects (one from the TempDate and one from the controller object), causing an error on the controller
                ICollection<SubdistributorCropsXMunicipality> auxCropsXMunicipality = (ICollection<SubdistributorCropsXMunicipality>)TempData["CropsXMunicipality"];
                foreach (SubdistributorCropsXMunicipality item in auxCropsXMunicipality)
                {
                    model.CropsXMunicipality.Add(new SubdistributorCropsXMunicipality()
                    {
                        Id = item.Id,
                        AddressMunicipalityAddressStateId = item.AddressMunicipalityAddressStateId,
                        Crop = item.Crop,
                        AddressMunicipalityId = item.AddressMunicipalityId,
                        Municipality = item.Municipality,
                        Cat_CropId = item.Cat_CropId,
                        SubdistributorId = model.Id
                    });
                }
                auxCropsXMunicipality = null;
                TempData["CropsXMunicipality"] = model.CropsXMunicipality;
            }

            List<AddressMunicipality> auxMunicipalities;
            PSD.Controller.DistributorController distributorController = null;
            switch (postAction)
            {
                case "getMunicipalities": //get municipalities for the selected addressState
                    distributorController = new PSD.Controller.DistributorController(Configurations);
                    auxMunicipalities = distributorController.GetMunicipalitiesByState(selectedAddressState);
                    //ViewBag.SelectedPostalCode = auxPostalCode;
                    ViewBag.AvailableMunicipalities = auxMunicipalities;
                    ViewBag.AddressStates = AddressStates(selectedAddressState);
                    return View(model);
                case "addInfluence": //add "area de influencia" to the list
                    distributorController = new PSD.Controller.DistributorController(Configurations);
                    auxMunicipalities = distributorController.GetMunicipalitiesByState(selectedAddressState);
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

                            model.CropsXMunicipality.Add(new SubdistributorCropsXMunicipality()
                            {
                                AddressMunicipalityAddressStateId = selectedAddressState
                                ,
                                Crop = distributorController.GetCropById(itemCropId)
                                ,
                                AddressMunicipalityId = itemMunicipalityId
                                ,
                                Municipality = municipalityName
                                ,
                                Cat_CropId = itemCropId
                                ,
                                SubdistributorId = model.Id
                            });
                        }
                    }
                    TempData["CropsXMunicipality"] = model.CropsXMunicipality;
                    ViewBag.AddressStates = AddressStates();
                    return View(model);
                case "updateSubdistributor":
                    model.Type = selectedType;

                    List<Model.SubdistributorCommercialName> businesses = new List<SubdistributorCommercialName>();
                    bool isFirstCommercialName = true;
                    foreach (string item in selectedCommercialNames.Split('/'))
                    {
                        businesses.Add(new SubdistributorCommercialName() { Name = item, SubdistributorId = model.Id, IsMain = isFirstCommercialName });
                        isFirstCommercialName = false;
                    }
                    model.CommercialNames = businesses;

                    if (controller.UpdateSubdistributor(model) && controller.ResultManager.IsCorrect)
                    {
                        NotifyUser(messageOk: controller.ResultManager.ToDescriptionString(), messageDebug: controllerTraceId + "EmployeeCreate.831 employee created", resultDetails: controller.ErrorManager.ToStringList());
                        return RedirectToAction("Index");
                    }
                    NotifyUser(resultManager: controller.ResultManager);
                    return RedirectToAction("Index");
                default:
                    break;
            }
            ViewBag.AddressStates = AddressStates();
            return View(model);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeRTVOperation)]
        public ActionResult SubdistributorDelete(int id)
        {
            if (controller.DeleteSubdistributor(id) || controller.ResultManager.IsCorrect)
                NotifyUser(messageOk: "Subdistribuidor eliminado correctamente");
            else
                NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        #region Contacts

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView)]
        public ActionResult ContactIndex(int id)
        {
            Subdistributor subdistributor = controller.RetrieveSubdistributor(id);
            ViewBag.Id = subdistributor.Id;
            ViewBag.SubdistributorName = subdistributor.BusinessName;

            List<SubdistributorContact> contacts = controller.ContactsRetrieveAll(id);

            if (controller.ResultManager.IsCorrect)
            {
                return View(contacts);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView)]
        public ActionResult ContactAdd(int id)
        {
            ViewBag.SubdistributorId = id;
            return View(new SubdistributorContact());
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactAdd(SubdistributorContact item)
        {
            if (controller.ContactAdd(item) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto agregado correctamente");
                return RedirectToAction("ContactIndex", new { id = item.SubdistributorId });
            }

            ViewBag.SubdistributorId = item.SubdistributorId;
            NotifyUser(resultManager: controller.ResultManager);
            return View(item);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView)]
        public ActionResult ContactEdit(int Id)
        {
            SubdistributorContact contactToEdit = controller.ContactRetrieve(Id);
            int subdistributorId = contactToEdit.SubdistributorId;

            if (controller.ResultManager.IsCorrect)
            {
                return View(contactToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("ContactIndex", new { id = subdistributorId });
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactEdit(SubdistributorContact model)
        {
            if (controller.ContactUpdate(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto editado correctamente");
                return RedirectToAction("ContactIndex", new { id = model.SubdistributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [Authorization(allowedRoles: Controller.UserRole.AppAdmin + "," + Controller.UserRole.EmployeeManagerOperation + "," + Controller.UserRole.EmployeeManagerView + "," + Controller.UserRole.EmployeeRTVOperation + "," + Controller.UserRole.EmployeeRTVView + "," + Controller.UserRole.CustomerSubdistributorOperation + "," + Controller.UserRole.CustomerSubdistributorView)]
        public ActionResult ContactDelete(int Id)
        {
            SubdistributorContact contactToDelete = controller.ContactRetrieve(Id);
            int subdistributorId = contactToDelete.SubdistributorId;

            if (controller.ContactDelete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contacto eliminado correctamente");
                return RedirectToAction("ContactIndex", new { id = subdistributorId });
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("ContactIndex", new { id = subdistributorId });
        }

        #endregion
    }
}
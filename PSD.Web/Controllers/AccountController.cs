using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Security;
using PSD.Model;
using PSD.Controller;
using PSD.Repository;

namespace PSD.Web.Controllers
{
    public class AccountController : _BaseWebController
    {
        Controller.AccountController controller;

        public AccountController()
            : base("AccountController")
        {
            controller = new Controller.AccountController(Configurations);
        }

        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult Index()
        {
            PSD.Model.User auxUser = controller.GetUser(CurrentUser.Id);
            if(auxUser == null)
            {
                return RedirectToError(errorDefault, "El usuario actualmente logueado '[" + CurrentUser.Id + "] " + CurrentUser.Nick + "' no se encontró en la base de datos, puede que haya sido eliminado mientras estaba logueado");
            }
            string userRole = auxUser.RolesXUser.FirstOrDefault().Cat_UserRole.IdB;
            switch (userRole)
            {
                case "sysadmin":
                case "appadmin":
                    return View(controller.GetBayerEmployee(CurrentUser.Id));
                case "employee-manager_operation":
                case "employee-rtv_operation":
                    return RedirectToAction("IndexBayerEmployee");
                case "customer-distributor_operation":
                case "customer-distributor_view":
                    return RedirectToAction("IndexDistributorEmployee");
                case "customer-subdistributor_operation":
                case "customer-subdistributor_view":
                    return RedirectToAction("IndexSubdistributorEmployee");
                default:
                    break;
            }
            return RedirectToError(errorDefault, "Unexpected user role: '" + userRole + "'");
        }
        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult IndexBayerEmployee()
        {
            PSD.Model.BayerEmployee auxUser = controller.GetBayerEmployee(CurrentUser.Id);
            PSD.Controller.EmployeeController employeeController = new EmployeeController(Configurations);
            List<Cat_Zone> auxZones = (List<Cat_Zone>)employeeController.RetrieveEmployeeZones(auxUser.MunicipalitiesXEmployee);
            List<string> zoneNames = new List<string>();
            foreach (Cat_Zone item in auxZones)
            {
                zoneNames.Add(item.Name);
            }

            ViewBag.EmployeeZoneNames = PSD.Common.Strings.ToString(zoneNames);
            return View(auxUser);
        }
        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult IndexDistributorEmployee()
        {
            PSD.Model.DistributorEmployee auxUser = controller.GetDistributorEmployee(CurrentUser.Id);
            return View(auxUser);
        }
        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult IndexSubdistributorEmployee()
        {
            PSD.Model.SubdistributorEmployee auxUser = controller.GetSubdistributorEmployee(CurrentUser.Id);
            return View(auxUser);
        }
        public ActionResult LoginByToken(string token)
        {
            if (!controller.ValidateLoginToken(token) | controller.ErrorManager.InError)
            {
                NotifyUser(messageError: controller.ErrorManager.Errors[0].Description, messageDebug: controller.ErrorManager.Errors[0].Detail, resultDetails: controller.ErrorManager.ToStringList());
                return RedirectToHome();
            }

            TempData["currentUser"] = controller.User;
            switch (controller.User.Cat_UserStatus.IdB)
            {
                case "toconfirm": //toconfirm, then confirm user
                    return RedirectToAction("NewUserConfirmation", "Account", new { token = token });//, "Account", controller.User //not used since doesn't work if lazy loading enabled (for any not already loaded subobjects)
                case "tocomplete": //tocomplete, then password reset
                case "active": //active, then password reset
                    return RedirectToAction("PasswordReset");
                case "disabled": //disabled, then password reset
                    return RedirectToError("El usuario se encuentra deshabilitado, comuníquese con su administrador para rehabilitarlo", "LoginByToken.711 Invalid user status is disabled so login is not allowed (even for password reset)");
                default: //deleted
                    return RedirectToError(errorDefault, "LoginByToken.791 Invalid user status to perform this action");
            }

            return RedirectToError(errorDefault, "LoginByToken.911 Unexpected End of method");
        }

        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorization(allowedRoles: UserRole.All)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string userPassword, string userPasswordConfirm)
        {

            if (controller.UpdatePassword(CurrentUser.Id, userPassword, userPasswordConfirm) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View();
        }

        public ActionResult NewUserConfirmation(string token)
        {
            //Deprecated, using token instead, this is cleaner and can help to reuse this function
            //this 'NewUserConfirmation' method can be only accessed by comming from 'LoginByToken' method, which will always set up this user object into TempData
            //User model = (Model.User)TempData["currentUser"];
            if (string.IsNullOrWhiteSpace(token))
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "NewUserConfirmation.511 User object was not received");
                return RedirectToError();
            }

            PSD.Model.User model = controller.GetUserByToken(token);
            //TODO:handle model null or error

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUserConfirmation(User model, string userPassword, string userPasswordConfirm)
        {

            if (controller.ConfirmUserAccount(model, userPassword, userPasswordConfirm) && controller.ResultManager.IsCorrect)
            {
                if (!controller.Login(controller.User.NickName, userPassword) || !controller.ResultManager.IsCorrect)
                {
                    NotifyUser(messageError: controller.ResultManager.ToDescriptionString(), messageDebug: controller.ErrorManager.ToDescriptionString());
                    return View(model);
                }

                switch (controller.User.Cat_UserStatus.IdB)
                {
                    case "tocomplete":
                        NotifyUser(messageOk: "Su cuenta ha sido confirmada", message: "Favor de verificar/capturar la información requerida para completar su perfil");
                        return RedirectToAction("Edit");
                    case "active":
                        NotifyUser(messageOk: "Su cuenta ha sido activada");
                        return RedirectToAction("Index");
                    default:
                        return RedirectToError();
                }
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult Edit()
        {
            PSD.Model.User auxUser = controller.GetUser(CurrentUser.Id);
            string userRole = auxUser.RolesXUser.FirstOrDefault().Cat_UserRole.IdB;
            switch (userRole)
            {
                case "sysadmin":
                case "appadmin":
                    return View(controller.GetBayerEmployee(CurrentUser.Id));
                case "employee-manager_operation":
                case "employee-rtv_operation":
                    return RedirectToAction("EditBayerEmployee");
                case "customer-distributor_operation":
                case "customer-distributor_view":
                    return RedirectToAction("EditDistributorEmployee");
                case "customer-subdistributor_operation":
                case "customer-subdistributor_view":
                    return RedirectToAction("EditSubdistributorEmployee");
                default:
                    break;
            }
            return RedirectToError(errorDefault, "Unexpected user Role '" + userRole + "'");
        }

        [Authorization(allowedRoles: UserRole.All)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BayerEmployee model)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Edit.511 User object was not received");
                return RedirectToError();
            }

            if (controller.UpdateUserDetails(model, CurrentUser.InRolesString) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString());
                return RedirectToAction("Index");
            }

            NotifyUser(messageError: controller.ErrorManager.ToDescriptionString());
            return RedirectToHome();
        }

        [Authorization(allowedRoles: UserRole.EmployeeManagerOperation + "," + UserRole.EmployeeManagerView + "," + UserRole.EmployeeRTVOperation + "," + UserRole.EmployeeRTVView)]
        public ActionResult EditBayerEmployee()
        {
            PSD.Model.BayerEmployee auxUser = controller.GetBayerEmployee(CurrentUser.Id);
            PSD.Controller.EmployeeController employeeController = new EmployeeController(Configurations);
            List<Cat_Zone> auxZones = (List<Cat_Zone>)employeeController.RetrieveEmployeeZones(auxUser.MunicipalitiesXEmployee);
            List<string> zoneNames = new List<string>();
            foreach (Cat_Zone item in auxZones)
            {
                zoneNames.Add(item.Name);
            }

            ViewBag.EmployeeZoneNames = PSD.Common.Strings.ToString(zoneNames);
            //ViewBag.EmployeeZoneIds = EmployeeMunicipalitiesToZonesSelectList(auxUser.MunicipalitiesXEmployee);
            return View(auxUser);
        }

        [Authorization(allowedRoles: UserRole.All)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBayerEmployee(BayerEmployee model/*, int selectedZone*/)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Edit.511 User object was not received");
                return RedirectToError();
            }

            if (controller.UpdateBayerEmployee(model/*, selectedZone*/) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: controller.ResultManager.ToDescriptionString());
                return RedirectToAction("Index");
            }

            NotifyUser(messageError: controller.ErrorManager.ToDescriptionString());
            return RedirectToHome();
        }
        [Authorization(allowedRoles: UserRole.All)]
        public ActionResult EditDistributorEmployee()
        {
            ViewBag.AddressStates = AddressStates();
            DistributorEmployee employee = controller.GetDistributorEmployee(CurrentUser.Id);
            if (employee.Distributor.CropsXMunicipality != null)
            {
                TempData["CropsXMunicipality"] = employee.Distributor.CropsXMunicipality;
            }
            return View(employee);
        }

        [Authorization(allowedRoles: UserRole.All)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDistributorEmployee(DistributorEmployee model, string postAction = "", int selectedAddressState = -1, List<int> selectedMunicipalities = null, List<int> selectedCrops = null)
        {
            if (model == null)
            {
                return RedirectToError(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
            }

            DistributorController distributorController = new DistributorController(Configurations);
            if (TempData["CropsXMunicipality"] != null)
            {
                model.Distributor.CropsXMunicipality = (ICollection<DistributorCropsXMunicipality>)TempData["CropsXMunicipality"];
                TempData["CropsXMunicipality"] = model.Distributor.CropsXMunicipality;
            }

            List<AddressMunicipality> auxMunicipalities;

            switch (postAction)
            {
                case "getMunicipalities": //get municipalities for the selected addressState
                    auxMunicipalities = distributorController.GetMunicipalitiesByState(selectedAddressState);
                    //ViewBag.SelectedPostalCode = auxPostalCode;
                    ViewBag.AvailableMunicipalities = auxMunicipalities;
                    ViewBag.AddressStates = AddressStates(selectedAddressState);
                    return View(model);
                case "addInfluence": //add "area de influencia" to the list
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

                            model.Distributor.CropsXMunicipality.Add(new DistributorCropsXMunicipality()
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
                                DistributorId = model.DistributorId
                            });
                            TempData["CropsXMunicipality"] = model.Distributor.CropsXMunicipality;
                        }
                    }
                    ViewBag.AddressStates = AddressStates();
                    return View(model);
                case "updateDistributor":
                    if (controller.UpdateDistributorEmployee(model) && controller.ResultManager.IsCorrect)
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

        [Authorization(allowedRoles: UserRole.CustomerSubdistributorOperation + "," + UserRole.CustomerSubdistributorView)]
        public ActionResult EditSubdistributorEmployee()
        {
            ViewBag.AddressStates = AddressStates();
            SubdistributorEmployee employee = controller.GetSubdistributorEmployee(CurrentUser.Id);
            if (employee.Subdistributor.CropsXMunicipality != null)
            {
                TempData["CropsXMunicipality"] = employee.Subdistributor.CropsXMunicipality;
            }
            return View(employee);
        }

        [Authorization(allowedRoles: UserRole.All)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubdistributorEmployee(SubdistributorEmployee model)
        {
            if (model == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "Edit.511 User object was not received");
                return RedirectToError();
            }

            if (controller.UpdateSubdistributorEmployee(model) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Perfil actualizado");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToHome();
        }


        public ActionResult Login(string returnUrl = "", string message = "", string messageError = "", string messageOk = "", string messageDebug = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            NotifyUser(message: message, messageError: messageError, messageOk: messageOk, messageDebug: messageDebug);
            return View(new Web.Models.UserLoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.UserLoginModel model, bool UserRemember = false, string returnUrl = "")
        {
            if (!ModelState.IsValid) return View(model);

            if (controller.Login(model.NickName, model.Password) && controller.ResultManager.IsCorrect)
            {   
                if (controller.HasPasswordExpired)
                {
                    NotifyUser(messageError: "Su contraseña ha expirado, por favor defina una nueva contraseña");
                    return RedirectToAction("ChangePassword");
                }

                ///TODO: redirect to additional info if status is 'confirmed', or maybe better to put that validation at authorization or a site filter
                if (CurrentUser.StatusIdB == "tocomplete")
                {
                    NotifyUser("Favor de verificar/completar su información de perfil");
                    return RedirectToAction("Edit");
                }
                return RedirectToLocal(returnUrl);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        public ActionResult ForgotPassword(string nickName = "")
        {
            return View(new Web.Models.UserLoginModel() { NickName = nickName });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Models.UserLoginModel model, bool UserRemember = false, string returnUrl = "")
        {
            if (!ModelState.IsValid) return View(model);

            if (!controller.SetLoginToken(model.NickName) || !controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return View(model);
            }

            NotifyUser(messageOk: "Se ha enviado un mensaje a su correo electrónico, en el encontrará una liga con la que podrá reiniciar su contraseña");

            return RedirectToLocal(returnUrl);
        }
        public ActionResult PasswordReset()
        {
            Model.User user = (Model.User)TempData["currentUser"];

            if (user == null)
            {
                NotifyUser(messageError: errorDefault, messageDebug: controllerTraceId + "PasswordReset.511 User object was not received");
                return RedirectToError();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordReset(User model, string userPassword, string userPasswordConfirm)
        {

            if (controller.UpdatePassword(model.Id, userPassword, userPasswordConfirm) & controller.ResultManager.IsCorrect)
            {
                switch (controller.User.Cat_UserStatus.IdB)
                {
                    case "toconfirm":
                    case "tocomplete":
                    case "active":
                        NotifyUser(messageOk: "Su contraseña ha sido reiniciada, puede loguearse para acceder al sitio");
                        return RedirectToLogin();
                    default:
                        return RedirectToError();
                }
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        public ActionResult Logout()
        {
            Identity.TerminateSession();
            return RedirectToLogin(messageOk: "Sessión finalizada");
        }

    }
}
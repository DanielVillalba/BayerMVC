using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Common;
using PSD.Model;
using PSD.Model.Filters;
using System.Text.RegularExpressions;
using PSD.Model.Filters.PipelineAndFilters.DistributorFilters;

namespace PSD.Web.Controllers
{
    public class _BaseWebController : System.Web.Mvc.Controller
    {
        #region Attributes
        private static bool? debugEnabled;
        private static PSD.Controller.IAppConfiguration configurations;
        #endregion

        #region Properties

        public string controllerTraceId = "";
        public static string errorDefault = "Ocurrio un erorr, intente de nuevo o comuníquese con el administrador.";//final space for ensuring the show of the message container in case the resource label has no text
        public static PSD.Security.Entity.User CurrentUser { get { return PSD.Security.Identity.CurrentUser; } }

        /// <summary>
        /// Returns the sile system path where the images are being stored
        /// </summary>
        public static string ImageStoragePath
        {
            get
            {
                return Configurations.NewsImageStoragePath;
            }
        }

        /// <summary>
        /// Indicates if the site is running on debug mode, the application behavior will change for:
        /// Error messages: true->will display error details as db erors or code exceptions | false: will display only the high level user errors
        /// </summary>
        public static bool DebugEnabled
        {
            get
            {
                try
                {
                    if (debugEnabled == null) debugEnabled = Configurations.IsDebugEnabled;//Convert.ToBoolean(ConfigurationManager.AppSettings["DebugEnabled"]);
                    return (bool)debugEnabled;
                }
                catch { }
                return false;
            }
        }

        public static PSD.Controller.IAppConfiguration Configurations
        {
            get
            {
                if (configurations == null)
                {
                    try
                    {
                        configurations = PSD.Controller._BaseController.RetrieveConfigurations(new AppConfiguration());
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Excepción al recuperar las configuraciones de la aplicación. Detalle: " + ex.Message);
                    }
                }
                return configurations;
            }
        }
        #endregion

        #region Constructors
        public _BaseWebController(string controllerTraceId)
        {
            this.controllerTraceId = controllerTraceId;
        }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageError"></param>
        /// <param name="messageOk"></param>
        /// <param name="messageDebug"></param>
        /// <param name="resultScript">[optional] script to be run when page loads, can contain any javascript, doesn't need to add the script tags</param>
        public void NotifyUser(string message = "", string messageError = "", string messageOk = "", string messageDebug = "", List<string> resultDetails = null, string resultScript = "", Exception ex = null, ResultManager resultManager = null)
        {
            //deprecated:'now can have ex:a script returned with no message attached' if (string.IsNullOrWhiteSpace(message) && string.IsNullOrWhiteSpace(messageError) && string.IsNullOrWhiteSpace(messageOk)) messageError = errorDefault;//by default threat as error with default msg
            if (!string.IsNullOrWhiteSpace(message)) TempData["message"] += message;
            if (!string.IsNullOrWhiteSpace(messageError)) TempData["messageError"] += messageError;
            if (!string.IsNullOrWhiteSpace(messageOk)) TempData["messageOk"] += messageOk;
            if (!string.IsNullOrWhiteSpace(resultScript)) TempData["resultScript"] += resultScript;
            if (DebugEnabled) TempData["messageDebug"] += messageDebug + ((ex != null) ? " Exception: " + ex.Message + " InnerException:" + ex.InnerException : "");

            if (resultManager != null)
            {
                if (resultManager.IsCorrect)
                {
                    TempData["messageOk"] += resultManager.ToDescriptionString();
                }
                else
                {
                    TempData["messageError"] += resultManager.ToDescriptionString();
                    if (DebugEnabled) { TempData["messageDebug"] += resultManager.ToDetailString(); }
                }
            }
        }

        /// <summary>
        /// Returns a javascript script (with no script tags) to open a new popup window
        /// </summary>
        /// <param name="title">The window title///TODO:pending to make this work</param>
        /// <param name="url">the url that window will open, can be a html page, a .pdf file, etc (pending to validate other types)</param>
        /// <param name="width">[Optional][default:850] in pixels</param>
        /// <param name="height">[Optional][default:600] in pixels</param>
        /// <returns></returns>
        public static string PopUpWindow(string title, string url, int width = 850, int height = 600)
        {
            string vf;
            vf = "PopUpWindowDefault('" + title + "', '" + url + "');";
            return vf;
        }

        #region Url redirections

        /// <summary>
        /// Redirects to application login page
        /// </summary>
        /// <param name="returnUrl">[Optional][Default:Home]After login will return to this page</param>
        /// <param name="messageError">[Optional] an error to be displayed in the login page to indicate the reason why the login is needed (ex: need to be logged in, current user doesn't have access to the required resource)</param>
        /// <returns></returns>
        public ActionResult RedirectToLogin(string returnUrl = "", string message = "", string messageError = "", string messageOk = "")
        {
            return RedirectToAction("Login", "Account", new { returnUrl = returnUrl, message = message, messageError = messageError, messageOk = messageOk, });
        }
        public ActionResult RedirectToError(string messageError = "", string messageDebug = "")
        {
            if (string.IsNullOrWhiteSpace(messageError)) messageError = errorDefault;
            return RedirectToAction("Error", "Home", new { area = "", messageError = messageError });
        }
        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        protected ActionResult RedirectToLocal(string returnUrl = "")
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (returnUrl != "") NotifyUser(message: "No se ha podido regresar a la pagina '" + returnUrl + "' ya que se encuentra fuera de este dominio.", messageDebug: controllerTraceId + "RedirectToLocal.111: Redirect Url was outside this domain '" + returnUrl + "'");
                return RedirectToHome();
            }
        }

        #endregion

        #region Catalogs
        public static IEnumerable<SelectListItem> CatUserRolesEmployee()
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (Cat_UserRole item in PSD.Controller._BaseController.CatUserRolesBayer())
            {
                aux = new SelectListItem();
                aux.Value = item.IdB;
                aux.Text = item.Name;
                vf.Add(aux);
            }
            return vf;
        }
        public static IEnumerable<SelectListItem> CatZones()
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (Cat_Zone item in PSD.Controller._BaseController.CatZones())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                vf.Add(aux);
            }
            return vf;
        }
        public static IEnumerable<SelectListItem> AddressStates(int selectedId = -1)
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (AddressState item in PSD.Controller._BaseController.CatAddressStates())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                if (item.Id == selectedId) aux.Selected = true;
                vf.Add(aux);
            }
            return vf;
        }
        public static IEnumerable<SelectListItem> CatCrops(int selectedId = -1)
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (Cat_Crop item in PSD.Controller._BaseController.CatCrops())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                if (item.Id == selectedId) aux.Selected = true;
                vf.Add(aux);
            }
            return vf;
        }
        public static IEnumerable<SelectListItem> CatCropCategories(int selectedId = -1)
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (Cat_CropCategory item in PSD.Controller._BaseController.CatCropCategories())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                if (item.Id == selectedId) aux.Selected = true;
                vf.Add(aux);
            }
            return vf;
        }
        public static IEnumerable<SelectListItem> AddressMunicipalities()
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (AddressMunicipality item in PSD.Controller._BaseController.CatAddressMunicipalities())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                vf.Add(aux);
            }
            return vf;
        }

        public static IEnumerable<SelectListItem> Rtvs()
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (BayerEmployee item in PSD.Controller._BaseController.GetAllEmployeeRTVs())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                vf.Add(aux);
            }
            return vf;
        }
        public static IEnumerable<SelectListItem> Grvs()
        {
            List<SelectListItem> vf = new List<SelectListItem>();
            SelectListItem aux;
            foreach (BayerEmployee item in PSD.Controller._BaseController.GetAllEmployeeGRVs())
            {
                aux = new SelectListItem();
                aux.Value = item.Id.ToString();
                aux.Text = item.Name;
                vf.Add(aux);
            }
            return vf;
        }

        public static IEnumerable<SelectListItem> SubdistributorContactAreas()
        {
            List<SelectListItem> subdistributorContactAreas = new List<SelectListItem>();
            subdistributorContactAreas = Controller._BaseController.SubdistributorContactAreas()
                                                                   .Select(x => new SelectListItem
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name
                                                                   }).ToList();
            return subdistributorContactAreas;
        }

        public static IEnumerable<SelectListItem> DistributorContactAreas()
        {
            List<SelectListItem> distributorContactAreas = new List<SelectListItem>();
            distributorContactAreas = Controller._BaseController.DistributorContactAreas()
                                                                   .Select(x => new SelectListItem
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name
                                                                   }).ToList();
            return distributorContactAreas;
        }

        public static IEnumerable<SelectListItem> DistributorBranchContactAreas()
        {
            List<SelectListItem> distributorBranchContactAreas = new List<SelectListItem>();
            distributorBranchContactAreas = Controller._BaseController.DistributorBranchContactAreas()
                                                                   .Select(x => new SelectListItem
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name
                                                                   }).ToList();
            return distributorBranchContactAreas;
        }

        public static AddressPostalCode GetPostalCodeData(int? postalCodeId)
        {
            return Controller._BaseController.GetAdressPostalCode(postalCodeId) ?? new AddressPostalCode();
        }

        public static string GetMunicipalityName(int? id)
        {
            return Controller._BaseController.GetMunicipalityNameById(id);
        }

        public static string GetStateName(int? id)
        {
            return Controller._BaseController.GetStateNameById(id);
        }

        public static string GetColonyName(int? id)
        {
            return Controller._BaseController.GetColonyNameById(id);
        }

        #region News

        public static List<News> GetLatestNews()
        {
            return Controller._BaseController.GetLatestNews();
        }

        public static string CleanHtmlCode(string rawHtml)
        {
            return Regex.Replace(rawHtml, "<.*?>", string.Empty);
        }

        public static string MiniatureParagraphFormatter(string paragraph)
        {
            string result = CleanHtmlCode(paragraph);
            result = result.Length > 200 ? result.Substring(0, 200) + "..." : result;
            return result;
        }
        #endregion

        #region ContentManagement

        public static string GetJumbotronContent()
        {
            string result = HttpUtility.HtmlDecode(Controller._BaseController.GetJumbotronContent());
            return result;
        }

        public static string GetContactPageContent()
        {
            string result = HttpUtility.HtmlDecode(Controller._BaseController.GetContactPageContent());
            return result;
        }

        #endregion

        #endregion

        #region Common gets

        #endregion

        #region SelectList methods
        protected List<SelectListItem> EmployeeMunicipalitiesToZonesSelectList(ICollection<MunicipalitiesXEmployee> employeeMunicipalities)
        {
            PSD.Controller.EmployeeController employeeController = new Controller.EmployeeController(Configurations);
            List<Cat_Zone> auxZones = (List<Cat_Zone>)employeeController.RetrieveEmployeeZones(employeeMunicipalities);
            List<SelectListItem> employeeZonesList = (List<SelectListItem>)CatZones();
            foreach (SelectListItem listItem in employeeZonesList)
            {
                foreach (Cat_Zone item in auxZones)
                {
                    if (listItem.Value == item.Id.ToString()) listItem.Selected = true;
                }
            }
            return employeeZonesList;
        }

        public List<SelectListItem> GetDistributors(List<string> zones)
        {
            // replace the current filtering for the new pipeline implementation
            List<Distributor> auxList = Controller._BaseController.RetrieveDistributorFilteredByZones(zones).ToList();
            List<SelectListItem> returnList = new List<SelectListItem>();
            foreach (Distributor item in auxList)
            {
                returnList.Add(new SelectListItem() { Value =  item.Id.ToString(), Text = item.SelectItemName });
            }
            return returnList;
        }

        #endregion

        #endregion

        #region Routing methods

        public static List<Models.UrlAreaModel> SiteTree = new List<Models.UrlAreaModel>() { 
            new Models.UrlAreaModel() { Id= "", DisplayName = "Inicio", Controllers = new List<Models.UrlControllerModel>(){ 
                    new Models.UrlControllerModel() { Id = "home", DisplayName = "Inicio", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Inicio" } 
                        }
                    }
                    , new Models.UrlControllerModel() { Id = "contacto", DisplayName = "Contacto", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "ContactoIndex" } 
                        }
                    }
                    , new Models.UrlControllerModel() { Id = "account", DisplayName = "Cuenta", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Mi Cuenta" } 
                        , new Models.UrlActionModel() { Id = "login", DisplayName = "Login" } 
                        , new Models.UrlActionModel() { Id = "editaccount", DisplayName = "Editar mi cuenta" } 
                        }
                    }
                }
            }
            , new Models.UrlAreaModel() { Id= "administration", DisplayName = "Administración", Controllers = new List<Models.UrlControllerModel>(){ 
                    new Models.UrlControllerModel() { Id = "employee", DisplayName = "Empleados", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de empleados" + DateTime.Now.Year } 
                        , new Models.UrlActionModel() { Id = "employeedetail", DisplayName = "Detalle de empleado"} 
                        , new Models.UrlActionModel() { Id = "employeeedit", DisplayName = "Editar empleado"} 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "catalog", DisplayName = "Catálogos", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Catálogos" + DateTime.Now.Year } 
                        , new Models.UrlActionModel() { Id = "cropindex", DisplayName = "Catálogo de cultivos"} 
                        , new Models.UrlActionModel() { Id = "cropadd", DisplayName = "Alta de cultivo"} 
                        , new Models.UrlActionModel() { Id = "cropedit", DisplayName = "Edición de cultivo"} 
                        }
                    }
                }
            }
            , new Models.UrlAreaModel() { Id= "contracts", DisplayName = "Convenios", Controllers = new List<Models.UrlControllerModel>(){ 
                    new Models.UrlControllerModel() { Id = "bayertodistributor", DisplayName = "Convenios con Distribuidores", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de convenios" } 
                        , new Models.UrlActionModel() { Id = "create", DisplayName = "Crear convenio"} 
                        , new Models.UrlActionModel() { Id = "detail", DisplayName = "Detalle de convenio"} 
                        , new Models.UrlActionModel() { Id = "edit", DisplayName = "Edición de convenio"} 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "tosubdistributor", DisplayName = "Convenios con Subdistribuidores/Agricultores", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de convenios" } 
                        , new Models.UrlActionModel() { Id = "create", DisplayName = "Crear convenio"} 
                        , new Models.UrlActionModel() { Id = "detail", DisplayName = "Detalle de convenio"} 
                        , new Models.UrlActionModel() { Id = "edit", DisplayName = "Edición de convenio"} 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "mydistributorcontracts", DisplayName = "Mis Convenios", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado" } 
                        , new Models.UrlActionModel() { Id = "detail", DisplayName = "Detalle de convenio"} 
                        , new Models.UrlActionModel() { Id = "edit", DisplayName = "Edición de convenio"} 
                        }
                    }
                }
            }
            , new Models.UrlAreaModel() { Id= "content", DisplayName = "Contenido", Controllers = new List<Models.UrlControllerModel>(){ 
                    new Models.UrlControllerModel() { Id = "news", DisplayName = "Noticias", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de noticias" } 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "link", DisplayName = "Ligas a otros sitios", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de ligas" } 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "directory", DisplayName = "Directorio", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de directorio" } 
                        }
                    }
                }
            }
            , new Models.UrlAreaModel() { Id= "contentmanagement", DisplayName = "Administración de Contenido", Controllers = new List<Models.UrlControllerModel>(){ 
                    new Models.UrlControllerModel() { Id = "news", DisplayName = "Noticias", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de noticias" } 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "link", DisplayName = "Ligas a otros sitios", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de ligas" } 
                        }
                    },
                    new Models.UrlControllerModel() { Id = "directory", DisplayName = "Directorio", Actions = new List<Models.UrlActionModel>(){
                        new Models.UrlActionModel() { Id = "index", DisplayName = "Listado de directorio" } 
                        }
                    }
                }
            }
        };

        public static string GetAreaTitle(string area)
        {
            area = area.ToLower();
            foreach (Models.UrlAreaModel anArea in SiteTree)
            {
                if (anArea.Id == area) return anArea.DisplayName;
            }
            return "[" + area + "]";
        }
        public static string GetControllerTitle(string area, string controller)
        {
            area = area.ToLower();
            controller = controller.ToLower();
            foreach (Models.UrlAreaModel anArea in SiteTree)
            {
                if (anArea.Id == area)
                {
                    foreach (Models.UrlControllerModel aController in anArea.Controllers)
                    {
                        if (aController.Id == controller) return aController.DisplayName;
                    }
                }
            }
            return "[" + controller + "]";
        }
        public static string GetActionTitle(string area, string controller, string action)
        {
            area = area.ToLower();
            controller = controller.ToLower();
            action = action.ToLower();
            foreach (Models.UrlAreaModel anArea in SiteTree)
            {
                if (anArea.Id == area)
                {
                    foreach (Models.UrlControllerModel aController in anArea.Controllers)
                    {
                        if (aController.Id == controller)
                        {
                            foreach (Models.UrlActionModel anAction in aController.Actions)
                            {
                                if (anAction.Id == action) return anAction.DisplayName;
                            }
                        }
                    }
                }
            }
            return "[" + action + "]";
        }
        #endregion

        #region Others

        #endregion

        ///TODO: implement enum for user role IdBs across site
    }
    public class AppConfiguration : PSD.Controller.IAppConfiguration
    {
        public bool IsDebugEnabled { get; set; }
        public string HostUrl
        {
            get { return System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + "/"; }
        }

        public string PasswordValidFormatPattern { get; set; }

        public string PasswordInvalidFormatMessage { get; set; }

        public int PasswordExpireDays { get; set; }


        public string MailServerSmtpAddress { get; set; }

        public int MailServerSmtpPort { get; set; }

        public string MailServerFromEmail { get; set; }

        public string MailServerFromPassword { get; set; }

        public string MailServerFromDisplayName { get; set; }

        public bool MailServerMailIsBodyHtml { get; set; }

        public bool MailServerMailUseSSL { get; set; }

        public string MailServerMailToCC { get; set; }

        public string MailServerMailToCCO { get; set; }

        public string MailServerContactPageMailTo { get; set; }


        public string LayoutEmailContractDistributor_SendTodistributor_Subject { get; set; }

        public string LayoutEmailContractDistributor_SendTodistributor_Body { get; set; }

        public string LayoutEmailContactPage_Subject { get; set; }

        public string LayoutEmailContactPage_Body { get; set; }


        public int[,] BenefitProgram_Coupon_Discount { get; set; }

        public int[,] BenefitProgram_Coupon_Promotion { get; set; }
        public bool BenefitProgram_Coupon_IsOpen { get; set; }
        public bool BenefitProgram_Coupon_S1_IsOpen { get; set; }
        public bool BenefitProgram_Coupon_S2_IsOpen { get; set; }
        public bool BenefitProgram_Coupon_S1_IsCalculated { get; set; }
        public bool BenefitProgram_Coupon_S2_IsCalculated { get; set; }
        public bool BenefitProgram_Coupon_IsCalculated { get; set; }

        public string NewsImageStoragePath { get; set; }
    }
}
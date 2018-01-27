using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Security.Entity;
using PSD.Common;
using PSD.Repository;
using PSD.Model;
using PSD.Security;
using PSD.Model.Filters.PipelineAndFilters.DistributorFilters;

namespace PSD.Controller
{
    public class _BaseController
    {
        #region Attributes
        private ErrorManager errorManager;
        private ResultManager resultManager;
        private UnitOfWork repository;
        private IAppConfiguration configurations;
        #endregion

        #region Properties
        public UnitOfWork Repository { 
            get
            {
                if (repository == null || repository.IsDisposed) repository = new UnitOfWork(new PSDContext());
                return repository;
            }
        }
        public ErrorManager ErrorManager
        {
            get
            {
                if (errorManager == null) errorManager = new ErrorManager();
                return errorManager;
            }
        }
        public ResultManager ResultManager
        {
            get
            {
                if (resultManager == null) resultManager = new ResultManager();
                return resultManager;
            }
        }
        public static Security.Entity.User CurrentUser { get { return Security.Identity.CurrentUser; } }
        public string Trace { get; private set; }
        public IAppConfiguration Configurations { get { return configurations; } set { configurations = value; } }

        public static string ErrorDefault = "Ocurrio un error, intente de nuevo o comuníquese con su administrador";
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public _BaseController(string trace, IAppConfiguration configurations)
        {
            Trace = "PSD.Controller." + trace;
            this.configurations = configurations;
        }


        #region Catalogs

        public static IEnumerable<Cat_UserRole> CatUserRolesAppAdmin()
        {
            //TODO:find better solution
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.UserRoles.GetAppAdminRoles(new string[] { "appadmin" });
        }
        public static IEnumerable<Cat_UserRole> CatUserRolesBayer()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.UserRoles.GetEmployeeRoles(new string[] { "appadmin", "employee-manager_view", "employee-manager_operation", "employee-rtv_operation", "employee-rtv_view" });
        }
        public static IEnumerable<Cat_UserRole> CatUserRolesDistributor()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.UserRoles.GetEmployeeRoles(new string[] { "customer-distributor_operation", "customer-distributor_view", });
        }
        public static IEnumerable<Cat_UserRole> CatUserRolesSubdistributor()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.UserRoles.GetCustomerRoles(new string[] { "customer-subdistributor_operation", "customer-subdistributor_view" });
        }

        public static IEnumerable<Cat_Zone> CatZones()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.Zones.GetAll();
        }
        public static IEnumerable<Cat_Crop> CatCrops()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.Crops.GetAll();
        }
        public static IEnumerable<Cat_CropCategory> CatCropCategories()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.CropCategories.GetAll();
        }

        public static IEnumerable<AddressState> CatAddressStates()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AddressStates.GetAll();
        }

        public static IEnumerable<AddressMunicipality> CatAddressMunicipalities()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AddressMunicipalities.GetAll();
        }

        public static IEnumerable<BayerEmployee> GetAllEmployeeRTVs()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.BayerEmployees.GetAllRtvs();
        }

        public static IEnumerable<BayerEmployee> GetAllEmployeeGRVs()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.BayerEmployees.GetAllGrvs();
        }

        public static IEnumerable<Cat_SubdistributorContactArea> SubdistributorContactAreas()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.SubdistributorContactAreas.GetAll();
        }

        public static IEnumerable<Cat_DistributorContactArea> DistributorContactAreas()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.DistributorContactAreas.GetAll();
        }

        public static IEnumerable<Cat_DistributorBranchContactArea> DistributorBranchContactAreas()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.DistributorBranchContactAreas.GetAll();
        }

        public static string GetMunicipalityNameById(int? id)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AddressMunicipalities.GetAll().Where(x => x.Id == id).FirstOrDefault().Name;
        }

        public static string GetStateNameById(int? id)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AddressStates.GetAll().Where(x => x.Id == id).FirstOrDefault().Name;
        }

        public static string GetColonyNameById(int? id)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AddressColonies.GetAll().Where(x => x.Id == id).FirstOrDefault().Name;
        }

        public static IQueryable<Distributor> RetrieveDistributorFilteredByZones(List<string> zones)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            IQueryable<Distributor> itemsToFilter = Repository.Distributors.GetAllToFilter();
            DistributorFilteringPipeline pipeline = new DistributorFilteringPipeline();
            pipeline.Register(new ZoneListFilter(zones));

            return pipeline.Process(itemsToFilter);
        }

        /// <summary>
        /// This will validate if a given mail address is currently being used into the system,
        /// if an User id is provided it will considered that the email to check is from an existing user then
        /// we will look for a match only on different users
        /// </summary>
        /// <param name="emailToCheck"></param>
        /// <param name="isUpdatingUserId"></param>
        /// <returns></returns>
        public static bool IsMailAddressCurrentlyUsed(string emailToCheck, int? isUpdatingUserId = null)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());

            bool result = isUpdatingUserId.HasValue
                    ? Repository.Persons.GetAll().Any(x => string.Equals(x.EMail, emailToCheck) && x.User.Id != isUpdatingUserId.Value)
                    : Repository.Persons.GetAll().Any(x => string.Equals(x.EMail, emailToCheck) && x.User.Cat_UserStatusId != 2);

            return result;
        }

        /// <summary>
        /// Get the distributor zone. The zone is calculated from the subdistributor address municipality, it could be the municipality is not assigned to no zone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSubdistributorZone(int id)
        {
            return Repository.Subdistributors.RetrieveSubdistributorZone(id);
        }

        public List<string> GetSubdistributorZones(int subdistributorId)
        {
            return Repository.Subdistributors.RetrieveSubdistributorZones(subdistributorId);
        }

        /// <summary>
        /// This will provide a list of zones assigned to a BayerEmployee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetBayerEmployeeZones(int id)
        {
            BayerEmployee employeeData = Repository.BayerEmployees.Get(id);
            List<Cat_Zone> auxZones = Repository.Zones.GetByMunicipalities(employeeData.MunicipalitiesXEmployee).ToList();
            List<string> zoneNames = new List<string>();
            foreach (Cat_Zone item in auxZones)
            {
                zoneNames.Add(item.Name);
            }

            return zoneNames;
        }

        /// <summary>
        /// Retrieve DistributorId data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetDistributorId(int userId)
        {
            DistributorEmployee employeeData = Repository.DistributorEmployees.Get(userId);
            return employeeData.DistributorId;
        }

        /// <summary>
        /// Retrieve DistributorId data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetSubdistributorId(int userId)
        {
            SubdistributorEmployee employeeData = Repository.SubdistributorEmployees.Get(userId);
            return employeeData.SubdistributorId;
        }


        #endregion

        #region DistributorBranches

        public static AddressPostalCode GetAdressPostalCode(int? postalCodeId)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AddressPostalCodes.GetAll().Where(x => x.Id == postalCodeId).FirstOrDefault();
        }

        #endregion

        #region News

        public static List<News> GetLatestNews(int nNews = 3)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());

            List<News> newsFeed = new List<News>();
            if (Identity.CurrentUser.IsInRole(UserRole.AppAdmin + "," + UserRole.SysAdmin + "," + UserRole.EmployeeManagerOperation + "," + UserRole.EmployeeRTVOperation))
            {
                newsFeed = Repository.News.GetAll()
                                     .Where(x => x.IsPublished)
                                     .ToList();
            }
            else if (Identity.CurrentUser.IsInRole(UserRole.CustomerDistributorOperation + "," + UserRole.CustomerDistributorView))
            {
                newsFeed = Repository.News.GetAll()
                                     .Where(x => x.IsPublished && x.IsDistributorVisible)
                                     .ToList();
            }
            else if (Identity.CurrentUser.IsInRole(UserRole.CustomerSubdistributorOperation + "," + UserRole.CustomerSubdistributorView))
            {
                // checking subdistributor type, if farmer or else
                if (string.Equals(Identity.CurrentUser.UserType, "Agricultor"))
                    newsFeed = Repository.News.GetAll()
                                         .Where(x => x.IsPublished && x.IsFarmerVisible)
                                         .ToList();
                else
                    newsFeed = Repository.News.GetAll()
                                         .Where(x => x.IsPublished && x.IsSubdistributorVisible)
                                         .ToList();
            }
            else if (!Identity.CurrentUser.IsLogged)
            {
                newsFeed = Repository.News.GetAll()
                                     .Where(x => x.IsPublished && x.IsNonAuthenticatedVisible)
                                     .ToList();
            }


            return newsFeed.OrderByDescending(x => x.PublishDate.Value).Take(nNews).ToList();
        }

        #endregion

        #region ContentManagement

        /// <summary>
        /// Contruct the content to be displayed on the start page's jumbotron
        /// </summary>
        /// <returns></returns>
        public static string GetJumbotronContent()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            StringBuilder content = new StringBuilder();

            ContentData title = Repository.ContentData.GetAll()
                                          .Where(x => string.Equals(x.Key, "StartPageTitle"))
                                          .FirstOrDefault();

            ContentData subtitle = Repository.ContentData.GetAll()
                                             .Where(x => string.Equals(x.Key, "StartPageSubtitle"))
                                             .FirstOrDefault();

            ContentData paragraph = Repository.ContentData.GetAll()
                                              .Where(x => string.Equals(x.Key, "StartPageParagraph"))
                                             .FirstOrDefault();

            ContentData button = Repository.ContentData.GetAll()
                                           .Where(x => string.Equals(x.Key, "StartPageButton"))
                                           .FirstOrDefault();

            content.Append(title != null ? title.Value : string.Empty);
            content.Append(subtitle != null ? subtitle.Value : string.Empty);
            content.Append(paragraph != null ? paragraph.Value : string.Empty);
            content.Append(button != null ? button.Value : string.Empty);

            return content.ToString();
        }

        public static string GetContactPageContent()
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            string content = string.Empty;
            ContentData item = Repository.ContentData.GetAll()
                                         .Where(x => string.Equals(x.Key, "ContactPageContent"))
                                         .FirstOrDefault();
            content = item != null
                    ? item.Value
                    : "No ContactPageContent key found in DB, please add content";

            return content;
        }
        #endregion

        #region Tools

        protected bool SendEmail(string toSingleEMail, string messageSubject, string messageBody, Dictionary<string, string> emailParams)
        {
            PSD.Util.Mailer mailer = new Util.Mailer(Configurations.MailServerFromEmail, Configurations.MailServerFromPassword, Configurations.MailServerSmtpAddress, Configurations.MailServerSmtpPort, Configurations.MailServerMailUseSSL);
            if (mailer.SendSingle(toSingleEMail, messageSubject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ResultManager.Add("Error al enviar mensaje a '" + toSingleEMail + "'", Trace + "SendEmail.911 Error al enviar mensaje a '" + toSingleEMail + "'. Detalle: " + mailer.ResultDetails[0]);
                return false;
            }
        }

        #endregion

        #endregion

        #region Others
        #endregion

        #region AppConfiguration
        public static AppConfiguration RetrieveConfiguration(string configurationKey)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());
            return Repository.AppConfigurations.GetByKey(configurationKey);
        }
        public static IAppConfiguration RetrieveConfigurations(IAppConfiguration appConfigurations)
        {
            UnitOfWork Repository = new UnitOfWork(new PSDContext());

            ///TODO: change to getall configurations from bd, then foreach and switch to set each (to make just 1 call to db)
            try { appConfigurations.IsDebugEnabled = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.IsDebugEnabled).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.IsDebugEnabled = true; }

            try { appConfigurations.MailServerFromEmail = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerFromEmail).Value; }
            catch { appConfigurations.MailServerFromEmail = ""; }

            try { appConfigurations.MailServerFromDisplayName = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerFromDisplayName).Value; }
            catch { appConfigurations.MailServerFromDisplayName = ""; }

            try { appConfigurations.MailServerFromPassword = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerFromPassword).Value; }
            catch { appConfigurations.MailServerFromPassword = ""; }

            try { appConfigurations.MailServerMailIsBodyHtml = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerMailIsBodyHtml).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.MailServerMailIsBodyHtml = true; }

            try { appConfigurations.MailServerMailToCC = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerMailToCC).Value; }
            catch { appConfigurations.MailServerMailToCC = ""; }

            try { appConfigurations.MailServerMailToCCO = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerMailToCCO).Value; }
            catch { appConfigurations.MailServerMailToCCO = ""; }

            try { appConfigurations.MailServerMailUseSSL = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerMailUseSSL).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.MailServerMailUseSSL = false; }

            try { appConfigurations.MailServerSmtpAddress = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerSmtpAddress).Value; }
            catch { appConfigurations.MailServerSmtpAddress = ""; }

            try { appConfigurations.MailServerSmtpPort = Convert.ToInt32(Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.MailServerSmtpPort).Value); }
            catch { appConfigurations.MailServerSmtpPort = 25; }

            try { appConfigurations.MailServerContactPageMailTo = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.MailServerContactPageMailTo).Value; }
            catch { appConfigurations.MailServerContactPageMailTo = "prueba1@roomie-it.org"; }

            //Layout emails
            try { appConfigurations.LayoutEmailContractDistributor_SendTodistributor_Subject = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.LayoutEmailContractDistributor_SendTodistributor_Subject).Value; }
            catch { appConfigurations.LayoutEmailContractDistributor_SendTodistributor_Subject = "[no subject]"; }
            try { appConfigurations.LayoutEmailContractDistributor_SendTodistributor_Body = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.LayoutEmailContractDistributor_SendTodistributor_Body).Value; }
            catch { appConfigurations.LayoutEmailContractDistributor_SendTodistributor_Body = "[no body]"; }
            try { appConfigurations.LayoutEmailContactPage_Subject = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.LayoutEmailContactPage_Subject).Value; }
            catch { appConfigurations.LayoutEmailContactPage_Subject = "[no subject]"; }
            try { appConfigurations.LayoutEmailContactPage_Body = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.LayoutEmailContactPage_Body).Value; }
            catch { appConfigurations.LayoutEmailContactPage_Body = "[no body]"; }

            //benefit programs
            //*coupons
            try
            {
                string[] auxDiscountRow = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_Discount).Value.Split('|');
                appConfigurations.BenefitProgram_Coupon_Discount = new Int32[10, 5];
                int a = 0, b = 0;
                foreach (string itemRow in auxDiscountRow)
                {
                    string[] auxDiscountElement = itemRow.Split(':');
                    foreach (string itemElement in auxDiscountElement)
                    {
                        appConfigurations.BenefitProgram_Coupon_Discount[a, b++] = Convert.ToInt32(itemElement);
                    }
                    a++; b = 0;
                }

            }
            catch { appConfigurations.BenefitProgram_Coupon_Discount = new Int32[5, 10]; }

            try
            {
                string[] auxDiscountRow = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_Promotion).Value.Split('|');
                appConfigurations.BenefitProgram_Coupon_Promotion = new Int32[12, 2];
                int a = 0, b = 0;
                foreach (string itemRow in auxDiscountRow)
                {
                    string[] auxPromotionElement = itemRow.Split(':');
                    foreach (string itemElement in auxPromotionElement)
                    {
                        appConfigurations.BenefitProgram_Coupon_Promotion[a, b++] = Convert.ToInt32(itemElement);
                    }
                    a++; b = 0;
                }

            }
            catch { appConfigurations.BenefitProgram_Coupon_Promotion = new Int32[12, 2]; }

            /*deprecated, matrix only contained 1 row (percentage)
            try { 
                string[] auxPromotions = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_Promotion).Value.Split(':');
                appConfigurations.BenefitProgram_Coupon_Promotion = new Int32[12];
                int i=0;
                foreach(string item in auxPromotions)
                { appConfigurations.BenefitProgram_Coupon_Promotion[i++] = Convert.ToInt32(item); }
            }
            catch { appConfigurations.BenefitProgram_Coupon_Promotion = new Int32[12]; }
            */
            try { appConfigurations.BenefitProgram_Coupon_S1_IsOpen = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsOpen).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.BenefitProgram_Coupon_S1_IsOpen = false; }

            try { appConfigurations.BenefitProgram_Coupon_S2_IsOpen = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsOpen).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.BenefitProgram_Coupon_S2_IsOpen = false; }

            try { appConfigurations.BenefitProgram_Coupon_IsOpen = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_IsOpen).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.BenefitProgram_Coupon_IsOpen = false; }

            try { appConfigurations.BenefitProgram_Coupon_S1_IsCalculated = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsCalculated).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.BenefitProgram_Coupon_S1_IsCalculated = false; }

            try { appConfigurations.BenefitProgram_Coupon_S2_IsCalculated = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsCalculated).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.BenefitProgram_Coupon_S2_IsCalculated = false; }

            try { appConfigurations.BenefitProgram_Coupon_IsCalculated = Repository.AppConfigurations.GetByKey(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_IsCalculated).Value.Trim().ToLower() == "true" ? true : false; }
            catch { appConfigurations.BenefitProgram_Coupon_IsCalculated = false; }


            //Password management
            try { appConfigurations.PasswordValidFormatPattern = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.PasswordValidFormatPattern).Value; }
            catch { appConfigurations.PasswordValidFormatPattern = ""; }
            try { appConfigurations.PasswordInvalidFormatMessage = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.PasswordInvalidFormatMessage).Value; }
            catch { appConfigurations.PasswordInvalidFormatMessage = ""; }
            try { appConfigurations.PasswordExpireDays = Convert.ToInt32(Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.PasswordExpireDays).Value); }
            catch { appConfigurations.PasswordExpireDays = 49; }

            //News management
            try { appConfigurations.NewsImageStoragePath = Repository.AppConfigurations.GetByKey(Model.Keys.AppConfigurationKey.NewsImagesStoragePath).Value; }
            catch { appConfigurations.NewsImageStoragePath = "~/Content/Media/Shared/Pictures"; }

            return appConfigurations;
        }

        #endregion

    }
    public sealed class UserRole
    {
        public const string All = "*",
        SysAdmin = "sysadmin",
        AppAdmin = "appadmin",
        EmployeeManagerOperation = "employee-manager_operation",
        EmployeeManagerView = "employee-manager_view",
        EmployeeRTVOperation = "employee-rtv_operation",
        EmployeeRTVView = "employee-rtv_view",
        CustomerDistributorOperation = "customer-distributor_operation",
        CustomerDistributorView = "customer-distributor_view",
        CustomerSubdistributorOperation = "customer-subdistributor_operation",
        CustomerSubdistributorView = "customer-subdistributor_view";
    }
    public interface IAppConfiguration
    {
        string HostUrl { get; }
        bool IsDebugEnabled { get; set; }
        
        //Password Management
        string PasswordValidFormatPattern { get; set; }
        string PasswordInvalidFormatMessage { get; set; }
        int PasswordExpireDays { get; set; }

        //Email Server
        string MailServerSmtpAddress { get; set; }
        int MailServerSmtpPort { get; set; }
        string MailServerFromEmail { get; set; }
        string MailServerFromPassword { get; set; }
        string MailServerFromDisplayName { get; set; }
        bool MailServerMailIsBodyHtml { get; set; }
        bool MailServerMailUseSSL { get; set; }
        string MailServerMailToCC { get; set; }
        string MailServerMailToCCO { get; set; }
        string MailServerContactPageMailTo { get; set; }

        //Email layouts
        string LayoutEmailContractDistributor_SendTodistributor_Subject { get; set; }
        string LayoutEmailContractDistributor_SendTodistributor_Body { get; set; }
        string LayoutEmailContactPage_Subject { get; set; }
        string LayoutEmailContactPage_Body { get; set; }

        //Benefit programs
        Int32[,] BenefitProgram_Coupon_Discount { get; set; }
        Int32[,] BenefitProgram_Coupon_Promotion { get; set; }
        bool BenefitProgram_Coupon_S1_IsOpen { get; set; }
        bool BenefitProgram_Coupon_S2_IsOpen { get; set; }
        bool BenefitProgram_Coupon_IsOpen { get; set; }
        bool BenefitProgram_Coupon_S1_IsCalculated { get; set; }
        bool BenefitProgram_Coupon_S2_IsCalculated { get; set; }
        bool BenefitProgram_Coupon_IsCalculated { get; set; }

        //News management
        string NewsImageStoragePath { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Security;
using PSD.Model.Filters.PipelineAndFilters.DistributorFilters;

namespace PSD.Controller
{
    public class DistributorController : _BaseController
    {
        public DistributorController(IAppConfiguration configurations)
            : base("DistributorController.", configurations)
        {

        }

        /// <summary>
        /// This method provides the required object for the Pipeline filtering
        /// </summary>
        /// <returns></returns>
        public IQueryable<Distributor> RetrieveAllToFilter()
        {
            return Repository.Distributors.GetAllToFilter();
        }

        /// <summary>
        /// Provides a list of Distributor as a result of filtering the DB on specific filters
        /// </summary>
        /// <returns></returns>
        public List<Distributor> FilteredItems()
        {
            ResultManager.IsCorrect = false;
            List<Distributor> items = new List<Distributor>();
            try
            {
                // implement new pipeline filtering
                var distributorList = RetrieveAllToFilter();
                DistributorFilteringPipeline pipeline = new DistributorFilteringPipeline();

                if (Identity.CurrentUser.IsInRole(UserRole.EmployeeManagerOperation + "," + UserRole.EmployeeManagerView + "," + UserRole.EmployeeRTVOperation + "," + UserRole.EmployeeRTVView))
                {
                    List<string> zones = GetBayerEmployeeZones(CurrentUser.Id);
                    pipeline.Register(new ZoneListFilter(zones));
                }

                items = pipeline.Process(distributorList).ToList();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Index.311: Error while retrieving Distributor list from DB: " + ex.Message);
            }
            return items;
        }

        public IEnumerable<Distributor> RetrieveDistributorList()
        {
            return Repository.Distributors.GetAll();
        }
        public IEnumerable<Distributor> RetrieveDistributorListByzone(int zoneId)
        {
            return Repository.Distributors.GetAll();//TODO:filter by zone
        }
        public Distributor RetrieveDistributor(int id)
        {
            return Repository.Distributors.Get(id);
        }
        public AddressPostalCode GetPostalCode(string postalCode)
        {
            return Repository.AddressPostalCodes.GetByName(postalCode);
        }
        public List<AddressMunicipality> GetMunicipalitiesByState(int addressStateId)
        {
            return Repository.AddressMunicipalities.GetByStateId(addressStateId).ToList();
        }
        public Cat_Crop GetCropById(int id)
        {
            return Repository.Crops.Get(id);
        }
        /*
        public bool CreateDistributor(Model.DistributorEmployee model)
        {
            string token = PSD.Common.Random.Token(20);
            Model.DistributorEmployee distributorEmployee = null;

            PSD.Repository.UnitOfWork myRepository = new Repository.UnitOfWork(new Repository.PSDContext());

            try
            {
                //user principal
                User user = User.NewEmpty();
                user.Person = null;
                user.Cat_UserStatusId = 4;//created
                //user.Cat_UserStatus = Repository.UserStatuses.Get(4);///TODO: why do I need this if i already set statusId (doesn't update automatically unless i restart app)
                user.NickName = model.EMail;
                user.FailedLoginAttempts = 0;
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = PSD.Common.Dates.Today;
                                
                Model.RolesXUser rolesXUser = new RolesXUser();
                rolesXUser.UserId = user.Id;
                rolesXUser.Cat_UserRoleId = 7;//7:distributor (principal)
                user.RolesXUser = new List<RolesXUser>() { rolesXUser };

                distributorEmployee = DistributorEmployee.NewEmpty();
                distributorEmployee.Distributor = null;
                distributorEmployee.EMail = model.EMail;
                distributorEmployee.Name = model.Distributor.CommercialName;
                distributorEmployee.User = user;
                
                
                //user view
                User userView = User.NewEmpty();
                userView.Person = null;
                userView.Cat_UserStatusId = 4;//created
                //userView.Cat_UserStatus = Repository.UserStatuses.Get(4);///TODO: why do I need this if i already set statusId (doesn't update automatically unless i restart app)
                userView.NickName = "";//no email at first
                userView.FailedLoginAttempts = 0;
                userView.LoginToken = "";
                userView.LoginTokenGeneratedDate = null;
                
                Model.RolesXUser rolesXUserView = new RolesXUser();
                rolesXUserView.UserId = userView.Id;
                rolesXUserView.Cat_UserRoleId = 8;//8:distributor(view)
                userView.RolesXUser = new List<RolesXUser>() { rolesXUserView };

                Model.DistributorEmployee distributorEmployeeView = DistributorEmployee.NewEmpty();
                distributorEmployeeView.Distributor = null;
                distributorEmployeeView.EMail = model.EMail;
                distributorEmployeeView.Name = model.Distributor.CommercialName + "(consulta)";
                distributorEmployeeView.User = userView;
                //employee.Cat_ZoneId = model.Cat_ZoneId == -1? null : model.Cat_ZoneId;

                //distributor
                /*
                List<Model.BusinessName> auxBusiness = model.Distributor.BusinessNames.ToList();//first is principal, second is secondary
                List<Model.BusinessName> businesses = new List<BusinessName>();//first is principal, second is secondary
                foreach(string item in auxBusiness[1].Name.Split('/'))
                {
                    businesses.Add(new BusinessName() { Name = item, IdB = item, IsMain = false });                
                }
                
                distributor.BusinessNames = businesses;
                */
        /*
                //distributor
                Model.Distributor distributor = Distributor.NewEmpty();
                
                //TODO:set address
                distributor.Address = null;
                distributor.AddressId = null;

                distributor.IdB = model.Distributor.IdB;
                distributor.BusinessName = model.Distributor.BusinessName;
                distributor.CommercialName = model.Distributor.CommercialName;
                distributor.WebSite = model.Distributor.WebSite;
                distributor.DistributorUsers.Add(distributorEmployee);
                distributor.DistributorUsers.Add(distributorEmployeeView);
                distributor.CropsXMunicipality = model.Distributor.CropsXMunicipality;

                myRepository.Distributors.Add(distributor);

                myRepository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "exception while creating distributor", ex);
                ResultManager.IsCorrect = false;
                ResultManager.Add(ErrorDefault, "");
                return false;
            }

            if (SendUserInvitationEmail(distributorEmployee.User))
            {
                ResultManager.Add("El distribuidor se ha creado correctamente", "");
            }
            else
            {
                //ResultManager.IsCorrect = false;
                ResultManager.Add("El distribuidor se ha creado correctamente, sin embargo hubo un problema al enviar la invitación de correo", "Puede reenviar la invitación desde la página de detalle de usuario");
            }
            return ResultManager.IsCorrect;
        }
        */
        
        public bool CreateDistributor(Model.DistributorEmployee model)
        {
            // validating unique mail
            if (IsMailAddressCurrentlyUsed(model.EMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            string token = PSD.Common.Random.Token(20);
            Model.DistributorEmployee distributorEmployee = null;
            
            try
            {
                //user principal
                User user = User.NewEmpty();
                user.Person = null;
                user.Cat_UserStatusId = 4;//created
                user.NickName = model.EMail;
                user.FailedLoginAttempts = 0;
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = PSD.Common.Dates.Today;
                                
                Model.RolesXUser rolesXUser = new RolesXUser();
                rolesXUser.UserId = user.Id;
                rolesXUser.Cat_UserRoleId = 7;//7:distributor (principal)
                user.RolesXUser = new List<RolesXUser>() { rolesXUser };

                distributorEmployee = DistributorEmployee.NewEmpty();
                distributorEmployee.Distributor = null;
                distributorEmployee.EMail = model.EMail;
                distributorEmployee.Name = model.Distributor.CommercialName;
                distributorEmployee.User = user;
                
                
                //user view
                User userView = User.NewEmpty();
                userView.Person = null;
                userView.Cat_UserStatusId = 4;//created
                userView.NickName = "";//no email at first
                userView.FailedLoginAttempts = 0;
                userView.LoginToken = "";
                userView.LoginTokenGeneratedDate = null;
                
                Model.RolesXUser rolesXUserView = new RolesXUser();
                rolesXUserView.UserId = userView.Id;
                rolesXUserView.Cat_UserRoleId = 8;//8:distributor(view)
                userView.RolesXUser = new List<RolesXUser>() { rolesXUserView };

                Model.DistributorEmployee distributorEmployeeView = DistributorEmployee.NewEmpty();
                distributorEmployeeView.Distributor = null;
                distributorEmployeeView.EMail = model.EMail;
                distributorEmployeeView.Name = model.Distributor.CommercialName + "(consulta)";
                distributorEmployeeView.User = userView;
                //employee.Cat_ZoneId = model.Cat_ZoneId == -1? null : model.Cat_ZoneId;

                //distributor
                Model.Distributor distributor = Distributor.NewEmpty();
                
                //TODO:set address
                AddressColony auxColony = Repository.AddressColonies.Get((int)model.Distributor.Address.AddressColonyId);                
                Address bnAddress = new Address();
                bnAddress.AddressStateId = auxColony.AddressStateId;
                bnAddress.AddressMunicipalityId = auxColony.AddressMunicipalityId;
                bnAddress.AddressPostalCodeId = auxColony.AddressPostalCodeId;
                bnAddress.AddressColonyId = auxColony.Id;
                bnAddress.Street = model.Distributor.Address.Street;
                bnAddress.NumberExt = model.Distributor.Address.NumberExt;
                bnAddress.NumberInt = model.Distributor.Address.NumberInt;
                distributor.Address = bnAddress;

                distributor.IdB = Repository.AppConfigurations.IdBCounterGetNextDistributor();
                distributor.BusinessName = model.Distributor.BusinessName;
                distributor.CommercialName = model.Distributor.CommercialName;
                distributor.WebSite = model.Distributor.WebSite;
                distributor.DistributorUsers.Add(distributorEmployee);
                distributor.DistributorUsers.Add(distributorEmployeeView);
                distributor.CropsXMunicipality = model.Distributor.CropsXMunicipality;

                distributor.CurrentContract = null;

                Repository.Distributors.Add(distributor);

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "exception while creating distributor", ex);
                ResultManager.IsCorrect = false;
                ResultManager.Add(ErrorDefault, "");
                return false;
            }

            if (SendUserInvitationEmail(distributorEmployee.User))
            {
                ResultManager.Add("El distribuidor se ha creado correctamente", "");
            }
            else
            {
                //ResultManager.IsCorrect = false;
                ResultManager.Add("El distribuidor se ha creado correctamente, sin embargo hubo un problema al enviar la invitación de correo", "Puede reenviar la invitación desde la página de detalle de usuario");
            }
            return ResultManager.IsCorrect;
        }

        public bool DeleteDistributor(int distributorId)
        {
            ResultManager.IsCorrect = false;

            // validate bayer contract associated
            bool hasBayerContract = Repository.ContractsDistributor.GetAll().Any(x => x.DistributorId == distributorId);
            if (hasBayerContract)
            {
                ResultManager.Add(ErrorDefault, Trace + "DeleteDistributor.### El Distribuidor seleccionado no se puede eliminar ya que existe un contracto con Bayer existente.");
                return false;
            }

            // validate subdistributor - bayer associated
            bool isAssociatedToSubdistributorContract = Repository.DistributorPurchasesXContractSubdistributors.GetAll().Any(x => x.DistributorId == distributorId);
            if (isAssociatedToSubdistributorContract)
            {
                ResultManager.Add(ErrorDefault, Trace + "DeleteDistributor.### El Distribuidor seleccionado no se puede eliminar ya que esta asociado a un contracto con Subdistibuidor - Bayer existente.");
                return false;
            }

            // now proceed with the delete operation
            try
            {
                #region Distributor Branch Delete
                // remove associated Distributor Branch Contacts
                IEnumerable<DistributorBranch> DistributorBranches = Repository.DistributorBranches.GetAll().Where(x => x.DistributorId == distributorId);
                foreach (DistributorBranch branch in DistributorBranches)
                {
                    IEnumerable<int> distributorBranchContactIds = Repository.DistributorBranchContacts.GetAll().Where(x => x.DistributorBranchId == branch.Id).Select(y => y.Id);
                    foreach (int branchContactId in distributorBranchContactIds)
                    {
                        Repository.DistributorBranchContacts.Remove(branchContactId);
                    }
                }

                // remove associated Distributor Branches
                IEnumerable<int> distributorBranchIds = DistributorBranches.Select(x => x.Id);
                foreach (int branchId in distributorBranchIds)
                {
                    Repository.DistributorBranches.Remove(branchId);
                }

                // remove associated Distributor Branches address
                IEnumerable<int> distributorBranchAddressIds = DistributorBranches.Select(x => x.AddressId);
                foreach (int branchAddressId in distributorBranchAddressIds)
                {
                    Repository.Addresses.Remove(branchAddressId);
                }
                #endregion

                #region Distributor Contact Delete

                IEnumerable<int> distributorContactIds = Repository.DistributorContacts.GetAll().Where(x => x.DistributorId == distributorId).Select(y => y.Id);
                foreach(int contactId in distributorContactIds)
                {
                    Repository.DistributorContacts.Remove(contactId);
                }

                #endregion

                #region Distributor Crops x Municipality Delete

                IEnumerable<int> distributorCropsxMunicipalityIds = Repository.DistributorCropsXMunicipality.GetAll().Where(x => x.DistributorId == distributorId).Select(y => y.Id);
                foreach(int cropxMunicipalityId in distributorCropsxMunicipalityIds)
                {
                    Repository.DistributorCropsXMunicipality.Remove(cropxMunicipalityId);
                }

                #endregion

                #region Distributor entity delete

                // rmeove address directly associated to distributor
                int? distributorAddressId = Repository.Distributors.Get(distributorId).AddressId;
                if (distributorAddressId.HasValue)
                {
                    Repository.Addresses.Remove(distributorAddressId.Value);
                }

                // remove distributor entity itself
                Repository.Distributors.Remove(distributorId);

                // remove data from Person_DistributorEmployee entity
                IEnumerable<int> personIds = Repository.DistributorEmployees.GetAll().Where(x => x.DistributorId == distributorId).Select(y => y.Id);
                foreach(int personId in personIds)
                {
                    // delete from DistributorEmployees
                    Repository.DistributorEmployees.Remove(personId);
                    // delete from Person
                    Repository.Persons.Remove(personId);
                    // delete from User
                    Repository.Users.Remove(personId);
                    // delete from RolesXUser
                    Repository.RolesXUser.Remove(personId);
                }

                #endregion

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "Exception while deleting distributor", ex);
                ResultManager.IsCorrect = false;
            }

            return ResultManager.IsCorrect;
        }

        public bool SendUserInvitationEmail(int userId)
        {
            User user = null;
            string token = PSD.Common.Random.Token(20);
            user = Repository.Users.Get(userId);
            if (user == null)
            {
                ErrorManager.Add(Trace + "SendInvitationEmail.211", "Error while retrieving user from database", "No user was found with Id '" + userId + "'");
                return false;
            }
            try
            {
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = PSD.Common.Dates.Today;
                Repository.Complete();
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "SendInvitationEmail.311", "Exception was thrown while retrieving user's token from database", "User Id '" + user.Id + "'", ex);
            }

            return SendUserInvitationEmail(user);
        }
        private bool SendUserInvitationEmail(User user)
        {
            string tokenUrl = Configurations.HostUrl + "Account/LoginByToken?token=" + user.LoginToken;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() { { "userNickName", user.NickName }, { "message", "Hemos creado una cuenta para usted, de click en la liga de abajo para activar su cuenta:" }, { "tokenUrl", tokenUrl } };
            string messageBody = "<h2>Información de su nueva cuenta</h2><h3>Bayer - Portal de Servicios al Distribuidor</h3><p>@userNickName</p><p>@message</p><p><a href='@tokenUrl'>Activar su cuenta</a></p>";
            string subject = "Informacion de su nueva cuenta - Bayer Portal de Servicios al Distribuidor";
            PSD.Util.Mailer mailer = new Util.Mailer("prueba1@roomie-it.org", "Roomie@01", "www.roomie-it.org", 25, false);
            if (mailer.SendSingle(user.NickName, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendInvitationEmail.911", "Error while sending email to employee '" + user.NickName + "'", mailer.ResultDetails[0]);
                return false;
            }
        }       

        #region Branches

        /// <summary>
        /// Retrieve IEnumerable of DistributoBranch objects related to one DistributorId
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public IEnumerable<DistributorBranch> GetDistributorBranchesByDistributorId(int distributorId = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (distributorId == -1 || distributorId == 0)
            {//no distributor id was received
                ResultManager.Add(ErrorDefault, Trace + "GetDistributorBranchesByDistributorId.131 No se recibio el id del distribuidor para recuperar sucursales");
                return null;
            }

            IEnumerable<DistributorBranch> distributorBranches = new List<DistributorBranch>();
            try
            {
                distributorBranches = Repository.DistributorBranches.GetAll()
                                                .Where(x => x.DistributorId == distributorId)
                                                .ToList();
                if (distributorBranches == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "GetDistributorBranchesByDistributorId.511 No se encontró un distribuidor con id '" + distributorId + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "GetDistributorBranchesByDistributorId.511 Excepción al obtener el listado de sucursales de distribuidor: " + ex.Message);
            }

            return distributorBranches;
        }

        /// <summary>
        /// Retrieve IEnumerable of DistributoBranch objects related to one DistributorId
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public IEnumerable<AddressColony> GetAvailableColoniesByPostalCode(int postalCode = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (postalCode == -1 || postalCode == 0)
            {//no distributor id was received
                ResultManager.Add(ErrorDefault, Trace + "GetAvailableColoniesByPostalCode.131 No se recibio el id del codigo postal para recuperar colonias");
                return null;
            }

            IEnumerable<AddressColony> availableColonies = new List<AddressColony>();
            try
            {
                availableColonies = Repository.AddressColonies.GetAll()
                                              .Where(x => x.AddressPostalCodeId == postalCode)
                                              .ToList();
                if (availableColonies == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "GetAvailableColoniesByPostalCode.511 No se encontraron colonias con el codigo postal id '" + postalCode + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "GetDistributorBranchesByDistributorId.511 Excepción al obtener el listado de sucursales de distribuidor: " + ex.Message);
            }

            return availableColonies;
        }


        public bool DistributorBranchAdd(DistributorBranch item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchAdd.111 No se recibio el objeto sucursal del distribuidor a crear");
                return false;
            }
            if (item.DistributorId == -1 || item.DistributorId == 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchAdd.111 No se recibio el id del distribuidor");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre de la sucursal no puede estar vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Address.Street))
            {
                ResultManager.Add("Campo calle no puede estar vacio");
                return false;
            }
            if (item.Address.AddressPostalCodeId == 0)
            {
                ResultManager.Add("Campo codigo postal id no puede ser zero");
                return false;
            }

            //insert new item
            try
            {
                DistributorBranch auxBranch = new DistributorBranch();
                auxBranch.DistributorId = item.DistributorId;
                auxBranch.Name = item.Name;
                auxBranch.Address = item.Address;

                Repository.DistributorBranches.Add(auxBranch);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchAdd.511 Excepción al agregar sucursal al distribuidor con id '" + item.DistributorId + "': " + ex.Message);
            }
            return false;
        }

        public bool DistributorBranchDelete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no distributor branch id was received
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchDelete.131 No se recibio el id de sucursal a eliminar");
                return false;
            }

            //delete item
            try
            {
                DistributorBranch itemToDelete = Repository.DistributorBranches.Get(id);

                Repository.Addresses.Remove(itemToDelete.AddressId);
                Repository.DistributorBranches.Remove(id);

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchDelete.511 Excepción al eliminar sucursal con id '" + id + "': " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Retrieve one sucursal based on its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DistributorBranch DistributorBranchRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no distributor branch id was received
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchRetrieve.131 No se recibio el id de sucursal");
                return null;
            }

            DistributorBranch auxDistributorBranch = null;
            try
            {
                auxDistributorBranch = Repository.DistributorBranches.Get(id);
                if (auxDistributorBranch == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "DistributorBranchRetrieve.511 No se encontró una sucursal con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchRetrieve.511 Excepción al obtener la sucursal: " + ex.Message);
            }

            return auxDistributorBranch;
        }

        public bool DistributorBranchUpdate(DistributorBranch item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchUpdate.111 No se recibio el objeto sucursal a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no branch id was received
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchUpdate.131 No se recibio el id de sucursal a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre de la sucursal no puede estar vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Address.Street))
            {
                ResultManager.Add("Campo calle no puede estar vacio");
                return false;
            }
            if (item.Address.AddressPostalCodeId == 0)
            {
                ResultManager.Add("Campo codigo postal id no puede ser zero");
                return false;
            }

            //update item
            try
            {
                DistributorBranch auxDIstributorBranch = Repository.DistributorBranches.Get(item.Id);
                if (auxDIstributorBranch == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "DistributorBranchUpdate.311 Sucursal con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }

                auxDIstributorBranch.Name = item.Name;
                auxDIstributorBranch.Address.AddressColonyId = item.Address.AddressColonyId;
                auxDIstributorBranch.Address.AddressMunicipalityId = item.Address.AddressMunicipalityId;
                auxDIstributorBranch.Address.AddressPostalCodeId = item.Address.AddressPostalCodeId;
                auxDIstributorBranch.Address.AddressColonyId = item.Address.AddressColonyId;
                auxDIstributorBranch.Address.Street = item.Address.Street;
                auxDIstributorBranch.Address.NumberExt = item.Address.NumberExt;
                auxDIstributorBranch.Address.NumberInt = item.Address.NumberInt;
                
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "DistributorBranchUpdate.511 Excepción al editar sucursal con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }

        #endregion

        #region Branch Contacts

        /// <summary>
        /// Retrieve all the contacts assoicated to one subdistributor id
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public List<DistributorBranchContact> BranchContactsRetrieveAll(int branchId = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (branchId == -1 || branchId == 0)
            {//no branch id was received
                ResultManager.Add(ErrorDefault, Trace + "BranchContactsRetrieveAll.131 No se recibio el id del branch para recuperar contactos");
                return null;
            }

            List<DistributorBranchContact> distributorBranchContacts = new List<DistributorBranchContact>();
            try
            {
                distributorBranchContacts = Repository.DistributorBranchContacts
                                                   .Find(x => x.DistributorBranchId == branchId)
                                                   .ToList();
                if (distributorBranchContacts == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "BranchContactsRetrieveAll.511 No se encontró un branch con id '" + branchId.ToString() + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactsRetrieveAll.511 Excepción al obtener el listado de contactos de branch: " + ex.Message);
            }

            return distributorBranchContacts;
        }

        /// <summary>
        /// Retrieve one contact based on its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DistributorBranchContact BranchContactRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no conatact id was received
                ResultManager.Add(ErrorDefault, Trace + "BranchContactRetrieve.131 No se recibio el id del contacto");
                return null;
            }

            DistributorBranchContact auxContact = null;
            try
            {
                auxContact = Repository.DistributorBranchContacts.Get(id);
                if (auxContact == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "BranchContactRetrieve.511 No se encontró un contacto con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactRetrieve.511 Excepción al obtener el contacto a editar: " + ex.Message);
            }

            return auxContact;
        }

        public bool BranchContactAdd(DistributorBranchContact item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactAdd.111 No se recibio el objeto contacto de la sucursal a crear");
                return false;
            }
            if (item.DistributorBranchId == -1 || item.DistributorBranchId == 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactAdd.111 No se recibio el id de la sucursal");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.FullName))
            {
                ResultManager.Add("El nombre del contacto no puede estar vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Role))
            {
                ResultManager.Add("El puesto no puede estar vacio");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.EMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + item.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.AssistantEMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email de asistente '" + item.AssistantEMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            //insert new item
            try
            {
                DistributorBranchContact auxContact = new DistributorBranchContact();
                auxContact.DistributorBranchId = item.DistributorBranchId;
                auxContact.DistributorBranch = Repository.DistributorBranches.Get(item.DistributorBranchId);
                auxContact.FullName = item.FullName;
                auxContact.Role = item.Role;
                auxContact.PhoneNumber = item.PhoneNumber;
                auxContact.PhoneNumberExt = item.PhoneNumberExt;
                auxContact.CellPhone = item.CellPhone;
                auxContact.EMail = item.EMail;
                auxContact.AssistantFullName = item.AssistantFullName;
                auxContact.AssistantPhoneNumber = item.AssistantPhoneNumber;
                auxContact.AssistantPhoneNumberExt = item.AssistantPhoneNumberExt;
                auxContact.AssistantCellPhone = item.AssistantCellPhone;
                auxContact.AssistantEMail = item.AssistantEMail;
                auxContact.Cat_DistributorBranchContactAreaId = item.Cat_DistributorBranchContactAreaId;
                auxContact.DistributorBranchContactArea = Repository.DistributorBranchContactAreas.Get(item.Cat_DistributorBranchContactAreaId.HasValue
                                                                                                 ? item.Cat_DistributorBranchContactAreaId.Value
                                                                                                 : 0);
                Repository.DistributorBranchContacts.Add(auxContact);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactAdd.511 Excepción al agregar contacto a sucursal con id '" + item.DistributorBranchId + "': " + ex.Message);
            }
            return false;
        }

        public bool BranchContactUpdate(DistributorBranchContact item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactUpdate.111 No se recibio el objeto del contacto a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no contact id was received
                ResultManager.Add(ErrorDefault, Trace + "BranchContactUpdate.131 No se recibio el id del contacto a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.FullName))
            {
                ResultManager.Add("El nombre del contacto no puede estar vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Role))
            {
                ResultManager.Add("El puesto no puede estar vacio");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.EMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + item.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.AssistantEMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email de asistente '" + item.AssistantEMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            //update item
            try
            {
                DistributorBranchContact auxContact = Repository.DistributorBranchContacts.Get(item.Id);
                if (auxContact == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "BranchContactUpdate.311 Contacto con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }
                //auxContact.SubdistributorId = item.SubdistributorId;
                //auxContact.Subdistributor = Repository.Subdistributors.Get(item.SubdistributorId);
                auxContact.FullName = item.FullName;
                auxContact.Role = item.Role;
                auxContact.PhoneNumber = item.PhoneNumber;
                auxContact.PhoneNumberExt = item.PhoneNumberExt;
                auxContact.CellPhone = item.CellPhone;
                auxContact.EMail = item.EMail;
                auxContact.AssistantFullName = item.AssistantFullName;
                auxContact.AssistantPhoneNumber = item.AssistantPhoneNumber;
                auxContact.AssistantPhoneNumberExt = item.AssistantPhoneNumberExt;
                auxContact.AssistantCellPhone = item.AssistantCellPhone;
                auxContact.AssistantEMail = item.AssistantEMail;
                auxContact.Cat_DistributorBranchContactAreaId = item.Cat_DistributorBranchContactAreaId;
                auxContact.DistributorBranchContactArea = Repository.DistributorBranchContactAreas.Get(item.Cat_DistributorBranchContactAreaId.HasValue
                                                                                                 ? item.Cat_DistributorBranchContactAreaId.Value
                                                                                                 : 0);

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactUpdate.511 Excepción al editar el contacto con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }
        public bool BranchContactDelete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no contact id was received
                ResultManager.Add(ErrorDefault, Trace + "BranchContactDelete.131 No se recibio el id del contacto a eliminar");
                return false;
            }

            //delete item
            try
            {
                Repository.DistributorBranchContacts.Remove(id);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "BranchContactDelete.511 Excepción al eliminar el contacto con id '" + id + "': " + ex.Message);
            }
            return false;
        }


        #endregion

        #region Contacts

        /// <summary>
        /// Retrieve all the contacts assoicated to one subdistributor id
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public List<DistributorContact> ContactsRetrieveAll(int distributorId = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (distributorId == -1 || distributorId == 0)
            {//no subdistributor id was received
                ResultManager.Add(ErrorDefault, Trace + "ContactsRetrieveAll.131 No se recibio el id del distribuidor para recuperar contactos");
                return null;
            }

            List<DistributorContact> distributorContacts = new List<DistributorContact>();
            try
            {
                distributorContacts = Repository.DistributorContacts
                                                   .Find(x => x.DistributorId == distributorId)
                                                   .ToList();
                if (distributorContacts == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ContactsRetrieveAll.511 No se encontró un distribuidor con id '" + distributorContacts + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactsRetrieveAll.511 Excepción al obtener el listado de contactos de distribuidor a editar: " + ex.Message);
            }

            return distributorContacts;
        }

        /// <summary>
        /// Retrieve one contact based on its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DistributorContact ContactRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no conatact id was received
                ResultManager.Add(ErrorDefault, Trace + "ContactRetrieve.131 No se recibio el id del contacto");
                return null;
            }

            DistributorContact auxContact = null;
            try
            {
                auxContact = Repository.DistributorContacts.Get(id);
                if (auxContact == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ContactRetrieve.511 No se encontró un contacto con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactRetrieve.511 Excepción al obtener el contacto a editar: " + ex.Message);
            }

            return auxContact;
        }


        public bool ContactAdd(DistributorContact item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactAdd.111 No se recibio el objeto contacto del subdistribuidor a crear");
                return false;
            }
            if (item.DistributorId == -1 || item.DistributorId == 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactAdd.111 No se recibio el id del distribuidor");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.FullName))
            {
                ResultManager.Add("El nombre del contacto no puede estar vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Role))
            {
                ResultManager.Add("El puesto no puede estar vacio");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.EMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + item.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.AssistantEMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email de asistente '" + item.AssistantEMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            //insert new item
            try
            {
                DistributorContact auxContact = new DistributorContact();
                auxContact.DistributorId = item.DistributorId;
                auxContact.Distributor = Repository.Distributors.Get(item.DistributorId);
                auxContact.FullName = item.FullName;
                auxContact.Role = item.Role;
                auxContact.PhoneNumber = item.PhoneNumber;
                auxContact.PhoneNumberExt = item.PhoneNumberExt;
                auxContact.CellPhone = item.CellPhone;
                auxContact.EMail = item.EMail;
                auxContact.AssistantFullName = item.AssistantFullName;
                auxContact.AssistantPhoneNumber = item.AssistantPhoneNumber;
                auxContact.AssistantPhoneNumberExt = item.AssistantPhoneNumberExt;
                auxContact.AssistantCellPhone = item.AssistantCellPhone;
                auxContact.AssistantEMail = item.AssistantEMail;
                auxContact.DistributorContactAreaId = item.DistributorContactAreaId;
                auxContact.DistributorContactArea = Repository.DistributorContactAreas.Get(item.DistributorContactAreaId.HasValue
                                                                                                 ? item.DistributorContactAreaId.Value
                                                                                                 : 0);
                Repository.DistributorContacts.Add(auxContact);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactAdd.511 Excepción al agregar contacto al distribuidor con id '" + item.DistributorId + "': " + ex.Message);
            }
            return false;
        }

        public bool ContactUpdate(DistributorContact item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactUpdate.111 No se recibio el objeto del contacto a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no contact id was received
                ResultManager.Add(ErrorDefault, Trace + "ContactUpdate.131 No se recibio el id del contacto a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.FullName))
            {
                ResultManager.Add("El nombre del contacto no puede estar vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Role))
            {
                ResultManager.Add("El puesto no puede estar vacio");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.EMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + item.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(item.AssistantEMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email de asistente '" + item.AssistantEMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            //update item
            try
            {
                DistributorContact auxContact = Repository.DistributorContacts.Get(item.Id);
                if (auxContact == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ContactUpdate.311 Contacto con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }
                //auxContact.SubdistributorId = item.SubdistributorId;
                //auxContact.Subdistributor = Repository.Subdistributors.Get(item.SubdistributorId);
                auxContact.FullName = item.FullName;
                auxContact.Role = item.Role;
                auxContact.PhoneNumber = item.PhoneNumber;
                auxContact.PhoneNumberExt = item.PhoneNumberExt;
                auxContact.CellPhone = item.CellPhone;
                auxContact.EMail = item.EMail;
                auxContact.AssistantFullName = item.AssistantFullName;
                auxContact.AssistantPhoneNumber = item.AssistantPhoneNumber;
                auxContact.AssistantPhoneNumberExt = item.AssistantPhoneNumberExt;
                auxContact.AssistantCellPhone = item.AssistantCellPhone;
                auxContact.AssistantEMail = item.AssistantEMail;
                auxContact.DistributorContactAreaId = item.DistributorContactAreaId;
                auxContact.DistributorContactArea = Repository.DistributorContactAreas.Get(item.DistributorContactAreaId.HasValue
                                                                                                 ? item.DistributorContactAreaId.Value
                                                                                                 : 0);

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactUpdate.511 Excepción al editar el contacto con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }

        public bool ContactDelete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no contact id was received
                ResultManager.Add(ErrorDefault, Trace + "ContactDelete.131 No se recibio el id del contacto a eliminar");
                return false;
            }

            //delete item
            try
            {
                Repository.DistributorContacts.Remove(id);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactDelete.511 Excepción al eliminar el contacto con id '" + id + "': " + ex.Message);
            }
            return false;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Security;
using PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters;

namespace PSD.Controller
{
    public class SubdistributorController : _BaseController
    {
        public SubdistributorController(IAppConfiguration configurations)
            : base("SubdistributorController.", configurations)
        {

        }

        public IQueryable<Subdistributor> RetrieveAllToFilter()
        {
            return Repository.Subdistributors.GetAllToFilter();
        }

        /// <summary>
        /// Provides a list of Subdistributor as a result of filtering the DB on specific filters
        /// </summary>
        /// <returns></returns>
        public List<Subdistributor> FilteredItems()
        {
            ResultManager.IsCorrect = false;
            List<Subdistributor> items = new List<Subdistributor>();
            try
            {
                // implement new pipeline filtering
                var subdistributorList = RetrieveAllToFilter();
                SubdistributorFilteringPipeline pipeline = new SubdistributorFilteringPipeline();

                if (Identity.CurrentUser.IsInRole(UserRole.EmployeeManagerOperation + "," + UserRole.EmployeeManagerView + "," + UserRole.EmployeeRTVOperation + "," + UserRole.EmployeeRTVView))
                {
                    List<string> zones = GetBayerEmployeeZones(CurrentUser.Id);
                    pipeline.Register(new ZoneListFilter(zones));
                }

                if (Identity.CurrentUser.IsInRole(UserRole.CustomerDistributorOperation + "," + UserRole.CustomerDistributorView))
                {
                    pipeline.Register(new RelatedByActiveContractDistributorIdFilter(CurrentUser.Id));
                }

                items = pipeline.Process(subdistributorList).ToList();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Index.311: Error while retrieving Subdistributor list from DB: " + ex.Message);
            }
            return items;
        }

        public IEnumerable<Subdistributor> RetrieveSubdistributorList()
        {
            return Repository.Subdistributors.GetAll();
        }
        public Subdistributor RetrieveSubdistributor(int id)
        {
            Subdistributor auxItem;
            try
            {
                auxItem = Repository.Subdistributors.Get(id);
            }
            catch(Exception ex)
            {
                ResultManager.Add(ErrorDefault, "Excepción al tratar de obtener el subdistirbuidor con id '" + id + "'", ex);
                return null;
            }
            if(auxItem == null)
            {
                ResultManager.Add("No se encontró al subdistribuidor", "No se encontró un subdistirbuidor con id '" + id + "' en la base de datos");
            }
            else
            {
                ResultManager.IsCorrect = true;
            }
            return auxItem;
        }
        public bool CreateSubdistributor(Subdistributor model)
        {
            ResultManager.Clear();

            //sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.111 No se recibió el modelo");
                return false;
            }
            if (model.BNAddress == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.151 El modelo recibido no contiene el campo BNAddress");
                return false;
            }
            if (model.SubdistributorEmployees == null || model.SubdistributorEmployees.FirstOrDefault() == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.171 El modelo recibido no contiene el campo SubdistributorEmployee[0]");
                return false;
            }

            //business validations
            if (model.SubdistributorEmployees == null || model.SubdistributorEmployees.FirstOrDefault() == null || string.IsNullOrWhiteSpace(model.SubdistributorEmployees.FirstOrDefault().EMail))
            {
                ResultManager.Add("El correo electrónico del dueño no puede estar vacio", Trace + "UpdateSubdistributor.211 El campo 'SubdistributorEmployees[0].EMail' esta vacio");
                return false;
            }
            if (model.BNAddress.AddressColonyId == null || model.BNAddress.AddressColonyId < 1)
            {
                ResultManager.Add("Se debe seleccionar la dirección del representante legal", Trace + "UpdateSubdistributor.211 El campo 'BNAddress.AddressColonyId' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.BNAddress.Street))
            {
                ResultManager.Add("Se debe seleccionar la calle, en la dirección del representante legal", Trace + "UpdateSubdistributor.211 El campo 'BNAddress.Street' esta vacio");
                return false;
            }
            if (model.RTV_BayerEmployeeId < 1)
            {
                ResultManager.Add("Se debe tener asignado un RTV", Trace + "UpdateSubdistributor.211 El campo 'RTV_BayerEmployeeId' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.Type) || model.Type == "-1")
            {
                ResultManager.Add("Se debe seleccionar el tipo Subdistribuidor ó Agricultor", Trace + "UpdateSubdistributor.211 El campo 'Type' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.BusinessName))
            {
                ResultManager.Add("Se debe indicar la razón social", Trace + "UpdateSubdistributor.211 El campo 'BusinessName' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.BNLegalRepresentative))
            {
                ResultManager.Add("Se debe indicar el nombre del representante legal", Trace + "UpdateSubdistributor.211 El campo 'BNLegalRepresentative' esta vacio");
                return false;
            }
            if (model.CommercialNames.Count <= 0 || model.CommercialNames.FirstOrDefault().Name.Length == 0)
            {
                ResultManager.Add("Se debe indicar al menos un nombre comercial", Trace + "UpdateSubdistributor.211 El campo 'CommercialNames' esta vacio");
                return false;
            }
            string emailToCheck = model.SubdistributorEmployees.Count > 0 ? model.SubdistributorEmployees.FirstOrDefault().EMail : string.Empty;
            if (IsMailAddressCurrentlyUsed(emailToCheck))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + emailToCheck + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            string token = PSD.Common.Random.Token(20);
            Model.SubdistributorEmployee subdistributorEmployee = null;

            try
            {
                //user principal
                User user = User.NewEmpty();
                user.Person = null;
                user.Cat_UserStatusId = 4;//created
                user.Cat_UserStatus = Repository.UserStatuses.Get(4);///TODO: why do I need this if i already set statusId (doesn't update automatically unless i restart app)
                user.NickName = model.SubdistributorEmployees.FirstOrDefault().EMail;
                user.FailedLoginAttempts = 0;
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = PSD.Common.Dates.Today;

                Model.RolesXUser rolesXUser = new RolesXUser();
                rolesXUser.UserId = user.Id;
                rolesXUser.Cat_UserRoleId = 9;//9:subdistributor (principal)
                user.RolesXUser = new List<RolesXUser>() { rolesXUser };

                subdistributorEmployee = new SubdistributorEmployee();
                subdistributorEmployee.Subdistributor = null;
                subdistributorEmployee.EMail = model.SubdistributorEmployees.FirstOrDefault().EMail;
                subdistributorEmployee.Name = model.CommercialNames.FirstOrDefault().Name;
                subdistributorEmployee.User = user;
                                
                /*
                //user view
                User userView = User.NewEmpty();
                userView.Person = null;
                userView.Cat_UserStatusId = 4;//created
                userView.Cat_UserStatus = Repository.UserStatuses.Get(4);///TODO: why do I need this if i already set statusId (doesn't update automatically unless i restart app)
                userView.NickName = "";//no email at first
                userView.FailedLoginAttempts = 0;
                userView.LoginToken = "";
                userView.LoginTokenGeneratedDate = null;

                Model.RolesXUser rolesXUserView = new RolesXUser();
                rolesXUserView.UserId = userView.Id;
                rolesXUserView.Cat_UserRoleId = 8;//8:distributor(view)
                userView.RolesXUser = new List<RolesXUser>() { rolesXUserView };

                Model.SubdistributorEmployee distributorEmployeeView = SubdistributorEmployee();
                distributorEmployeeView.Distributor = null;
                distributorEmployeeView.EMail = model.EMail;
                distributorEmployeeView.Name = model.Distributor.CommercialName + "(consulta)";
                distributorEmployeeView.User = userView;
                //employee.Cat_ZoneId = model.Cat_ZoneId == -1? null : model.Cat_ZoneId;
                */

                //TODO:create from model info
                Address bnAddress = new Address();
                AddressColony auxColony = Repository.AddressColonies.Get((int)model.BNAddress.AddressColonyId);
                bnAddress.AddressStateId = auxColony.AddressStateId;
                bnAddress.AddressMunicipalityId = auxColony.AddressMunicipalityId;
                bnAddress.AddressPostalCodeId = auxColony.AddressPostalCodeId;
                bnAddress.AddressColonyId = auxColony.Id;
                bnAddress.Street = model.BNAddress.Street;
                bnAddress.NumberExt = model.BNAddress.NumberExt;
                bnAddress.NumberInt = model.BNAddress.NumberInt;
                

                //subdistributor
                Model.Subdistributor subddistributor = new Subdistributor();

                //TODO:set address
                subddistributor.BNAddress = bnAddress;

                // add set of additional address
                Address address1 = new Address();
                address1.AddressStateId = null;
                address1.AddressMunicipalityId = null;
                address1.AddressPostalCodeId = null;
                address1.AddressColonyId = null;
                address1.Street = string.Empty;
                address1.NumberExt = string.Empty;
                address1.NumberInt = string.Empty;

                Address address2 = new Address();
                address2.AddressStateId = null;
                address2.AddressMunicipalityId = null;
                address2.AddressPostalCodeId = null;
                address2.AddressColonyId = null;
                address2.Street = string.Empty;
                address2.NumberExt = string.Empty;
                address2.NumberInt = string.Empty;

                Address address3 = new Address();
                address3.AddressStateId = null;
                address3.AddressMunicipalityId = null;
                address3.AddressPostalCodeId = null;
                address3.AddressColonyId = null;
                address3.Street = string.Empty;
                address3.NumberExt = string.Empty;
                address3.NumberInt = string.Empty;

                AddressesXSubdistributor addressPerSubdistributor1 = new AddressesXSubdistributor();
                addressPerSubdistributor1.Address = address1;
                addressPerSubdistributor1.Subdistributor = subddistributor;

                AddressesXSubdistributor addressPerSubdistributor2 = new AddressesXSubdistributor();
                addressPerSubdistributor2.Address = address2;
                addressPerSubdistributor2.Subdistributor = subddistributor;

                AddressesXSubdistributor addressPerSubdistributor3 = new AddressesXSubdistributor();
                addressPerSubdistributor3.Address = address3;
                addressPerSubdistributor3.Subdistributor = subddistributor;

                subddistributor.Addresses.Add(addressPerSubdistributor1);
                subddistributor.Addresses.Add(addressPerSubdistributor2);
                subddistributor.Addresses.Add(addressPerSubdistributor3);

                subddistributor.IdB = Repository.AppConfigurations.IdBCounterGetNextSubdistributor();
                subddistributor.Type = model.Type;
                subddistributor.BNLegalRepresentative = model.BNLegalRepresentative;
                subddistributor.RTV_BayerEmployeeId = model.RTV_BayerEmployeeId;
                subddistributor.RTVCreator_BayerEmployeeId = (int)model.RTV_BayerEmployeeId;
                subddistributor.BusinessName = model.BusinessName;
                subddistributor.CommercialNames = model.CommercialNames;
                subddistributor.WebSite = model.WebSite;
                subddistributor.SubdistributorEmployees.Add(subdistributorEmployee);
                //subddistributor.DistributorUsers.Add(distributorEmployeeView);
                subddistributor.CropsXMunicipality = model.CropsXMunicipality;

                Repository.Subdistributors.Add(subddistributor);

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, "exception while creating subddistributor", ex);
                return false;
            }

            if (SendUserInvitationEmail(subdistributorEmployee.User))
            {
                ResultManager.Add("El subdistribuidor se ha creado correctamente", "");
            }
            else
            {
                ResultManager.Add("El subdistribuidor se ha creado correctamente, sin embargo hubo un problema al enviar la invitación de correo", "Puede reenviar la invitación desde la página de detalle de usuario");
            }
            return ResultManager.IsCorrect;
        }
        public bool UpdateSubdistributor(Subdistributor model)
        {
            ResultManager.Clear();

            //sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.111 No se recibió el modelo");
                return false;
            }
            if (model.Id < 1)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.311 El modelo recibido no contiene el id a actualizar");
                return false;
            }
            if (model.BNAddress == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.211 El modelo recibido no contiene el campo BNAddress");
                return false;
            }

            //business validations
            if (string.IsNullOrWhiteSpace(model.SubdistributorEmployees.FirstOrDefault().EMail))
            {
                ResultManager.Add("El correo electrónico del dueño no puede estar vacio", Trace + "UpdateSubdistributor.211 El campo 'SubdistributorEmployees[0].EMail' esta vacio");
                return false;
            }
            if (model.BNAddress.AddressColonyId <= 0)
            {
                ResultManager.Add("Se debe seleccionar la dirección del representante legal", Trace + "UpdateSubdistributor.211 El campo 'BNAddress.AddressColonyId' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.BNAddress.Street))
            {
                ResultManager.Add("Se debe seleccionar la calle, en la dirección del representante legal", Trace + "UpdateSubdistributor.211 El campo 'BNAddress.Street' esta vacio");
                return false;
            }
            if (model.RTV_BayerEmployeeId <= 0)
            {
                ResultManager.Add("Se debe tener asignado un RTV", Trace + "UpdateSubdistributor.211 El campo 'RTV_BayerEmployeeId' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.Type))
            {
                ResultManager.Add("Se debe seleccionar el tipo Subdistribuidor ó Agricultor", Trace + "UpdateSubdistributor.211 El campo 'Type' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.BusinessName))
            {
                ResultManager.Add("Se debe indicar la razón social", Trace + "UpdateSubdistributor.211 El campo 'BusinessName' esta vacio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.BNLegalRepresentative))
            {
                ResultManager.Add("Se debe indicar el nombre del representante legal", Trace + "UpdateSubdistributor.211 El campo 'BNLegalRepresentative' esta vacio");
                return false;
            }
            if (model.CommercialNames.Count <= 0)
            {
                ResultManager.Add("Se debe indicar al menos un nombre comercial", Trace + "UpdateSubdistributor.211 El campo 'CommercialNames' esta vacio");
                return false;
            }
            string emailToCheck = model.SubdistributorEmployees.Count > 0 ? model.SubdistributorEmployees.FirstOrDefault().EMail : string.Empty;
            int subdistributorPersonId = model.SubdistributorEmployees.Count > 0 ? model.SubdistributorEmployees.FirstOrDefault().Id : 0;
            if (IsMailAddressCurrentlyUsed(emailToCheck, subdistributorPersonId))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + emailToCheck + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            Subdistributor auxSubdistributor = null;
            int auxSubdistributorId = -1;
            bool auxFoundItem = false;

            try
            {
                //retrieve subdistributor from db
                auxSubdistributor = Repository.Subdistributors.Get(model.Id);

                //sys validations (continue)
                //-subdistributor id exists in db
                if(auxSubdistributor == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributor.311 No se encontró un subdistribuidor con id '" + model.Id + "' en la base de datos");
                }
                //business validations
                //TODO: business validations (empty fields, etc.)

                //update subdistributor details
                //auxSubdistributor.IdB = model.IdB;
                auxSubdistributor.Type = model.Type;
                auxSubdistributor.BNLegalRepresentative = model.BNLegalRepresentative;
                auxSubdistributor.RTV_BayerEmployeeId = model.RTV_BayerEmployeeId;
                auxSubdistributor.BusinessName = model.BusinessName;
                auxSubdistributor.WebSite = model.WebSite;

                //update commercial names
                if(auxSubdistributor.CommercialNames.Count == model.CommercialNames.Count)
                {//same number of commercial names
                    //update names
                    List<SubdistributorCommercialName> auxNewCommercialNames = model.CommercialNames.ToList();
                    int counter = 0;
                    foreach(SubdistributorCommercialName item in auxSubdistributor.CommercialNames)
                    {
                        item.Name = auxNewCommercialNames[counter++].Name;
                    }
                }
                else
                {//different number of commercial names
                    //remove old commercial names
                    while (auxSubdistributor.CommercialNames.Count > 0)
                    {
                        Repository.SubdistributorCommercialNames.Remove(auxSubdistributor.CommercialNames.FirstOrDefault());
                    }
                    //assign current list
                    auxSubdistributor.CommercialNames = model.CommercialNames;
                }
               

                //update cropsxmunicipality
                if (model.CropsXMunicipality.Count == 0)
                {
                    auxSubdistributor.CropsXMunicipality.Clear();
                }
                else
                {
                    //add new items
                    foreach (SubdistributorCropsXMunicipality itemNow in model.CropsXMunicipality)
                    {
                        if (itemNow.Id > 0)
                        {//preexistent item, do nothing
                        }
                        else
                        {//new item, add
                            auxSubdistributor.CropsXMunicipality.Add(new SubdistributorCropsXMunicipality()
                            {
                                AddressMunicipalityAddressStateId = itemNow.AddressMunicipalityAddressStateId,
                                AddressMunicipalityId = itemNow.AddressMunicipalityId,
                                Cat_CropId = itemNow.Cat_CropId,
                                SubdistributorId = model.Id
                            });
                        }
                    }

                    //delete removed items
                    List<SubdistributorCropsXMunicipality> auxCropsXMunicipalityToRemove = new List<SubdistributorCropsXMunicipality>();
                    foreach (SubdistributorCropsXMunicipality itemOld in auxSubdistributor.CropsXMunicipality)
                    {
                        foreach (SubdistributorCropsXMunicipality itemNow in model.CropsXMunicipality)
                        {
                            if (itemNow.Id == itemOld.Id)
                            {
                                auxFoundItem = true;
                                break;
                            }
                        }
                        if (!auxFoundItem) auxCropsXMunicipalityToRemove.Add(itemOld);
                    }
                    foreach (SubdistributorCropsXMunicipality item in auxCropsXMunicipalityToRemove)
                    {
                        auxSubdistributor.CropsXMunicipality.Remove(item);
                    }
                }

                //-BNAddress
                AddressColony auxColony = Repository.AddressColonies.Get((int)model.BNAddress.AddressColonyId);
                auxSubdistributor.BNAddress.AddressStateId = auxColony.AddressStateId;
                auxSubdistributor.BNAddress.AddressMunicipalityId = auxColony.AddressMunicipalityId;
                auxSubdistributor.BNAddress.AddressPostalCodeId = auxColony.AddressPostalCodeId;
                auxSubdistributor.BNAddress.AddressColonyId = auxColony.Id;
                auxSubdistributor.BNAddress.Street = model.BNAddress.Street;
                auxSubdistributor.BNAddress.NumberExt = model.BNAddress.NumberExt;
                auxSubdistributor.BNAddress.NumberInt = model.BNAddress.NumberInt;
                
                // Update Addresses related via AddressesXSubdistributor entity
                foreach(AddressesXSubdistributor address in model.Addresses)
                {
                    Address relatedAddress = Repository.Addresses.Get(address.Address.Id);
                    relatedAddress.AddressColonyId = address.Address.AddressColonyId;
                    relatedAddress.AddressMunicipalityId = address.Address.AddressMunicipalityId;
                    relatedAddress.AddressPostalCodeId = address.Address.AddressPostalCodeId;
                    relatedAddress.AddressStateId = address.Address.AddressStateId;
                    relatedAddress.NumberExt = address.Address.NumberExt ?? string.Empty;
                    relatedAddress.NumberInt = address.Address.NumberInt ?? string.Empty;
                    relatedAddress.Street = address.Address.Street ?? string.Empty;
                }

                //TODO: update employees
                //Model.SubdistributorEmployee subdistributorEmployee = null;
            

                //update subdistributor users info
                SubdistributorEmployee auxSubdistributorEmployeeOwner = auxSubdistributor.SubdistributorEmployees.FirstOrDefault();
                if (auxSubdistributorEmployeeOwner.EMail != model.SubdistributorEmployees.FirstOrDefault().EMail)
                {
                    auxSubdistributorEmployeeOwner.EMail = model.SubdistributorEmployees.FirstOrDefault().EMail;
                    auxSubdistributorEmployeeOwner.User.NickName = auxSubdistributorEmployeeOwner.EMail;
                    auxSubdistributorId = auxSubdistributorEmployeeOwner.Id;
                }
                model = null;
                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, "exception while creating subddistributor", ex);
                return false;
            }

            if (true)//TODO:send email to subdistributor about update (it could be the user email changed, what to do then, send another token?) SendNotificationUpdatedEmail(auxSubdistributorId))
            {
                ResultManager.Add("El subdistribuidor se ha actualizado correctamente", "");
            }
            else
            {
                ResultManager.Add("El subdistribuidor se ha actualizado correctamente, sin embargo hubo un problema al enviar la notificación de correo");
            }
            return ResultManager.IsCorrect;
        }

        public bool DeleteSubdistributor(int subdistributorId)
        {
            ResultManager.IsCorrect = false;

            // validate bayer contract associated
            bool hasBayerContract = Repository.ContractsSubdistributor.GetAll().Any(x => x.SubdistributorId == subdistributorId);
            if (hasBayerContract)
            {
                ResultManager.Add(ErrorDefault, Trace + "DeleteSubdistributor.### El Subdistribuidor seleccionado no se puede eliminar ya que existe un contracto con Bayer existente.");
                return false;
            }

            // now proceed with the delete operation
            try
            {
                #region Subdistributor Contact Delete

                IEnumerable<int> subdistributorContactIds = Repository.SubdistributorContacts.GetAll().Where(x => x.SubdistributorId == subdistributorId).Select(y => y.Id);
                foreach (int contactId in subdistributorContactIds)
                {
                    Repository.SubdistributorContacts.Remove(contactId);
                }

                #endregion

                #region Subdistributor Crops x Municipality Delete

                IEnumerable<int> subdistributorCropsxMunicipalityIds = Repository.SubdistributorCropsXMunicipality.GetAll().Where(x => x.SubdistributorId == subdistributorId).Select(y => y.Id);
                foreach (int cropxMunicipalityId in subdistributorCropsxMunicipalityIds)
                {
                    Repository.SubdistributorCropsXMunicipality.Remove(cropxMunicipalityId);
                }

                #endregion

                #region Subdistributor Comercial Names Delete

                IEnumerable<int> subdistributorComercialNameIds = Repository.SubdistributorCommercialNames.GetAll().Where(x => x.SubdistributorId == subdistributorId).Select(y => y.Id);
                foreach (int comercialNameId in subdistributorComercialNameIds)
                {
                    Repository.SubdistributorCommercialNames.Remove(comercialNameId);
                }

                #endregion

                #region Distributor entity delete

                // rmeove address directly associated to distributor
                IEnumerable<AddressesXSubdistributor> adressesXSubdistributorList = Repository.AddressesXSubdistributor.GetAll().Where(x => x.SubdistributorId == subdistributorId);

                // address associated via AddressXSubdistributor entity delete
                IEnumerable<int> addressIds = adressesXSubdistributorList.Select(x => x.AddressId);
                foreach (int addressId in addressIds)
                {
                    Repository.Addresses.Remove(addressId);
                }

                IEnumerable<int> addressSubdistibutorIds = adressesXSubdistributorList.Select(x => x.Id);
                foreach (int addressSubdistributorId in addressSubdistibutorIds)
                {
                    Repository.AddressesXSubdistributor.Remove(addressSubdistributorId);
                }
                // address BN associated detete
                Subdistributor item = Repository.Subdistributors.Get(subdistributorId);
                Repository.Addresses.Remove(item.BNAddressId);

                // remove distributor entity itself
                Repository.Subdistributors.Remove(subdistributorId);

                // remove data from Person_DistributorEmployee entity
                IEnumerable<int> personIds = Repository.SubdistributorEmployees.GetAll().Where(x => x.SubdistributorId == subdistributorId).Select(y => y.Id);
                foreach (int personId in personIds)
                {
                    // delete from DistributorEmployees
                    Repository.SubdistributorEmployees.Remove(personId);
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
                ErrorManager.Add(Trace + "", ErrorDefault, "Exception while deleting subdistributor", ex);
                ResultManager.IsCorrect = false;
            }

            return ResultManager.IsCorrect;
        }

        public bool SendNotificationUpdatedEmail(int userId)
        {
            User user = null;
            string token = PSD.Common.Random.Token(20);
            user = Repository.Users.Get(userId);
            if (user == null)
            {
                ErrorManager.Add(Trace + "SendNotificationUpdatedEmail.211", "Error while retrieving user from database", "No user was found with Id '" + userId + "'");
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
                ErrorManager.Add(Trace + "SendNotificationUpdatedEmail.311", "Exception was thrown while retrieving user's token from database", "User Id '" + user.Id + "'", ex);
            }

            return SendNotificationUpdatedEmail(user);
        }
        private bool SendNotificationUpdatedEmail(User user)
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
                ErrorManager.Add(Trace + "SendNotificationUpdatedEmail.911", "Error while sending email to employee '" + user.NickName + "'", mailer.ResultDetails[0]);
                return false;
            }
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

        #region Contacts

        /// <summary>
        /// Retrieve all the contacts assoicated to one subdistributor id
        /// </summary>
        /// <param name="subdistributorId"></param>
        /// <returns></returns>
        public List<SubdistributorContact> ContactsRetrieveAll(int subdistributorId = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (subdistributorId == -1 || subdistributorId == 0)
            {//no subdistributor id was received
                ResultManager.Add(ErrorDefault, Trace + "ContactsRetrieveAll.131 No se recibio el id del subdistribuidor para recuperar contactos");
                return null;
            }

            List<SubdistributorContact> subDistributorContacts = new List<SubdistributorContact>();
            try
            {
                subDistributorContacts = Repository.SubdistributorContacts
                                                   .Find(x => x.SubdistributorId == subdistributorId)
                                                   .ToList();
                if (subDistributorContacts == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ContactsRetrieveAll.511 No se encontró un subdistribuidor con id '" + subdistributorId + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactsRetrieveAll.511 Excepción al obtener el listado de contactos de subdistribuidor a editar: " + ex.Message);
            }

            return subDistributorContacts;
        }

        /// <summary>
        /// Retrieve one contact based on its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubdistributorContact ContactRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no conatact id was received
                ResultManager.Add(ErrorDefault, Trace + "ContactRetrieve.131 No se recibio el id del contacto");
                return null;
            }

            SubdistributorContact auxContact = null;
            try
            {
                auxContact = Repository.SubdistributorContacts.Get(id);
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

        public bool ContactAdd(SubdistributorContact item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactAdd.111 No se recibio el objeto contacto del subdistribuidor a crear");
                return false;
            }
            if (item.SubdistributorId == -1 || item.SubdistributorId == 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactAdd.111 No se recibio el id del subdistribuidor");
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
                SubdistributorContact auxContact = new SubdistributorContact();
                auxContact.SubdistributorId = item.SubdistributorId;
                auxContact.Subdistributor = Repository.Subdistributors.Get(item.SubdistributorId);
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
                auxContact.Cat_SubdistributorContactAreaId = item.Cat_SubdistributorContactAreaId;
                auxContact.SubdistributorContactArea = Repository.SubdistributorContactAreas.Get(item.Cat_SubdistributorContactAreaId.HasValue 
                                                                                                 ? item.Cat_SubdistributorContactAreaId.Value 
                                                                                                 : 0);
                Repository.SubdistributorContacts.Add(auxContact);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContactAdd.511 Excepción al agregar contacto al subdistribuidor con id '" + item.SubdistributorId + "': " + ex.Message);
            }
            return false;
        }

        public bool ContactUpdate(SubdistributorContact item)
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
                SubdistributorContact auxContact = Repository.SubdistributorContacts.Get(item.Id);
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
                auxContact.Cat_SubdistributorContactAreaId = item.Cat_SubdistributorContactAreaId;
                auxContact.SubdistributorContactArea = Repository.SubdistributorContactAreas.Get(item.Cat_SubdistributorContactAreaId.HasValue
                                                                                                 ? item.Cat_SubdistributorContactAreaId.Value
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
                Repository.SubdistributorContacts.Remove(id);
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

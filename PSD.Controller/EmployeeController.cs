using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Security;

namespace PSD.Controller
{
    public class EmployeeController : _BaseController
    {
        public EmployeeController(IAppConfiguration configurations)
            : base("EmployeeController.", configurations)
        {

        }

        public IEnumerable<BayerEmployee> RetrieveEmployeeList()
        {
            return Repository.BayerEmployees.GetAll();
        }
        public BayerEmployee RetrieveEmployee(int id)
        {
            ResultManager.Clear();
            BayerEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.BayerEmployees.Get(id);
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveEmployee.111: Error al obtener item con id  '" + id + "'. Exception:", ex);
            }
            return auxEmployee;
        }
        public IEnumerable<Cat_Zone> RetrieveEmployeeZones(List<AddressMunicipality> municipalities)
        {
            return Repository.Zones.GetByMunicipalities(municipalities);
        }
        public IEnumerable<Cat_Zone> RetrieveEmployeeZones(ICollection<MunicipalitiesXEmployee> municipalities)
        {
            return Repository.Zones.GetByMunicipalities(municipalities);
        }
        public IEnumerable<Cat_Zone> RetrieveEmployeeZones(int bayerEmployeeId)
        {
            return Repository.BayerEmployees.GetZones(bayerEmployeeId);
        }
        public IEnumerable<int?> RetrieveEmployeeZoneIds(int bayerEmployeeId)
        {
            return Repository.BayerEmployees.GetZoneIds(bayerEmployeeId);
        }
        public bool CreateEmployee(Model.BayerEmployee model/*, string selectedRoleIdB*/)//Model.Employee employee)
        {
            ResultManager.Clear();
            string token = PSD.Common.Random.Token(20);
            Model.User user = null;

            try
            {
                user = new User();
                user.Cat_UserStatusId = 4;
                user.Cat_UserStatus = Repository.UserStatuses.Get(4);///TODO: why do I need this if i already set statusId (doesn't update automatically unless i restart app)
                user.NickName = model.EMail;
                user.FailedLoginAttempts = 0;
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = PSD.Common.Dates.Today;

                Model.RolesXUser rolesXUser = new RolesXUser();
                rolesXUser.UserId = user.Id;
                rolesXUser.Cat_UserRoleId = model.User.RolesXUser.First().Cat_UserRoleId;//Repository.UserRoles.GetByRoleIdB(selectedRoleIdB).Id;
                //Repository.RolesXUser.Add(rolesXUser);
                user.RolesXUser = new List<RolesXUser>() { rolesXUser };

                Model.BayerEmployee employee = new BayerEmployee();
                employee.IdB = model.IdB;
                employee.Name = string.IsNullOrWhiteSpace(model.Name) ? "" : model.Name;
                employee.LastNameF = string.IsNullOrWhiteSpace(model.LastNameF) ? "" : model.LastNameF;
                employee.LastNameM = string.IsNullOrWhiteSpace(model.LastNameM) ? "" : model.LastNameM;
                employee.EMail = model.EMail;
                employee.User = user;

                Model.MunicipalitiesXEmployee municipalityXEmployee;
                List<Model.MunicipalitiesXEmployee> municipalitiesXEmployee = new List<MunicipalitiesXEmployee>();
                foreach (MunicipalitiesXEmployee item in model.MunicipalitiesXEmployee)
                {
                    municipalityXEmployee = new MunicipalitiesXEmployee();
                    municipalityXEmployee.AddressMunicipalityAddressStateId = item.AddressMunicipalityAddressStateId;
                    municipalityXEmployee.AddressMunicipalityId = item.AddressMunicipalityId;
                    municipalityXEmployee.BayerEmployeeId = employee.Id;
                    municipalitiesXEmployee.Add(municipalityXEmployee);
                }
                employee.MunicipalitiesXEmployee = municipalitiesXEmployee;

                Repository.BayerEmployees.Add(employee);

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "exception while creating employee", ex);
                ResultManager.Add(ErrorDefault, "No se ha creado el nuevo usuario");
                return false;
            }

            if (SendUserInvitationEmail(user))
            {
                ResultManager.Add("El usuario se ha creado correctamente", "");
            }
            else
            {
                //ResultManager.IsCorrect = false;
                ResultManager.Add("El usuario se ha creado correctamente, sin embargo hubo un problema al enviar la invitación de correo", "Puede reenviar la invitación desde la página de detalle de usuario");
            }
            return ResultManager.IsCorrect;
        }

        public bool CreateBayerEmployee(Model.BayerEmployee model, List<int?> zones = null)//Model.Employee employee)
        {
            if (zones == null) zones = new List<int?>();

            string token = PSD.Common.Random.Token(20);
            Model.User user = null;

            //business validations
            //*is email already used
            if (IsMailAddressCurrentlyUsed(model.EMail))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }
            //*is zone(s) already assigned
            foreach (int item in zones)
            {
                MunicipalitiesXEmployee auxMunicipality = null;
                switch (model.User.RolesXUser.First().Cat_UserRoleId)
                {
                    case 3:
                        auxMunicipality = Repository.BayerEmployees.GRVAssignedToZone(item);
                        if (auxMunicipality != null)
                        {
                            ResultManager.Add("La zona '" + auxMunicipality.Municipality.Zone.Name + "' ya esta asignada al GRV '" + auxMunicipality.BayerEmployee.NameDisplay + "'", Trace + "CreateBayerEmployee.311 Zona en conflicto 'id:" + item + "'");
                            return false;
                        }
                        break;
                    case 5:
                        auxMunicipality = Repository.BayerEmployees.GRVAssignedToZone(item);
                        if (auxMunicipality != null)
                        {
                            ResultManager.Add("La zona '" + auxMunicipality.Municipality.Zone.Name + "' ya esta asignada al RTV '" + auxMunicipality.BayerEmployee.NameDisplay + "'", Trace + "CreateBayerEmployee.321 Zona en conflicto 'id:" + item + "'");
                            return false;
                        }
                        break;
                    default:
                        ResultManager.Add(ErrorDefault, Trace + "CreateBayerEmployee.391 rol de usuario no esperado'" + model.User.RolesXUser.First().Cat_UserRoleId + "'");
                        return false;
                }
            }

            try
            {
                user = new User();
                user.Cat_UserStatusId = 4;
                user.Cat_UserStatus = Repository.UserStatuses.Get(4);///TODO: why do I need this if i already set statusId (doesn't update automatically unless i restart app)
                user.NickName = model.EMail;
                user.FailedLoginAttempts = 0;
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = PSD.Common.Dates.Today;

                Model.RolesXUser rolesXUser = new RolesXUser();
                rolesXUser.UserId = user.Id;
                rolesXUser.Cat_UserRoleId = model.User.RolesXUser.First().Cat_UserRoleId;//Repository.UserRoles.GetByRoleIdB(selectedRoleIdB).Id;
                //Repository.RolesXUser.Add(rolesXUser);
                user.RolesXUser = new List<RolesXUser>() { rolesXUser };

                Model.BayerEmployee employee = new BayerEmployee();
                employee.IdB = model.IdB;
                employee.Name = string.IsNullOrWhiteSpace(model.Name) ? "" : model.Name;
                employee.LastNameF = string.IsNullOrWhiteSpace(model.LastNameF) ? "" : model.LastNameF;
                employee.LastNameM = string.IsNullOrWhiteSpace(model.LastNameM) ? "" : model.LastNameM;
                employee.EMail = model.EMail;
                employee.User = user;

                //zones
                IEnumerable<AddressMunicipality> municipalitiesXZone = Repository.AddressMunicipalities.GetByZoneIds(zones);
                MunicipalitiesXEmployee municipalityXEmployee;
                List<MunicipalitiesXEmployee> municipalitiesXEmployee = new List<MunicipalitiesXEmployee>();
                foreach (AddressMunicipality item in municipalitiesXZone)
                {
                    municipalityXEmployee = new MunicipalitiesXEmployee();
                    municipalityXEmployee.AddressMunicipalityAddressStateId = item.AddressStateId;
                    municipalityXEmployee.AddressMunicipalityId = item.Id;
                    municipalityXEmployee.BayerEmployeeId = employee.Id;
                    municipalitiesXEmployee.Add(municipalityXEmployee);
                }
                employee.MunicipalitiesXEmployee = municipalitiesXEmployee;

                Repository.BayerEmployees.Add(employee);

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "exception while creating employee", ex);
                ResultManager.IsCorrect = false;
                ResultManager.Add(ErrorDefault, "No se ha creado el nuevo usuario");
                return false;
            }

            if (SendUserInvitationEmail(user))
            {
                ResultManager.Add("El usuario se ha creado correctamente, se le ha enviado un correo con la invitación a activar su cuenta", "");
            }
            else
            {
                ResultManager.Add("El usuario se ha creado correctamente, sin embargo hubo un problema al enviar la invitación de correo");
            }
            return ResultManager.IsCorrect;
        }

        public bool UpdateBayerEmployee(BayerEmployee model, List<int?> selectedZones)
        {
            if (model == null) return false;

            //business validations
            //*is email already used
            if (IsMailAddressCurrentlyUsed(model.EMail, model.Id))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }
            //*is zone(s) already assigned
            foreach (int item in selectedZones)
            {
                MunicipalitiesXEmployee auxMunicipality = null;
                switch (model.User.RolesXUser.First().Cat_UserRoleId)
                {
                    case 3:
                        auxMunicipality = Repository.BayerEmployees.GRVAssignedToZone(item);
                        if (auxMunicipality != null && auxMunicipality.BayerEmployeeId != model.Id)
                        {
                            ResultManager.Add("La zona '" + auxMunicipality.Municipality.Zone.Name + "' ya esta asignada al GRV '" + auxMunicipality.BayerEmployee.NameDisplay + "'", Trace + "CreateBayerEmployee.311 Zona en conflicto 'id:" + item + "'");
                            return false;
                        }
                        break;
                    case 5:
                        auxMunicipality = Repository.BayerEmployees.RTVAssignedToZone(item);
                        if (auxMunicipality != null && auxMunicipality.BayerEmployeeId != model.Id)
                        {
                            ResultManager.Add("La zona '" + auxMunicipality.Municipality.Zone.Name + "' ya esta asignada al RTV '" + auxMunicipality.BayerEmployee.NameDisplay + "'", Trace + "CreateBayerEmployee.321 Zona en conflicto 'id:" + item + "'");
                            return false;
                        }
                        break;
                    default:
                        ResultManager.Add(ErrorDefault, Trace + "CreateBayerEmployee.391 rol de usuario no esperado'" + model.User.RolesXUser.First().Cat_UserRoleId + "'");
                        return false;
                }
            }

            BayerEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.BayerEmployees.Get(model.Id);
                //User.Cat_UserStatusId = model.Cat_UserStatusId;
                auxEmployee.IdB = model.IdB;
                auxEmployee.User.NickName = model.EMail;
                auxEmployee.EMail = model.EMail;
                auxEmployee.Name = model.Name;
                auxEmployee.LastNameF = model.LastNameF;
                auxEmployee.LastNameM = model.LastNameM;
                auxEmployee.PhoneNumber = model.PhoneNumber;

                List<MunicipalitiesXEmployee> auxMXE = new List<MunicipalitiesXEmployee>();
                bool auxExist;

                //update selected zones
                if (selectedZones == null)//is there a zone selected on this last change?
                {//none selected, remove all
                    foreach (MunicipalitiesXEmployee item in auxEmployee.MunicipalitiesXEmployee)
                    {
                        //auxEmployee.MunicipalitiesXEmployee.Remove(item);
                        Repository.MunicipalitiesXEmployee.Remove(item.Id);
                    }
                }
                else
                {//some selected, update
                    //identify records to delete (not selected anymore)
                    foreach (MunicipalitiesXEmployee item in auxEmployee.MunicipalitiesXEmployee)
                    {
                        auxExist = false;
                        foreach (int itemSelectedZoneId in selectedZones)
                        {
                            if (item.Municipality.Cat_ZoneId == itemSelectedZoneId)
                            {
                                auxExist = true;
                                break;
                            }
                        }
                        if (!auxExist)
                        {
                            auxMXE.Add(item);
                        }
                    }
                    //delete records (not selected anymore)
                    foreach (MunicipalitiesXEmployee item in auxMXE)
                    {
                        Repository.MunicipalitiesXEmployee.Remove(item.Id);
                    }
                    auxMXE.Clear();

                    //identify records to add (not yet in db)
                    List<AddressMunicipality> municipalitiesXZone = (List<AddressMunicipality>)Repository.AddressMunicipalities.GetByZoneIds(selectedZones);
                    foreach (AddressMunicipality item in municipalitiesXZone)
                    {
                        auxExist = false;
                        foreach (MunicipalitiesXEmployee itemMXE in auxEmployee.MunicipalitiesXEmployee)
                        {
                            if (item.Id == itemMXE.AddressMunicipalityId)
                            {
                                auxExist = true;
                                break;
                            }
                        }
                        if (!auxExist)
                        {
                            auxMXE.Add(new MunicipalitiesXEmployee()
                            {
                                AddressMunicipalityAddressStateId = item.AddressStateId,
                                AddressMunicipalityId = item.Id,
                                BayerEmployeeId = auxEmployee.Id
                            });
                        }
                    }
                    //insert records (not yet in db)
                    foreach (MunicipalitiesXEmployee item in auxMXE)
                    {
                        auxEmployee.MunicipalitiesXEmployee.Add(item);
                    }
                    auxMXE.Clear();
                }

                Repository.Complete();
                ResultManager.Add("Perfil actualizado", "");
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add("", ErrorDefault, "", ex);
                ResultManager.Add(ErrorDefault, "");
            }

            return false;
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
            if (SendEmail(user.NickName, subject, messageBody, emailParams))
            {
                ResultManager.IsCorrect = true;
                return true;
            }
            else
            {
                ResultManager.Add("Error al enviar el correo", Trace + "SendInvitationEmail.911 Error while sending email to employee '" + user.NickName + "'");
                return false;
            }
        }
        public bool Delete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Delete.111 No se recibió el id del usuario Bayer a eliminar");
                return false;
            }

            //mark item as deleted
            try
            {
                BayerEmployee item = Repository.BayerEmployees.Get(id);
                if (item == null)
                {
                    throw new Exception("No se encontró un usuario Bayer con id '" + id + "'");
                }
                item.User.Cat_UserStatusId = 2;//2:deleted
                item.User.Hash = null;
                item.User.Salt = null;
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Delete.511 Excepción al eliminar el usuario Bayer con id '" + id + "'", ex);
            }
            return false;
        }

        /// <summary>
        /// This will switch Cat_UserStatusId beetween 1 (active) and 2 (deactive)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentStatus"></param>
        /// <returns></returns>
        public bool ChangeUserStatus(int userId)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            // no user Id received
            if (userId == -1 || userId == 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "ChangeUserStatus. No se recibio el id del usuario a cambiar estatus");
                return false;
            }

            try
            {
                User auxUser = Repository.Users.Get(userId);
                if (auxUser == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ChangeUserStatus. El usuario con el Id '" + userId.ToString() + "' no existe en la BD");
                    return false;
                }

                if (auxUser.Cat_UserStatusId != 1 && auxUser.Cat_UserStatusId != 3)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ChangeUserStatus. El usuario con el Id '" + userId.ToString() + "' tiene un estatus diferente a (Activo / Deshabilitado), por favor intenta de nuevo.");
                    return false;
                }

                auxUser.Cat_UserStatusId = auxUser.Cat_UserStatusId == 1 ? 3 : 1;
                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ChangeUserStatus. Excepción al cambiar el estatus del usuario con Id '" + userId.ToString() + "': " + ex.Message);
                ResultManager.IsCorrect = false;
            }
            return ResultManager.IsCorrect;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Security;

namespace PSD.Controller
{
    public class AccountController : _BaseController
    {
        #region Attributes
        Model.User user;
        #endregion

        #region Properties
        public Model.User User { get { return user; } set { user = value; } }
        public bool HasPasswordExpired { get; set; }
        #endregion

        #region Constructors
        public AccountController(IAppConfiguration configurations)
            : base("AccountController.", configurations)
        {

        }
        
        #endregion

        #region Methods
        public User GetUser(int id)
        {
            return Repository.Users.Get(id);
        }
        public User GetUserByToken(string token)
        {
            return Repository.Users.GetByToken(token);
        }
        
        public BayerEmployee GetBayerEmployee(int id)
        {
            return Repository.BayerEmployees.Get(id);
        }
        public DistributorEmployee GetDistributorEmployee(int id)
        {
            return Repository.DistributorEmployees.Get(id);
        }
        public SubdistributorEmployee GetSubdistributorEmployee(int id)
        {
            return Repository.SubdistributorEmployees.Get(id);
        }
        public bool Login(string nickName, string password)
        {
            //business validations
            //-has nickname
            if (string.IsNullOrWhiteSpace(nickName))
            {
                ResultManager.Add("Credenciales inválidas, no se indicó el correo del usuario", Trace + "Login.111 No nickname was provided. ");
                return false;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                ResultManager.Add("Credenciales inválidas, no se indicó la contraseña", Trace + "Login.121 No password was provided (empty passwords are not allowed on any case)");
                return false;
            }

            nickName = nickName.Trim();
            password = password.Trim();


            PSD.Model.User userLogin = null;
            IEnumerable<PSD.Model.RolesXUser> userRoles;
            try
            {
                userLogin = Repository.Users.GetByNickName(nickName);
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Login.311 Error while trying to retrieve user '" + nickName + "' from database. ", ex);
                return false;
            }

            if (userLogin == null)
            {
                ResultManager.Add("Credenciales inválidas", Trace + "Login.411 User with nickname '" + nickName + "'not found in DB");
                return false;
            }

            //validate user able to perform login
            switch(userLogin.Cat_UserStatus.IdB)
            {
                case "active":
                case "toconfirm":
                case "tocomplete":
                    break;
                case "disabled":
                    ResultManager.Add("Credenciales inválidas, el usuario '" + nickName + "' esta deshabilitado. Contacte a su administrador para rehabilitarlo", Trace + "Login.511 user '" + nickName + "' was disabled");
                    return false;
                case "deleted":
                    ResultManager.Add("Credenciales inválidas, usuario no encontrado.", Trace + "Login.511 user '" + nickName + "' was deleted");
                    return false;
                default: //unknown status
                    ResultManager.Add(ErrorDefault, Trace + "Login.531 User nickname '" + nickName + "' is on an unknown status '" + userLogin.Cat_UserStatus.IdB + "'");
                    return false;
            }
            if (userLogin.FailedLoginAttempts >= 5)
            {
                try
                {
                    userLogin.Cat_UserStatusId = Repository.UserStatuses.Get(3).Id;//disabled ///TODO: change to dynamic status id set by IdB
                }
                catch(Exception ex)
                {
                    ResultManager.Add("El usuario esta inhabilitado", Trace + "Login.551 Exception while trying to inactivate user (due max failed login attempts reached)", ex);
                    return false;
                }
                ResultManager.Add("Credenciales inválidas. Por seguridad, el usuario ha sido deshabilitado. Contacte a su administrador para rehabilitarlo", Trace + "Login.631 User has been disabled");
                return false;
            }

            //validate user password
            bool resultPasswordValid = false;
            if (string.IsNullOrWhiteSpace(userLogin.Salt) || string.IsNullOrWhiteSpace(userLogin.Hash))
            {
                ResultManager.Add("El usuario no tiene un login asociado", Trace + "Login.561 User nickname '" + nickName + "' has not yet defined a login password (hash/salt)");
                return false;
            }
            else
            {
                try
                {
                    if (Identity.ValidatePassword(password, userLogin.Salt, userLogin.Hash))
                    {
                        resultPasswordValid = true;
                    }
                }
                catch (Exception ex)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Login.571 Exception while trying to validate password for user nickname '" + nickName + "'", ex);
                    return false;
                }
            }
            if (!resultPasswordValid)
            {
                ResultManager.Add("Credenciales inválidas", Trace + "581. Invalid password for user with nickname '" + nickName + "'");

                //increase fail login attempts counter
                try
                {
                    userLogin.FailedLoginAttempts++;
                }
                catch (Exception ex)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Login.611 Exception while trying to inactivate user due max failed login attempts reached", ex);
                    return false;
                }

                //verify if max login attempts reached so it needs to be disabled
                if (userLogin.FailedLoginAttempts >= 5)
                {
                    try
                    {
                        userLogin.Cat_UserStatusId = Repository.UserStatuses.Get(3).Id;//disabled ///TODO: change to dynamic status id set by IdB
                        ResultManager.Add("El usuario ha sido deshabilitado debido a que ha alcanzado el número máximo de intentos de logueo fallidos permitido, contacte a su administrador para reactivar su usuario", Trace + "Login.631 User is disabled so can't login");
                    }
                    catch (Exception ex)
                    {
                        ResultManager.Add(ErrorDefault, Trace + "Login.651 Exception while trying to inactivate user due max failed login attempts reached", ex);
                        return false;
                    }
                }
                Repository.Complete();
                return false;
            }

            //at this point credentials are valid, perform login

            // validate password expiracy, false or true either way we need to login
            HasPasswordExpired = userLogin.LastPasswordChangeDate.HasValue
                                ? (DateTime.Today - userLogin.LastPasswordChangeDate.Value).Days > Configurations.PasswordExpireDays
                                : true;

            //-update db info about login
            try
            {
                userLogin.FailedLoginAttempts = 0;
                userLogin.LastLoginDate = Common.Dates.Today;
                //Repository.Complete();
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Login.711 Error while trying to update user info about login on DB", ex);
                return false;
            }

            //-create user session object (recover db user details)
            try
            {
                userRoles = userLogin.RolesXUser;//repository.RolesXUser.GetUserRoles(userLogin.Id);
                if (userRoles == null)
                { throw new Exception("Error while trying to get user roles, userId '" + userLogin.Id + "'"); }

                string[] userRolesArray = new string[userRoles.Count()];
                string[] userRoleIdBsArray = new string[userRoles.Count()];
                int i = 0;
                foreach (PSD.Model.RolesXUser item in userRoles)
                { 
                    userRolesArray[i] = item.Cat_UserRole.IdB;
                    userRoleIdBsArray[i++] = item.Cat_UserRole.Name;
                }

                string employeeId = "";
                string userType = "";
                string parentId = "";
                switch(userRolesArray[0])
                {
                    case "sysadmin": 
                    case "appadmin": 
                    case "employee-manager_operation":
                    case "employee-manager_view":
                    case "employee-rtv_operation":
                    case "employee-rtv_view":
                        employeeId = Repository.BayerEmployees.Get(userLogin.Person.Id).IdB; break;
                    case "customer-distributor_operation":
                    case "customer-distributor_view":
                        Distributor auxDistributor = Repository.DistributorEmployees.Get(userLogin.Person.Id).Distributor;
                        employeeId = auxDistributor.IdB;
                        parentId = auxDistributor.Id.ToString();
                        break;
                    case "customer-subdistributor_operation":
                    case "customer-subdistributor_view":
                        Subdistributor auxSubdistributor = Repository.SubdistributorEmployees.Get(userLogin.Person.Id).Subdistributor;
                        employeeId = auxSubdistributor.IdB;
                        userType = auxSubdistributor.Type;
                        parentId = auxSubdistributor.Id.ToString();
                        break;
                    default: employeeId = ""; break;

                }

                Security.Entity.User auxUser = new Security.Entity.User(
                    userLogin.Id.ToString()
                    , employeeId
                    , userLogin.NickName
                    , userLogin.Person.Name + " " + userLogin.Person.LastNameF ///TODO:implement displayname on partial model class
                    , userLogin.Person.EMail
                    , userRolesArray
                    , userRoleIdBsArray
                    , userLogin.Cat_UserStatus.IdB
                    , userLogin.Cat_UserStatus.Name
                    , userType
                    , parentId
                );

                if (!Identity.InitSession(auxUser))
                {
                    throw new Exception("Error while performing 'InitSession' for user.");
                }

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Login.811 Error while creating user session object", ex);
                return false;
            }

            if (!ResultManager.IsCorrect)
            {
                return false;
            }

            return ResultManager.IsCorrect;
        }
        public bool SetLoginToken(string nickName)
        {
            ResultManager.Clear();
            
            Model.User user;
            try
            {
                user = Repository.Users.GetByNickName(nickName);
            }
            catch(Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "SetLoginToken.311 Exception when trying to retrieve user by nickname", ex);
                return false;
            }

            if(user == null)
            {
                ResultManager.Add(Trace + "SetLoginToken.331", "Correo no encontrado");
                return false;
            }

            //validate if user state is good to set token
            switch (user.Cat_UserStatus.IdB)
            {
                case "active":
                case "toconfirm":
                case "tocomplete":
                    break;
                case "disabled":
                    ResultManager.Add("El usuario se encuentra deshabilitado, contacte a su administrador para rehabilitar el usuario antes de intentar reiniciar su contraseña.", Trace + "SetLoginToken.511 user nickname '" + nickName + "' is 'disabled'");
                    return false;
                case "deleted":
                    ResultManager.Add("Usuario no encontrado", Trace + "SetLoginToken.521 user nickname '" + nickName + "' is 'deleted'");
                    return false;
                default: //unknown status
                    ResultManager.Add(ErrorDefault, Trace + "SetLoginToken.531 user nickname '" + nickName + "' is on an unknown status '" + user.Cat_UserStatus.IdB + "'");
                    return false;
            }

            string token = PSD.Common.Random.Digits(5).ToString() + PSD.Common.Random.Digits(5).ToString() + PSD.Common.Random.Digits(5).ToString() + PSD.Common.Random.Digits(5).ToString();
            string tokenUrl = Configurations.HostUrl + "Account/LoginByToken?token=" + token;
            
            string auxUserFullName = string.Empty;
            string auxUserEmail = string.Empty;

            //set token
            try
            {
                //clear password details
                user.Hash = null;
                user.Salt = null;
                user.FailedLoginAttempts = 0;
                user.LastPasswordChangeDate = Common.Dates.Today;

                //set token details
                user.LoginToken = token;
                user.LoginTokenGeneratedDate = Common.Dates.Today;

                //save aux user details to send email (user.Person won't be available after .Complete method is called)
                auxUserFullName = user.Person.NameDisplay;
                auxUserEmail = user.Person.EMail;

                Repository.Complete();
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "SetLoginToken.611 Exception when trying to clear password details and set token", ex);
                return false;
            }

            
            //Send email with token url
            if (SendUserTokenEmail(auxUserFullName, auxUserEmail, tokenUrl))
            {
                //ResultManager.Add("Se ha enviado un correo a '" + auxUserEmail + "' con instrucciones para reestablecer su contraseña");
                ResultManager.IsCorrect = true;
            }
            else
            {
                //if email was not send, method needs to return false since there's no way the customer can get access to the login token
                
                //deprecated: error to user in ResultManager is added by the email sender method, so this other message detail is not necesary
                ResultManager.Add(ErrorDefault);
            }
            return ResultManager.IsCorrect;
        }
        public bool ValidateLoginToken(string token)
        {
            if(string.IsNullOrWhiteSpace(token))
            {
                ErrorManager.Add(Trace + "LoginByToken.111", "No se recibió ningún token", "Token was null or empty");
                return false;
            }

            token = token.Trim();
            try
            {
                user = Repository.Users.GetByToken(token);
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "LoginByToken.311", "Error al validar token", "Error while trying to retrieve user for token '" + token + "' from database.", ex);
                return false;
            }

            if(user == null)
            {
                ErrorManager.Add(Trace + "LoginByToken.351", "Token inválido", "No user found for token '" + token + "' in database.");
                return false;
            }
            
            //validations
            if (user.LoginTokenGeneratedDate == null)
            {
                ErrorManager.Add(Trace + "LoginByToken.511", "Token inválido", "Token '" + token + "' is incorrectly setup in database, it does not have a value set for 'LoginTokenGeneratedDate'.");
                return false;
            }
            if(user.LoginTokenGeneratedDate.Value.AddDays(15) <= PSD.Common.Dates.Today)
            {
                ErrorManager.Add(Trace + "LoginByToken.531", "Token inválido", "Token '" + token + "' it's 'LoginTokenGeneratedDate' was '" + Common.Dates.ToString(user.LoginTokenGeneratedDate.Value, Common.Enums.DateFormat.IngDate) + "'.");
                return false;
            }

            return true;
        }

        
        public bool ConfirmUserAccount(User model, string userNewPassword, string userNewPasswordConfirm)
        {
            ResultManager.Clear();

            //system validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdatePassword.111 No se recibio el modelo");
                return false;
            }
            if (model.Id < 1)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdatePassword.111 El modelo no contiene un valor Id");
                return false;
            }
            //business validations
            if (string.IsNullOrWhiteSpace(userNewPassword))
            {
                ResultManager.Add("Se debe indicar una contraseña", Trace + "UpdatePassword.311");
                return false;
            }
            if (!Identity.IsValidPassWordFormat(Configurations.PasswordValidFormatPattern, userNewPassword))
            {
                ResultManager.Add(Configurations.PasswordInvalidFormatMessage);
                return false;
            }
            if (!string.Equals(userNewPassword, userNewPasswordConfirm))
            {
                ResultManager.Add("La contraseña y la confirmación de contraseña no coinciden", Trace + "UpdatePassword.321");
                return false;
            }

            Model.User newUser;            
            try
            {
                //retrieve user from DB
                newUser = Repository.Users.Get(model.Id);

                if(newUser.Cat_UserStatus.IdB != "toconfirm")
                {
                    ResultManager.Add(ErrorDefault, Trace + "ConfirmUserAccount.311 The user '" + newUser.NickName + "' was attempted to be confirmed while being in an unexpected status. Status expected 'created', but current user status in DB is '" + newUser.Cat_UserStatus.IdB + "'");
                    ResultManager.IsCorrect = false;
                    return false;
                }

                //update password info
                string auxSalt;
                newUser.Hash = PSD.Security.Identity.HashPassword(userNewPassword, out auxSalt);
                newUser.Salt = auxSalt;
                newUser.FailedLoginAttempts = 0;
                newUser.LastLoginDate = PSD.Common.Dates.Today;
                newUser.LastPasswordChangeDate = PSD.Common.Dates.Today;
                newUser.LoginToken = null;
                newUser.LoginTokenGeneratedDate = null;
                
                //default info
                
                //updated info
                //newUser.NickName = model.NickName;
                //newUser.Person.EMail = model.NickName;

                //Repository.RolesXUser.IsInRole(newUser.Id, "appadmin"))
                bool userRequiresInitialEdit = true;
                foreach (RolesXUser aUserRole in newUser.RolesXUser)
                {
                    switch(aUserRole.Cat_UserRole.IdB)
                    {
                        case "sysadmin":
                        case "appadmin":
                            userRequiresInitialEdit = false;
                            break;
                        case "employee-manager_view":
                        case "employee-rtv_view":
                        case "employee-manager_operation":
                        case "employee-rtv_operation":
                            break;
                    }
                    
                }
                if (userRequiresInitialEdit)
                {
                    newUser.Cat_UserStatusId = Repository.UserStatuses.Get(5).Id;//user 'confirmed' ///TODO:change to dynamic get by 'IdB'
                }
                else
                {
                    newUser.Cat_UserStatusId = Repository.UserStatuses.Get(1).Id;//user 'active' ///TODO:change to dynamic get by 'IdB'
                }
                
                //update DB                
                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch(Exception ex)
            {
                ErrorManager.Add(Trace + "CreateUser.531", ErrorDefault, "Exception thrown when trying to create user '" + model.NickName + "'", detailException: ex);
                return false;
            }

            User = newUser;
            return true;
        }

        public bool UpdatePassword(int userId, string userNewPassword, string userNewPasswordConfirm)
        {
            ResultManager.Clear();
            
            //system validations
            if (userId < 1)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdatePassword.111 No se recibio el parametro 'userId'");
                return false;
            }
            //business validations
            if (string.IsNullOrWhiteSpace(userNewPassword))
            {
                ResultManager.Add("Se debe indicar una contraseña", Trace + "UpdatePassword.311");
                return false;
            }
            if (!Identity.IsValidPassWordFormat(Configurations.PasswordValidFormatPattern, userNewPassword))
            {
                ResultManager.Add(Configurations.PasswordInvalidFormatMessage);
                return false;
            }
            if (!string.Equals(userNewPassword, userNewPasswordConfirm))
            {
                ResultManager.Add("La contraseña y la confirmación de contraseña no coinciden", Trace + "UpdatePassword.321");
                return false;
            }
                                    
            Model.User auxUser;
            try
            {
                //retrieve user from DB
                auxUser = Repository.Users.Get(userId);

                if (auxUser.Cat_UserStatus.IdB != "created" && auxUser.Cat_UserStatus.IdB != "active" && auxUser.Cat_UserStatus.IdB != "confirmed")
                {
                    ResultManager.Add(ErrorDefault, Trace + "PerformPasswordReset.311 Se intentó realizar un reseteo de contraseña al usuario '" + auxUser.NickName + "' mientras se encontraba en un estatus inesperado. Se estperaba estatus 'created' o 'active', pero el estatus en BD es '" + auxUser.Cat_UserStatus.IdB + "'");
                    return false;
                }
                
                //update password info
                string auxSalt;
                auxUser.Hash = PSD.Security.Identity.HashPassword(userNewPassword, out auxSalt);
                auxUser.Salt = auxSalt;
                auxUser.FailedLoginAttempts = 0;
                auxUser.LastLoginDate = PSD.Common.Dates.Today;
                auxUser.LastPasswordChangeDate = PSD.Common.Dates.Today;
                auxUser.LoginToken = null;
                auxUser.LoginTokenGeneratedDate = null;

                //update DB                
                Repository.Complete();
            
                User = auxUser;
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "PerformPasswordReset.531 Exception thrown when trying to retrieve user with Id '" + userId + "'", ex);
                return false;
            }

            ResultManager.Add("Contraseña actualizada correctamente");
            return ResultManager.IsCorrect;
        }

        /*deprecated: param userId only instead of all user object, so we can reuse method
        public bool PerformPasswordReset(User model, string userPassword)
        {
            Model.User newUser;
            try
            {
                //retrieve user from DB
                newUser = Repository.Users.Get(model.Id);

                if (newUser.Cat_UserStatus.IdB != "created" && newUser.Cat_UserStatus.IdB != "active")
                {
                    ErrorManager.Add(Trace + "CreateUser.311", ErrorDefault, "The user '" + model.NickName + "' was attempted to perform password reset while being in an unexpected status. Status expected 'created' or 'active', but current user status in DB is '" + newUser.Cat_UserStatus.IdB + "'");
                    return false;
                }

                //update password info
                string auxSalt;
                newUser.Hash = PSD.Security.Identity.HashPassword(userPassword, out auxSalt);
                newUser.Salt = auxSalt;
                newUser.FailedLoginAttempts = 0;
                newUser.LastLoginDate = PSD.Common.Dates.Today;
                newUser.LastPasswordChangeDate = PSD.Common.Dates.Today;
                newUser.LoginToken = null;
                newUser.LoginTokenGeneratedDate = null;

                //update DB                
                Repository.Complete();
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "CreateUser.531", ErrorDefault, "Exception thrown when trying to create user '" + model.NickName + "'", detailException: ex);
                return false;
            }

            User = newUser;
            return true;
        }
*/
        public bool UpdateUserDetails(BayerEmployee model, string userRoleIdB)
        {
            if (model == null) return false;

            if (IsMailAddressCurrentlyUsed(model.EMail, CurrentUser.Id))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            BayerEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.BayerEmployees.Get(model.Id);
                //User.Cat_UserStatusId = model.Cat_UserStatusId;
                auxEmployee.User.NickName = model.EMail;
                auxEmployee.EMail = model.EMail;
                auxEmployee.Name = model.Name;
                auxEmployee.LastNameF = model.LastNameF;
                auxEmployee.LastNameM = model.LastNameM;
                auxEmployee.PhoneNumber = model.PhoneNumber;


                Repository.Complete();
                ResultManager.Add("Perfil actualizado", "");
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add("", ErrorDefault, "", ex);
                ResultManager.Add(ErrorDefault, "");
                ResultManager.IsCorrect = false;
            }

            return false;
        }
        public bool UpdateBayerEmployee(BayerEmployee model/*, int selectedZone*/)
        {
            if (model == null) return false;

            if (IsMailAddressCurrentlyUsed(model.EMail, CurrentUser.Id))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            BayerEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.BayerEmployees.Get(model.Id);
                auxEmployee.User.NickName = model.EMail;
                auxEmployee.EMail = model.EMail;
                auxEmployee.Name = model.Name;
                auxEmployee.LastNameF = model.LastNameF;
                auxEmployee.LastNameM = model.LastNameM;
                auxEmployee.PhoneNumber = model.PhoneNumber;

                //if profile was not completed, complete
                if (auxEmployee.User.Cat_UserStatusId == 5) auxEmployee.User.Cat_UserStatusId = 1;

                /*Deprecated: user won't have ability to change zone on it's own
                //update selected zones
                List<MunicipalitiesXEmployee> auxMXE = new List<MunicipalitiesXEmployee>();
                if (selectedZone == null || selectedZone == -1 || selectedZone == 0)//is there a zone selected on this last change?
                {//none selected, remove all
                    foreach (MunicipalitiesXEmployee item in auxEmployee.MunicipalitiesXEmployee)
                    {
                        //auxEmployee.MunicipalitiesXEmployee.Remove(item);
                        Repository.MunicipalitiesXEmployee.Remove(item.Id);
                    }
                }
                else
                {//zone selected, update
                    //identify records to delete (not selected anymore)
                    foreach (MunicipalitiesXEmployee item in auxEmployee.MunicipalitiesXEmployee)
                    {
                        if (item.Municipality.Cat_ZoneId == selectedZone)
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
                    List<AddressMunicipality> municipalitiesXZone = (List<AddressMunicipality>)Repository.AddressMunicipalities.GetByZoneId(selectedZone);
                    bool auxExist;
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
                */

                Repository.Complete();
                ResultManager.Add("Perfil actualizado", "");
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                //Repository.Dispose();
                ErrorManager.Add("", ErrorDefault, "", ex);
                ResultManager.Add(ErrorDefault, "");
                ResultManager.IsCorrect = false;
            }

            return false;
        }
        public bool UpdateDistributorEmployee(DistributorEmployee model)
        {
            if (model == null) return false;

            if (IsMailAddressCurrentlyUsed(model.EMail, model.Id))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            DistributorEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.DistributorEmployees.Get(model.Id);
                auxEmployee.User.NickName = model.EMail;
                auxEmployee.EMail = model.EMail;
                //auxEmployee.Name = model.Name;
                //auxEmployee.LastNameF = model.LastNameF;
                //auxEmployee.LastNameM = model.LastNameM;
                //auxEmployee.PhoneNumber = model.PhoneNumber;

                //auxEmployee.Distributor.IdB = model.Distributor.IdB;
                auxEmployee.Distributor.BusinessName = model.Distributor.BusinessName;
                auxEmployee.Distributor.CommercialName = model.Distributor.CommercialName;
                auxEmployee.Distributor.WebSite = model.Distributor.WebSite;

                //-address
                AddressColony auxColony = Repository.AddressColonies.Get((int)model.Distributor.Address.AddressColonyId);
                auxEmployee.Distributor.Address.AddressStateId = auxColony.AddressStateId;
                auxEmployee.Distributor.Address.AddressMunicipalityId = auxColony.AddressMunicipalityId;
                auxEmployee.Distributor.Address.AddressPostalCodeId = auxColony.AddressPostalCodeId;
                auxEmployee.Distributor.Address.AddressColonyId = auxColony.Id;
                auxEmployee.Distributor.Address.Street = model.Distributor.Address.Street;
                auxEmployee.Distributor.Address.NumberExt = model.Distributor.Address.NumberExt;
                auxEmployee.Distributor.Address.NumberInt = model.Distributor.Address.NumberInt;


                //if profile was not completed, complete
                if (auxEmployee.User.Cat_UserStatusId == 5) auxEmployee.User.Cat_UserStatusId = 1;

                Repository.Complete();
                ResultManager.Add("Perfil actualizado", "");
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                //Repository.Dispose();
                ErrorManager.Add("", ErrorDefault, "", ex);
                ResultManager.Add(ErrorDefault, "");
                ResultManager.IsCorrect = false;
            }

            return false;
        }
        public bool UpdateSubdistributorEmployee(SubdistributorEmployee model)
        {
            ResultManager.Clear();

            //sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributorEmployee.111 No se recibió el modelo");
                return false;
            }
            if (model.Id == 0 || model.Id == -1)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributorEmployee.121 El modelo recibido no contiene el id a actualizar");
                return false;
            }
            if (model.Subdistributor.BNAddress == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateSubdistributorEmployee.211 El modelo recibido no contiene el campo BNAddress");
                return false;
            }
            //business validation
            if (string.IsNullOrWhiteSpace(model.EMail))
            {
                ResultManager.Add("El correo electrónico no puede estar vacio", Trace + "UpdateSubdistributorEmployee.211 El campo EMail esta vacio");
                return false;
            }
            if (model.Subdistributor.BNAddress.AddressColonyId <= 0)
            {
                ResultManager.Add("Se debe seleccionar la dirección del representante legal", Trace + "UpdateSubdistributorEmployee.211 El campo BNAddress esta vacio");
                return false;
            }
            if (IsMailAddressCurrentlyUsed(model.EMail, CurrentUser.Id))
            {
                ResultManager.Add(ErrorDefault, Trace + "La direccion de email '" + model.EMail + "' actualmente esta asignada a otro usuario, por favor ingresa una diferente.");
                return false;
            }

            SubdistributorEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.SubdistributorEmployees.Get(model.Id);
                auxEmployee.User.NickName = model.EMail;
                auxEmployee.EMail = model.EMail;
                //auxEmployee.Name = model.Name;
                //auxEmployee.LastNameF = model.LastNameF;
                //auxEmployee.LastNameM = model.LastNameM;
                //auxEmployee.PhoneNumber = model.PhoneNumber;

                //auxEmployee.Subdistributor.BusinessName = model.Subdistributor.BusinessName;
                //auxEmployee.Subdistributor.CommercialNames = model.Distributor.CommercialName;
                auxEmployee.Subdistributor.WebSite = model.Subdistributor.WebSite;

                //-BNAddress
                AddressColony auxColony = Repository.AddressColonies.Get((int)model.Subdistributor.BNAddress.AddressColonyId);
                auxEmployee.Subdistributor.BNAddress.AddressStateId = auxColony.AddressStateId;
                auxEmployee.Subdistributor.BNAddress.AddressMunicipalityId = auxColony.AddressMunicipalityId;
                auxEmployee.Subdistributor.BNAddress.AddressPostalCodeId = auxColony.AddressPostalCodeId;
                auxEmployee.Subdistributor.BNAddress.AddressColonyId = auxColony.Id;
                auxEmployee.Subdistributor.BNAddress.Street = model.Subdistributor.BNAddress.Street;
                auxEmployee.Subdistributor.BNAddress.NumberExt = model.Subdistributor.BNAddress.NumberExt;
                auxEmployee.Subdistributor.BNAddress.NumberInt = model.Subdistributor.BNAddress.NumberInt;
                
                //if profile was not completed, complete
                if (auxEmployee.User.Cat_UserStatusId == 5) auxEmployee.User.Cat_UserStatusId = 1;

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "911. Excepción al actualizar la información Mi Cuenta del subdistribuidor", ex);
            }

            return ResultManager.IsCorrect;
        }
        /*
        public bool UpdateSubdistributorEmployee(SubdistributorEmployee model)
        {
            if (model == null) return false;

            SubdistributorEmployee auxEmployee = null;
            try
            {
                auxEmployee = Repository.SubdistributorEmployees.Get(model.Id);
                auxEmployee.User.NickName = model.EMail;
                auxEmployee.EMail = model.EMail;
                //auxEmployee.Name = model.Name;
                //auxEmployee.LastNameF = model.LastNameF;
                //auxEmployee.LastNameM = model.LastNameM;
                //auxEmployee.PhoneNumber = model.PhoneNumber;

                //auxEmployee.Distributor.IdB = model.Distributor.IdB;
                auxEmployee.Subdistributor.BusinessName = model.Distributor.BusinessName;
                auxEmployee.Subdistributor.CommercialName = model.Distributor.CommercialName;
                auxEmployee.Subdistributor.WebSite = model.Distributor.WebSite;

                //-address
                AddressColony auxColony = Repository.AddressColonies.Get((int)model.Distributor.Address.AddressColonyId);
                auxEmployee.Distributor.Address.AddressStateId = auxColony.AddressStateId;
                auxEmployee.Distributor.Address.AddressMunicipalityId = auxColony.AddressMunicipalityId;
                auxEmployee.Distributor.Address.AddressPostalCodeId = auxColony.AddressPostalCodeId;
                auxEmployee.Distributor.Address.AddressColonyId = auxColony.Id;
                auxEmployee.Distributor.Address.Street = model.Distributor.Address.Street;
                auxEmployee.Distributor.Address.NumberExt = model.Distributor.Address.NumberExt;
                auxEmployee.Distributor.Address.NumberInt = model.Distributor.Address.NumberInt;


                //if profile was not completed, complete
                if (auxEmployee.User.Cat_UserStatusId == 5) auxEmployee.User.Cat_UserStatusId = 1;

                Repository.Complete();
                ResultManager.Add("Perfil actualizado", "");
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                //Repository.Dispose();
                ErrorManager.Add("", ErrorDefault, "", ex);
                ResultManager.Add(ErrorDefault, "");
                ResultManager.IsCorrect = false;
            }

            return false;
        }
        */
        #endregion

        #region Others
        private bool SendUserTokenEmail(string auxUserFullName, string auxUserEmail, string tokenUrl)
        {
            ResultManager.Clear();

            Dictionary<string, string> emailParams = new Dictionary<string, string>() { { "userDisplayName", auxUserFullName }, { "message", "Recibimos una solicitud para reiniciar su contrase&ntilde;a, en caso de que usted haya enviado esta solicitud de click en la liga de abajo para reiniciar su contrase&ntilde;a:" }, { "tokenUrl", tokenUrl } };
            string messageBody = "<h2>Reinicio de contraseña</h2><h3>Mi Portal Bayer</h3><p>@userDisplayName</p><p>@message</p><p><a href='@tokenUrl'>reiniciar contrase&ntilde;a</a></p>";
            string subject = "Reinicio de contraseña - Mi Portal Bayer";
            if (SendEmail(auxUserEmail, subject, messageBody, emailParams))
            {
                ResultManager.IsCorrect = true;
            }

            return ResultManager.IsCorrect;
        }
        #endregion

    }
}

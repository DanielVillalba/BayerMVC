using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class User
    {
        public static User NewEmpty()
        {
            return new User()
            {
                Cat_UserStatusId = -1,
                FailedLoginAttempts = 0,
                Hash = null,
                LastLoginDate = null,
                LastPasswordChangeDate = null,
                LoginToken = null,
                LoginTokenGeneratedDate = null,
                NickName = string.Empty,
                Salt = null,
                Person = Person.NewEmpty(),
                RolesXUser = new List<RolesXUser>()
            };
        }
        public string InRoleNames
        {
            get
            {
                StringBuilder auxRoles = new StringBuilder();
                int roleCount = 1, nRoles = RolesXUser.Count;
                foreach (RolesXUser aRol in this.RolesXUser)
                {
                    auxRoles.Append(aRol.Cat_UserRole.Name + (roleCount < nRoles ? ", " : ""));
                    roleCount++;
                }
                return auxRoles.ToString();
            }
        }
        public bool IsInRole(string roleIdB)
        {
            StringBuilder auxRoles = new StringBuilder();
            foreach (RolesXUser aRol in this.RolesXUser)
            {
                if (aRol.Cat_UserRole.IdB == roleIdB) return true;
            }
            return false;
        }
        public bool IsInRoles(string roleIdBs, char separator = ',')
        {
            string[] roles = roleIdBs.Split(separator);
            StringBuilder auxRoles = new StringBuilder();
            foreach (RolesXUser aRol in this.RolesXUser)
            {
                foreach (string item in roles)
                {
                    if (aRol.Cat_UserRole.IdB == item) return true;
                }
            }
            return false;
        }
    }
}

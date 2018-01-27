using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PSD.Model;
using PSD.Repository.Core.Repositories;

namespace PSD.Repository.Persistence.Repositories
{
    public class RolesXUserRepository : Repository<RolesXUser>, IRolesXUserRepository
    {
        public RolesXUserRepository(PSDContext context)
            : base(context)
        {
        }
        /// <summary>
        /// Get the list of usr roles for the given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<RolesXUser> GetUserRoles(int userId)
        {
            return PSDContext.RolesXUser.Where(x => x.UserId == userId)
                //.Include(x => x.Cat_UserRole)
                .ToList();
        }
        public bool IsInRole(int userId, string roleIdB)
        {
            return PSDContext.RolesXUser.Where(x => x.UserId == userId && x.Cat_UserRole.IdB == roleIdB).Count() > 0 ? true : false;
        }

    }
}
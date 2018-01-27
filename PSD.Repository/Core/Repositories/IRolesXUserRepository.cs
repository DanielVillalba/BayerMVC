using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface IRolesXUserRepository : IRepository<RolesXUser>
    {
        IEnumerable<RolesXUser> GetUserRoles(int userId);
        bool IsInRole(int userId, string roleIdB);
    }
}
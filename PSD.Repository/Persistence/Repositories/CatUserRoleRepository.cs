using System.Linq;

using System.Collections.Generic;
using PSD.Model;
using PSD.Repository.Core.Repositories;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatUserRoleRepository : Repository<Cat_UserRole>, ICatUserRoleRepository
    {
        public CatUserRoleRepository(PSDContext context)
            : base(context)
        {
        }

        public Cat_UserRole GetByRoleIdB(string roleIdB)
        {
            return PSDContext.UserRoles.FirstOrDefault(x => x.IdB == roleIdB);
        }
        public IEnumerable<Cat_UserRole> GetAppAdminRoles(string[] appAdminRoleIdBs)
        {
            return PSDContext.UserRoles.Where(x => appAdminRoleIdBs.Contains(x.IdB)).ToList();
        }
        public IEnumerable<Cat_UserRole> GetEmployeeRoles(string[] employeeRoleIdBs)
        {
            return PSDContext.UserRoles.Where(x => employeeRoleIdBs.Contains(x.IdB)).ToList();
        }
        public IEnumerable<Cat_UserRole> GetCustomerRoles(string[] customerRoleIdBs)
        {
            return PSDContext.UserRoles.Where(x => customerRoleIdBs.Contains(x.IdB)).ToList();
        }
    }
}
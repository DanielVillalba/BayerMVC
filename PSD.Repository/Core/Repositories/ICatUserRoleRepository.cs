using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface ICatUserRoleRepository : IRepository<Cat_UserRole>
    {
        Cat_UserRole GetByRoleIdB(string roleIdB);
        IEnumerable<Cat_UserRole> GetAppAdminRoles(string[] employeeRoleIdBs);
        IEnumerable<Cat_UserRole> GetEmployeeRoles(string[] employeeRoleIdBs);
        IEnumerable<Cat_UserRole> GetCustomerRoles(string[] customerRoleIdBs);
    }
}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class BayerEmployeeRepository : Repository<BayerEmployee>, IBayerEmployeeRepository
    {
        public BayerEmployeeRepository(PSDContext context)
            : base(context)
        {
        }

        /*
        public new BayerEmployee Get(int Id)
        {
            return PSDContext.BayerEmployees.Include(x => x.User)
                .FirstOrDefault(x => x.User.Cat_UserStatusId != 2 && x.Id == Id);
                //.Include(x => x.User);
        }*/
        public new IEnumerable<BayerEmployee> GetAll()
        {
            return PSDContext.BayerEmployees
                .Where(x => x.User.Cat_UserStatusId != 2)
                .Include(x => x.User)
                .ToList();
            //.ThenInclude(person => person.)
        }
        public IEnumerable<BayerEmployee> GetAllRtvs()
        {
            return PSDContext.BayerEmployees
                .Where(x => x.User.Cat_UserStatusId != 2 && x.User.RolesXUser.FirstOrDefault().Cat_UserRoleId == 5)
                .Include(x => x.User)
                .ToList();
            //.ThenInclude(person => person.)
        }
        public IEnumerable<BayerEmployee> GetAllGrvs()
        {
            return PSDContext.BayerEmployees
                .Where(x => x.User.Cat_UserStatusId != 2 && x.User.RolesXUser.FirstOrDefault().Cat_UserRoleId == 3)
                .Include(x => x.User)
                .ToList();
            //.ThenInclude(person => person.)
        }
        public IEnumerable<Cat_Zone> GetZones(int bayerEmployeeId)
        {
            return PSDContext.MunicipalitiesXEmployee.Where(x => x.BayerEmployeeId == bayerEmployeeId).Select(x => x.Municipality.Zone).Distinct().ToList();
        }
        public IEnumerable<int?> GetZoneIds(int bayerEmployeeId)
        {
            return PSDContext.MunicipalitiesXEmployee.Where(x => x.BayerEmployeeId == bayerEmployeeId).Select(x => x.Municipality.Cat_ZoneId).Distinct().ToList();
        }
        public bool ZoneHasGRVAssigned(int zoneId)
        {
            if (PSDContext.MunicipalitiesXEmployee.Where(x => x.Municipality.Cat_ZoneId == zoneId && x.BayerEmployee.User.RolesXUser.FirstOrDefault().Cat_UserRole.IdB == "employee-manager_operation").Count() > 0) return true;
            return false;
        }
        public bool ZoneHasRTVAssigned(int zoneId)
        {
            if (PSDContext.MunicipalitiesXEmployee.Where(x => x.Municipality.Cat_ZoneId == zoneId && x.BayerEmployee.User.RolesXUser.FirstOrDefault().Cat_UserRole.IdB == "employee-rtv_operation").Count() > 0) return true;
            return false;
        }
        public MunicipalitiesXEmployee GRVAssignedToZone(int zoneId)
        {
            return PSDContext.MunicipalitiesXEmployee.Where(x => x.Municipality.Cat_ZoneId == zoneId && x.BayerEmployee.User.RolesXUser.FirstOrDefault().Cat_UserRole.IdB == "employee-manager_operation").FirstOrDefault();
        }
        public MunicipalitiesXEmployee RTVAssignedToZone(int zoneId)
        {
            return PSDContext.MunicipalitiesXEmployee.Where(x => x.Municipality.Cat_ZoneId == zoneId && x.BayerEmployee.User.RolesXUser.FirstOrDefault().Cat_UserRole.IdB == "employee-rtv_operation").FirstOrDefault();
        }

        /*
        public User GetByNickName(string nickName)
        {
            return PSDContext.Users.FirstOrDefault(a => a.NickName == nickName);
        }*/
    }
}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class DistributorEmployeeRepository : Repository<DistributorEmployee>, IDistributorEmployeeRepository
    {
        public DistributorEmployeeRepository(PSDContext context)
            : base(context)
        {
        }
        public new IEnumerable<DistributorEmployee> GetAll()
        {
            return PSDContext.DistributorEmployees
                .Where(x => x.User.Cat_UserStatusId != 2)
                .Include(x => x.User)
                .ToList();
            //.ThenInclude(person => person.)
        }
        
        /*
        public User GetByNickName(string nickName)
        {
            return PSDContext.Users.FirstOrDefault(a => a.NickName == nickName);
        }*/
    }
}
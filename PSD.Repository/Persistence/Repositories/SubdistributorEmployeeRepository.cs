using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorEmployeeRepository : Repository<SubdistributorEmployee>, ISubdistributorEmployeeRepository
    {
        public SubdistributorEmployeeRepository(PSDContext context)
            : base(context)
        {
        }
        public new IEnumerable<SubdistributorEmployee> GetAll()
        {
            return PSDContext.SubdistributorEmployees
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
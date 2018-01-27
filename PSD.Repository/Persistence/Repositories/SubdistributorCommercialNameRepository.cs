using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorCommercialNameRepository : Repository<SubdistributorCommercialName>, ISubdistributorCommercialNameRepository
    {
        public SubdistributorCommercialNameRepository(PSDContext context)
            : base(context)
        {
        }
/*
        public new IEnumerable<Employee> GetAll()
        {
            return PSDContext.Employees
                .Where(x => x.Person.User.Cat_UserStatusId != 2)
                .Include(x => x.Person.User)
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
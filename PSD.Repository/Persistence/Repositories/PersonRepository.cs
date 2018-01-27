using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(PSDContext context)
            : base(context)
        {
        }

        /*public User GetByNickName(string nickName)
        {
            return PSDContext.Users.FirstOrDefault(a => a.NickName == nickName);
        }*/
    }
}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class DistributorContactRepository : Repository<DistributorContact>, IDistributorContactRepository
    {
        public DistributorContactRepository(PSDContext context)
            : base(context)
        {
        }
        /*
        public User GetByNickName(string nickName)
        {
            return PSDContext.Users.FirstOrDefault(a => a.NickName == nickName);
        }*/
    }
}
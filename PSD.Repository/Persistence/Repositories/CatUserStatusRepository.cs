using System.Data.Entity;
using System.Linq;
using PSD.Model;
using PSD.Repository.Core.Repositories;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatUserStatusRepository : Repository<Cat_UserStatus>, ICatUserStatusRepository
    {
        public CatUserStatusRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
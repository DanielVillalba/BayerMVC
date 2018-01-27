using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatSubdistributorContactAreaRepository : Repository<Cat_SubdistributorContactArea>, ICatSubdistributorContactAreaRepository
    {
        public CatSubdistributorContactAreaRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatDistributorBranchContactAreaRepository : Repository<Cat_DistributorBranchContactArea>, ICatDistributorBranchContactAreaRepository
    {
        public CatDistributorBranchContactAreaRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
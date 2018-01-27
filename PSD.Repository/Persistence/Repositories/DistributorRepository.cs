using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class DistributorRepository : Repository<Distributor>, IDistributorRepository
    {
        public DistributorRepository(PSDContext context)
            : base(context)
        {
        }

        public IQueryable<Distributor> GetAllToFilter()
        {
            return PSDContext.Distributors.OrderByDescending(x => x.Id);
        }
    }
}
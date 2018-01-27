using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorContactRepository : Repository<SubdistributorContact>, ISubdistributorContactRepository
    {
        public SubdistributorContactRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class AddressesXSubdistributorRepository : Repository<AddressesXSubdistributor>, IAddressesXSubdistributorRepository
    {
        public AddressesXSubdistributorRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
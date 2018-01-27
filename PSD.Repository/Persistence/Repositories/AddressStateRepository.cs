using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class AddressStateRepository : Repository<AddressState>, IAddressStateRepository
    {
        public AddressStateRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
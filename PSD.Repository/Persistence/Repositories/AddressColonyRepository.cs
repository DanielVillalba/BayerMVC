using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class AddressColonyRepository : Repository<AddressColony>, IAddressColonyRepository
    {
        public AddressColonyRepository(PSDContext context)
            : base(context)
        {
        }
        public new AddressColony Get(int id)
        {
            return PSDContext.AddressColonies.FirstOrDefault(a => a.Id == id );
        }
    }
}
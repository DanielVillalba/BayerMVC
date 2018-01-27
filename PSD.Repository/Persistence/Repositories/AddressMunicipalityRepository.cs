using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class AddressMunicipalityRepository : Repository<AddressMunicipality>, IAddressMunicipalityRepository
    {
        public AddressMunicipalityRepository(PSDContext context)
            : base(context)
        {
        }

        public IEnumerable<AddressMunicipality> GetByZoneId(int zoneId)
        {
            return PSDContext.AddressMunicipalities.Where(x => x.Cat_ZoneId == zoneId).ToList();
        }
        public IEnumerable<AddressMunicipality> GetByZoneIds(List<int?> zoneId)
        {
            return PSDContext.AddressMunicipalities.Where(x => zoneId.Contains(x.Cat_ZoneId)).ToList();
        }
        public IEnumerable<AddressMunicipality> GetByStateId(int stateId)
        {
            return PSDContext.AddressMunicipalities.Where(x => x.AddressStateId == stateId).ToList();
        }
    }
}
using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface IAddressMunicipalityRepository : IRepository<AddressMunicipality>
    {
        IEnumerable<AddressMunicipality> GetByZoneId(int zoneId);
        IEnumerable<AddressMunicipality> GetByZoneIds(List<int?> zoneId);
        IEnumerable<AddressMunicipality> GetByStateId(int stateId);
    }
}
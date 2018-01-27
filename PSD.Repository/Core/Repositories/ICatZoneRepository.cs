using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface ICatZoneRepository : IRepository<Cat_Zone>
    {
        IEnumerable<Cat_Zone> GetByMunicipalityId(int municipalityId);
        IEnumerable<Cat_Zone> GetByMunicipalityIds(ICollection<int> municipalityIds);
        IEnumerable<Cat_Zone> GetByMunicipalities(ICollection<AddressMunicipality> municipalities);
        IEnumerable<Cat_Zone> GetByMunicipalities(ICollection<MunicipalitiesXEmployee> municipalities);
    }
}
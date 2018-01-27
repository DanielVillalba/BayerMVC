using System.Data.Entity;
using System.Linq;
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatZoneRepository : Repository<Cat_Zone>, ICatZoneRepository
    {
        public CatZoneRepository(PSDContext context)
            : base(context)
        {
        }
        public IEnumerable<Cat_Zone> GetByMunicipalityId(int municipalityId)
        {
            /*List<AddressMunicipality> auxMunicipalities = PSDContext.AddressMunicipalities.Where(x => x.Id == municipalityId).Distinct().ToList();
            List<Cat_Zone> vf = new List<Cat_Zone>();
            foreach(AddressMunicipality item in auxMunicipalities)
            {
                if(!vf.Find(item.Zone))
                {

                }
            }
            return vf;*/
            return PSDContext.AddressMunicipalities.Where(x => x.Id == municipalityId).Select(x => x.Zone).Distinct().ToList();
        }
        public IEnumerable<Cat_Zone> GetByMunicipalityIds(ICollection<int> municipalityIds)
        {
            return PSDContext.AddressMunicipalities.Where(x => municipalityIds.Contains(x.Id)).Select(x => x.Zone).Distinct().ToList();
        }
        public IEnumerable<Cat_Zone> GetByMunicipalities(ICollection<AddressMunicipality> municipalities)
        {
            List<int> auxMunicipalities = new List<int>();
            foreach (AddressMunicipality item in municipalities)
            {
                auxMunicipalities.Add(item.Id);
            }
            return GetByMunicipalityIds(auxMunicipalities);
        }
        public IEnumerable<Cat_Zone> GetByMunicipalities(ICollection<MunicipalitiesXEmployee> municipalities)
        {
            List<int> auxMunicipalities = new List<int>();
            foreach (MunicipalitiesXEmployee item in municipalities)
            {
                auxMunicipalities.Add(item.AddressMunicipalityId);
            }
            return GetByMunicipalityIds(auxMunicipalities);
        }

    }
}
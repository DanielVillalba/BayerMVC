using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorRepository : Repository<Subdistributor>, ISubdistributorRepository
    {
        public SubdistributorRepository(PSDContext context)
            : base(context)
        {
        }

        public string RetrieveSubdistributorZone(int id)
        {
            return PSDContext.Subdistributors.Where(x => x.Id == id).FirstOrDefault().BNAddress.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name;
        }

        public List<string> RetrieveSubdistributorZones(int id)
        {
            List<string> zones = new List<string>();

            Subdistributor item = PSDContext.Subdistributors.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                bool isAvailableName = item.BNAddress != null &&
                                       item.BNAddress.AddressColony != null &&
                                       item.BNAddress.AddressColony.AddressPostalCode != null &&
                                       item.BNAddress.AddressColony.AddressPostalCode.AddressMunicipality != null;

                var BNAddressZone = isAvailableName
                                    ? item.BNAddress.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name
                                    : null;

                if (!string.IsNullOrEmpty(BNAddressZone))
                    zones.Add(BNAddressZone);

                foreach (AddressesXSubdistributor additionalAddress in item.Addresses)
                {
                    isAvailableName = additionalAddress.Address != null &&
                                      additionalAddress.Address.AddressColony != null &&
                                      additionalAddress.Address.AddressColony.AddressPostalCode != null &&
                                      additionalAddress.Address.AddressColony.AddressPostalCode.AddressMunicipality != null;

                    var additionalAddressZone = isAvailableName
                                                ? additionalAddress.Address.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name
                                                : null;

                    if (!string.IsNullOrEmpty(additionalAddressZone) && !zones.Contains(additionalAddressZone))
                        zones.Add(additionalAddressZone);
                }
            }

            return zones;
        }

        public IQueryable<Subdistributor> GetAllToFilter()
        {
            return PSDContext.Subdistributors.OrderByDescending(x => x.Id);
        }
    }
}
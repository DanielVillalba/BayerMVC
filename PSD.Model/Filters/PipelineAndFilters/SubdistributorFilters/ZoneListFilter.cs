using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters
{
    public class ZoneListFilter : IFilter<IQueryable<Subdistributor>>
    {
        private List<string> _zones;

        public ZoneListFilter(List<string> zones)
        {
            _zones = zones;
        }

        public IQueryable<Subdistributor> Execute(IQueryable<Subdistributor> input)
        {
            IQueryable<Subdistributor> result;

            result = _zones == null
                    ? input
                    : input.Where(x => _zones.Any(zones => zones.Equals(x.BNAddress.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name)));

            return result;
        }
    }
}

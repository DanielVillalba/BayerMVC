using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PSD.Model.Filters.PipelineAndFilters.DistributorFilters
{
    public class ZoneListFilter : IFilter<IQueryable<Distributor>>
    {
        private List<string> _zones;

        /// <summary>
        /// This will filter by a list of zones.
        /// </summary>
        /// <param name="zones"></param>
        public ZoneListFilter(List<string> zones)
        {
            _zones = zones;
        }

        public IQueryable<Distributor> Execute(IQueryable<Distributor> input)
        {
            IQueryable<Distributor> result;

            result = _zones == null
                    ? input
                    : input.Where(x => _zones.Any(y => y.Equals(x.Address.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name)));
            
            return result;
        }
    }
}

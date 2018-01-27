using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System.Linq;

namespace PSD.Model.Filters.PipelineAndFilters.DistributorFilters
{
    public class ZoneFilter : IFilter<IQueryable<Distributor>>
    {
        private string _zone;

        public ZoneFilter(string zone)
        {
            _zone = zone;
        }
        public IQueryable<Distributor> Execute(IQueryable<Distributor> input)
        {
            IQueryable<Distributor> result;

            result = _zone == null
                    ? input
                    : input.Where(x => x.Address.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name == _zone);

            return result;
        }
    }
}

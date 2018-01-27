using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters
{
    public class ZoneFilter : IFilter<IQueryable<Subdistributor>>
    {
        private string _zone;

        public ZoneFilter(string zone)
        {
            _zone = zone;
        }

        public IQueryable<Subdistributor> Execute(IQueryable<Subdistributor> input)
        {
            IQueryable<Subdistributor> result;

            result = _zone == null
                    ? input
                    : input.Where(x => x.BNAddress.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name == _zone);

            return result;
        }
    }
}

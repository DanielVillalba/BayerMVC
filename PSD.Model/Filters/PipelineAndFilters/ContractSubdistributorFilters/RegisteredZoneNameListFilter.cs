using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class RegisteredZoneNameListFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private List<string> _zoneNames;

        public RegisteredZoneNameListFilter(List<string> zoneNames)
        {
            _zoneNames = zoneNames;
        }
        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = _zoneNames == null
                    ? input
                    : input.Where(x => _zoneNames.Any(y => y.Equals(x.RegisteredZoneName)));

            return result;
        }
    }
}

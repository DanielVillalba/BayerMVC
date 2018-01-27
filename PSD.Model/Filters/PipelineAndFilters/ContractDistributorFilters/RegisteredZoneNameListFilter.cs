using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class RegisteredZoneNameListFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private List<string> _zoneNames;

        public RegisteredZoneNameListFilter(List<string> zoneNames)
        {
            _zoneNames = zoneNames;    
        }
        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = _zoneNames == null
                    ? input
                    : input.Where(x => _zoneNames.Any(y => y.Equals(x.RegisteredZoneName)));

            return result;
        }
    }
}

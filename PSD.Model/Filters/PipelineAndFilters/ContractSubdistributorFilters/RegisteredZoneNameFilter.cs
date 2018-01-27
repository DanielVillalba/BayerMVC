using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class RegisteredZoneNameFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private string _registeredZone;
        public RegisteredZoneNameFilter(string registeredZone)
        {
            _registeredZone = registeredZone;
        }

        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = _registeredZone == null
                    ? input
                    : input.Where(x => string.Equals(x.RegisteredZoneName, _registeredZone));

            return result;
        }
    }
}

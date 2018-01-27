using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class StatusIdListFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private List<int> _statusIds;
        public StatusIdListFilter(List<int> statusIds)
        {
            _statusIds = statusIds;
        }

        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => _statusIds.Any(y => y.Equals(x.ContractSubdistributorStatusId)));

            return result;
        }
    }
}

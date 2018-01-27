using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class StatusIdListFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private List<int> _statusIds;
        public StatusIdListFilter(List<int> statusIds)
        {
            _statusIds = statusIds;
        }

        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => _statusIds.Any(y => y.Equals(x.ContractDistributorStatusId)));

            return result;
        }
    }
}

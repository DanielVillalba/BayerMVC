using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class DistributorIdListFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private List<int> _distributorIds;
        public DistributorIdListFilter(List<int> distributorIds)
        {
            _distributorIds = distributorIds;
        }

        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => _distributorIds.Any(y => y.Equals(x.DistributorId)));

            return result;
        }
    }
}

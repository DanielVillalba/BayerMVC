using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class DistributorIdFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private int _distributorId;
        public DistributorIdFilter(int distributorId)
        {
            _distributorId = distributorId;
        }
        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => x.DistributorId == _distributorId);

            return result;
        }
    }
}

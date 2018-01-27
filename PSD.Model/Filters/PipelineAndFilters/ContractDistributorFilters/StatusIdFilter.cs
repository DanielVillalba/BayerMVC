using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class StatusIdFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private int _statusId;
        public StatusIdFilter(int statusId)
        {
            _statusId = statusId;
        }

        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => x.ContractDistributorStatusId == _statusId);

            return result;
        }
    }
}

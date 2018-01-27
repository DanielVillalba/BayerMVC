using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class StatusIdFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private int _statusId;
        public StatusIdFilter(int statusId)
        {
            _statusId = statusId;
        }

        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => x.ContractSubdistributorStatusId == _statusId);

            return result;
        }
    }
}

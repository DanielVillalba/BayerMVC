using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class GRVAssignedFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private int _GRVEmployeeId;
        public GRVAssignedFilter(int GRVEmployeeId)
        {
            _GRVEmployeeId = GRVEmployeeId;
        }
        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => x.GRVBayerEmployeeId == _GRVEmployeeId);

            return result;
        }
    }
}

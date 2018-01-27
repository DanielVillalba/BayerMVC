using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class AssignedGRVFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private int _grvId;
        public AssignedGRVFilter(int grvId)
        {
            _grvId = grvId;
        }
        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => x.GRVBayerEmployeeId == _grvId);

            return result;
        }
    }
}

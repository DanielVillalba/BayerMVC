using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class SubdistributorIdListFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private List<int> _subdistributorIds;
        public SubdistributorIdListFilter(List<int> subdistributorIds)
        {
            _subdistributorIds = subdistributorIds;
        }

        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => _subdistributorIds.Any(y => y.Equals(x.SubdistributorId)));

            return result;
        }
    }
}

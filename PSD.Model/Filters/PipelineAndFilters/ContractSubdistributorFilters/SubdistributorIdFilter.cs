using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class SubdistributorIdFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private int _subdistributorId;
        public SubdistributorIdFilter(int subdistributorId)
        {
            _subdistributorId = subdistributorId;
        }
        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => x.SubdistributorId == _subdistributorId);

            return result;
        }
    }
}

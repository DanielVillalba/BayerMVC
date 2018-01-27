using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class StateFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private int _stateId;

        public StateFilter(int stateId)
        {
            _stateId = stateId;
        }

        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => x.Subdistributor.BNAddress.AddressStateId.Value == _stateId);

            return result;
        }
    }
}

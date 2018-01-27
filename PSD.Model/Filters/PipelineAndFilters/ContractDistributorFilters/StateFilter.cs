using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class StateFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private int _stateId;

        public StateFilter(int stateId)
        {
            _stateId = stateId;
        }

        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => x.Distributor.Address.AddressStateId.Value == _stateId);

            return result;
        }
    }
}

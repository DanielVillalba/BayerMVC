using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.DistributorFilters
{
    public class StateFilter : IFilter<IQueryable<Distributor>>
    {
        private int _stateId;

        public StateFilter(int stateId)
        {
            _stateId = stateId;
        }

        public IQueryable<Distributor> Execute(IQueryable<Distributor> input)
        {
            IQueryable<Distributor> result;

            result = input.Where(x => x.Address.AddressStateId.Value == _stateId);

            return result;
        }
    }
}

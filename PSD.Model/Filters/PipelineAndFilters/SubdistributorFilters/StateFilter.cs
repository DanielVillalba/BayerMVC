using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters
{
    public class StateFilter : IFilter<IQueryable<Subdistributor>>
    {
        private int _stateId;

        public StateFilter(int statedId)
        {
            _stateId = statedId;
        }

        public IQueryable<Subdistributor> Execute(IQueryable<Subdistributor> input)
        {
            IQueryable<Subdistributor> result;

            result = input.Where(x => x.BNAddress.AddressStateId == _stateId);

            return result;
        }
    }
}

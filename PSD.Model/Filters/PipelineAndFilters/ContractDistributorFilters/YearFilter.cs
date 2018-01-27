using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    public class YearFilter : IFilter<IQueryable<ContractDistributor>>
    {
        private int _year;
        public YearFilter(int year)
        {
            _year = year;
        }

        public IQueryable<ContractDistributor> Execute(IQueryable<ContractDistributor> input)
        {
            IQueryable<ContractDistributor> result;

            result = input.Where(x => x.Year == _year);

            return result;
        }
    }
}

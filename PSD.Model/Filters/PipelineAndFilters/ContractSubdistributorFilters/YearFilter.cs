using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters
{
    public class YearFilter : IFilter<IQueryable<ContractSubdistributor>>
    {
        private int _year;
        public YearFilter(int year)
        {
            _year = year;
        }

        public IQueryable<ContractSubdistributor> Execute(IQueryable<ContractSubdistributor> input)
        {
            IQueryable<ContractSubdistributor> result;

            result = input.Where(x => x.Year == _year);

            return result;
        }
    }
}

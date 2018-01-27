using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters
{
    /// <summary>
    /// Pipeline which to select final list of applicable agents
    /// </summary>
    public class ContractDistributorFilteringPipeline : Pipeline<IQueryable<ContractDistributor>>
    {
        /// <summary>
        /// Method which executes the filter on a given Input
        /// </summary>
        /// <param name="input">Input on which filtering
        /// needs to happen as implementing in individual filters</param>
        /// <returns></returns>
        public override IQueryable<ContractDistributor> Process(IQueryable<ContractDistributor> input)
        {
            foreach (var filter in filters)
            {
                input = filter.Execute(input);
            }

            return input;
        }
    }
}

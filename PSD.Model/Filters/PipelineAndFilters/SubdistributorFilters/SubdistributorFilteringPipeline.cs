using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters
{
    /// <summary>
    /// Pipeline which to select final list of applicable agents
    /// </summary>
    public class SubdistributorFilteringPipeline : Pipeline<IQueryable<Subdistributor>>
    {
        /// <summary>
        /// Method which executes the filter on a given Input
        /// </summary>
        /// <param name="input">Input on which filtering
        /// needs to happen as implementing in individual filters</param>
        /// <returns></returns>
        public override IQueryable<Subdistributor> Process(IQueryable<Subdistributor> input)
        {
            foreach (var filter in filters)
            {
                input = filter.Execute(input);
            }

            return input;
        }
    }
}

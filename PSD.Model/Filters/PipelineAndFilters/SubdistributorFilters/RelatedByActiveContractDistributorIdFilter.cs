using PSD.Model.Filters.PipelineAndFilters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters
{
    public class RelatedByActiveContractDistributorIdFilter : IFilter<IQueryable<Subdistributor>>
    {
        private int _distributorId;

        public RelatedByActiveContractDistributorIdFilter(int distributorId)
        {
            _distributorId = distributorId;
        }

        public IQueryable<Subdistributor> Execute(IQueryable<Subdistributor> input)
        {
            IQueryable<Subdistributor> result;

            result = input.Where(subdistributor => 
                            subdistributor.Contracts.Where(contracts => 
                                contracts.ContractSubdistributorStatusId == 1 && 
                                contracts.DistributorPurchases.Where(purchases => 
                                    purchases.DistributorId == _distributorId).Any())
                          .Any());

            return result;
        }
    }
}

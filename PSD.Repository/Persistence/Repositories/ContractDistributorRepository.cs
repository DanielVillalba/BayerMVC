using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class ContractDistributorRepository : Repository<ContractDistributor>, IContractDistributorRepository
    {
        public ContractDistributorRepository(PSDContext context)
            : base(context)
        {
        }
        public new IEnumerable<ContractDistributor> GetAll()
        {
            return PSDContext.ContractsDistributor
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        public IQueryable<ContractDistributor> GetAllToFilter()
        {
            return PSDContext.ContractsDistributor.OrderByDescending(x => x.Id);
        }
    }
}
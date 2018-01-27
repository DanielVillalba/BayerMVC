using PSD.Model;
using System.Collections.Generic;
using System.Linq;

namespace PSD.Repository.Core.Repositories
{
    public interface IContractDistributorRepository : IRepository<ContractDistributor>
    {
        IQueryable<ContractDistributor> GetAllToFilter();
    }
}
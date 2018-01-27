using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatContractDistributorStatusRepository : Repository<Cat_ContractDistributorStatus>, ICatContractDistributorStatusRepository
    {
        public CatContractDistributorStatusRepository(PSDContext context)
            : base(context)
        {
        }
        
    }
}
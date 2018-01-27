using PSD.Model;
using System.Collections.Generic;
using System.Linq;

namespace PSD.Repository.Core.Repositories
{
    public interface IContractSubdistributorRepository : IRepository<ContractSubdistributor>
    {
        IQueryable<ContractSubdistributor> GetAllToFilter();
        List<ContractSubdistributor> GetAllActive();
        ContractSubdistributor GetLastSubdistributorContract(int subdistributorId);
        ContractSubdistributor GetPastYearSubdistributorContract(int subdistributorId);
    }
}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class ContractSubdistributorRepository : Repository<ContractSubdistributor>, IContractSubdistributorRepository
    {
        public ContractSubdistributorRepository(PSDContext context)
            : base(context)
        {
        }
        public new IEnumerable<ContractSubdistributor> GetAll()
        {
            return PSDContext.ContractsSubdistributor
                .OrderByDescending(x => x.Id)
                .ToList();
        }
        public List<ContractSubdistributor> GetAllActive()
        {
            return PSDContext.ContractsSubdistributor
                .Where(x => x.ContractSubdistributorStatus.IdB == "active")
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        public ContractSubdistributor GetPastYearSubdistributorContract(int subdistributorId)
        {
            return PSDContext.ContractsSubdistributor
                .FirstOrDefault(x => x.SubdistributorId == subdistributorId && x.Year == (PSD.Common.Dates.Today.Year - 1));
        }
        public ContractSubdistributor GetLastSubdistributorContract(int subdistributorId)
        {
            return PSDContext.ContractsSubdistributor
                .OrderByDescending(x => x.Id).FirstOrDefault(x => x.SubdistributorId == subdistributorId);
        }

        public IQueryable<ContractSubdistributor> GetAllToFilter()
        {
            return PSDContext.ContractsSubdistributor.OrderByDescending(x => x.Id);
        }

    }
}
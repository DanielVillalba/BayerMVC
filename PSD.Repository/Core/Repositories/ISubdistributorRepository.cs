using PSD.Model;
using System.Collections.Generic;
using System.Linq;

namespace PSD.Repository.Core.Repositories
{
    public interface ISubdistributorRepository : IRepository<Subdistributor>
    {
        string RetrieveSubdistributorZone(int id);

        List<string> RetrieveSubdistributorZones(int id);

        IQueryable<Subdistributor> GetAllToFilter();
    }
}
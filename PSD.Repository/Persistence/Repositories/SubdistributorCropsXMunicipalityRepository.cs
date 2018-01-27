using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorCropsXMunicipalityRepository : Repository<SubdistributorCropsXMunicipality>, ISubdistributorCropsXMunicipalityRepository
    {
        public SubdistributorCropsXMunicipalityRepository(PSDContext context)
            : base(context)
        {
        }
    }
}
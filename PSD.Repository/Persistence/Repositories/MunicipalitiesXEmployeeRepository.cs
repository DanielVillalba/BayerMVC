using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class MunicipalitiesXEmployeeRepository : Repository<MunicipalitiesXEmployee>, IMunicipalitiesXEmployeeRepository
    {
        public MunicipalitiesXEmployeeRepository(PSDContext context)
            : base(context)
        {
            
        }
    }
}
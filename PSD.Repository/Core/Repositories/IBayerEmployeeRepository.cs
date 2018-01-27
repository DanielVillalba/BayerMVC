using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface IBayerEmployeeRepository : IRepository<BayerEmployee>
    {
        IEnumerable<Cat_Zone> GetZones(int bayerEmployeeId);
        IEnumerable<int?> GetZoneIds(int bayerEmployeeId);
        IEnumerable<BayerEmployee> GetAllRtvs();
        IEnumerable<BayerEmployee> GetAllGrvs();
        bool ZoneHasGRVAssigned(int zoneId);
        bool ZoneHasRTVAssigned(int zoneId);
        MunicipalitiesXEmployee GRVAssignedToZone(int zoneId);
        MunicipalitiesXEmployee RTVAssignedToZone(int zoneId);
    }
}
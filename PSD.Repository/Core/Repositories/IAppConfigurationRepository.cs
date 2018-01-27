using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface IAppConfigurationRepository : IRepository<AppConfiguration>
    {
        AppConfiguration GetByKey(string key);
        string IdBCounterGetNextDistributor();
        string IdBCounterGetNextSubdistributor();
        string IdBCounterGetNextContractDistributor();
        string IdBCounterGetNextContractSubdistributor();
    }
}
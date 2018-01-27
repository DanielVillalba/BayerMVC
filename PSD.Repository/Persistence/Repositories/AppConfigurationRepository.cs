using System.Data.Entity;
using System.Linq;
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System;

using PSD.Model.Keys;

namespace PSD.Repository.Persistence.Repositories
{
    public class AppConfigurationRepository : Repository<AppConfiguration>, IAppConfigurationRepository
    {
        public AppConfigurationRepository(PSDContext context)
            : base(context)
        {
        }
        public AppConfiguration GetByKey(string key)
        {
            return PSDContext.AppConfigurations.FirstOrDefault(x=>x.Key == key);
        }

        /// <summary>
        /// Get the counter and updates the next count
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valueStaticPart"></param>
        /// <returns></returns>
        private string IdBCounterGetNext(string key, int counterLenght)
        {
            AppConfiguration auxEntitycounter = GetByKey(key);
            string vf = auxEntitycounter.Value;
            int currentCount = Convert.ToInt32(vf);
            currentCount++;
            auxEntitycounter.Value = currentCount.ToString("D" + counterLenght);
            Context.SaveChanges();
            return vf;
        }

        public string IdBCounterGetNextDistributor()
        {
            AppConfiguration auxPrefixConfig = GetByKey(AppConfigurationKey.IdBCounterDistributorPrefix);
            if (auxPrefixConfig == null) throw new Exception("Error al obtener el prefijo del id de distribuidor");

            string auxCounterNext = IdBCounterGetNext(AppConfigurationKey.IdBCounterDistributor, 5);
            if (string.IsNullOrWhiteSpace(auxCounterNext)) throw new Exception("Error al calcular el siguiente id de distribuidor");

            return auxPrefixConfig.Value + auxCounterNext;
        }

        public string IdBCounterGetNextSubdistributor()
        {
            AppConfiguration auxPrefixConfig = GetByKey(AppConfigurationKey.IdBCounterSubdistributorPrefix);
            if (auxPrefixConfig == null) throw new Exception("Error al obtener el prefijo del id de subdistribuidor");
            string auxCounterNext = IdBCounterGetNext(AppConfigurationKey.IdBCounterSubdistributor, 5);
            if (string.IsNullOrWhiteSpace(auxCounterNext)) throw new Exception("Error al calcular el siguiente id de subdistribuidor");
            return auxPrefixConfig.Value + auxCounterNext;
        }

        public string IdBCounterGetNextContractDistributor()
        {
            AppConfiguration auxPrefixConfig = GetByKey(AppConfigurationKey.IdBCounterContractDistributorPrefix);
            if (auxPrefixConfig == null) throw new Exception("Error al obtener el prefijo del id de convenio de distribuidor");

            string auxCounterNext = IdBCounterGetNext(AppConfigurationKey.IdBCounterContractDistributor, 5);
            if (string.IsNullOrWhiteSpace(auxCounterNext)) throw new Exception("Error al calcular el siguiente id de convenio de distribuidor");

            return auxPrefixConfig.Value + auxCounterNext;
        }

        public string IdBCounterGetNextContractSubdistributor()
        {
            AppConfiguration auxPrefixConfig = GetByKey(AppConfigurationKey.IdBCounterContractSubdistributorPrefix);
            if (auxPrefixConfig == null) throw new Exception("Error al obtener el prefijo del id de convenio de subdistribuidor");

            string auxCounterNext = IdBCounterGetNext(AppConfigurationKey.IdBCounterContractSubdistributor, 5);
            if (string.IsNullOrWhiteSpace(auxCounterNext)) throw new Exception("Error al calcular el siguiente id de convenio de subdistribuidor");

            return auxPrefixConfig.Value + auxCounterNext;
        }

    }

}
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class AddressPostalCodeRepository : Repository<AddressPostalCode>, IAddressPostalCodeRepository
    {
        public AddressPostalCodeRepository(PSDContext context)
            : base(context)
        {
        }

        public AddressPostalCode GetByName(string postalCodeName)
        {
            return PSDContext.AddressPostalCodes.FirstOrDefault(x => x.Name == postalCodeName);
        }
    }
}
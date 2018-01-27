using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface IAddressPostalCodeRepository : IRepository<AddressPostalCode>
    {
        AddressPostalCode GetByName(string postalCodeName);
    }
}
using OgsysCRM.Models;
using System.Collections.Generic;

namespace OgsysCRM.DAL
{
    public interface IAddressRepo
    {
        IEnumerable<Address> GetAddresses();
        void InsertAddress(Address address);
        void DeleteAddress(int addressId);
        void UpdateAddress(Address address);
        void Save();
    }
}

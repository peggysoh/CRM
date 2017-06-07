using OgsysCRM.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OgsysCRM.DAL
{
    public class AddressRepo : IAddressRepo
    {
        private AppContext context;

        public AddressRepo(AppContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all addresses.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Address> GetAddresses()
        {
            return context.Addresses.ToList();
        }

        /// <summary>
        /// Inserts the address.
        /// </summary>
        /// <param name="address">The address.</param>
        public void InsertAddress(Address address)
        {
            context.Addresses.Add(address);
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="addressId">The address ID.</param>
        public void DeleteAddress(int addressId)
        {
            Address address = context.Addresses.Find(addressId);
            context.Addresses.Remove(address);
        }

        /// <summary>
        /// Updates the address.
        /// </summary>
        /// <param name="user">The address.</param>
        public void UpdateAddress(Address address)
        {
            context.Entry(address).State = EntityState.Modified;
        }

        /// <summary>
        /// Saves context.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
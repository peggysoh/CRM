using OgsysCRM.Models;
using OgsysCRM.Models.DataTables;
using System.Collections.Generic;

namespace OgsysCRM.DAL
{
    public interface ICustomerRepo
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerById(int customerId);
        void InsertCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        void UpdateCustomer(Customer customer);
        void Save();
        IEnumerable<Customer> GetAllCustomersForTable(ColumnViewModel sortedColumn = null, SearchViewModel searchBy = null);
    }
}

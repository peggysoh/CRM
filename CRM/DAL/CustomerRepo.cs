using System;
using System.Collections.Generic;
using System.Linq;
using OgsysCRM.Models.DataTables;
using OgsysCRM.Models;
using System.Data.Entity;

namespace OgsysCRM.DAL
{
    public class CustomerRepo : ICustomerRepo
    {
        private AppContext context;

        public CustomerRepo(AppContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            return context.Customers.ToList();
        }

        /// <summary>
        /// Gets the customer by Id.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns></returns>
        public Customer GetCustomerById(int customerId)
        {
            return context.Customers.Find(customerId);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void InsertCustomer(Customer customer)
        {
            context.Customers.Add(customer);
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="userId">The customer ID.</param>
        public void DeleteCustomer(int customerId)
        {
            Customer customer = context.Customers.Find(customerId);
            context.Customers.Remove(customer);
        }

        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void UpdateCustomer(Customer customer)
        {
            context.Entry(customer).State = EntityState.Modified;
        }

        /// <summary>
        /// Saves context.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Gets Customers for datatable
        /// </summary>
        public IEnumerable<Customer> GetAllCustomersForTable(ColumnViewModel sortedColumn = null, SearchViewModel searchBy = null)
        {
            var customerList = GetCustomers();
            
            if (searchBy != null && !string.IsNullOrEmpty(searchBy.Value))
            {
                var term = searchBy.Value.ToLower();
                customerList = customerList.Where(x => x.FirstName.ToLower().Contains(term) ||
                                                  x.LastName.ToLower().Contains(term) ||
                                                  x.CompanyName.ToLower().Contains(term) ||
                                                  x.Email.ToLower().Contains(term) ||
                                                  x.Phone.Contains(term));
            }

            // sorted by column and order
            if (sortedColumn == null)
                return customerList.OrderBy(x => x.FirstName);

            switch (sortedColumn.Data)
            {
                case "Id":
                    {
                        customerList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? customerList.OrderBy(x => x.Id)
                            : customerList.OrderByDescending(x => x.Id);
                    }
                    break;

                case "Name":
                    {
                        customerList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? customerList.OrderBy(x => x.FirstName)
                            : customerList.OrderByDescending(x => x.FirstName);
                    }
                    break;

                case "CompanyName":
                    {
                        customerList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? customerList.OrderBy(x => x.CompanyName)
                            : customerList.OrderByDescending(x => x.CompanyName);
                    }
                    break;

                case "Email":
                    {
                        customerList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? customerList.OrderBy(x => x.Email)
                            : customerList.OrderByDescending(x => x.Email);
                    }
                    break;                    
            }

            return customerList;
        }
    }
}
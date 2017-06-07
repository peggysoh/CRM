namespace OgsysCRM.Models
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }

        public CustomerViewModel()
        {
            Customer = new Customer();
            Customer.Id = 0;
            Customer.FirstName = "";
            Customer.LastName = "";
            Customer.CompanyName = "";
            Customer.Email = "";
            Customer.Phone = "";
            Customer.Address = new Address();
        }

        public CustomerViewModel(Customer customer)
        {
            Customer = customer;
        }
    }
}
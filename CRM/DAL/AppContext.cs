using OgsysCRM.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace OgsysCRM.DAL
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Note> Notes { get; set; }

        //public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<Customer> Customers { get; set; }
        //public virtual DbSet<Address> Addresses { get; set; }
        //public virtual DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

}
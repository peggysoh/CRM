using NUnit.Framework;
using OgsysCRM.DAL;
using OgsysCRM.Models;
using System.Data.Entity;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace OgsysCRM.Tests
{
    [TestFixture]
    public class CustomerTest
    {
        [Test]
        public void CreateCustomer()
        {
            // Arrange
            var mockContext = new Mock<AppContext>();
            var mockSet = new Mock<DbSet<Customer>>();
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            var customerService = new CustomerRepo(mockContext.Object);
            var data = new Customer()
            {
                Id = 0,
                AddressId = 1
            };

            // Act
            customerService.InsertCustomer(data);
            customerService.Save();

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Customer>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(data.Id > 0);
        }

        [Test]
        public void GetAllCustomer()
        {
            // Arrange
            var data = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "AAA", AddressId = 1 },
                new Customer { Id = 2, FirstName = "BBB", AddressId = 2 },
                new Customer { Id = 3, FirstName = "CCC", AddressId = 3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppContext>();
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            var customerService = new CustomerRepo(mockContext.Object);

            // Act
            var result = customerService.GetCustomers().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("AAA", result[0].FirstName);
            Assert.AreEqual("BBB", result[1].FirstName);
            Assert.AreEqual("CCC", result[2].FirstName);
        }
        
        [Test]
        public void UpdateCustomer()
        {
            // Arrange
            var data = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "AAA", AddressId = 1 },
                new Customer { Id = 2, FirstName = "BBB", AddressId = 2 },
                new Customer { Id = 3, FirstName = "CCC", AddressId = 3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppContext>();

            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            var customerService = new CustomerRepo(mockContext.Object);

            // Act
            var foo = customerService.GetCustomerById(1);
            foo.FirstName = "DDD";
            customerService.UpdateCustomer(foo);
            customerService.Save();
            var result = customerService.GetCustomerById(1);

            // Assert
            Assert.AreEqual("DDD", result.FirstName);
        }

        [Test]
        public void DeleteCustomer()
        {
            // Arrange
            var mockContext = new Mock<AppContext>();
            var mockSet = new Mock<DbSet<Customer>>();
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            var customerService = new CustomerRepo(mockContext.Object);

            var data = new Customer()
            {
                Id = 1,
                AddressId = 1
            };
            customerService.InsertCustomer(data);
            customerService.Save();

            // Act
            customerService.DeleteCustomer(1);
            customerService.Save();
            var result = customerService.GetCustomerById(1);

            // Assert
            Assert.IsNull(result);
        }
    }
}
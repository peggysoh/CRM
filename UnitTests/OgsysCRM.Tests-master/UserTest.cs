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
    public class UserTest
    {
        [Test]
        public void CreateUser()
        {
            // Arrange
            var mockContext = new Mock<AppContext>();
            var mockSet = new Mock<DbSet<User>>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var userService = new UserRepo(mockContext.Object);
            var data = new User { Id = 0, Email = "test", Password = "AAA"  };

            // Act
            userService.InsertUser(data);
            userService.Save();

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(data.Id > 0);
        }

        [Test]
        public void GetAllUsers()
        {
            // Arrange
            var data = new List<User>
            {
                new User { Id = 1, Email = "test", Password = "AAA"  },
                new User { Id = 2, Email = "test", Password = "BBB"  },
                new User { Id = 3, Email = "test", Password = "CCC"  },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var userService = new UserRepo(mockContext.Object);

            // Act
            var result = userService.GetUsers().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);
        }

        [Test]
        public void UpdateUser()
        {
            // Arrange
            var data = new List<User>
            {
                new User { Id = 1, Email = "test", Password = "AAA"  },
                new User { Id = 2, Email = "test", Password = "BBB"  },
                new User { Id = 3, Email = "test", Password = "CCC"  },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppContext>();

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var userService = new UserRepo(mockContext.Object);

            // Act
            var foo = userService.GetUserById(1);
            foo.Email = "success";
            userService.UpdateUser(foo);
            userService.Save();
            var result = userService.GetUserById(1);

            // Assert
            Assert.AreEqual("success", result.Email);
        }

        [Test]
        public void DeleteUser()
        {
            // Arrange
            var mockContext = new Mock<AppContext>();
            var mockSet = new Mock<DbSet<User>>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var userService = new UserRepo(mockContext.Object);

            var data = new User { Id = 1, Email = "test", Password = "AAA" };

            userService.InsertUser(data);
            userService.Save();

            // Act
            userService.DeleteUser(1);
            userService.Save();
            var result = userService.GetUserById(1);
            
            // Assert
            Assert.IsNull(result);
        }
    }
}
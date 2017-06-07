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
    public class NoteTest
    {
        [Test]
        public void CreateNote()
        {
            // Arrange
            var mockContext = new Mock<AppContext>();
            var mockSet = new Mock<DbSet<Note>>();
            mockContext.Setup(m => m.Notes).Returns(mockSet.Object);
            var noteService = new NoteRepo(mockContext.Object);
            var data = new Note()
            {
                Id = 0,
                CustomerId = 1,
                UserId = 1
            };

            // Act
            noteService.InsertNote(data);
            noteService.Save();

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Note>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(data.Id > 0);
        }

        [Test]
        public void GetAllNotes()
        {
            // Arrange
            var data = new List<Note>
            {
                new Note { Id = 1, CustomerId = 1, UserId = 1 },
                new Note { Id = 2, CustomerId = 2, UserId = 2 },
                new Note { Id = 3, CustomerId = 3, UserId = 3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Note>>();
            mockSet.As<IQueryable<Note>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Note>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Note>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Note>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppContext>();
            mockContext.Setup(m => m.Notes).Returns(mockSet.Object);
            var noteService = new NoteRepo(mockContext.Object);

            // Act
            var result = noteService.GetNotes().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);
        }

        [Test]
        public void UpdateNote()
        {
            // Arrange
            var data = new List<Note>
            {
                new Note { Id = 1, CustomerId = 1, UserId = 1 },
                new Note { Id = 2, CustomerId = 2, UserId = 2 },
                new Note { Id = 3, CustomerId = 3, UserId = 3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Note>>();
            mockSet.As<IQueryable<Note>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Note>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Note>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Note>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppContext>();

            mockContext.Setup(m => m.Notes).Returns(mockSet.Object);
            var noteService = new NoteRepo(mockContext.Object);

            // Act
            var foo = noteService.GetNoteById(1); 
            foo.CustomerId = 2;
            noteService.UpdateNote(foo);
            noteService.Save();
            var result = noteService.GetNoteById(1);

            // Assert
            Assert.AreEqual(2, result.CustomerId);
        }

        [Test]
        public void DeleteNote()
        {
            // Arrange
            var mockContext = new Mock<AppContext>();
            var mockSet = new Mock<DbSet<Note>>();
            mockContext.Setup(m => m.Notes).Returns(mockSet.Object);
            var noteService = new NoteRepo(mockContext.Object);

            var data = new Note { Id = 1, CustomerId = 1, UserId = 1 };

            noteService.InsertNote(data);
            noteService.Save();

            // Act
            noteService.DeleteNote(1);
            noteService.Save();
            var result = noteService.GetNoteById(1);

            // Assert
            Assert.IsNull(result);
        }
    }
}
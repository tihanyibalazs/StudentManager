using Microsoft.EntityFrameworkCore;
using StudentManager.Models;
using StudentManager.Services;
using StudentManager.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace StudentManagerTest
{
    public class StudentManagerxUnitTest : IDisposable
    {

        private StudentManagerContext _context;
        private StudentManagerService _service;
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        public StudentManagerxUnitTest()
        {
            var options = new DbContextOptionsBuilder<StudentManagerContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            _context = new StudentManagerContext(options);
            _context.Database.EnsureCreated();
            _service = new StudentManagerService(_context);
        }

        internal async void AddStudent()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Teszt Peti",
                ClassYear = 6,
                PhoneNumber = "+36208769384",
                BirthDate = new DateTime(2000, 04, 21)
            };
            await _service.AddNewStudentAsync(viewModel);
        }

        [Fact]
        public async void AddNewValidStudentAsyncTest()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Teszt Peti",
                ClassYear = 6,
                PhoneNumber = "+36208769384",
                BirthDate = new DateTime(2000, 04, 21)
            };

            var studentTest = new Student
            {
                Name = "Teszt Peti",
                ClassYear = 6,
                PhoneNumber = "+36208769384",
                BirthDate = new DateTime(2000, 04, 21)
            };
            Assert.True(await _service.AddNewStudentAsync(viewModel));
            var studentCreated = await _context.Students.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Assert.Equal(studentTest, studentCreated);
        }

        [Fact]
        public async void AddNewStudentAsyncTestMissingName()
        {
            var viewModel = new CreateStudentViewModel
            {
                ClassYear = 6,
                PhoneNumber = "+36208769384",
                BirthDate = new DateTime(2000, 04, 21)
            };

            Assert.False(await _service.AddNewStudentAsync(viewModel));
        }

        [Fact]
        public async void AddNewStudentAsyncTestMissingPhoneNumber()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Próba Peti",
                ClassYear = 3,
                BirthDate = new DateTime(2000, 04, 21)
            };

            Assert.False(await _service.AddNewStudentAsync(viewModel));
        }

        [Fact]
        public async void AddNewStudentAsyncTestMissingBirthDate()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Próba Peti",
                ClassYear = 3,
                PhoneNumber = "+36208769384"
            };

            Assert.False(await _service.AddNewStudentAsync(viewModel));
        }

        [Fact]
        public async void AddNewStudentAsyncTestMissingClass()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Próba Peti",
                PhoneNumber = "+36208769384",
                BirthDate = new DateTime(2000, 04, 21)
            };

            Assert.False(await _service.AddNewStudentAsync(viewModel));
        }

        [Fact]
        public async void AddNewStudentAsyncTestFutureBirthDate()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Próba Peti",
                PhoneNumber = "+36208769384",
                BirthDate = DateTime.Now.AddDays(1)
            };

            Assert.False(await _service.AddNewStudentAsync(viewModel));
        }

        [Fact]
        public async void AddNewStudentAsyncTestOutOfClassRange()
        {
            var viewModel = new CreateStudentViewModel
            {
                Name = "Próba Peti",
                PhoneNumber = "+36208769384",
                BirthDate = new DateTime(2000, 04, 21),
                ClassYear = 13
            };

            Assert.False(await _service.AddNewStudentAsync(viewModel));
        }

        [Fact]
        public async void AddNewMarkAsyncTest()
        {
            AddStudent();

            var viewModel = new CreateMarkViewModel
            {
                StudentId = 1,
                Value = 3
            };

            Assert.True(await _service.AddNewMarkAsync(viewModel));

            var mark = await _context.Marks.Where(x => x.Id == 1).FirstOrDefaultAsync();

            Assert.Equal(3, mark.Value);
            Assert.Equal(1, mark.StudentId);
        }

        [Fact]
        public async void AddNewMarkAsyncTestMissingStudentId()
        {
            AddStudent();

            var viewModel = new CreateMarkViewModel
            {
                Value = 3
            };

            Assert.False(await _service.AddNewMarkAsync(viewModel));
        }

        [Fact]
        public async void AddNewMarkAsyncTestMissingValue()
        {
            AddStudent();

            var viewModel = new CreateMarkViewModel
            {
                StudentId = 1
            };

            Assert.False(await _service.AddNewMarkAsync(viewModel));
        }

        [Fact]
        public async void AddNewMarkAsyncTestValueOutOfRange()
        {
            AddStudent();

            var viewModel = new CreateMarkViewModel
            {
                StudentId = 1,
                Value = 6
            };

            Assert.False(await _service.AddNewMarkAsync(viewModel));
        }
    }
}

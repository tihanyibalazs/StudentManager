using Microsoft.EntityFrameworkCore;
using StudentManager.Models;
using StudentManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Services
{
    public class StudentManagerService : IStudentManagerService
    {
        private StudentManagerContext _context;

        public StudentManagerService(StudentManagerContext context)
        {
            _context = context;
        }

        public async Task<Boolean> AddNewStudentAsync(CreateStudentViewModel viewmodel)
        {

            var context = new ValidationContext(viewmodel);
            if (!Validator.TryValidateObject(viewmodel, context, null, true))
                return false;

            var NewStudent = new Student
            {
                Name = viewmodel.Name,
                PhoneNumber = viewmodel.PhoneNumber,
                BirthDate = viewmodel.BirthDate,
                ClassYear = viewmodel.ClassYear
            };

            _context.Students.Add(NewStudent);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<Student>> PagedStudentsAsync(int pageNumber, int studentsOnOnePage)
        {
            return await _context.Students
                .OrderBy(x => x.Name)
                .Skip(pageNumber * studentsOnOnePage)
                .Take(studentsOnOnePage)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentStatisticViewModel>> PagedStudentStatisticsAsync(int pageNumber, int studentsOnOnePage)
        {
            List<StudentStatisticViewModel> StatisticsList = new List<StudentStatisticViewModel>();

            var StudentsList = await _context.Students
                .Where(x => x.MarksAverage != 0)
                .OrderByDescending(x => x.MarksAverage)
                .Skip(pageNumber * studentsOnOnePage)
                .Take(studentsOnOnePage)
                .AsNoTracking()
                .ToListAsync();

            foreach (Student student in StudentsList)
            {
                if (_context.Marks.Any(x => x.Student.Id == student.Id))
                {
                    var newStatistic = new StudentStatisticViewModel();
                    newStatistic.Name = student.Name;
                    newStatistic.BestMark = await _context.Marks.Where(x => x.Student.Id == student.Id).MaxAsync(x => x.Value);
                    newStatistic.MarksAverage = student.MarksAverage.ToString("0.##");
                    newStatistic.TotalOneMark = await _context.Marks.Where(x => x.Student.Id == student.Id).CountAsync(x => x.Value == 1);
                    StatisticsList.Add(newStatistic);
                }
            }
            return StatisticsList;
        }

        public async Task<Boolean> AddNewMarkAsync(CreateMarkViewModel viewmodel)
        {
            var validatorContext = new ValidationContext(viewmodel);
            if (!Validator.TryValidateObject(viewmodel, validatorContext, null, true))
                return false;

            var student = await _context.Students.Where(x => x.Id == viewmodel.StudentId).FirstOrDefaultAsync();

            if (student == null)
                return false;

            var newMark = new Mark
            {
                Value = viewmodel.Value,
                Student = student,
                StudentId = student.Id
            };

            _context.Marks.Add(newMark);

            if (await _context.Marks.Where(x => x.StudentId == student.Id).AnyAsync())
            {
                double sum = (await _context.Marks.Where(x => x.StudentId == student.Id).SumAsync(x => x.Value)) + newMark.Value;
                double count = (await _context.Marks.Where(x => x.StudentId == student.Id).CountAsync()) + 1;

                student.MarksAverage = sum / count;
            } else
            {
                student.MarksAverage = newMark.Value;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<int> AllStudentNumberAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> AllStatisticsNumberAsync()
        {
            return await _context.Marks.GroupBy(x => x.StudentId).CountAsync();
        }
    }
}

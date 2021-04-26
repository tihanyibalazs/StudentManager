using StudentManager.Models;
using StudentManager.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManager.Services
{
    public interface IStudentManagerService
    {
        Task<bool> AddNewStudentAsync(CreateStudentViewModel viewmodel);
        Task<int> AllStatisticsNumberAsync();
        Task<int> AllStudentNumberAsync();
        Task<bool> AddNewMarkAsync(CreateMarkViewModel viewmodel);
        Task<IEnumerable<Student>> PagedStudentsAsync(int pageNumber, int studentsOnOnePage);
        Task<IEnumerable<StudentStatisticViewModel>> PagedStudentStatisticsAsync(int pageNumber, int studentsOnOnePage);
    }
}
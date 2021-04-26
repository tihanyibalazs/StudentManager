using StudentManager.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.ViewModels
{
    public class CreateStudentViewModel
    {
        [DisplayName("Student Name: ")]
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(40, ErrorMessage = "Student name length is over 40 character")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DisplayName("Phone number: ")]
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("Birth date: ")]
        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        [StudentManagerDateAttribute]
        public DateTime BirthDate { get; set; }

        [DisplayName("Class: ")]
        [Required(ErrorMessage = "Class is required")]
        [Range(1,12, ErrorMessage = "Class has to be in range of 1 to 12")]
        public int ClassYear { get; set; }
    }
}

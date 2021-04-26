using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassYear { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public double MarksAverage { get; set; }

        public override bool Equals(object obj)
        {

            if (obj == null || !this.GetType().Equals(obj.GetType()))
                return false;

            var student = obj as Student;
            
            if (student.Name != Name)
                return false;

            if (student.ClassYear != ClassYear)
                return false;

            if (student.BirthDate != BirthDate)
                return false;

            if (student.PhoneNumber != PhoneNumber)
                return false;

            if (student.MarksAverage != MarksAverage)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return new { Id, Name, ClassYear, PhoneNumber, MarksAverage }.GetHashCode();
        }
    }
}

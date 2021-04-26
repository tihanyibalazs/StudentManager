using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.Models
{
    public class StudentManagerDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            
            if (d > DateTime.Now)
                return false;

            if (d < DateTime.Now.AddYears(-100))
                return false;

            return true;
        }
    }
}

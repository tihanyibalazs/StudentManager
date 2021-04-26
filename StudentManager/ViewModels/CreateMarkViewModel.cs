using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.ViewModels
{
    public class CreateMarkViewModel
    {
        [Required]
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "Value is required")]
        [Range(1,5, ErrorMessage = "Value has to be in range of 1 to 5")]
        public int Value { get; set; }
    }
}

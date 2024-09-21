using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public enum Gender 
    {
        [EnumMember(Value ="Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }
    public enum EmployeeType
    {
        FullTime = 1,
        PartTime = 2
    }
    public class Employee:ModelBase
    {
        
        [Required(ErrorMessage ="Name is required")]
        [MaxLength (25,ErrorMessage ="Max Name Length 25 characters")]
        [MinLength(5, ErrorMessage = "Max Name Length 5 characters")]
        public string Name { get; set; }
        [Range(18,60)]
        public int? Age { get; set; }
        [RegularExpression(@"^[A-Za-z\s]{2,50}$",
                            ErrorMessage = "Invalid address. Please ensure the address includes a city name ")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }
        public bool IsDelete { get; set; } //soft Delete
        public Gender Gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }
        
        public int? departmentId { get; set; }//fk
        public Department? department { get; set; }


    }
}

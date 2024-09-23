﻿using System;
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
        
        [Required]
        [MaxLength (25)]
        [MinLength(5)]
        public string Name { get; set; }
      
        public int? Age { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
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
        public string ImageName { get; set; }


    }
}

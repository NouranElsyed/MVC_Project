using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee>Employee)
        {
            Employee.Property(E => E.Salary)
                 .HasColumnType("decimal(18,2)");
            Employee.Property(E => E.Gender)
                .HasConversion
                (
                (Gender) => Gender.ToString(),
                (GenderAsString) => (Gender)Enum.Parse(typeof(Gender), GenderAsString,true)
                );
        }
    }
}

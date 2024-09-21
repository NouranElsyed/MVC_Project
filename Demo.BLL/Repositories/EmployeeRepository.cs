using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>  , IEmployeeRepository
    {


        public EmployeeRepository(MvcDbContext dbContext) : base(dbContext) { }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return _dbContext.Employees
                    .Where(e => e.Address.ToLower().Contains(address.ToLower()))
                    .Include(E=>E.department);
        }
    }
}

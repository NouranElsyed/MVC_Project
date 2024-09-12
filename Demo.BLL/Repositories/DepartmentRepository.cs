using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MvcDbContext _dbContext;
        public DepartmentRepository(MvcDbContext context)
        {
            _dbContext = context;
        }
        public int Add(Department department)
        {
            _dbContext.Add(department);
            return _dbContext.SaveChanges();
        }

        public int Delete(Department department)
        {
            _dbContext.Remove(department);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll() => _dbContext.Departments.ToList();


        public Department GetById(int id) => _dbContext.Departments.Find(id);
        //{
        //var department = from D in _dbContext.Departments.Local
        //                 where D.Id == id 
        //                 select D;
        // return department.FirstOrDefault();
        //}
        

        public int Update(Department department)
        {

            _dbContext.Update(department);
            return _dbContext.SaveChanges();
        }
    }
}

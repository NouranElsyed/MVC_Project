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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly MvcDbContext _dbContext;
        public GenericRepository(MvcDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task AddAsync(T item)
        {
          await  _dbContext.Set<T>().AddAsync(item);
        
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        
        }

        public async Task<IEnumerable<T>> GetAllAsync(){
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _dbContext.Employees.Include(E => E.department).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
            }

        }


        public async Task<T> GetByIdAsync(int id) =>await  _dbContext.FindAsync<T>(id);

    
        public void Update(T item)
        {
            _dbContext.Update(item);
           
        }

     
    }
}

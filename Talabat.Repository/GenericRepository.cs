using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()

        {
            if (typeof(T) == typeof(Product))
                return (IReadOnlyList<T>)await _dbcontext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            else
                return await _dbcontext.Set<T>().ToListAsync();
        }

      

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplySpec(specifications).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplySpec(specifications).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpec(ISpecifications<T> specifications)
        {
            return  SpecificationsEvaluator<T>.GetQuery(_dbcontext.Set<T>(), specifications);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplySpec(specifications).CountAsync();
        }

        public async Task Add(T item)
        {
            await _dbcontext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbcontext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbcontext.Set<T>().Update(item);
        }
    }
}

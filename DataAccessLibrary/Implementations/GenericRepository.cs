using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly vnrdntaiContext context;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(vnrdntaiContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(Guid id)
        {
            T entity = GetAsync(id).Result;
            if(entity == null)
            {
                throw new Exception("Entity does not exist!");
            }
            Delete(entity);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync(string sqlQuery)
        {
            return await dbSet.FromSqlRaw(sqlQuery).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool isTracked = true)
        {
            if (isTracked)
            {
                List<T> cachedDatas = await dbSet.ToListAsync();
                return cachedDatas;
            } 
            else
            {
                List<T> cachesDatas = await dbSet.AsNoTracking().ToListAsync();
                return cachesDatas;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] otherEntities)
        {
            IQueryable<T> entities = null;
            foreach(string other in otherEntities)
            {
                if(entities == null)
                {
                    entities = dbSet.Include(other);
                }
                else
                {
                    entities = entities.Include(other);
                }
            }
            return await entities.ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> GetAsync(params Guid[] id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}

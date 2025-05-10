﻿
using MexicanRestaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace MexicanRestaurant.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context {  get; set; }

        private DbSet<T> _dbSet {  get; set; }

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task AddAsync(T Entity)
        {
            await _dbSet.AddAsync(Entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T Entity=await _dbSet.FindAsync(id);
            _dbSet.Remove(Entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }
            foreach(string include in options.GetIncludes())
            {
                query = query.Include(include);
            }
            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string primaryKeyName = key?.Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        }

        public async Task UpdateAsync(T Entity)
        {
           
            _context.Update(Entity);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<T>> GetAllByIdAsync<Tkey>(Tkey id, string propertyName, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;

            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }

            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }

            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            // Filter by the specified property name and id
            query = query.Where(e => EF.Property<Tkey>(e, propertyName).Equals(id));

            return await query.ToListAsync();
        }


    }
}

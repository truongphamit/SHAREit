using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public class Repository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(TEntity o)
        {
            await _context.Set<TEntity>().AddAsync(o);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var itemToDelete = await _context.Set<TEntity>().FindAsync(id);
            if (itemToDelete != null)
            {
                _context.Remove(itemToDelete);
                await _context.SaveChangesAsync();
            }                 
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);   
        }

        public async Task<List<TEntity>> getAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task Update(int id, TEntity o)
        {
            var itemToUpdate = await _context.Set<TEntity>().FindAsync(id);
            if (itemToUpdate != null)
            {
                itemToUpdate = o;
                await _context.SaveChangesAsync();
            }            
        }

        public async Task<List<TEntity>> paginate(int perPage,int page)
        {
            return await _context.Set<TEntity>().Take(perPage)
                                 .Skip((page -1) * perPage)
                                 .ToListAsync();   
        }
    }
}
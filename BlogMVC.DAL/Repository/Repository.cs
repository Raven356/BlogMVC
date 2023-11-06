using BlogMVC.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BlogMVCContext _context;
        private bool disposedValue;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(BlogMVCContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public async Task<TEntity> GetById(int? id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<TEntity> GetById(string id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}

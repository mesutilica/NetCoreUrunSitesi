﻿using DAL.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        internal DatabaseContext _context; // içi boş context nesnesi tanımladık
        internal DbSet<T> _dbSet; // içi boş dbSet nesnesi tanımladık
        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public int Add(T entity)
        {
            _dbSet.Add(entity);
            return SaveChanges();
        }

        public async Task AddAsync(T entity) // asenkron metotlarda task in önüne async takısı eklenmelidir!
        {
            //await dbSet.AddAsync(entity); // asenkron metotları kullanırken await anahtar kelimesi kullanılır
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            //dbSet.Remove(entity);
            _context.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Include(expression);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
        }

        public IQueryable<T> GetAllInclude(string table)
        {
            return _dbSet.Include(table);
        }

        public IQueryable<T> GetAllInclude(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Include(expression);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}

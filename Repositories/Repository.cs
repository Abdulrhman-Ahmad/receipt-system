﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Core.Data;
using MVC_Core.Models;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MVC_Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class // Constrain, T must be a Class
    {
        #region Dependency Injection
        private readonly ApplicationDbContext _context;

        /*
         * This Constructor is Injected using ASP.NET Core Dependency Injection system (Built-in Feature in ASP.NET)
         * builder.Services.AddScooped(typeof(IRepository<T>, typeof(SchoolRepository))); => Register Injection of type generic
         */
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<T>> GetAll(string[]? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                foreach (var item in include)
                    query = query.Include(item);

            return await query.ToListAsync();
        }
        #endregion

        #region GetPage
        public async Task<IEnumerable<T>> GetPage(int pageIndex, int pageSize, string[]? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                foreach (var item in include)
                    query = query.Include(item);

            return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        #endregion

        #region GetById
        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        #endregion

        #region Find
        public async Task<T> Find(Expression<Func<T, bool>> match, string[]? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                foreach (var item in include)
                    query = query.Include(item);

            return await query.FirstOrDefaultAsync(match);
        }
        #endregion

        #region Add
        public async Task<bool> Add(T entity)
        {
            #region Null Exception
            if (entity == null)
            {
                return false;
            }
            #endregion

            #region Check if data Added Successfully
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            #endregion
        }
        #endregion

        #region Delete
        public async Task<bool> Delete(T entity)
        {
            #region Null Exception
            if (entity == null)
            {
                return false;
            }
            #endregion

            #region Check if data deleted successfully
            try
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            #endregion
        }
        #endregion

        #region Update
        public async Task<bool> Update(T entity)
        {
            #region Null Exception
            if (entity == null)
            {
                return false;
            }
            #endregion

            #region Check if data updated successfully
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            #endregion
        }
        #endregion

        #region Count
        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }
        #endregion

        #region GetListItems
        public async Task<IEnumerable<SelectListItem>> GetListItems(Expression<Func<T, SelectListItem>> match, string[]? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                foreach (var item in include)
                    query = query.Include(item);

            return await query.Select(match).Distinct().ToListAsync();      // Bug. I wrote distinct to aviod repeated results 
        }
        #endregion
    }
}
﻿using BachMaiCR.DBMapping.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace BachMaiCR.DataAccess
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected BACHMAICRContext DbContext;
        protected System.Data.Entity.DbSet<T> DbSet;

        public EFRepository(System.Data.Entity.DbContext dbContext)
        {
            if (dbContext == null)
                throw new Exception("EFRepository::initialize::dbContext::Canot null");
            this.DbContext = (BACHMAICRContext)dbContext;
            this.DbSet = this.DbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return (IQueryable<T>)this.DbSet;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Where(predicate);
        }

        public T GetById(int id)
        {
            return this.DbSet.SqlQuery(string.Format("SELECT * FROM {0} WHERE {1} = @Id", (object)this.DbContext.GetTableName<T>(), (object)this.DbContext.GetTableKeyName<T>()), (object)new SqlParameter("Id", (object)id)).FirstOrDefault<T>();
        }

        public void Delete(T entity)
        {
            this.DbContext.Entry<T>(entity).State = EntityState.Deleted;
            this.Save();
        }

        public void Add(T entity)
        {
            this.DbContext.Entry<T>(entity).State = EntityState.Added;
            this.Save();
        }

        public void Update(T entity)
        {
            this.DbContext.Entry<T>(entity).State = EntityState.Modified;
            this.Save();
        }

        public int Save()
        {
            return this.DbContext.SaveChanges();
        }
    }
}
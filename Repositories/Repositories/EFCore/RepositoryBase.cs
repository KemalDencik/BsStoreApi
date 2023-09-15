using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    //Repo için kendi interfacemizi kullandık. Where şartı sadece class tanımları geçerli olsun diye
    //varlıklar için genel bir CRUD (Create, Read, Update, Delete) işlemlerini sağlar.
    //veritabanı işlemlerini merkezi bir şekilde yönetmek kolaylaşır.
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        //Varlık Oluşturma
        public void Create(T entity) => _context.Set<T>().Add(entity);

        //Varlık Silme
        public void Delete(T entity) => _context?.Set<T>().Remove(entity);

        //trackChanges true ise değişiklikler izlenir değilse izlenmez
        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().AsNoTracking() :
            _context.Set<T>();

        //trackChanges true ise değişiklikler izlenir değilse izlenmez
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
            bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().Where(expression).AsNoTracking() :
            _context.Set<T>().Where(expression);

        //Varlık Güncelleme
        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
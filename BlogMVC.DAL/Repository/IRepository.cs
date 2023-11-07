using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.DAL.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(int? id);

        IEnumerable<TEntity> GetAll();

        Task<TEntity> GetById(int? id);

        Task<TEntity> GetById(string id);
    }
}

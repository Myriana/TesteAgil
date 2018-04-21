using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public abstract class RepositorioBase<TEntity> : IDisposable
      where TEntity : class
    {
        protected Contexto.Contexto db = new Contexto.Contexto();
        public IQueryable<TEntity> GetAll()
        {
            return db.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return GetAll().Where(predicate).AsQueryable();
        }

        public TEntity Find(params object[] key)
        {
            return db.Set<TEntity>().Find(key);
        }

        public void Atualizar(TEntity obj)
        {
            db.Entry(obj).State = EntityState.Modified;
        }

        public void Commit()
        {
            db.SaveChanges();
        }

        public void Attach(TEntity obj)
        {
            db.Set<TEntity>().Attach(obj);
        }

        public void Adicionar(TEntity obj)
        {
            db.Set<TEntity>().Add(obj);
        }

        public void Deletar(TEntity obj)
        {
            db.Set<TEntity>().Remove(obj);
        }

        public void ExcluirVarios(Func<TEntity, bool> predicate)
        {
            db.Set<TEntity>()
                .Where(predicate).ToList()
                .ForEach(del => db.Set<TEntity>().Remove(del));
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}

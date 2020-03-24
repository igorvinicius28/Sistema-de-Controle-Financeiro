using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Enumeradores;
using FCMoney.Repositorio.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Repositorio.Repositorios.Util
{
    public class BaseRepositorio<T> : IBaseRepositorio<T> where T : class
    {
        protected readonly ApplicationDbContext Contexto;

        public BaseRepositorio(ApplicationDbContext contexto)
        {
            Contexto = contexto;
        }

        public BaseRepositorio()
        {
            Contexto = new ApplicationDbContext();
        }

        public void Cadastrar(T obj)
        {
            Contexto.Set<T>().Add(obj);
            Contexto.SaveChanges();
        }

        public T ConsultarPorId(int id)
        {
            return Contexto.Set<T>().Find(id);
        }

        public IEnumerable<T> ConsulteLista()
        {
            return Contexto.Set<T>().Select(x => x);
        }

        public void Atualizar(T obj)
        {
            Contexto.Entry(obj).State = EntityState.Modified;
            Contexto.SaveChanges();
        }

        public void Excluir(T obj)
        {
            Contexto.Set<T>().Remove(obj);
            Contexto.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

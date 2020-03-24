using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Dominio.Contratos
{
    public interface IBaseRepositorio<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Cadastrar(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity ConsultarPorId(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> ConsulteLista();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Atualizar(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Excluir(TEntity obj);

        void Dispose();
    }
}

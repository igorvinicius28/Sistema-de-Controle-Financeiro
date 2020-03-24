using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades;
using FCMoney.Repositorio.Contexto;
using FCMoney.Repositorio.Repositorios.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Repositorio.Repositorios.RepositorioUsuario
{
    public class RepositorioDadosPessoais : BaseRepositorio<DadosPessoais>, IRepositorioDadosPessoais
    {
        private ApplicationDbContext Contexto;

        public RepositorioDadosPessoais(ApplicationDbContext fcMoney) : base(fcMoney)
        {

        }

        public RepositorioDadosPessoais()
        {
            Contexto = new ApplicationDbContext();
        }

        public void Atualizar(DadosPessoais dadosPessoais)
        {
            var dados =  Contexto.Set<DadosPessoais>().FirstOrDefault(x => x.UsuarioId == dadosPessoais.UsuarioId);
            dados.Nome = dadosPessoais.Nome;
            dados.Sobrenome = dadosPessoais.Sobrenome;
            dados.CPF = dadosPessoais.CPF;
            dados.Telefone = dadosPessoais.Telefone;
            dados.UsuarioId = dadosPessoais.UsuarioId;

            Contexto.SaveChanges();
        }

        public DadosPessoais ConsultePorId(string id)
        {
            return Contexto.Set<DadosPessoais>().FirstOrDefault(x => x.UsuarioId == id);
        }

        public void Cadastrar(DadosPessoais dadosPessoais)
        {
            Contexto.Set<DadosPessoais>().Add(dadosPessoais);
            Contexto.SaveChanges(); 
        }
    }
}

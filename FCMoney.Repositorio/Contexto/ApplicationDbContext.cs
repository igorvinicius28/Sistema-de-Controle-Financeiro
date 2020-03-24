using FCMoney.Dominio.Autenticacao;
using FCMoney.Dominio.Entidades;
using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Dominio.Entidades.Movimentacoes;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FCMoney.Repositorio.Contexto
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Usuarios")
        {

        }

        public DbSet<Client> Client { get; set; }

        public DbSet<Claims> Claims { get; set; }

        public DbSet<Movimentacao> Movimentacao { get; set; }

        public DbSet<Contas> Contas { get; set; }

        public DbSet<DadosPessoais> DadosPessoais { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();  
        }
        */
    }
}

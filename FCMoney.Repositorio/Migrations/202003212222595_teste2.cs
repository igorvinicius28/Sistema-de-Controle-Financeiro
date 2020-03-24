namespace FCMoney.Repositorio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teste2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contas", "Valor", c => c.Double());
            AlterColumn("dbo.Movimentacaos", "Valor", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movimentacaos", "Valor", c => c.Double(nullable: false));
            AlterColumn("dbo.Contas", "Valor", c => c.Double(nullable: false));
        }
    }
}

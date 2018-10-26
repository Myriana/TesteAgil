namespace Uvv.TesteAgil.Dados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TADBversao2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teste", "CasoTeste_CasoTesteId", "dbo.CasoTeste");
            DropIndex("dbo.Teste", new[] { "CasoTeste_CasoTesteId" });
            RenameColumn(table: "dbo.Teste", name: "CasoTeste_CasoTesteId", newName: "CasoTesteId");
            AlterColumn("dbo.Teste", "Situacao", c => c.Int(nullable: false));
            AlterColumn("dbo.Teste", "CasoTesteId", c => c.Int(nullable: false));
            AlterColumn("dbo.TipoErro", "Gravidade", c => c.Int(nullable: false));
            CreateIndex("dbo.Teste", "CasoTesteId");
            AddForeignKey("dbo.Teste", "CasoTesteId", "dbo.CasoTeste", "CasoTesteId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teste", "CasoTesteId", "dbo.CasoTeste");
            DropIndex("dbo.Teste", new[] { "CasoTesteId" });
            AlterColumn("dbo.TipoErro", "Gravidade", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.Teste", "CasoTesteId", c => c.Int());
            AlterColumn("dbo.Teste", "Situacao", c => c.String(nullable: false, maxLength: 50, unicode: false));
            RenameColumn(table: "dbo.Teste", name: "CasoTesteId", newName: "CasoTeste_CasoTesteId");
            CreateIndex("dbo.Teste", "CasoTeste_CasoTesteId");
            AddForeignKey("dbo.Teste", "CasoTeste_CasoTesteId", "dbo.CasoTeste", "CasoTesteId");
        }
    }
}

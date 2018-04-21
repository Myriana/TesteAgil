namespace Uvv.TesteAgil.Dados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracaoDois : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlanoTeste", "Sprint_SprintId", "dbo.Sprint");
            DropIndex("dbo.PlanoTeste", new[] { "Sprint_SprintId" });
            RenameColumn(table: "dbo.PlanoTeste", name: "Sprint_SprintId", newName: "SprintId");
            AlterColumn("dbo.PlanoTeste", "SprintId", c => c.Int(nullable: false));
            CreateIndex("dbo.PlanoTeste", "SprintId");
            AddForeignKey("dbo.PlanoTeste", "SprintId", "dbo.Sprint", "SprintId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlanoTeste", "SprintId", "dbo.Sprint");
            DropIndex("dbo.PlanoTeste", new[] { "SprintId" });
            AlterColumn("dbo.PlanoTeste", "SprintId", c => c.Int());
            RenameColumn(table: "dbo.PlanoTeste", name: "SprintId", newName: "Sprint_SprintId");
            CreateIndex("dbo.PlanoTeste", "Sprint_SprintId");
            AddForeignKey("dbo.PlanoTeste", "Sprint_SprintId", "dbo.Sprint", "SprintId");
        }
    }
}

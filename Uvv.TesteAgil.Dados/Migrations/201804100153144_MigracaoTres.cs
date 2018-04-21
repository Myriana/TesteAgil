namespace Uvv.TesteAgil.Dados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracaoTres : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CasoTeste", "CenarioTeste_CenarioTesteId", "dbo.CenarioTeste");
            DropForeignKey("dbo.CenarioTeste", "Funcionalidade_FuncionalidadeId", "dbo.Funcionalidade");
            DropForeignKey("dbo.CenarioTeste", "PlanoTeste_PlanoTesteId", "dbo.PlanoTeste");
            DropIndex("dbo.CasoTeste", new[] { "CenarioTeste_CenarioTesteId" });
            DropIndex("dbo.CenarioTeste", new[] { "Funcionalidade_FuncionalidadeId" });
            DropIndex("dbo.CenarioTeste", new[] { "PlanoTeste_PlanoTesteId" });
            RenameColumn(table: "dbo.CasoTeste", name: "CenarioTeste_CenarioTesteId", newName: "CenarioTesteId");
            RenameColumn(table: "dbo.CenarioTeste", name: "Funcionalidade_FuncionalidadeId", newName: "FuncionalidadeId");
            RenameColumn(table: "dbo.CenarioTeste", name: "PlanoTeste_PlanoTesteId", newName: "PlanoTesteId");
            RenameColumn(table: "dbo.CenarioTeste", name: "ScriptTeste_ScriptTesteId", newName: "ScriptTesteId");
            RenameIndex(table: "dbo.CenarioTeste", name: "IX_ScriptTeste_ScriptTesteId", newName: "IX_ScriptTesteId");
            AlterColumn("dbo.CasoTeste", "Descricao", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.CasoTeste", "Categoria", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.CasoTeste", "Entrada", c => c.String(nullable: false, maxLength: 300, unicode: false));
            AlterColumn("dbo.CasoTeste", "RespostaEsperada", c => c.String(nullable: false, maxLength: 300, unicode: false));
            AlterColumn("dbo.CasoTeste", "CenarioTesteId", c => c.Int(nullable: false));
            AlterColumn("dbo.CenarioTeste", "Situacao", c => c.Int(nullable: false));
            AlterColumn("dbo.CenarioTeste", "FuncionalidadeId", c => c.Int(nullable: false));
            AlterColumn("dbo.CenarioTeste", "PlanoTesteId", c => c.Int(nullable: false));
            CreateIndex("dbo.CasoTeste", "CenarioTesteId");
            CreateIndex("dbo.CenarioTeste", "FuncionalidadeId");
            CreateIndex("dbo.CenarioTeste", "PlanoTesteId");
            AddForeignKey("dbo.CasoTeste", "CenarioTesteId", "dbo.CenarioTeste", "CenarioTesteId", cascadeDelete: true);
            AddForeignKey("dbo.CenarioTeste", "FuncionalidadeId", "dbo.Funcionalidade", "FuncionalidadeId", cascadeDelete: true);
            AddForeignKey("dbo.CenarioTeste", "PlanoTesteId", "dbo.PlanoTeste", "PlanoTesteId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CenarioTeste", "PlanoTesteId", "dbo.PlanoTeste");
            DropForeignKey("dbo.CenarioTeste", "FuncionalidadeId", "dbo.Funcionalidade");
            DropForeignKey("dbo.CasoTeste", "CenarioTesteId", "dbo.CenarioTeste");
            DropIndex("dbo.CenarioTeste", new[] { "PlanoTesteId" });
            DropIndex("dbo.CenarioTeste", new[] { "FuncionalidadeId" });
            DropIndex("dbo.CasoTeste", new[] { "CenarioTesteId" });
            AlterColumn("dbo.CenarioTeste", "PlanoTesteId", c => c.Int());
            AlterColumn("dbo.CenarioTeste", "FuncionalidadeId", c => c.Int());
            AlterColumn("dbo.CenarioTeste", "Situacao", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.CasoTeste", "CenarioTesteId", c => c.Int());
            AlterColumn("dbo.CasoTeste", "RespostaEsperada", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.CasoTeste", "Entrada", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.CasoTeste", "Categoria", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.CasoTeste", "Descricao", c => c.String(nullable: false, maxLength: 50, unicode: false));
            RenameIndex(table: "dbo.CenarioTeste", name: "IX_ScriptTesteId", newName: "IX_ScriptTeste_ScriptTesteId");
            RenameColumn(table: "dbo.CenarioTeste", name: "ScriptTesteId", newName: "ScriptTeste_ScriptTesteId");
            RenameColumn(table: "dbo.CenarioTeste", name: "PlanoTesteId", newName: "PlanoTeste_PlanoTesteId");
            RenameColumn(table: "dbo.CenarioTeste", name: "FuncionalidadeId", newName: "Funcionalidade_FuncionalidadeId");
            RenameColumn(table: "dbo.CasoTeste", name: "CenarioTesteId", newName: "CenarioTeste_CenarioTesteId");
            CreateIndex("dbo.CenarioTeste", "PlanoTeste_PlanoTesteId");
            CreateIndex("dbo.CenarioTeste", "Funcionalidade_FuncionalidadeId");
            CreateIndex("dbo.CasoTeste", "CenarioTeste_CenarioTesteId");
            AddForeignKey("dbo.CenarioTeste", "PlanoTeste_PlanoTesteId", "dbo.PlanoTeste", "PlanoTesteId");
            AddForeignKey("dbo.CenarioTeste", "Funcionalidade_FuncionalidadeId", "dbo.Funcionalidade", "FuncionalidadeId");
            AddForeignKey("dbo.CasoTeste", "CenarioTeste_CenarioTesteId", "dbo.CenarioTeste", "CenarioTesteId");
        }
    }
}

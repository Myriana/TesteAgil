namespace Uvv.TesteAgil.Dados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TADBversao1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CasoTeste",
                c => new
                    {
                        CasoTesteId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Categoria = c.String(nullable: false, maxLength: 100, unicode: false),
                        Entrada = c.String(nullable: false, maxLength: 300, unicode: false),
                        RespostaEsperada = c.String(nullable: false, maxLength: 300, unicode: false),
                        CenarioTesteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CasoTesteId)
                .ForeignKey("dbo.CenarioTeste", t => t.CenarioTesteId, cascadeDelete: true)
                .Index(t => t.CenarioTesteId);
            
            CreateTable(
                "dbo.CenarioTeste",
                c => new
                    {
                        CenarioTesteId = c.Int(nullable: false, identity: true),
                        Situacao = c.Int(nullable: false),
                        DataAtualizacao = c.DateTime(nullable: false),
                        FuncionalidadeId = c.Int(nullable: false),
                        PlanoTesteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CenarioTesteId)
                .ForeignKey("dbo.Funcionalidade", t => t.FuncionalidadeId, cascadeDelete: true)
                .ForeignKey("dbo.PlanoTeste", t => t.PlanoTesteId, cascadeDelete: true)
                .Index(t => t.FuncionalidadeId)
                .Index(t => t.PlanoTesteId);
            
            CreateTable(
                "dbo.Funcionalidade",
                c => new
                    {
                        FuncionalidadeId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                        Descricao = c.String(maxLength: 200, unicode: false),
                        Prioridade = c.Int(nullable: false),
                        Pontos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Testada = c.Boolean(nullable: false),
                        EstoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FuncionalidadeId);
            
            CreateTable(
                "dbo.PlanoTeste",
                c => new
                    {
                        PlanoTesteId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataCriacao = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        SprintId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlanoTesteId)
                .ForeignKey("dbo.Sprint", t => t.SprintId, cascadeDelete: true)
                .Index(t => t.SprintId);
            
            CreateTable(
                "dbo.Sprint",
                c => new
                    {
                        SprintId = c.Int(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        Observacao = c.String(maxLength: 300, unicode: false),
                        Projeto_ProjetoId = c.Int(),
                    })
                .PrimaryKey(t => t.SprintId)
                .ForeignKey("dbo.Projeto", t => t.Projeto_ProjetoId)
                .Index(t => t.Projeto_ProjetoId);
            
            CreateTable(
                "dbo.Estoria",
                c => new
                    {
                        EstoriaId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                        Descricao = c.String(maxLength: 200, unicode: false),
                        Pontos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Prioridade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EstoriaId);
            
            CreateTable(
                "dbo.Projeto",
                c => new
                    {
                        ProjetoId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjetoId);
            
            CreateTable(
                "dbo.Membro",
                c => new
                    {
                        MembroId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 200, unicode: false),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        Senha = c.String(nullable: false, maxLength: 100, unicode: false),
                        CPF = c.String(maxLength: 11, unicode: false),
                    })
                .PrimaryKey(t => t.MembroId);
            
            CreateTable(
                "dbo.Teste",
                c => new
                    {
                        TesteId = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Situacao = c.String(nullable: false, maxLength: 50, unicode: false),
                        DesenvolvedorId = c.Int(nullable: false),
                        TesterId = c.Int(nullable: false),
                        CasoTeste_CasoTesteId = c.Int(),
                        Desenvolvedor_MembroId = c.Int(),
                        Tester_MembroId = c.Int(),
                        Membro_MembroId = c.Int(),
                        Membro_MembroId1 = c.Int(),
                    })
                .PrimaryKey(t => t.TesteId)
                .ForeignKey("dbo.CasoTeste", t => t.CasoTeste_CasoTesteId)
                .ForeignKey("dbo.Membro", t => t.Desenvolvedor_MembroId)
                .ForeignKey("dbo.Membro", t => t.Tester_MembroId)
                .ForeignKey("dbo.Membro", t => t.Membro_MembroId)
                .ForeignKey("dbo.Membro", t => t.Membro_MembroId1)
                .Index(t => t.CasoTeste_CasoTesteId)
                .Index(t => t.Desenvolvedor_MembroId)
                .Index(t => t.Tester_MembroId)
                .Index(t => t.Membro_MembroId)
                .Index(t => t.Membro_MembroId1);
            
            CreateTable(
                "dbo.TipoErro",
                c => new
                    {
                        TipoErroId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        Gravidade = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.TipoErroId);
            
            CreateTable(
                "dbo.Passo",
                c => new
                    {
                        PassoId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        Numero = c.Int(nullable: false),
                        CasoTesteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PassoId)
                .ForeignKey("dbo.CasoTeste", t => t.CasoTesteId, cascadeDelete: true)
                .Index(t => t.CasoTesteId);
            
            CreateTable(
                "dbo.PlanoTesteFuncionalidade",
                c => new
                    {
                        PlanoTeste_PlanoTesteId = c.Int(nullable: false),
                        Funcionalidade_FuncionalidadeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PlanoTeste_PlanoTesteId, t.Funcionalidade_FuncionalidadeId })
                .ForeignKey("dbo.PlanoTeste", t => t.PlanoTeste_PlanoTesteId, cascadeDelete: true)
                .ForeignKey("dbo.Funcionalidade", t => t.Funcionalidade_FuncionalidadeId, cascadeDelete: true)
                .Index(t => t.PlanoTeste_PlanoTesteId)
                .Index(t => t.Funcionalidade_FuncionalidadeId);
            
            CreateTable(
                "dbo.EstoriaSprint",
                c => new
                    {
                        Estoria_EstoriaId = c.Int(nullable: false),
                        Sprint_SprintId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Estoria_EstoriaId, t.Sprint_SprintId })
                .ForeignKey("dbo.Estoria", t => t.Estoria_EstoriaId, cascadeDelete: true)
                .ForeignKey("dbo.Sprint", t => t.Sprint_SprintId, cascadeDelete: true)
                .Index(t => t.Estoria_EstoriaId)
                .Index(t => t.Sprint_SprintId);
            
            CreateTable(
                "dbo.MembroProjeto",
                c => new
                    {
                        Membro_MembroId = c.Int(nullable: false),
                        Projeto_ProjetoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Membro_MembroId, t.Projeto_ProjetoId })
                .ForeignKey("dbo.Membro", t => t.Membro_MembroId, cascadeDelete: true)
                .ForeignKey("dbo.Projeto", t => t.Projeto_ProjetoId, cascadeDelete: true)
                .Index(t => t.Membro_MembroId)
                .Index(t => t.Projeto_ProjetoId);
            
            CreateTable(
                "dbo.TipoErroTeste",
                c => new
                    {
                        TipoErro_TipoErroId = c.Int(nullable: false),
                        Teste_TesteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TipoErro_TipoErroId, t.Teste_TesteId })
                .ForeignKey("dbo.TipoErro", t => t.TipoErro_TipoErroId, cascadeDelete: true)
                .ForeignKey("dbo.Teste", t => t.Teste_TesteId, cascadeDelete: true)
                .Index(t => t.TipoErro_TipoErroId)
                .Index(t => t.Teste_TesteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Passo", "CasoTesteId", "dbo.CasoTeste");
            DropForeignKey("dbo.PlanoTeste", "SprintId", "dbo.Sprint");
            DropForeignKey("dbo.Sprint", "Projeto_ProjetoId", "dbo.Projeto");
            DropForeignKey("dbo.Teste", "Membro_MembroId1", "dbo.Membro");
            DropForeignKey("dbo.Teste", "Membro_MembroId", "dbo.Membro");
            DropForeignKey("dbo.Teste", "Tester_MembroId", "dbo.Membro");
            DropForeignKey("dbo.TipoErroTeste", "Teste_TesteId", "dbo.Teste");
            DropForeignKey("dbo.TipoErroTeste", "TipoErro_TipoErroId", "dbo.TipoErro");
            DropForeignKey("dbo.Teste", "Desenvolvedor_MembroId", "dbo.Membro");
            DropForeignKey("dbo.Teste", "CasoTeste_CasoTesteId", "dbo.CasoTeste");
            DropForeignKey("dbo.MembroProjeto", "Projeto_ProjetoId", "dbo.Projeto");
            DropForeignKey("dbo.MembroProjeto", "Membro_MembroId", "dbo.Membro");
            DropForeignKey("dbo.EstoriaSprint", "Sprint_SprintId", "dbo.Sprint");
            DropForeignKey("dbo.EstoriaSprint", "Estoria_EstoriaId", "dbo.Estoria");
            DropForeignKey("dbo.PlanoTesteFuncionalidade", "Funcionalidade_FuncionalidadeId", "dbo.Funcionalidade");
            DropForeignKey("dbo.PlanoTesteFuncionalidade", "PlanoTeste_PlanoTesteId", "dbo.PlanoTeste");
            DropForeignKey("dbo.CenarioTeste", "PlanoTesteId", "dbo.PlanoTeste");
            DropForeignKey("dbo.CenarioTeste", "FuncionalidadeId", "dbo.Funcionalidade");
            DropForeignKey("dbo.CasoTeste", "CenarioTesteId", "dbo.CenarioTeste");
            DropIndex("dbo.TipoErroTeste", new[] { "Teste_TesteId" });
            DropIndex("dbo.TipoErroTeste", new[] { "TipoErro_TipoErroId" });
            DropIndex("dbo.MembroProjeto", new[] { "Projeto_ProjetoId" });
            DropIndex("dbo.MembroProjeto", new[] { "Membro_MembroId" });
            DropIndex("dbo.EstoriaSprint", new[] { "Sprint_SprintId" });
            DropIndex("dbo.EstoriaSprint", new[] { "Estoria_EstoriaId" });
            DropIndex("dbo.PlanoTesteFuncionalidade", new[] { "Funcionalidade_FuncionalidadeId" });
            DropIndex("dbo.PlanoTesteFuncionalidade", new[] { "PlanoTeste_PlanoTesteId" });
            DropIndex("dbo.Passo", new[] { "CasoTesteId" });
            DropIndex("dbo.Teste", new[] { "Membro_MembroId1" });
            DropIndex("dbo.Teste", new[] { "Membro_MembroId" });
            DropIndex("dbo.Teste", new[] { "Tester_MembroId" });
            DropIndex("dbo.Teste", new[] { "Desenvolvedor_MembroId" });
            DropIndex("dbo.Teste", new[] { "CasoTeste_CasoTesteId" });
            DropIndex("dbo.Sprint", new[] { "Projeto_ProjetoId" });
            DropIndex("dbo.PlanoTeste", new[] { "SprintId" });
            DropIndex("dbo.CenarioTeste", new[] { "PlanoTesteId" });
            DropIndex("dbo.CenarioTeste", new[] { "FuncionalidadeId" });
            DropIndex("dbo.CasoTeste", new[] { "CenarioTesteId" });
            DropTable("dbo.TipoErroTeste");
            DropTable("dbo.MembroProjeto");
            DropTable("dbo.EstoriaSprint");
            DropTable("dbo.PlanoTesteFuncionalidade");
            DropTable("dbo.Passo");
            DropTable("dbo.TipoErro");
            DropTable("dbo.Teste");
            DropTable("dbo.Membro");
            DropTable("dbo.Projeto");
            DropTable("dbo.Estoria");
            DropTable("dbo.Sprint");
            DropTable("dbo.PlanoTeste");
            DropTable("dbo.Funcionalidade");
            DropTable("dbo.CenarioTeste");
            DropTable("dbo.CasoTeste");
        }
    }
}

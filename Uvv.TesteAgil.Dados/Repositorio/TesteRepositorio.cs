using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class TesteRepositorio : RepositorioBase<Teste>
    {
        public Teste ObterTestePorId(long id)
        {
            return db.Teste.FirstOrDefault(x => x.TesteId == id);
        }

        public List<Teste> ObterTestesPorCaso(long idCaso)
        {
            return db.Teste.Where(x => x.CasoTeste.CasoTesteId == idCaso).ToList();
        }

        public List<Teste> ObterTestesPorPlano(int idPlano)
        {
            var cenarios = db.PlanoTeste.FirstOrDefault(p => p.PlanoTesteId == idPlano).Cenarios;
            List<Teste> testes = new List<Teste>();
            if (cenarios == null) return null;
            foreach (var cenario in cenarios)
            {
                if (cenario.Casos != null)
                {
                    foreach (var caso in cenario.Casos)
                    {
                        if (caso.Testes != null)
                        {
                            foreach (var teste in caso.Testes)
                            {
                                testes.Add(teste);
                            }
                        }
                    }
                }
            }
            return testes;
        }
        public void RemoveTipoErro(int testeId, int tipoErroId)
        {
            var teste = db.Teste.Include("Erros").SingleOrDefault(t => t.TesteId == testeId);
            var tipoErro = db.TipoErro.Include("Testes").SingleOrDefault(t => t.TipoErroId == tipoErroId);
            teste.Erros.Remove(tipoErro);
            db.SaveChanges();
        }

        public List<Teste> ObterTestesPorDesenvolvedor(int idDesenvolvedor)
        {
            return db.Teste.Where(t => t.DesenvolvedorId == idDesenvolvedor)?.ToList();
        }

        public List<Teste> ObterTestesPorDesenvolvedor(int idProjeto, int idDesenvolvedor)
        {
            List<Teste> retorno = new List<Teste>();
            var sprints = db.Projeto.FirstOrDefault(p => p.ProjetoId == idProjeto).Sprints;
            if (sprints == null) return null;
            foreach (var sprint in sprints)
            {
                var plano = db.PlanoTeste.FirstOrDefault(p => p.SprintId == sprint.SprintId);
                if (plano == null) continue;
                var cenarios = plano.Cenarios;
                if (cenarios == null) continue;
                foreach (var cenario in cenarios)
                {
                    var casos = cenario.Casos;
                    if (casos == null) continue;
                    foreach (var caso in casos)
                    {
                        var testes = caso.Testes;
                        if (testes == null) continue;
                        foreach (var teste in testes)
                        {
                            retorno.Add(teste);
                        }
                    }
                }
            }
            return retorno;
        }

        public List<Teste> ObterTestesPorDesenvolvedor(int idProjeto, int idSprint, int idDesenvolvedor)
        {
            List<Teste> retorno = new List<Teste>();
            var sprint = db.Projeto.FirstOrDefault(p => p.ProjetoId == idProjeto).Sprints?.FirstOrDefault(s => s.SprintId == idSprint);
            if (sprint == null) return null;
            var plano = db.PlanoTeste.FirstOrDefault(p => p.SprintId == sprint.SprintId);
            if (plano == null) return null;
            var cenarios = plano.Cenarios;
            if (cenarios == null) return null;
            foreach (var cenario in cenarios)
            {
                var casos = cenario.Casos;
                if (casos == null) continue;
                foreach (var caso in casos)
                {
                    var testes = caso.Testes?.Where(d => d.DesenvolvedorId == idDesenvolvedor)?.ToList();
                    if (testes == null) continue;
                    foreach (var teste in testes)
                    {
                        retorno.Add(teste);
                    }
                }
            }

            return retorno;
        }

        public List<Teste> ObterTestesPorProjeto(int idProjeto)
        {
            List<Teste> retorno = new List<Teste>();
            var sprints = db.Projeto.FirstOrDefault(p => p.ProjetoId == idProjeto).Sprints;
            if (sprints == null) return null;
            foreach (var sprint in sprints)
            {
                var plano = db.PlanoTeste.FirstOrDefault(p => p.SprintId == sprint.SprintId);
                if (plano == null) continue;
                var cenarios = plano.Cenarios;
                if (cenarios == null) continue;
                foreach (var cenario in cenarios)
                {
                    var casos = cenario.Casos;
                    if (casos == null) continue;
                    foreach (var caso in casos)
                    {
                        var testes = caso.Testes;
                        if (testes == null) continue;
                        foreach (var teste in testes)
                        {
                            retorno.Add(teste);
                        }
                    }
                }
            }
            return retorno;
        }

        public List<Teste> ObterTestesPorProjeto(int idProjeto, int idSprint)
        {
            List<Teste> retorno = new List<Teste>();
            var sprint = db.Projeto.FirstOrDefault(p => p.ProjetoId == idProjeto).Sprints?.FirstOrDefault(s => s.SprintId == idSprint);
            var plano = db.PlanoTeste.FirstOrDefault(p => p.SprintId == sprint.SprintId);
            if (plano == null) return null; ;
            var cenarios = plano.Cenarios;
            if (cenarios == null) return null; ;
            foreach (var cenario in cenarios)
            {
                var casos = cenario.Casos;
                if (casos == null) continue;
                foreach (var caso in casos)
                {
                    var testes = caso.Testes;
                    if (testes == null) continue;
                    foreach (var teste in testes)
                    {
                        retorno.Add(teste);
                    }
                }
            }

            return retorno;
        }
    }
}

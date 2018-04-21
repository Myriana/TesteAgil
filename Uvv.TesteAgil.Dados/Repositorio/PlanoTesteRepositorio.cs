using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class PlanoTesteRepositorio : RepositorioBase<PlanoTeste>
    {
        public PlanoTeste ObterPlanoTestePorId(long id)
        {
            return db.PlanoTeste.FirstOrDefault(x => x.PlanoTesteId == id);
        }

        public PlanoTeste ObterPlanoTestePorSprint(long idSprint)
        {
            var planoTeste = db.PlanoTeste.FirstOrDefault(x => x.Sprint.SprintId == idSprint);
            return planoTeste;
        }

        public int ObterQuantidadePlanoTestePorProjeto(long idProjeto)
        {
            var projeto = db.Projeto.FirstOrDefault(x => x.ProjetoId == idProjeto);

            if (projeto == null)
                return 0;
            int quantidade = 0;
            foreach (var sprint in projeto.Sprints)
            {
                var plano = db.PlanoTeste.FirstOrDefault(x => x.Sprint.SprintId == sprint.SprintId);
                if (plano != null)
                    quantidade++;
            }
            return quantidade;
        }

        public bool ExistePlanoTeste(string descricao)
        {
            var plano = db.PlanoTeste.FirstOrDefault(x => x.Descricao.ToUpper() == descricao.ToUpper());
            if (plano == null) return false;
            return true;
        }

        public bool AdicionarPlano(PlanoTeste plano, Sprint sprint, List<Funcionalidade> funcionalidades)
        {
            
            db.PlanoTeste.Attach(plano);
            plano.Sprint = sprint;

            plano.Funcionalidades = new List<Funcionalidade>();
            foreach (var func in funcionalidades)
            {
                func.Planos = new List<PlanoTeste>();
                func.Planos.Add(plano);
                db.Funcionalidade.Add(func);

                plano.Funcionalidades.Add(func);
            }
            db.PlanoTeste.Add(plano);
            db.SaveChanges();
            return true;
        }
    }
}

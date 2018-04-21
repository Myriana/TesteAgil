using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class EstoriaRepositorio : RepositorioBase<Estoria>
    {
        public List<Estoria> ObterEstoriasPorSprint(decimal idSprint)
        {
            var sprint = db.Sprint.FirstOrDefault(x => x.SprintId == idSprint);
            if (sprint == null)
                return null;
            return sprint.Estorias.ToList();
        }

        public int ObterQuantidadeEstoriasPorSprint(decimal idSprint)
        {
            var sprint = db.Sprint.FirstOrDefault(x => x.SprintId == idSprint);
            if (sprint == null)
                return 0;
            if (sprint.Estorias == null)
                return 0;
            return sprint.Estorias.Count();
        }

        public int ObterQuantidadeEstoriasPorProjeto(decimal idProjeto)
        {
            var projeto = db.Projeto.FirstOrDefault(x => x.ProjetoId == idProjeto);
            if (projeto == null)
                return 0;
            int quantidade = 0;
            foreach (var sprint in projeto.Sprints)
            {
                if(sprint.Estorias != null)
                    quantidade = quantidade + sprint.Estorias.Count();
            }
            return quantidade;
        }
    }
}

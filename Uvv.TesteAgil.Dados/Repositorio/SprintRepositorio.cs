using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class SprintRepositorio : RepositorioBase<Sprint>
    {
        public Sprint ObterSprintPorId(decimal id)
        {
            return db.Sprint.FirstOrDefault(x => x.SprintId == id);
        }

        public List<Sprint> ObterSprintsPorProjeto(decimal idProjeto)
        {
            var sprints = db.Sprint.Where(x => x.Projeto.ProjetoId == idProjeto);
            return sprints.ToList();
        }

        public int ObterQuantidadeSprintsPorProjeto(decimal idProjeto)
        {
            var projeto = db.Projeto.FirstOrDefault(p => p.ProjetoId == idProjeto);
            if (projeto == null)
                return 0;
            if (projeto.Sprints == null)
                return 0;
            return projeto.Sprints.Count();
        }
    }
}

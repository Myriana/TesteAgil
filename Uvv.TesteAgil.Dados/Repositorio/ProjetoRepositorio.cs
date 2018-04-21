using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class ProjetoRepositorio : RepositorioBase<PlanoTeste>
    {
        public Projeto ObterProjetoPorId(long id)
        {
            return db.Projeto.FirstOrDefault(x => x.ProjetoId == id);
        }
        public List<Projeto> ObterProjetosPorMembro(long idMembro)
        {
            List<Projeto> retorno = new List<Projeto>();
            var projetos = db.Membro.FirstOrDefault(x => x.MembroId == idMembro)?.Projetos?.ToList();
            return projetos;
        }
    }
}

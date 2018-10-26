using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class MembroRepositorio : RepositorioBase<Membro>
    {
        public List<Membro> ObterMembrosPorProjeto(int idProjeto)
        {
            var membros = db.Projeto.FirstOrDefault(p => p.ProjetoId == idProjeto)?.Membros;
            return membros.ToList() ?? null;
        }
    }
}

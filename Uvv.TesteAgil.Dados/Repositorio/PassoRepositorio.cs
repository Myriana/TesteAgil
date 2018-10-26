using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class PassoRepositorio : RepositorioBase<Passo>
    {
        public Passo ObterPassoPorId(long id)
        {
            return db.Passo.FirstOrDefault(x => x.PassoId == id);
        }

        public List<Passo> ObterPassosPorCasos(long idCaso)
        {
            return db.Passo.Where(x => x.CasoTesteId == idCaso)?.ToList();
        }
    }
}

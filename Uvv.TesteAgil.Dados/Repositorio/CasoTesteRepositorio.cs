using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class CasoTesteRepositorio : RepositorioBase<CasoTeste>
    {
        public CasoTeste ObterCasoPorId(long id)
        {
            return db.CasoTeste.FirstOrDefault(x => x.CasoTesteId == id);
        }

        public List<CasoTeste> ObterCasosPorCenario(long idCenario)
        {
            return db.CasoTeste.Where(x => x.CenarioTeste.CenarioTesteId == idCenario)?.ToList();
        }

        public List<CasoTeste> ObterCasosPorPlano(long idPlano)
        {
            return db.CasoTeste.Where(x => x.CenarioTeste.PlanoTeste.PlanoTesteId == idPlano)?.ToList();
        }

        public void AdicionarPasso(Passo p)
        {
            db.Passo.Add(p);
        }

        public void DeletarPasso(Passo p)
        {
            db.Passo.Remove(p);
        }
    }
}

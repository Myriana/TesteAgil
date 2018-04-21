using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class CenarioTesteRepositorio : RepositorioBase<CenarioTeste>
    {
        public CenarioTeste ObterCenarioPorId(long id)
        {
            return db.CenarioTeste.FirstOrDefault(x => x.CenarioTesteId == id);
        }

        public List<CenarioTeste> ObterCenariosPorPlano(long idPlano)
        {
            return db.CenarioTeste.Where(x => x.PlanoTeste.PlanoTesteId == idPlano)?.ToList();
        }

        public CenarioTeste ObterCenarioPorFuncionalidade(int idFuncionalidade)
        {
            return db.CenarioTeste.FirstOrDefault(c => c.Funcionalidade.FuncionalidadeId == idFuncionalidade);
        }
    }
}

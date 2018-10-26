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
            List<CenarioTeste> cenariosRetorno = new List<CenarioTeste>();
            var cenarios = db.CenarioTeste.Where(x => x.PlanoTeste.PlanoTesteId == idPlano).ToList();

            foreach (var c in cenarios)
            {
                if (c.Funcionalidade == null)
                    c.Funcionalidade = db.Funcionalidade.Find(c.FuncionalidadeId);
                cenariosRetorno.Add(c);
            }

            return cenariosRetorno;
        }

        public CenarioTeste ObterCenarioPorFuncionalidade(int idFuncionalidade)
        {
            return db.CenarioTeste.FirstOrDefault(c => c.Funcionalidade.FuncionalidadeId == idFuncionalidade);
        }
    }
}

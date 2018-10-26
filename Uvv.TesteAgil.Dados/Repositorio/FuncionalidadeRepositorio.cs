using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class FuncionalidadeRepositorio : RepositorioBase<Funcionalidade>
    {
        public Funcionalidade ObterFuncionalidadePorId(long id)
        {
            return db.Funcionalidade.FirstOrDefault(x => x.FuncionalidadeId == id);
        }
        public Funcionalidade ObterFuncionalidadePorEstoriaId(long idEstoria)
        {
            return db.Funcionalidade.FirstOrDefault(x => x.EstoriaId == idEstoria);
        }
        public List<Funcionalidade> ObterFuncionalidadesPorPlano(long idPlano)
        {
            return db.PlanoTeste.FirstOrDefault(x => x.PlanoTesteId == idPlano).Funcionalidades?.ToList();
        }

        public List<Funcionalidade> ObterFuncionalidadesNaoUsadasPorPlano(long idPlano)
        {
            var funcionalidades = db.PlanoTeste.FirstOrDefault(p => p.PlanoTesteId == idPlano)?.Funcionalidades.Where(f => f.Testada == true);
            if(funcionalidades == null) return null;
            List<Funcionalidade> funcionalidadesNaoUsadas = new List<Funcionalidade>();
            foreach(var func in funcionalidades)
            {
                var existe = db.CenarioTeste.FirstOrDefault(c => c.Funcionalidade.FuncionalidadeId == func.FuncionalidadeId);
                if (existe == null)
                    funcionalidadesNaoUsadas.Add(func);
            }
            return funcionalidadesNaoUsadas;
        }

        public bool EstoriaUsadaNoPlano(int idPlano, int idEstoria)
        {
            var plano = db.PlanoTeste.FirstOrDefault(p => p.PlanoTesteId == idPlano);

            var funcionalidades = plano.Funcionalidades;
            if (funcionalidades == null) return false;

            var existe = funcionalidades.FirstOrDefault(f => f.EstoriaId == idEstoria);
            if (existe == null) return false;

            return true;
        }
    }
}

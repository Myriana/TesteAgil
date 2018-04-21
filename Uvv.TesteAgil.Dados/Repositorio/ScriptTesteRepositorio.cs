using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class ScriptTesteRepositorio : RepositorioBase<ScriptTeste>
    {
        public ScriptTeste ObterScriptTestePorId(int id)
        {
            return db.ScriptTeste.FirstOrDefault(x => x.ScriptTesteId == id);
        }

        public List<Passo> ObterPassosPorScript(int idScript)
        {
            return db.ScriptTeste.FirstOrDefault(x => x.ScriptTesteId == idScript).Passos.ToList();
        }

        public bool ExisteScriptNome(string nome)
        {
            var script = db.ScriptTeste.FirstOrDefault(x => x.Nome == nome);
            if (script == null) return false;
            return true;
        }

        public void DeletarTodosPassosScript(int idScript)
        {
            var passos = db.ScriptTeste.FirstOrDefault(x=>x.ScriptTesteId == idScript)?.Passos.ToList();
            if(passos != null)
            {
                foreach (var passo in passos)
                {
                    db.Passo.Remove(passo);
                }
            }           
        }

        public void AdicionarPassos(List<Passo> passos, ScriptTeste script)
        {
            if (script != null)
            {
                foreach (var passo in passos)
                {
                    passo.ScriptTeste = script;
                    db.Passo.Add(passo);
                }
            }
        }

        //public bool VerificarUso(int id)
        //{
        //    var testes = db.ScriptTeste.FirstOrDefault(x => x.ScriptTesteId == id).Testes;
        //    if (testes == null || testes.Count() == 0)
        //        return false;

        //        return true;
        //}

        public Passo ObterPassoPorId(int idPasso)
        {
            return db.Passo.FirstOrDefault(x => x.PassoId == idPasso);
        }
    }
}

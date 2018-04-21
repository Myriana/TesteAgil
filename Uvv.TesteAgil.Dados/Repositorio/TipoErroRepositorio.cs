using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Repositorio
{
    public class TipoErroRepositorio : RepositorioBase<TipoErro>
    {

        public TipoErro ObterTipoErroPorId(int id)
        {
            return db.TipoErro.FirstOrDefault(x => x.TipoErroId == id);
        }

        public bool VerificaUso(int id)
        {
            var qntTestes = db.TipoErro.FirstOrDefault(x => x.TipoErroId == id).Testes.Count();
            if (qntTestes == 0)
                return false;
            else
                return true;
        }

        public bool ExisteTipoErroDescricao(string descricao)
        {
            var tipoErro = db.TipoErro.FirstOrDefault(x => x.Descricao == descricao);
            if (tipoErro == null) return false;
            return true;
        }

        public List<TipoErro> ObterTiposErroPorPlanoTeste(int idPlano)
        {
            List<TipoErro> retorno = new List<TipoErro>();
            var cenarios = db.PlanoTeste.FirstOrDefault(x => x.PlanoTesteId == idPlano).Cenarios;

            foreach (var cenario in cenarios)
            {
                var casos = cenario.Casos;
                foreach (var caso in casos)
                {
                    var testes = caso.Testes;
                    foreach (var teste in testes)
                    {
                        if (teste.Erros != null && teste.Erros.Count > 0)
                        {
                            foreach(var erro in teste.Erros)
                                retorno.Add(erro);
                        }
                    }
                }
            }
            return retorno;
        }
    }
}

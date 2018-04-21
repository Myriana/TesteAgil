using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uvv.TesteAgil.Web.Util
{
    public class MensagensPadrao
    {
        public string mensagemSucesso(string nomeClasse, string acao)
        {
            return string.Format("{0} {1} com sucesso", nomeClasse, acao);
        }

        public string mensagemErro(string erro)
        {
            return string.Format("Ocorreu um erro: {0}", erro);
        }
    }
}
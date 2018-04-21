using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.DAL
{
    public class MembroDAL
    {
        private Contexto.Contexto db = new Contexto.Contexto();
        public Membro ObterMembroPorEmailESenha(string email, string senha)
        {
            var membro = db.Membro.FirstOrDefault(x => x.Email == email && x.Senha == senha);
            return membro;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.DAL
{
    public class PlanoTesteDAL
    {
        public void AdicionaPlano(PlanoTeste plano, List<Funcionalidade> funcionalidades) 
        {
            using (Contexto.Contexto contexto = new Contexto.Contexto())
            {
                var planoSalvo = contexto.PlanoTeste.Add(plano);
                contexto.SaveChanges();
                
                foreach(var func in funcionalidades)
                {
                    var funcionalidade = contexto.Funcionalidade.FirstOrDefault(f => f.EstoriaId == func.EstoriaId);
                    if(funcionalidade != null)
                    {
                        if (funcionalidade.Planos == null)
                            funcionalidade.Planos = new List<PlanoTeste>();
                        funcionalidade.Planos.Add(plano);
                        contexto.Funcionalidade.Attach(funcionalidade);
                    }
                    else
                    {
                        func.Planos = new List<PlanoTeste>();
                        func.Planos.Add(plano);
                        contexto.Funcionalidade.Add(func);
                    }                   
                }
                contexto.SaveChanges();
            }
        }       
    }
}

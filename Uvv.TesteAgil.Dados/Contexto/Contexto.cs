using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.Dados.Contexto
{
    public class Contexto : DbContext
    {
        public Contexto() 
            : base("TesteAgilConnection")
        {

        }
        
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<Sprint> Sprint { get; set; }
        public DbSet<Membro> Membro { get; set; }
        public DbSet<Estoria> Estoria { get; set; }

        public DbSet<PlanoTeste> PlanoTeste { get; set; }
        public DbSet<CenarioTeste> CenarioTeste { get; set; }
        public DbSet<CasoTeste> CasoTeste { get; set; }
        public DbSet<Teste> Teste { get; set; }
        public DbSet<Passo> Passo { get; set; }
        public DbSet<TipoErro> TipoErro { get; set; }
        public DbSet<Funcionalidade> Funcionalidade { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Funcionalidade
    {
        [Key]
        public int FuncionalidadeId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Nome { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Descricao { get; set; }
        public int Prioridade { get; set; }
        public decimal Pontos { get; set; }
        public bool Testada { get; set; }
        public int EstoriaId { get; set; }

        public ICollection<PlanoTeste> Planos { get; set; }
        public ICollection<CenarioTeste> Cenarios { get; set; }
    }
}

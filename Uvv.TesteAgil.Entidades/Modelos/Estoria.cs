using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Estoria
    {
        [Key]
        public int EstoriaId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Nome { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Descricao { get; set; }
        public decimal Pontos { get; set; }
        public int Prioridade { get; set; }

        public virtual ICollection<Sprint> Sprints { get; set; }
    }
}

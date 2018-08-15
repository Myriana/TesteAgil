using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class ScriptTeste
    {
        [Key]
        public int ScriptTesteId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Nome { get; set; }

        public int CasoTesteId { get; set; }
        public virtual CasoTeste CasoTeste { get; set; }
        public virtual ICollection<Passo> Passos { get; set; }
    }
}

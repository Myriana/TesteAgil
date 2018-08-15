using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class CasoTeste
    {
        [Key]
        public int CasoTesteId { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Categoria { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(300)]
        public string Entrada { get; set; }
        [Display(Name = "Resposta Esperada")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(300)]
        public string RespostaEsperada { get; set; }

        public int CenarioTesteId { get; set; }
        public virtual CenarioTeste CenarioTeste { get; set; }

        public virtual ICollection<Passo> Passos { get; set; }

        public virtual ICollection<Teste> Testes { get; set; }
    }
}

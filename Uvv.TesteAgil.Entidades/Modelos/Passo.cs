using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Passo
    {
        [Key]
        public int PassoId { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Descricao { get; set; }
        [Display(Name = "Número")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int Numero { get; set; }

        public int CasoTesteId { get; set; }
        public virtual CasoTeste CasoTeste { get; set; }
    }
}

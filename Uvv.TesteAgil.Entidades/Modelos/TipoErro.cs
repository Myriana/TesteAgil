using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class TipoErro
    {
        [Key]
        public int TipoErroId { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string Gravidade { get; set; }

        public ICollection<Teste> Testes { get; set; }
    }
}

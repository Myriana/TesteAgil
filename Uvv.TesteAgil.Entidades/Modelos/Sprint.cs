using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Sprint
    {
        [Key]
        public int SprintId { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Observação")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Observacao { get; set; }

        public virtual Projeto Projeto { get; set; }

        public virtual ICollection<Estoria> Estorias { get; set; }
    }
}

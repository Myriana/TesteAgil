using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Teste
    {
        [Key]
        public int TesteId { get; set; }

        [Display(Name = "Data do Teste")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Situacao { get; set; }

        public virtual CasoTeste CasoTeste { get; set; }
        public virtual Membro Desenvolvedor { get; set; }
        public virtual Membro Tester { get; set; }

        public virtual ICollection<TipoErro> Erros { get; set; }

    }
}

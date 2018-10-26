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
        public int Situacao { get; set; }

        public int CasoTesteId { get; set; }
        public virtual CasoTeste CasoTeste { get; set; }

        public int DesenvolvedorId { get; set; }
        public virtual Membro Desenvolvedor { get; set; }
        public int TesterId { get; set; }
        public virtual Membro Tester { get; set; }

        public virtual ICollection<TipoErro> Erros { get; set; }

    }
}

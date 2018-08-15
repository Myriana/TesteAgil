using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class PlanoTeste
    {
        [Key]
        public int PlanoTesteId { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataCriacao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataFim { get; set; }
        
        public int SprintId { get; set; }
        public virtual Sprint Sprint { get; set; }

        public virtual ICollection<Funcionalidade> Funcionalidades { get; set; }
        public virtual ICollection<CenarioTeste> Cenarios { get; set; }
    }
}

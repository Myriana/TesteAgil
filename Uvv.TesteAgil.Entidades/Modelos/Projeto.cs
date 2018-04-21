using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Projeto
    {
        [Key]
        public int ProjetoId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataFim { get; set; }

        public virtual ICollection<Sprint> Sprints { get; set; }
        public virtual ICollection<Membro> Membros { get; set; }
    }
}

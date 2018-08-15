using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class Membro
    {
        [Key]
        public int MembroId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Nome { get; set; }
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail inválido")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Senha { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(11)]
        public string CPF { get; set; }

        public virtual ICollection<Projeto> Projetos { get; set; }
        public virtual ICollection<Teste> TestesDesenvolvidos { get; set; }
        public virtual ICollection<Teste> TestesTestados { get; set; }
    }
}

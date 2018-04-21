using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uvv.TesteAgil.Entidades.Modelos
{
    public class CenarioTeste
    {
        [Key]
        public int CenarioTesteId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public int Situacao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime DataAtualizacao { get; set; }

        public int FuncionalidadeId { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
        public int PlanoTesteId { get; set; }
        public virtual PlanoTeste PlanoTeste { get; set; }
        public int? ScriptTesteId { get; set; }
        public virtual ScriptTeste ScriptTeste { get; set; }

        public virtual ICollection<CasoTeste> Casos { get; set; }
    }
}

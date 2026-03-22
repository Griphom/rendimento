using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("Transacoes", Schema = "dbo")]
    public partial class Transacoes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdTransacao { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdOperacao { get; set; }

        public Operacoes Operacoes { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal ValorOriginal { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int NumeroParcela { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal ValorParcela { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime DataDesconto { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal ValorDesconto { get; set; }
    }
}
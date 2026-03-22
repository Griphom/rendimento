using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("Operacoes", Schema = "dbo")]
    public partial class Operacoes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdOperacao { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdLote { get; set; }

        public Lotes Lotes { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdCliente { get; set; }

        public Clientes Clientes { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal ValorOriginal { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int QuantidadeParcelas { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal ValorParcela { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime InicioContrato { get; set; }

        [ConcurrencyCheck]
        public DateTime? FinalContrato { get; set; }

        [ConcurrencyCheck]
        public DateTime? UltimoDesconto { get; set; }

        [ConcurrencyCheck]
        public decimal? ValorUltimoDesconto { get; set; }

        [ConcurrencyCheck]
        public DateTime? ProximoDesconto { get; set; }

        [ConcurrencyCheck]
        public decimal? ValorProximoDesconto { get; set; }

        public ICollection<Transacoes> Transacoes { get; set; }
    }
}
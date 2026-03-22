using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("Lotes", Schema = "dbo")]
    public partial class Lotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdLote { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdCedente { get; set; }

        public Cedentes Cedentes { get; set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        public string IdentificadorExt { get; set; }

        [MaxLength(255)]
        [ConcurrencyCheck]
        public string Descricao { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime DataAquisicao { get; set; }

        [Column(TypeName="datetime2")]
        [ConcurrencyCheck]
        public DateTime? DataFInalizacao { get; set; }

        public ICollection<Operacoes> Operacoes { get; set; }
    }
}
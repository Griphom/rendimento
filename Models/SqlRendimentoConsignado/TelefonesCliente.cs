using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("TelefonesCliente", Schema = "dbo")]
    public partial class TelefonesCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdTelefone { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdCliente { get; set; }

        public Clientes Clientes { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string Prefixo { get; set; }

        [Required]
        [MaxLength(3)]
        [ConcurrencyCheck]
        public string DDD { get; set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        public string Numero { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string Apelido { get; set; }

        [ConcurrencyCheck]
        public bool SMS { get; set; }

        [ConcurrencyCheck]
        public bool Whatsapp { get; set; }

        [ConcurrencyCheck]
        public bool HorarioComercial { get; set; }

        [ConcurrencyCheck]
        public bool HorarioResidencial { get; set; }

        [ConcurrencyCheck]
        public bool Principal { get; set; }
    }
}
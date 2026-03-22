using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("EnderecosCliente", Schema = "dbo")]
    public partial class EnderecosCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdEndereco { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdCliente { get; set; }

        public Clientes Clientes { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string Apelido { get; set; }

        [ConcurrencyCheck]
        public string Logradouro { get; set; }

        [ConcurrencyCheck]
        public string Numero { get; set; }

        [ConcurrencyCheck]
        public string Complemento { get; set; }

        [ConcurrencyCheck]
        public string Bairro { get; set; }

        [ConcurrencyCheck]
        public string Municipio { get; set; }

        [MaxLength(2)]
        [ConcurrencyCheck]
        public string UF { get; set; }

        [ConcurrencyCheck]
        public string Pais { get; set; }

        [ConcurrencyCheck]
        public bool? Comercial { get; set; }

        [ConcurrencyCheck]
        public bool? Residencial { get; set; }

        [ConcurrencyCheck]
        public bool? Principal { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("Clientes", Schema = "dbo")]
    public partial class Clientes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdCliente { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long IdCedente { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Nome { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string CPF_CNPJ { get; set; }

        [ConcurrencyCheck]
        public DateTime? DataNascimento { get; set; }

        [ConcurrencyCheck]
        public string NomeMae { get; set; }

        [ConcurrencyCheck]
        public string NomePai { get; set; }

        [ConcurrencyCheck]
        public string CidadeNascimento { get; set; }

        [MaxLength(2)]
        [ConcurrencyCheck]
        public string UFNascimento { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string PaisNascimento { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string DocumentoIdentificacao { get; set; }

        [MaxLength(50)]
        [ConcurrencyCheck]
        public string OrgaoEmissor { get; set; }

        [ConcurrencyCheck]
        public string Email { get; set; }

        public ICollection<EnderecosCliente> EnderecosCliente { get; set; }

        public ICollection<Operacoes> Operacoes { get; set; }

        public ICollection<TelefonesCliente> TelefonesCliente { get; set; }
    }
}
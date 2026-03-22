using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("Cedentes", Schema = "dbo")]
    public partial class Cedentes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdCedente { get; set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        public string Nome { get; set; }

        [MaxLength(255)]
        [ConcurrencyCheck]
        public string Descricao { get; set; }

        public ICollection<Lotes> Lotes { get; set; }
    }
}
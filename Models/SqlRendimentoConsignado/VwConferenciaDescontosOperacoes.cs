using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicing.Models.sql_rendimento_consignado
{
    [Table("vw_ConferenciaDescontosOperacoes", Schema = "dbo")]
    public class VwConferenciaDescontosOperacoes
    {
        public long IdOperacao { get; set; }
        public long? IdCliente { get; set; }
        public long? IdLote { get; set; }
        public long NumeroParcela { get; set; }
        public DateTime? DataPrevistaDesconto { get; set; }
        public DateTime? DataDescontoReal { get; set; }
        public decimal? ValorPrevistoDesconto { get; set; }
        public decimal? ValorDescontado { get; set; }
        public decimal? ValorParcelaTransacao { get; set; }
        public decimal? DiferencaValorDesconto { get; set; }
        public long? QuantidadeTransacoesMes { get; set; }
        public bool ParcelaPrevista { get; set; }
        public bool ParcelaComTransacao { get; set; }
        public bool DivergenciaFaltaTransacao { get; set; }
        public bool DivergenciaValorDesconto { get; set; }
        public bool DivergenciaValorParcelaInformada { get; set; }
        public bool DivergenciaMesDesconto { get; set; }
        public string StatusConferencia { get; set; }
    }
}

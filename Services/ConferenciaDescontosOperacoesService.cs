using Microsoft.EntityFrameworkCore;
using Servicing.Data;
using Servicing.Models.sql_rendimento_consignado;
using System.Data;

namespace Servicing.Services
{
    public class ConferenciaDescontosOperacoesService
    {
        private readonly sql_rendimento_consignadoContext context;

        public ConferenciaDescontosOperacoesService(sql_rendimento_consignadoContext context)
        {
            this.context = context;
        }

        public async Task<IList<Cedentes>> GetCedentes()
        {
            return await context.Cedentes
                .AsNoTracking()
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<IList<Lotes>> GetLotesPorCedente(long idCedente)
        {
            return await context.Lotes
                .AsNoTracking()
                .Where(l => l.IdCedente == idCedente)
                .OrderBy(l => l.IdentificadorExt)
                .ToListAsync();
        }

        public async Task<IList<Clientes>> GetClientesPorCedente(long idCedente)
        {
            return await context.Clientes
                .AsNoTracking()
                .Where(c => c.IdCedente == idCedente)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<IList<ConferenciaDescontoDetalhadoItem>> GetTop100Detalhado(long idCedente, long? idLote, DateTime mesReferencia)
        {
            var inicioMes = new DateTime(mesReferencia.Year, mesReferencia.Month, 1);
            var inicioMesSeguinte = inicioMes.AddMonths(1);

            const string sql = @"
SELECT TOP (100)
    CAST(v.IdOperacao AS bigint) AS IdOperacao,
    CAST(v.IdCliente AS bigint) AS IdCliente,
    CAST(v.IdLote AS bigint) AS IdLote,
    CAST(ce.IdCedente AS bigint) AS IdCedente,
    ce.Nome AS NomeCedente,
    l.IdentificadorExt AS NomeLote,
    c.Nome AS NomeCliente,
    CAST(v.NumeroParcela AS bigint) AS NumeroParcela,
    v.DataPrevistaDesconto,
    v.DataDescontoReal,
    v.ValorPrevistoDesconto,
    v.ValorDescontado,
    v.DiferencaValorDesconto,
    CAST(v.QuantidadeTransacoesMes AS bigint) AS QuantidadeTransacoesMes,
    v.StatusConferencia,
    v.DivergenciaFaltaTransacao,
    v.DivergenciaValorDesconto,
    v.DivergenciaValorParcelaInformada,
    v.DivergenciaMesDesconto
FROM dbo.vw_ConferenciaDescontosOperacoes v
INNER JOIN dbo.Lotes l ON l.IdLote = v.IdLote
INNER JOIN dbo.Cedentes ce ON ce.IdCedente = l.IdCedente
LEFT JOIN dbo.Clientes c ON c.IdCliente = v.IdCliente
WHERE ce.IdCedente = @idCedente
  AND (@idLote IS NULL OR v.IdLote = @idLote)
  AND COALESCE(v.DataPrevistaDesconto, v.DataDescontoReal) >= @inicioMes
  AND COALESCE(v.DataPrevistaDesconto, v.DataDescontoReal) < @inicioMesSeguinte
ORDER BY CAST(v.NumeroParcela AS bigint), CAST(v.IdOperacao AS bigint);";

            var conn = context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            var pIdCedente = cmd.CreateParameter();
            pIdCedente.ParameterName = "@idCedente";
            pIdCedente.DbType = DbType.Int64;
            pIdCedente.Value = idCedente;
            cmd.Parameters.Add(pIdCedente);

            var pIdLote = cmd.CreateParameter();
            pIdLote.ParameterName = "@idLote";
            pIdLote.DbType = DbType.Int64;
            pIdLote.Value = idLote.HasValue ? idLote.Value : DBNull.Value;
            cmd.Parameters.Add(pIdLote);

            var pInicioMes = cmd.CreateParameter();
            pInicioMes.ParameterName = "@inicioMes";
            pInicioMes.DbType = DbType.DateTime2;
            pInicioMes.Value = inicioMes;
            cmd.Parameters.Add(pInicioMes);

            var pInicioMesSeguinte = cmd.CreateParameter();
            pInicioMesSeguinte.ParameterName = "@inicioMesSeguinte";
            pInicioMesSeguinte.DbType = DbType.DateTime2;
            pInicioMesSeguinte.Value = inicioMesSeguinte;
            cmd.Parameters.Add(pInicioMesSeguinte);

            var resultado = new List<ConferenciaDescontoDetalhadoItem>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resultado.Add(new ConferenciaDescontoDetalhadoItem
                {
                    IdOperacao = ToInt64(reader["IdOperacao"]),
                    IdCliente = ToNullableInt64(reader["IdCliente"]),
                    IdLote = ToNullableInt64(reader["IdLote"]),
                    IdCedente = ToNullableInt64(reader["IdCedente"]),
                    NomeCedente = reader["NomeCedente"] as string,
                    NomeLote = reader["NomeLote"] as string,
                    NomeCliente = reader["NomeCliente"] as string,
                    NumeroParcela = ToInt64(reader["NumeroParcela"]),
                    DataPrevistaDesconto = ToNullableDateTime(reader["DataPrevistaDesconto"]),
                    DataDescontoReal = ToNullableDateTime(reader["DataDescontoReal"]),
                    ValorPrevistoDesconto = ToNullableDecimal(reader["ValorPrevistoDesconto"]),
                    ValorDescontado = ToNullableDecimal(reader["ValorDescontado"]),
                    DiferencaValorDesconto = ToNullableDecimal(reader["DiferencaValorDesconto"]),
                    QuantidadeTransacoesMes = ToNullableInt64(reader["QuantidadeTransacoesMes"]),
                    StatusConferencia = reader["StatusConferencia"] as string,
                    DivergenciaFaltaTransacao = ToBoolean(reader["DivergenciaFaltaTransacao"]),
                    DivergenciaValorDesconto = ToBoolean(reader["DivergenciaValorDesconto"]),
                    DivergenciaValorParcelaInformada = ToBoolean(reader["DivergenciaValorParcelaInformada"]),
                    DivergenciaMesDesconto = ToBoolean(reader["DivergenciaMesDesconto"])
                });
            }

            return resultado;
        }

        private static long ToInt64(object value)
        {
            return Convert.ToInt64(value);
        }

        private static long? ToNullableInt64(object value)
        {
            return value == DBNull.Value ? null : Convert.ToInt64(value);
        }

        private static DateTime? ToNullableDateTime(object value)
        {
            return value == DBNull.Value ? null : Convert.ToDateTime(value);
        }

        private static decimal? ToNullableDecimal(object value)
        {
            return value == DBNull.Value ? null : Convert.ToDecimal(value);
        }

        private static bool ToBoolean(object value)
        {
            return value != DBNull.Value && Convert.ToBoolean(value);
        }
    }

    public class ConferenciaDescontoDetalhadoItem
    {
        public long IdOperacao { get; set; }
        public long? IdCliente { get; set; }
        public long? IdLote { get; set; }
        public long? IdCedente { get; set; }
        public string NomeCedente { get; set; }
        public string NomeLote { get; set; }
        public string NomeCliente { get; set; }
        public long NumeroParcela { get; set; }
        public DateTime? DataPrevistaDesconto { get; set; }
        public DateTime? DataDescontoReal { get; set; }
        public decimal? ValorPrevistoDesconto { get; set; }
        public decimal? ValorDescontado { get; set; }
        public decimal? DiferencaValorDesconto { get; set; }
        public long? QuantidadeTransacoesMes { get; set; }
        public string StatusConferencia { get; set; }
        public bool DivergenciaFaltaTransacao { get; set; }
        public bool DivergenciaValorDesconto { get; set; }
        public bool DivergenciaValorParcelaInformada { get; set; }
        public bool DivergenciaMesDesconto { get; set; }
    }
}

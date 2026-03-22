CREATE OR ALTER VIEW dbo.vw_ConferenciaDescontosOperacoes
AS
WITH Numeros AS
(
    SELECT TOP (1000)
        CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS int) AS NumeroParcela
    FROM sys.all_objects
),
ParcelasPrevistas AS
(
    SELECT
        o.IdOperacao,
        o.IdCliente,
        o.IdLote,
        n.NumeroParcela,
        CAST(DATEADD(MONTH, n.NumeroParcela - 1, o.InicioContrato) AS date) AS DataPrevistaDesconto,
        CAST(o.ValorParcela AS decimal(18,2)) AS ValorPrevistoDesconto,
        CAST(o.ValorOriginal AS decimal(18,2)) AS ValorOriginalOperacao,
        o.QuantidadeParcelas
    FROM dbo.Operacoes o
    INNER JOIN Numeros n ON n.NumeroParcela <= o.QuantidadeParcelas
),
TransacoesConsolidadas AS
(
    SELECT
        t.IdOperacao,
        t.NumeroParcela,
        MIN(CAST(t.DataDesconto AS date)) AS DataDesconto,
        CAST(SUM(ISNULL(t.ValorDesconto, 0)) AS decimal(18,2)) AS ValorDescontado,
        CAST(SUM(ISNULL(t.ValorParcela, 0)) AS decimal(18,2)) AS ValorParcelaTransacao,
        COUNT(*) AS QuantidadeTransacoesMes
    FROM dbo.Transacoes t
    GROUP BY
        t.IdOperacao,
        t.NumeroParcela
)
SELECT
    p.IdOperacao,
    p.IdCliente,
    p.IdLote,
    p.NumeroParcela,
    p.DataPrevistaDesconto,
    t.DataDesconto AS DataDescontoReal,
    p.ValorPrevistoDesconto,
    t.ValorDescontado,
    t.ValorParcelaTransacao,
    CAST(ISNULL(t.ValorDescontado, 0) - p.ValorPrevistoDesconto AS decimal(18,2)) AS DiferencaValorDesconto,
    t.QuantidadeTransacoesMes,
    CAST(1 AS bit) AS ParcelaPrevista,
    CAST(CASE WHEN t.IdOperacao IS NULL THEN 0 ELSE 1 END AS bit) AS ParcelaComTransacao,
    CAST(CASE WHEN t.IdOperacao IS NULL THEN 1 ELSE 0 END AS bit) AS DivergenciaFaltaTransacao,
    CAST(CASE WHEN t.IdOperacao IS NOT NULL AND ABS(ISNULL(t.ValorDescontado, 0) - p.ValorPrevistoDesconto) > 0.01 THEN 1 ELSE 0 END AS bit) AS DivergenciaValorDesconto,
    CAST(CASE WHEN t.IdOperacao IS NOT NULL AND ABS(ISNULL(t.ValorParcelaTransacao, 0) - p.ValorPrevistoDesconto) > 0.01 THEN 1 ELSE 0 END AS bit) AS DivergenciaValorParcelaInformada,
    CAST(CASE WHEN t.IdOperacao IS NOT NULL AND (YEAR(t.DataDesconto) <> YEAR(p.DataPrevistaDesconto) OR MONTH(t.DataDesconto) <> MONTH(p.DataPrevistaDesconto)) THEN 1 ELSE 0 END AS bit) AS DivergenciaMesDesconto,
    CASE
        WHEN t.IdOperacao IS NULL THEN 'TRANSACAO_NAO_ENCONTRADA'
        WHEN ABS(ISNULL(t.ValorDescontado, 0) - p.ValorPrevistoDesconto) > 0.01 THEN 'VALOR_DESCONTADO_DIVERGENTE'
        WHEN ABS(ISNULL(t.ValorParcelaTransacao, 0) - p.ValorPrevistoDesconto) > 0.01 THEN 'VALOR_PARCELA_TRANSACAO_DIVERGENTE'
        WHEN YEAR(t.DataDesconto) <> YEAR(p.DataPrevistaDesconto) OR MONTH(t.DataDesconto) <> MONTH(p.DataPrevistaDesconto) THEN 'MES_DESCONTO_DIVERGENTE'
        ELSE 'OK'
    END AS StatusConferencia
FROM ParcelasPrevistas p
LEFT JOIN TransacoesConsolidadas t
    ON t.IdOperacao = p.IdOperacao
   AND t.NumeroParcela = p.NumeroParcela

UNION ALL

SELECT
    t.IdOperacao,
    o.IdCliente,
    o.IdLote,
    t.NumeroParcela,
    NULL AS DataPrevistaDesconto,
    t.DataDesconto AS DataDescontoReal,
    NULL AS ValorPrevistoDesconto,
    t.ValorDescontado,
    t.ValorParcelaTransacao,
    NULL AS DiferencaValorDesconto,
    t.QuantidadeTransacoesMes,
    CAST(0 AS bit) AS ParcelaPrevista,
    CAST(1 AS bit) AS ParcelaComTransacao,
    CAST(0 AS bit) AS DivergenciaFaltaTransacao,
    CAST(0 AS bit) AS DivergenciaValorDesconto,
    CAST(0 AS bit) AS DivergenciaValorParcelaInformada,
    CAST(0 AS bit) AS DivergenciaMesDesconto,
    'TRANSACAO_EXTRA_SEM_PARCELA_PREVISTA' AS StatusConferencia
FROM TransacoesConsolidadas t
LEFT JOIN ParcelasPrevistas p
    ON p.IdOperacao = t.IdOperacao
   AND p.NumeroParcela = t.NumeroParcela
LEFT JOIN dbo.Operacoes o
    ON o.IdOperacao = t.IdOperacao
WHERE p.IdOperacao IS NULL;
GO

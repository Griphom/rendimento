using Microsoft.AspNetCore.Components;
using Radzen;
using Servicing.Services;
using CedenteEntity = Servicing.Models.sql_rendimento_consignado.Cedentes;
using LoteEntity = Servicing.Models.sql_rendimento_consignado.Lotes;

namespace Servicing.Components.Pages
{
    public partial class ConferenciaDescontos
    {
        [Inject]
        protected ConferenciaDescontosOperacoesService ConferenciaService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<CedenteEntity> cedentes = new List<CedenteEntity>();
        protected IList<LoteEntity> lotes = new List<LoteEntity>();
        protected IList<ConferenciaDescontoDetalhadoItem> resultados = new List<ConferenciaDescontoDetalhadoItem>();

        protected long? idCedenteSelecionado;
        protected long? idLoteSelecionado;
        protected DateTime mesSelecionado = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        protected override async Task OnInitializedAsync()
        {
            cedentes = await ConferenciaService.GetCedentes();
        }

        protected async Task OnCedenteChanged()
        {
            idLoteSelecionado = null;
            lotes = new List<LoteEntity>();

            if (idCedenteSelecionado.HasValue)
            {
                lotes = await ConferenciaService.GetLotesPorCedente(idCedenteSelecionado.Value);
            }

            resultados = new List<ConferenciaDescontoDetalhadoItem>();
        }

        protected async Task Buscar()
        {
            if (!idCedenteSelecionado.HasValue)
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Filtro obrigatorio", "Selecione um cedente.");
                return;
            }

            resultados = await ConferenciaService.GetTop100Detalhado(
                idCedenteSelecionado.Value,
                idLoteSelecionado,
                mesSelecionado);
        }
    }
}

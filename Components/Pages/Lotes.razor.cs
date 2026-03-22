using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using CedenteEntity = Servicing.Models.sql_rendimento_consignado.Cedentes;
using LoteEntity = Servicing.Models.sql_rendimento_consignado.Lotes;

namespace Servicing.Components.Pages
{
    public partial class Lotes
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<LoteEntity> lotes = new List<LoteEntity>();
        protected IList<CedenteEntity> cedentes = new List<CedenteEntity>();
        protected LoteEntity editModel = new LoteEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadCedentes();
            await LoadLotes();
        }

        protected async Task LoadCedentes()
        {
            var query = await Service.GetCedentes();
            cedentes = query.OrderBy(c => c.Nome).ToList();
        }

        protected async Task LoadLotes()
        {
            var query = await Service.GetLotes();
            lotes = query.OrderBy(c => c.IdLote).ToList();
        }

        protected void PrepareNewLote()
        {
            editModel = new LoteEntity();
            isEditing = false;
        }

        protected string GetCedenteNome(long idCedente)
        {
            return cedentes.FirstOrDefault(c => c.IdCedente == idCedente)?.Nome ?? idCedente.ToString();
        }

        protected void BeginEdit(LoteEntity item)
        {
            editModel = new LoteEntity
            {
                IdLote = item.IdLote,
                IdCedente = item.IdCedente,
                IdentificadorExt = item.IdentificadorExt,
                Descricao = item.Descricao,
                DataAquisicao = item.DataAquisicao,
                DataFInalizacao = item.DataFInalizacao
            };

            isEditing = true;
        }

        protected async Task SaveLote(LoteEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateLotes(model.IdLote, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Lote atualizado", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateLotes(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Lote criado", "Registro criado com sucesso.");
                }

                PrepareNewLote();
                await LoadLotes();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteLote(LoteEntity item)
        {
            var confirmed = await DialogService.Confirm($"Deseja realmente excluir o lote {item.IdentificadorExt}?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteLotes(item.IdLote);

                if (isEditing && editModel.IdLote == item.IdLote)
                {
                    PrepareNewLote();
                }

                await LoadLotes();
                NotificationService.Notify(NotificationSeverity.Success, "Lote removido", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }
    }
}

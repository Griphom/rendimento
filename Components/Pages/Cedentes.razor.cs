using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using CedenteEntity = Servicing.Models.sql_rendimento_consignado.Cedentes;

namespace Servicing.Components.Pages
{
    public partial class Cedentes
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<CedenteEntity> cedentes = new List<CedenteEntity>();
        protected CedenteEntity editModel = new CedenteEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadCedentes();
        }

        protected async Task LoadCedentes()
        {
            var query = await Service.GetCedentes();
            cedentes = query.OrderBy(c => c.IdCedente).ToList();
        }

        protected void PrepareNewCedente()
        {
            editModel = new CedenteEntity();
            isEditing = false;
        }

        protected void BeginEdit(CedenteEntity item)
        {
            editModel = new CedenteEntity
            {
                IdCedente = item.IdCedente,
                Nome = item.Nome,
                Descricao = item.Descricao
            };

            isEditing = true;
        }

        protected async Task SaveCedente(CedenteEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateCedentes(model.IdCedente, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Cedente atualizado", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateCedentes(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Cedente criado", "Registro criado com sucesso.");
                }

                PrepareNewCedente();
                await LoadCedentes();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteCedente(CedenteEntity item)
        {
            var confirmed = await DialogService.Confirm($"Deseja realmente excluir o cedente {item.Nome}?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteCedentes(item.IdCedente);

                if (isEditing && editModel.IdCedente == item.IdCedente)
                {
                    PrepareNewCedente();
                }

                await LoadCedentes();
                NotificationService.Notify(NotificationSeverity.Success, "Cedente removido", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }
    }
}

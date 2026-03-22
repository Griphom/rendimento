using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using TelefoneEntity = Servicing.Models.sql_rendimento_consignado.TelefonesCliente;

namespace Servicing.Components.Pages
{
    public partial class TelefonesCliente
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<TelefoneEntity> telefones = new List<TelefoneEntity>();
        protected TelefoneEntity editModel = new TelefoneEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadTelefones();
        }

        protected async Task LoadTelefones()
        {
            var query = await Service.GetTelefonesCliente();
            telefones = query.OrderBy(c => c.IdTelefone).ToList();
        }

        protected void PrepareNewTelefone()
        {
            editModel = new TelefoneEntity();
            isEditing = false;
        }

        protected void BeginEdit(TelefoneEntity item)
        {
            editModel = new TelefoneEntity
            {
                IdTelefone = item.IdTelefone,
                IdCliente = item.IdCliente,
                Prefixo = item.Prefixo,
                DDD = item.DDD,
                Numero = item.Numero,
                Apelido = item.Apelido,
                SMS = item.SMS,
                Whatsapp = item.Whatsapp,
                HorarioComercial = item.HorarioComercial,
                HorarioResidencial = item.HorarioResidencial,
                Principal = item.Principal
            };

            isEditing = true;
        }

        protected async Task SaveTelefone(TelefoneEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateTelefonesCliente(model.IdTelefone, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Telefone atualizado", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateTelefonesCliente(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Telefone criado", "Registro criado com sucesso.");
                }

                PrepareNewTelefone();
                await LoadTelefones();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteTelefone(TelefoneEntity item)
        {
            var confirmed = await DialogService.Confirm($"Deseja realmente excluir o telefone {item.DDD} {item.Numero}?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteTelefonesCliente(item.IdTelefone);

                if (isEditing && editModel.IdTelefone == item.IdTelefone)
                {
                    PrepareNewTelefone();
                }

                await LoadTelefones();
                NotificationService.Notify(NotificationSeverity.Success, "Telefone removido", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }
    }
}

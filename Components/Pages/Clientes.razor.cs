using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using ClienteEntity = Servicing.Models.sql_rendimento_consignado.Clientes;

namespace Servicing.Components.Pages
{
    public partial class Clientes
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<ClienteEntity> clientes = new List<ClienteEntity>();
        protected ClienteEntity editModel = new ClienteEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadClientes();
        }

        protected async Task LoadClientes()
        {
            var query = await Service.GetClientes();
            clientes = query.OrderBy(c => c.IdCliente).ToList();
        }

        protected void PrepareNewCliente()
        {
            editModel = new ClienteEntity();
            isEditing = false;
        }

        protected void BeginEdit(ClienteEntity item)
        {
            editModel = new ClienteEntity
            {
                IdCliente = item.IdCliente,
                IdCedente = item.IdCedente,
                Nome = item.Nome,
                CPF_CNPJ = item.CPF_CNPJ,
                DataNascimento = item.DataNascimento,
                NomeMae = item.NomeMae,
                NomePai = item.NomePai,
                CidadeNascimento = item.CidadeNascimento,
                UFNascimento = item.UFNascimento,
                PaisNascimento = item.PaisNascimento,
                DocumentoIdentificacao = item.DocumentoIdentificacao,
                OrgaoEmissor = item.OrgaoEmissor,
                Email = item.Email
            };

            isEditing = true;
        }

        protected async Task SaveCliente(ClienteEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateClientes(model.IdCliente, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Cliente atualizado", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateClientes(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Cliente criado", "Registro criado com sucesso.");
                }

                PrepareNewCliente();
                await LoadClientes();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteCliente(ClienteEntity item)
        {
            var confirmed = await DialogService.Confirm($"Deseja realmente excluir o cliente {item.Nome}?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteClientes(item.IdCliente);

                if (isEditing && editModel.IdCliente == item.IdCliente)
                {
                    PrepareNewCliente();
                }

                await LoadClientes();
                NotificationService.Notify(NotificationSeverity.Success, "Cliente removido", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }
    }
}

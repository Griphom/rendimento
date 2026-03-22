using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using ClienteEntity = Servicing.Models.sql_rendimento_consignado.Clientes;
using EnderecoEntity = Servicing.Models.sql_rendimento_consignado.EnderecosCliente;

namespace Servicing.Components.Pages
{
    public partial class EnderecosCliente
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<EnderecoEntity> enderecos = new List<EnderecoEntity>();
        protected IList<ClienteEntity> clientes = new List<ClienteEntity>();
        protected EnderecoEntity editModel = new EnderecoEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadClientes();
            await LoadEnderecos();
        }

        protected async Task LoadClientes()
        {
            var query = await Service.GetClientes();
            clientes = query.OrderBy(c => c.Nome).ToList();
        }

        protected async Task LoadEnderecos()
        {
            var query = await Service.GetEnderecosCliente();
            enderecos = query.OrderBy(c => c.IdEndereco).ToList();
        }

        protected void PrepareNewEndereco()
        {
            editModel = new EnderecoEntity();
            isEditing = false;
        }

        protected string GetClienteNome(long idCliente)
        {
            return clientes.FirstOrDefault(c => c.IdCliente == idCliente)?.Nome ?? idCliente.ToString();
        }

        protected void BeginEdit(EnderecoEntity item)
        {
            editModel = new EnderecoEntity
            {
                IdEndereco = item.IdEndereco,
                IdCliente = item.IdCliente,
                Apelido = item.Apelido,
                Logradouro = item.Logradouro,
                Numero = item.Numero,
                Complemento = item.Complemento,
                Bairro = item.Bairro,
                Municipio = item.Municipio,
                UF = item.UF,
                Pais = item.Pais,
                Comercial = item.Comercial,
                Residencial = item.Residencial,
                Principal = item.Principal
            };

            isEditing = true;
        }

        protected async Task SaveEndereco(EnderecoEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateEnderecosCliente(model.IdEndereco, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Endereco atualizado", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateEnderecosCliente(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Endereco criado", "Registro criado com sucesso.");
                }

                PrepareNewEndereco();
                await LoadEnderecos();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteEndereco(EnderecoEntity item)
        {
            var confirmed = await DialogService.Confirm("Deseja realmente excluir este endereco?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteEnderecosCliente(item.IdEndereco);

                if (isEditing && editModel.IdEndereco == item.IdEndereco)
                {
                    PrepareNewEndereco();
                }

                await LoadEnderecos();
                NotificationService.Notify(NotificationSeverity.Success, "Endereco removido", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }
    }
}

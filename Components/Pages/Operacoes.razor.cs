using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using ClienteEntity = Servicing.Models.sql_rendimento_consignado.Clientes;
using LoteEntity = Servicing.Models.sql_rendimento_consignado.Lotes;
using OperacaoEntity = Servicing.Models.sql_rendimento_consignado.Operacoes;

namespace Servicing.Components.Pages
{
    public partial class Operacoes
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<OperacaoEntity> operacoes = new List<OperacaoEntity>();
        protected IList<LoteEntity> lotes = new List<LoteEntity>();
        protected IList<ClienteEntity> clientes = new List<ClienteEntity>();
        protected OperacaoEntity editModel = new OperacaoEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadLotes();
            await LoadClientes();
            await LoadOperacoes();
        }

        protected async Task LoadLotes()
        {
            var query = await Service.GetLotes();
            lotes = query.OrderBy(c => c.IdentificadorExt).ToList();
        }

        protected async Task LoadClientes()
        {
            var query = await Service.GetClientes();
            clientes = query.OrderBy(c => c.Nome).ToList();
        }

        protected async Task LoadOperacoes()
        {
            var query = await Service.GetOperacoes();
            operacoes = query.OrderBy(c => c.IdOperacao).ToList();
        }

        protected void PrepareNewOperacao()
        {
            editModel = new OperacaoEntity();
            isEditing = false;
        }

        protected string GetLoteNome(long idLote)
        {
            var lote = lotes.FirstOrDefault(l => l.IdLote == idLote);
            return lote?.IdentificadorExt ?? idLote.ToString();
        }

        protected string GetClienteNome(long idCliente)
        {
            return clientes.FirstOrDefault(c => c.IdCliente == idCliente)?.Nome ?? idCliente.ToString();
        }

        protected void BeginEdit(OperacaoEntity item)
        {
            editModel = new OperacaoEntity
            {
                IdOperacao = item.IdOperacao,
                IdLote = item.IdLote,
                IdCliente = item.IdCliente,
                ValorOriginal = item.ValorOriginal,
                QuantidadeParcelas = item.QuantidadeParcelas,
                ValorParcela = item.ValorParcela,
                InicioContrato = item.InicioContrato,
                FinalContrato = item.FinalContrato,
                UltimoDesconto = item.UltimoDesconto,
                ValorUltimoDesconto = item.ValorUltimoDesconto,
                ProximoDesconto = item.ProximoDesconto,
                ValorProximoDesconto = item.ValorProximoDesconto
            };

            isEditing = true;
        }

        protected async Task SaveOperacao(OperacaoEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateOperacoes(model.IdOperacao, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Operacao atualizada", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateOperacoes(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Operacao criada", "Registro criado com sucesso.");
                }

                PrepareNewOperacao();
                await LoadOperacoes();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteOperacao(OperacaoEntity item)
        {
            var confirmed = await DialogService.Confirm($"Deseja realmente excluir a operacao {item.IdOperacao}?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteOperacoes(item.IdOperacao);

                if (isEditing && editModel.IdOperacao == item.IdOperacao)
                {
                    PrepareNewOperacao();
                }

                await LoadOperacoes();
                NotificationService.Notify(NotificationSeverity.Success, "Operacao removida", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }
    }
}

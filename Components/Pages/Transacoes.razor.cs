using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using ClienteEntity = Servicing.Models.sql_rendimento_consignado.Clientes;
using OperacaoEntity = Servicing.Models.sql_rendimento_consignado.Operacoes;
using TransacaoEntity = Servicing.Models.sql_rendimento_consignado.Transacoes;

namespace Servicing.Components.Pages
{
    public partial class Transacoes
    {
        [Inject]
        protected sql_rendimento_consignadoService Service { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected IList<TransacaoEntity> transacoes = new List<TransacaoEntity>();
        protected IList<OperacaoLookupItem> operacoesLookup = new List<OperacaoLookupItem>();
        protected TransacaoEntity editModel = new TransacaoEntity();
        protected bool isEditing;

        protected override async Task OnInitializedAsync()
        {
            await LoadOperacoesLookup();
            await LoadTransacoes();
        }

        protected async Task LoadOperacoesLookup()
        {
            var operacoesQuery = await Service.GetOperacoes();
            var clientesQuery = await Service.GetClientes();

            var operacoes = operacoesQuery.ToList();
            var clientes = clientesQuery.ToList();

            operacoesLookup = operacoes
                .Select(o => new OperacaoLookupItem
                {
                    Id = o.IdOperacao,
                    Nome = $"{o.IdOperacao} - {clientes.FirstOrDefault(c => c.IdCliente == o.IdCliente)?.Nome ?? "Sem cliente"}"
                })
                .OrderBy(o => o.Nome)
                .ToList();
        }

        protected async Task LoadTransacoes()
        {
            var query = await Service.GetTransacoes();
            transacoes = query.OrderBy(c => c.IdTransacao).ToList();
        }

        protected void PrepareNewTransacao()
        {
            editModel = new TransacaoEntity();
            isEditing = false;
        }

        protected string GetOperacaoNome(long idOperacao)
        {
            return operacoesLookup.FirstOrDefault(o => o.Id == idOperacao)?.Nome ?? idOperacao.ToString();
        }

        protected void BeginEdit(TransacaoEntity item)
        {
            editModel = new TransacaoEntity
            {
                IdTransacao = item.IdTransacao,
                IdOperacao = item.IdOperacao,
                ValorOriginal = item.ValorOriginal,
                NumeroParcela = item.NumeroParcela,
                ValorParcela = item.ValorParcela,
                DataDesconto = item.DataDesconto,
                ValorDesconto = item.ValorDesconto
            };

            isEditing = true;
        }

        protected async Task SaveTransacao(TransacaoEntity model)
        {
            try
            {
                if (isEditing)
                {
                    await Service.UpdateTransacoes(model.IdTransacao, model);
                    NotificationService.Notify(NotificationSeverity.Success, "Transacao atualizada", "Registro atualizado com sucesso.");
                }
                else
                {
                    await Service.CreateTransacoes(model);
                    NotificationService.Notify(NotificationSeverity.Success, "Transacao criada", "Registro criado com sucesso.");
                }

                PrepareNewTransacao();
                await LoadTransacoes();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao salvar", ex.Message);
            }
        }

        protected async Task DeleteTransacao(TransacaoEntity item)
        {
            var confirmed = await DialogService.Confirm($"Deseja realmente excluir a transacao {item.IdTransacao}?", "Confirmacao", new ConfirmOptions { OkButtonText = "Excluir", CancelButtonText = "Cancelar" });
            if (confirmed != true)
            {
                return;
            }

            try
            {
                await Service.DeleteTransacoes(item.IdTransacao);

                if (isEditing && editModel.IdTransacao == item.IdTransacao)
                {
                    PrepareNewTransacao();
                }

                await LoadTransacoes();
                NotificationService.Notify(NotificationSeverity.Success, "Transacao removida", "Registro excluido com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Erro ao excluir", ex.Message);
            }
        }

        protected class OperacaoLookupItem
        {
            public long Id { get; set; }
            public string Nome { get; set; }
        }
    }
}

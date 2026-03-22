using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Servicing.Data;

namespace Servicing.Controllers
{
    public partial class Exportsql_rendimento_consignadoController : ExportController
    {
        private readonly sql_rendimento_consignadoContext context;
        private readonly sql_rendimento_consignadoService service;

        public Exportsql_rendimento_consignadoController(sql_rendimento_consignadoContext context, sql_rendimento_consignadoService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/sql_rendimento_consignado/cedentes/csv")]
        [HttpGet("/export/sql_rendimento_consignado/cedentes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCedentesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCedentes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/cedentes/excel")]
        [HttpGet("/export/sql_rendimento_consignado/cedentes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCedentesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCedentes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/clientes/csv")]
        [HttpGet("/export/sql_rendimento_consignado/clientes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClientes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/clientes/excel")]
        [HttpGet("/export/sql_rendimento_consignado/clientes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClientes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/enderecoscliente/csv")]
        [HttpGet("/export/sql_rendimento_consignado/enderecoscliente/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEnderecosClienteToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEnderecosCliente(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/enderecoscliente/excel")]
        [HttpGet("/export/sql_rendimento_consignado/enderecoscliente/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEnderecosClienteToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEnderecosCliente(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/lotes/csv")]
        [HttpGet("/export/sql_rendimento_consignado/lotes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLotesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/lotes/excel")]
        [HttpGet("/export/sql_rendimento_consignado/lotes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLotesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/operacoes/csv")]
        [HttpGet("/export/sql_rendimento_consignado/operacoes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOperacoesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOperacoes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/operacoes/excel")]
        [HttpGet("/export/sql_rendimento_consignado/operacoes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOperacoesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOperacoes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/telefonescliente/csv")]
        [HttpGet("/export/sql_rendimento_consignado/telefonescliente/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTelefonesClienteToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTelefonesCliente(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/telefonescliente/excel")]
        [HttpGet("/export/sql_rendimento_consignado/telefonescliente/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTelefonesClienteToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTelefonesCliente(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/transacoes/csv")]
        [HttpGet("/export/sql_rendimento_consignado/transacoes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTransacoesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTransacoes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/sql_rendimento_consignado/transacoes/excel")]
        [HttpGet("/export/sql_rendimento_consignado/transacoes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTransacoesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTransacoes(), Request.Query, false), fileName);
        }
    }
}

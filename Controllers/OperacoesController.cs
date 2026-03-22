using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacoesController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public OperacoesController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacoes>>> GetOperacoes()
        {
            var query = await service.GetOperacoes();
            var items = await query.AsNoTracking().OrderBy(c => c.IdOperacao).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Operacoes>> GetOperacaoById(long id)
        {
            var item = await service.GetOperacoesByIdOperacao(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Operacoes>> CreateOperacao([FromBody] Operacoes operacao)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateOperacoes(operacao);
            return CreatedAtAction(nameof(GetOperacaoById), new { id = created.IdOperacao }, created);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<Operacoes>> UpdateOperacao(long id, [FromBody] Operacoes operacao)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != operacao.IdOperacao)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdOperacao enviado no corpo.");
            }

            var existing = await service.GetOperacoesByIdOperacao(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateOperacoes(id, operacao);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteOperacao(long id)
        {
            var existing = await service.GetOperacoesByIdOperacao(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteOperacoes(id);
            return NoContent();
        }
    }
}

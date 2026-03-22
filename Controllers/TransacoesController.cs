using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public TransacoesController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacoes>>> GetTransacoes()
        {
            var query = await service.GetTransacoes();
            var items = await query.AsNoTracking().OrderBy(c => c.IdTransacao).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Transacoes>> GetTransacaoById(long id)
        {
            var item = await service.GetTransacoesByIdTransacao(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Transacoes>> CreateTransacao([FromBody] Transacoes transacao)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateTransacoes(transacao);
            return CreatedAtAction(nameof(GetTransacaoById), new { id = created.IdTransacao }, created);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<Transacoes>> UpdateTransacao(long id, [FromBody] Transacoes transacao)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != transacao.IdTransacao)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdTransacao enviado no corpo.");
            }

            var existing = await service.GetTransacoesByIdTransacao(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateTransacoes(id, transacao);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteTransacao(long id)
        {
            var existing = await service.GetTransacoesByIdTransacao(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteTransacoes(id);
            return NoContent();
        }
    }
}

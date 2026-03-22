using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public LotesController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lotes>>> GetLotes()
        {
            var query = await service.GetLotes();
            var items = await query.AsNoTracking().OrderBy(c => c.IdLote).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Lotes>> GetLoteById(int id)
        {
            var item = await service.GetLotesByIdLote(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Lotes>> CreateLote([FromBody] Lotes lote)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateLotes(lote);
            return CreatedAtAction(nameof(GetLoteById), new { id = created.IdLote }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Lotes>> UpdateLote(int id, [FromBody] Lotes lote)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != lote.IdLote)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdLote enviado no corpo.");
            }

            var existing = await service.GetLotesByIdLote(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateLotes(id, lote);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLote(int id)
        {
            var existing = await service.GetLotesByIdLote(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteLotes(id);
            return NoContent();
        }
    }
}

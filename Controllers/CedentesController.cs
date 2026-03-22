using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CedentesController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public CedentesController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cedentes>>> GetCedentes()
        {
            var query = await service.GetCedentes();
            var items = await query.AsNoTracking().OrderBy(c => c.IdCedente).ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cedentes>> GetCedenteById(int id)
        {
            var item = await service.GetCedentesByIdCedente(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Cedentes>> CreateCedente([FromBody] Cedentes cedente)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateCedentes(cedente);
            return CreatedAtAction(nameof(GetCedenteById), new { id = created.IdCedente }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Cedentes>> UpdateCedente(int id, [FromBody] Cedentes cedente)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != cedente.IdCedente)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdCedente enviado no corpo.");
            }

            var existing = await service.GetCedentesByIdCedente(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateCedentes(id, cedente);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCedente(int id)
        {
            var existing = await service.GetCedentesByIdCedente(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteCedentes(id);
            return NoContent();
        }
    }
}

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelefonesClienteController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public TelefonesClienteController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TelefonesCliente>>> GetTelefonesCliente()
        {
            var query = await service.GetTelefonesCliente();
            var items = await query.AsNoTracking().OrderBy(c => c.IdTelefone).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TelefonesCliente>> GetTelefoneById(long id)
        {
            var item = await service.GetTelefonesClienteByIdTelefone(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<TelefonesCliente>> CreateTelefone([FromBody] TelefonesCliente telefone)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateTelefonesCliente(telefone);
            return CreatedAtAction(nameof(GetTelefoneById), new { id = created.IdTelefone }, created);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<TelefonesCliente>> UpdateTelefone(long id, [FromBody] TelefonesCliente telefone)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != telefone.IdTelefone)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdTelefone enviado no corpo.");
            }

            var existing = await service.GetTelefonesClienteByIdTelefone(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateTelefonesCliente(id, telefone);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteTelefone(long id)
        {
            var existing = await service.GetTelefonesClienteByIdTelefone(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteTelefonesCliente(id);
            return NoContent();
        }
    }
}

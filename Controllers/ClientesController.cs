using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public ClientesController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
        {
            var query = await service.GetClientes();
            var items = await query.AsNoTracking().OrderBy(c => c.IdCliente).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Clientes>> GetClienteById(long id)
        {
            var item = await service.GetClientesByIdCliente(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Clientes>> CreateCliente([FromBody] Clientes cliente)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateClientes(cliente);
            return CreatedAtAction(nameof(GetClienteById), new { id = created.IdCliente }, created);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<Clientes>> UpdateCliente(long id, [FromBody] Clientes cliente)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != cliente.IdCliente)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdCliente enviado no corpo.");
            }

            var existing = await service.GetClientesByIdCliente(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateClientes(id, cliente);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            var existing = await service.GetClientesByIdCliente(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteClientes(id);
            return NoContent();
        }
    }
}

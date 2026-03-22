using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecosClienteController : ControllerBase
    {
        private readonly sql_rendimento_consignadoService service;

        public EnderecosClienteController(sql_rendimento_consignadoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnderecosCliente>>> GetEnderecosCliente()
        {
            var query = await service.GetEnderecosCliente();
            var items = await query.AsNoTracking().OrderBy(c => c.IdEndereco).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<EnderecosCliente>> GetEnderecoById(long id)
        {
            var item = await service.GetEnderecosClienteByIdEndereco(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<EnderecosCliente>> CreateEndereco([FromBody] EnderecosCliente endereco)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = await service.CreateEnderecosCliente(endereco);
            return CreatedAtAction(nameof(GetEnderecoById), new { id = created.IdEndereco }, created);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<EnderecosCliente>> UpdateEndereco(long id, [FromBody] EnderecosCliente endereco)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (id != endereco.IdEndereco)
            {
                return BadRequest("O identificador da rota deve ser igual ao IdEndereco enviado no corpo.");
            }

            var existing = await service.GetEnderecosClienteByIdEndereco(id);
            if (existing == null)
            {
                return NotFound();
            }

            var updated = await service.UpdateEnderecosCliente(id, endereco);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteEndereco(long id)
        {
            var existing = await service.GetEnderecosClienteByIdEndereco(id);
            if (existing == null)
            {
                return NotFound();
            }

            await service.DeleteEnderecosCliente(id);
            return NoContent();
        }
    }
}

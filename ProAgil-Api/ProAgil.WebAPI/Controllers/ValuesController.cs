using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.WebAPI.Data;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ProAgilContext _context { get; }
        // public readonly DataContext Context; --> é a mesma coisa que a declaração acima
        public ValuesController(ProAgilContext context)
        {
            this._context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get() // com um metodo assincrono é criado uma threads para cada solicitação
        {
            try
            {
                var results = await _context.Eventos.ToListAsync();
                return Ok(results); //irá retonar a lista com todos os registros desse modelo que foi persistido no banco de dados
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await _context.Eventos.FirstOrDefaultAsync(x => x.Id == id);
                return Ok(results); //irá retonar a lista com todos os registros desse modelo que foi persistido no banco de dados
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }// irá retornar o registro que a PK for a mesma com a que foi informada
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            return null;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            return null;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return null;
        }
    }
}

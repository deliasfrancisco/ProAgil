using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProAgil.WebAPI.Controllers
{
	[Route("api/[controller]")]
	public class EventoController : ControllerBase
	{
		private readonly IProAgilRepository _repository;

		public EventoController(IProAgilRepository repository)
		{
			this._repository = repository;
		}

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repository.GetAllEventosAsyncBy(true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }
        }

        [HttpGet("getByEventoId{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var results = await _repository.GetAllEventosAsyncById(eventoId, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }
        }

        [HttpGet("getbyTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repository.GetAllEventosAsyncByTema(tema, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                _repository.Add(model);

				if (await _repository.SaveChangesAsync())
				{
                    return Created($"/api/evento/{model.EventoId}", model);
				}
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int eventoId, Evento model)
        {
            try
            {
                var evento = await _repository.GetAllEventosAsyncById(eventoId, false);

                if(evento == null)
                    return NotFound();

                _repository.Update(model);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.EventoId}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repository.GetAllEventosAsyncById(eventoId, false);

                if (evento == null)
                    return NotFound();

                _repository.Update(evento);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }

            return BadRequest();
        }
    }
}

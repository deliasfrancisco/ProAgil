using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProAgil.WebAPI.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
	public class EventoController : ControllerBase
	{
		private readonly IProAgilRepository _repository;
        private readonly IMapper _mapper;

		public EventoController(IProAgilRepository repository, IMapper mapper)
		{
			this._repository = repository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repository.GetAllEventosAsyncBy(true); // Lista de array
                var results = _mapper.Map<EventoDto[]>(eventos); // Add o IEnumerable quando o mapeamento receber como parametro uma lista

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Consulta falhou falhou {ex.Message}");
            }
        }

        [HttpGet("getByEventoId{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var evento = await _repository.GetAllEventosAsyncById(eventoId, true);
                var results = _mapper.Map<EventoDto>(evento);
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
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var results = _mapper.Map<Evento>(model);
                _repository.Add(results);

				if (await _repository.SaveChangesAsync())
				{
                    return Created($"/api/evento/{results.EventoId}", _mapper.Map<EventoDto>(model));
				}
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Consulta falhou falhou: {ex.Message}");
            }

            return BadRequest();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Put(EventoDto model)
        {
            try
            {
                var evento = await _repository.GetAllEventosAsyncById(model.EventoId, false);
                _mapper.Map(model, evento);

                if (evento == null)
                    return NotFound();

                _repository.Update(evento);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.EventoId}", _mapper.Map<EventoDto>(model));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou");
            }

            return BadRequest();
        }

        [HttpDelete("deletarById/{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repository.GetAllEventosAsyncById(eventoId, false);

                if (evento == null)
                    return NotFound();

                _repository.Delete(evento);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex + " Consulta falhou falhou");
            }

            return BadRequest();
        }
    }
}

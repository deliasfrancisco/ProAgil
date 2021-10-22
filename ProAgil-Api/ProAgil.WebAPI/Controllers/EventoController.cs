using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images"); // Diretorio que será armazenado as imagens
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName); // Combina o diretorio onde quer armazenar mais o diretorio atual da aplicação

				if (file.Length > 0)
				{
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", " ").Trim());

                    using (var stram  = new FileStream(fullPath, FileMode.Create))
					{
                        file.CopyTo(stram);
					}
				}

                return Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no upload da aplicação {ex.Message}");
            }
            return BadRequest("Erro ao executar do upload da aplicação");
        }

        [HttpGet("getByEventoId/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var evento = await _repository.GetAllEventosAsyncById(id, true);
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
                    return Created($"/api/evento/{results.Id}", _mapper.Map<EventoDto>(model));
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
				var evento = await _repository.GetAllEventosAsyncById(model.Id, false);

				//var evento = await _repository.GetAllEventosAsyncById(model.Id, false);
				//if (evento == null) return NotFound();

				//var idLotes = new List<int>();
				//var idRedesSociais = new List<int>();

				//model.Lotes.ForEach(item => idLotes.Add(item.Id));
				//model.RedesSociais.ForEach(item => idLotes.Add(item.Id));

				//var lotes = evento.Lotes.Where(x => !idLotes.Contains(x.Id)).ToList<Lote>();
				//var redesSociais = evento.RedesSociais.Where(x => !idRedesSociais.Contains(x.Id)).ToList<RedeSocial>();

				//if (lotes.Count > 0) _repository.DeleteRange(lotes.ToArray());

				//if (redesSociais.Count > 0) _repository.DeleteRange(redesSociais.ToArray());

				_mapper.Map(model, evento);

				if (evento == null)
					return NotFound();

				_repository.Update(evento);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(model));
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Consulta falhou falhou" + ex);
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

using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.WebAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAgil.Repository
{
	public class ProAgilRepository : IProAgilRepository
	{
		private readonly ProAgilContext _context;

		public ProAgilRepository(ProAgilContext context)
		{
			this._context = context;
			this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		public void Add<T>(T entity) where T : class
		{
			this._context.Add(entity);
		}

		public void Update<T>(T entity) where T : class
		{
			_context.Update(entity);
		}

		public void Delete<T>(T entity) where T : class
		{
			_context.Remove(entity);
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync()) > 0; // Como retorna um bool, ele retornará verdadeiro se for maior que 0
		}

		public async Task<Evento[]> GetAllEventosAsyncBy(bool includePalestrantes = false)
		{
			IQueryable<Evento> query = _context.Eventos
				.Include(e => e.Lotes)
				.Include(e => e.RedesSociais);

			if (includePalestrantes)
			{
				query = query
					.Include(p => p.PalestrantesEventos) // se for verdadeiro irá incluir o palestrante porem somente o id
					.ThenInclude(p => p.Palestrante); // então inclua tambem o palestrante
			}

			query = query.AsNoTracking()
				.OrderByDescending(c => c.DataEvento);
			return await query.ToArrayAsync();
		}

		public async Task<Evento> GetAllEventosAsyncById(int EventoId, bool includePalestrantes)
		{
			IQueryable<Evento> query = _context.Eventos
				.Include(e => e.Lotes)
				.Include(e => e.RedesSociais);

			if (includePalestrantes)
			{
				query = query.AsNoTracking()
					.Include(p => p.PalestrantesEventos) // se for verdadeiro irá incluir o palestrante porem somente o id
					.ThenInclude(p => p.Palestrante); // então inclua tambem o palestrante
			}

			query = query.OrderByDescending(c => c.DataEvento).Where(c => c.EventoId == EventoId);
			return await query.FirstOrDefaultAsync();
		}

		public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes)
		{
			IQueryable<Evento> query = _context.Eventos
				.Include(e => e.Lotes)
				.Include(e => e.RedesSociais);

			if (includePalestrantes)
			{
				query = query.AsNoTracking()
					.Include(p => p.PalestrantesEventos) // se for verdadeiro irá incluir o palestrante porem somente o id
					.ThenInclude(p => p.Palestrante); // então inclua tambem o palestrante
			}

			query = query.OrderByDescending(c => c.DataEvento).Where(c => c.Tema.Contains(tema));
			return await query.ToArrayAsync();
		}

		public async Task<Palestrante> GetAllPalestranteAsync(int PalestranteId, bool includeEventos = false)
		{
			IQueryable<Palestrante> query = _context.Palestrantes
				.Include(e => e.RedesSociais);

			if (includeEventos)
			{
				query = query.AsNoTracking()
					.Include(e => e.PalestrantesEventos)
					.ThenInclude(e => e.Evento);
			}

			query = query.OrderBy(c => c.Nome).Where(p => p.Id == PalestranteId);
			return await query.FirstOrDefaultAsync();
		}

		public async Task<Palestrante[]> GetAllPalestranteAsyncByName(string name, bool includeEventos = false)
		{
			IQueryable<Palestrante> query = _context.Palestrantes
				.Include(e => e.RedesSociais);

			if (includeEventos)
			{
				query = query.AsNoTracking()
					.Include(e => e.PalestrantesEventos)
					.ThenInclude(e => e.Evento);
			}

			query = query.Where(p => p.Nome.ToLower().Contains(name.ToLower()));
			return await query.ToArrayAsync();
		}
	}
}

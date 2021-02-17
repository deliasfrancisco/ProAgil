using ProAgil.Domain;
using System.Threading.Tasks;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        // Geral
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveChangesAsync();

        // Listagem
        Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsyncBy(bool includePalestrantes);
        Task<Evento> GetAllEventosAsyncById(int EventoId, bool includePalestrantes);

        //Palestrante
        Task<Palestrante[]> GetAllPalestranteAsyncByName(string name, bool includeEventos);
        Task<Palestrante> GetAllPalestranteAsync(int PalestranteId, bool includeEventos);

    }
}
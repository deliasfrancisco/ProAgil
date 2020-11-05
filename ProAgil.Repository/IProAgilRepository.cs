namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        //Geral
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;

        Task<bool> SaveChangesAsync();

        //Eventos
        Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsyncBy(bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsyncById(int EventoId, bool includePalestrantes);

        //Palestrante
        Task<Evento[]> GetAllPalestranteAsyncByName(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllPalestranteAsync(string PalestranteId, bool includePalestrantes);

    }
}
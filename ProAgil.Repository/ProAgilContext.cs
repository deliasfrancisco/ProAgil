using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.WebAPI.Data
{
    public class ProAgilContext: DbContext
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base(options){}

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder ModelBuilder){
            ModelBuilder.Entity<PalestranteEvento>()
            .HasKey(p => new {p.EventoId, p.PalestranteId});   // especificando a relação de n para n e definindo quais são os identificadores
        }
    }
}
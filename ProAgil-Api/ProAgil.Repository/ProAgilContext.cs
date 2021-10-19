using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.WebAPI.Data
{
    public class ProAgilContext: IdentityDbContext<User,Role,int,
                                                   IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                   IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base(options){}

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(u => new { u.UserId, u.RoleId });

                userRole.HasOne(r => r.Role) // Não permite que seja criado um registro em useRoles com role vazia
                .WithMany(x => x.UserRoles)
                .HasForeignKey(s => s.RoleId)
                .IsRequired();

                userRole.HasOne(r => r.User) // Não permite que seja criado um registro em useRoles com user vazio
                .WithMany(x => x.UserRoles)
                .HasForeignKey(s => s.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<PalestranteEvento>()
            .HasKey(p => new {p.EventoId, p.PalestranteId});   // especificando a relação de n para n e definindo quais são os identificadores
        }
    }
}
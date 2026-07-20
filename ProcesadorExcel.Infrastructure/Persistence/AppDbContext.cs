using Microsoft.EntityFrameworkCore;
using ProcesadorExcel.Domain.Entities;

namespace ProcesadorExcel.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Docente> Docentes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Docente>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
                b.Property(e => e.Valor).IsRequired();
            });
        }
    }
}

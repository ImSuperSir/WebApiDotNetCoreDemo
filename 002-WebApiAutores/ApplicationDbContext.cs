using _002_WebApiAutores.Entidades;
using Microsoft.EntityFrameworkCore;

namespace _002_WebApiAutores
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDbContext> options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutorLibro>()  /*This is for an intermediate table, it represents a many to many relationship*/
                .HasKey(x => new { x.AutorId , x.LibroId});

        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        public DbSet<AutorLibro> AutorLibros { get; set; }

    }
}

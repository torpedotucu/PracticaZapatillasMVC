using Microsoft.EntityFrameworkCore;
using PracticaZapatillasMVC.Models;

namespace PracticaZapatillasMVC.Data
{
    public class ZapasContext:DbContext
    {
        public ZapasContext(DbContextOptions<ZapasContext> options)
            : base(options) { }

        public DbSet<Zapatilla> Zapatillas { get; set; }
        public DbSet<ImagenZapatillas> Imagenes { get; set; }
    }
}

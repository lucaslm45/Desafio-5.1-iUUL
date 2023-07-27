using Externo.Models;
using Microsoft.EntityFrameworkCore;

namespace Externo.Data
{
    /// <summary>
    /// Contexto da aplicação com todas as tabelas utilizadas
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }
        public DbSet<Cobranca> Cobrancas { get; set; }
        public DbSet<EmailMensagem> Emails { get; set; }
    }
}

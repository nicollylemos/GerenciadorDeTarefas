using Microsoft.EntityFrameworkCore;
using GerenciadorTarefasApp.Models;

namespace GerenciadorTarefasApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Cria a tabela Tarefas no banco de dados
        public DbSet<Tarefas> Tarefas { get; set; }
        
        
    }
}
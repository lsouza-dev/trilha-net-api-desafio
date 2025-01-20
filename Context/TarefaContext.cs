using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Context
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Status)
                .HasConversion<string>(); // Mapeia o enum para texto no banco
        }
    }
}
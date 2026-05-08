using Microsoft.EntityFrameworkCore;
using quizzz.Models;

namespace quizzz.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Указываем путь к твоему файлу базы
            optionsBuilder.UseSqlite("Data Source=QuizGame.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Автоматически добавляем админа при первом запуске
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "123",
                Role = "Admin"
            });
        }
    }
}
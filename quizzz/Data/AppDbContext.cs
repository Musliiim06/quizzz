using Microsoft.EntityFrameworkCore;
using quizzz.Models;

namespace quizzz.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.QuizResult> QuizResults { get; set; }

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

            // Seed some recent quiz results for demo purposes if none exist
            modelBuilder.Entity<Models.QuizResult>().HasData(
                new Models.QuizResult { Id = 1, UserId = 1, QuizName = "C# Advanced Concepts", Score = 92, CompletedAt = System.DateTime.UtcNow.AddHours(-2) },
                new Models.QuizResult { Id = 2, UserId = 1, QuizName = "Database Design Patterns", Score = 88, CompletedAt = System.DateTime.UtcNow.AddDays(-1) },
                new Models.QuizResult { Id = 3, UserId = 1, QuizName = "SOLID Principles", Score = 95, CompletedAt = System.DateTime.UtcNow.AddDays(-2) }
            );
        }
    }
}
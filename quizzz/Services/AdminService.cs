using quizzz.Data;
using quizzz.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace quizzz.Services
{
    public class AdminService
    {
        private readonly AppDbContext _context;

        public AdminService()
        {
            _context = new AppDbContext();
            // Гарантируем, что база данных создана
            _context.Database.EnsureCreated();
            // If no users except admin exist, seed some demo users
            if (!_context.Users.Any(u => u.Role != "Admin"))
            {
                var rnd = new System.Random();
                for (int i = 1; i <= 8; i++)
                {
                    var user = new User { Username = $"player{i}", Password = "pass", Role = "Player" };
                    _context.Users.Add(user);
                }
                _context.SaveChanges();

                // Create some demo quiz results for those users
                var users = _context.Users.Where(u => u.Role != "Admin").ToList();
                int idCounter = 10;
                foreach (var u in users)
                {
                    int count = rnd.Next(1, 6);
                    for (int j = 0; j < count; j++)
                    {
                        _context.QuizResults.Add(new Models.QuizResult
                        {
                            UserId = u.Id,
                            QuizName = (j % 2 == 0) ? "C# Advanced Concepts" : "Database Design Patterns",
                            Score = rnd.Next(60, 100),
                            CompletedAt = System.DateTime.UtcNow.AddHours(-rnd.Next(1, 200))
                        });
                    }
                }
                _context.SaveChanges();
            }
        }

        // --- ЛОГИКА ПОЛЬЗОВАТЕЛЕЙ ---

        // Вход в систему
        public User Login(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Получить всех пользователей (для панели админа или High Scores)
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Удалить участника по ID
        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null && user.Role != "Admin") // Админа удалять нельзя
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        // --- ЛОГИКА ВОПРОСОВ ---

        // Добавить вопрос
        public void AddQuestion(string text, string a, string b, string c, string d, string correct)
        {
            var question = new Question
            {
                Text = text,
                OptionA = a,
                OptionB = b,
                OptionC = c,
                OptionD = d,
                CorrectAnswer = correct
            };
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        // Удалить вопрос (если админ ошибся)
        public void DeleteQuestion(int questionId)
        {
            var question = _context.Questions.Find(questionId);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
        }

        // Получить все вопросы для игры
        public List<Question> GetAllQuestions()
        {
            return _context.Questions.ToList();
        }

        // --- ДАННЫЕ ДЛЯ DASHBOARD ---

        public int GetQuizzesCompletedCount(int userId)
        {
            return _context.QuizResults.Count(r => r.UserId == userId);
        }

        public double GetAverageScore(int userId)
        {
            var list = _context.QuizResults.Where(r => r.UserId == userId).ToList();
            if (list.Count == 0) return 0;
            return list.Average(r => r.Score);
        }

        public List<Models.QuizResult> GetRecentResults(int userId, int take = 10)
        {
            return _context.QuizResults.Where(r => r.UserId == userId).OrderByDescending(r => r.CompletedAt).Take(take).ToList();
        }
    }
}
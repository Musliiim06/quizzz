using quizzz.Data;
using quizzz.Models;
using System.Collections.Generic;
using System.Linq;

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
    }
}
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
        }

        // Вход в систему: проверяет логин и возвращает пользователя с его ролью
        public User Login(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Метод для админа: добавление нового вопроса в базу
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

        // Метод для игрока: получить все вопросы для теста
        public List<Question> GetAllQuestions()
        {
            return _context.Questions.ToList();
        }
    }
}
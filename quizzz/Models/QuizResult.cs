using System;

namespace quizzz.Models
{
    public class QuizResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string QuizName { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}

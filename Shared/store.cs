using System;
using System.Security.Cryptography;
using System.Web;

namespace Quiz.Shared
{
    public static class store
    {
        public static string Category { get; set; }
        public static List<QuizQuestion>? currentQuiz { get; set; }
        public static QuizAnswer[]? currentAnswers { get; set; }
        public static Random random = new Random();
    }

    public class QuizQuestion
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_Answer { get; set; }
        public string[] Incorrect_Answers { get; set; }

        public string[] AllAnswers { get; private set; }

        public void DecodeAll()
        {
            Category = HttpUtility.UrlDecode(Category);
            Type = HttpUtility.UrlDecode(Type);
            Difficulty = HttpUtility.UrlDecode(Difficulty);
            Question = HttpUtility.UrlDecode(Question);
            Correct_Answer = HttpUtility.UrlDecode(Correct_Answer);
            for(int i = 0; i < Incorrect_Answers.Length; i++)
            {
                Incorrect_Answers[i] = HttpUtility.UrlDecode(Incorrect_Answers[i]);
            }
        }
        public void CombineAnswers()
        {
            List<string> Answers = new List<string>();
            for(int i = 0; i < Incorrect_Answers.Length; i++)
            {
                Answers.Add(Incorrect_Answers[i]);
            }
            Answers.Add(Correct_Answer);
            AllAnswers = new string[Answers.Count];
            //True/False
            if(Answers.Count == 2 && Answers[0] == "True" || Answers[0] == "False")
            {
                AllAnswers = new string[2] {"True", "False" };
                return;
            }
            Answers.ForEach((answer) =>
            {
                bool found = false;
                while(!found)
                {
                    int r = store.random.Next(Answers.Count);
                    if (AllAnswers[r] == null)
                    {
                        AllAnswers[r] = answer;
                        found = true;
                    }
                }
            });
        }
    }

    public class QuizAnswer
    {
        public QuizQuestion Question { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }

        public QuizAnswer(QuizQuestion question, string answer)
        {
            Answer = answer;
            Question = question;
            if(Question.Correct_Answer == Answer) Correct = true;
            else Correct = false;
        }
    }
}

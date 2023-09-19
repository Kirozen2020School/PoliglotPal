using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PolyglotPal_KimRozenberg
{
    internal class ENG_HE_Sentence
    {
        public string instructions {  get; set; }
        public string correctAnswer { get; set; }
        public List<string> words { get; set; }

        public ENG_HE_Sentence(string instructions, string correctAnswer, List<string> words)
        {
            this.instructions = instructions;
            this.correctAnswer = correctAnswer;
            this.words = words;

            this.words = ShaffleWords(this.words);
        }

        private static List<string> ShaffleWords(List<string> words)
        {
            Random rng = new Random();

            int n = words.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = words[k];
                words[k] = words[n];
                words[n] = value;
            }
            return words;
        }

        public bool IsCorrectAnswer(List<string> ans)
        {
            string answer = "";
            for (int i = 0; i < ans.Count; i++)
            {
                answer += ans[i] + " ";
            }
            answer = answer.Substring(0, answer.Length - 1);

            return answer.Equals(this.correctAnswer);
        }
    }
}
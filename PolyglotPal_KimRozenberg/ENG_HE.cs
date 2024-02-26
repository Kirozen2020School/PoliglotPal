
namespace PolyglotPal_KimRozenberg
{
    internal class ENG_HE
    {
        //מחזיק את התרגום של המילה בשפה השנייה
        private string other { get; set; }
        //מחזיק את המילה באנגלית
        private string english { get; set; }

        public string ENGLISH { get => english; set => english = value; }
        public string OTHER { get => other; set => other = value; }

        public ENG_HE(string other, string english)
        {
            this.other = other;
            this.english = english;
        }
    }
}
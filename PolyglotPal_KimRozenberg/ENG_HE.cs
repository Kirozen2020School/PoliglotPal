
namespace PolyglotPal_KimRozenberg
{
    internal class ENG_HE
    {
        //מחזיק את התרגום של המילה בשפה השנייה
        public string HE { get; set; }
        //מחזיק את המילה באנגלית
        public string ENG { get; set; }

        public ENG_HE(string hE, string eNG)
        {
            HE = hE;
            ENG = eNG;
        }
    }
}
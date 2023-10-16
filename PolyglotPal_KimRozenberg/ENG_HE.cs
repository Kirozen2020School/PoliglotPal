using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolyglotPal_KimRozenberg
{
    internal class ENG_HE
    {
        public string HE { get; set; }
        public string ENG { get; set; }

        public ENG_HE(string hE, string eNG)
        {
            HE = hE;
            ENG = eNG;
        }
    }
}
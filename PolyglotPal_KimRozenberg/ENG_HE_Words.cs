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
    internal class ENG_HE_Words
    {
        public string ENGword { get; set; }
        public string HEword {  get; set; }

        public ENG_HE_Words(string eNGword, string hEword)
        {
            ENGword = eNGword;
            HEword = hEword;
        }

    }
}
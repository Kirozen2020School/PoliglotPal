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
    internal class Account
    {
        public string username {  get; set; }
        public string lastname {  get; set; }
        public string firstname { get; set; }
        public int totalex {  get; set; }
        public int totaltasks {  get; set; }
        public string datejoining { get; set; }
        public string prorfilepic { get; set; }
        public string backgroundpic { get; set; }
    }
}
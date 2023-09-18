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
    [Activity(Label = "activity_ProfilePage")]
    public class activity_ProfilePage : Activity
    {
        Button btnGotToTaskPageFromProfilePage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_ProfilePage);
            // Create your application here
        }
    }
}
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
    [Activity(Label = "activity_MainPage")]
    public class activity_MainPage : Activity
    {
        Button btnGoToProfilePageFromTaskPage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_MainPage);
            // Create your application here

            InitViews();
        }

        private void InitViews()
        {
            
        }
    }
}
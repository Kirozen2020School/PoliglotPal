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
    [Activity(Label = "activity_CreateTranslationToSentence")]
    public class activity_CreateTranslationToSentence : Activity
    {
        TextView tvSentence;
        Button btnCheck;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_CreateTranslationToASentence);
            // Create your application here

            InitViews();
        }

        private void InitViews()
        {
            tvSentence = FindViewById<TextView>(Resource.Id.tvSentence);

            btnCheck = FindViewById<Button>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            
        }
    }
}
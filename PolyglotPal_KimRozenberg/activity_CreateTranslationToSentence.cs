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
        Button btnCheck, btnClearAns;
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

            btnClearAns = FindViewById<Button>(Resource.Id.btnClearAns);
            btnCheck = FindViewById<Button>(Resource.Id.btnCheck);

            btnClearAns.Click += BtnClearAns_Click;
            btnCheck.Click += BtnCheck_Click;
        }

        private void BtnClearAns_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            
        }
    }
}
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
        ImageButton btnExitLevel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_CreateTranslationToASentence);
            // Create your application here

            InitViews();
            InitLevel();
        }

        private void InitLevel()
        {
            Random rnd = new Random();
            
        }

        private void InitViews()
        {
            tvSentence = FindViewById<TextView>(Resource.Id.tvSentence);

            btnClearAns = FindViewById<Button>(Resource.Id.btnClearAns);
            btnCheck = FindViewById<Button>(Resource.Id.btnCheck);

            btnClearAns.Click += BtnClearAns_Click;
            btnCheck.Click += BtnCheck_Click;

            btnExitLevel = FindViewById<ImageButton>(Resource.Id.btnExitFromTranslateSentence);
            btnExitLevel.Click += BtnExitLevel_Click;
        }

        private void BtnExitLevel_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
            Finish();
        }

        private void BtnClearAns_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            if (CheckLevel())
            {
                Random random = new Random();
                int id = random.Next(0, 1);

                if (id == 0)
                {

                    Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                    StartActivity(intent);
                    Finish();
                }
                else if (id == 1)
                {

                    Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                    StartActivity(intent);
                    Finish();
                }
            }
            else
            {
                Random random = new Random();
                int id = random.Next(0, 1);

                if (id == 0)
                {

                    Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                    StartActivity(intent);
                    Finish();
                }
                else if (id == 1)
                {

                    Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                    StartActivity(intent);
                    Finish();
                }
            }
        }

        private bool CheckLevel()
        {
            /*need to add check*/

            return false;
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_TaskWordToWord")]
    public class activity_TaskWordToWord : Activity
    {
        Button btnENG1, btnENG2, btnENG3, btnENG4, btnHE1, btnHE2, btnHE3, btnHE4;
        Button btnNextLevel;
        ImageButton btnExitLevel;

        List<ENG_HE_Words> words;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_TaskWordToWord);
            // Create your application here

            InitViews();
            InitWords();


        }

        private void InitWords()
        {
            words = new List<ENG_HE_Words>();
            ENG_HE_Words temp;
            using (var reader = new StreamReader(@"\Resources\OneToOneTranslateWord_HE_ENG.txt"))
            {
                while(reader.EndOfStream == false)
                {
                    var line = reader.ReadLine().Split(' ');
                    temp = new ENG_HE_Words(line[0], line[1]);
                }
            }
        }

        private void InitViews()
        {
            btnENG1 = FindViewById<Button>(Resource.Id.btnENG1);
            btnENG2 = FindViewById<Button>(Resource.Id.btnENG2);
            btnENG3 = FindViewById<Button>(Resource.Id.btnENG3);
            btnENG4 = FindViewById<Button>(Resource.Id.btnENG4);
            btnHE1 = FindViewById<Button>(Resource.Id.btnHE1);
            btnHE2 = FindViewById<Button>(Resource.Id.btnHE2);
            btnHE3 = FindViewById<Button>(Resource.Id.btnHE3);
            btnHE4 = FindViewById<Button>(Resource.Id.btnHE4);

            btnNextLevel = FindViewById<Button>(Resource.Id.btnNextLevel);
            btnNextLevel.Click += BtnNextLevel_Click;

            btnExitLevel = FindViewById<ImageButton>(Resource.Id.btnExitFromPairsPage);
            btnExitLevel.Click += BtnExitLevel_Click;
            
        }

        private void BtnExitLevel_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
            Finish();
        }

        private void BtnNextLevel_Click(object sender, EventArgs e)
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
}
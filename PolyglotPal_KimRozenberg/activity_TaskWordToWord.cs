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
using System.Web;
using System.Net;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_TaskWordToWord")]
    public class activity_TaskWordToWord : Activity
    {
        Button btnENG1, btnENG2, btnENG3, btnENG4, btnHE1, btnHE2, btnHE3, btnHE4;
        Button btnNextLevel;
        ImageButton btnExitLevel;
        Android.App.AlertDialog d;

        List<ENG_HE_Words> words;
        int xp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_TaskWordToWord);
            // Create your application here

            if(Intent.Extras != null)
            {
                xp = Intent.GetIntExtra("XP", 0);
            }

            InitViews();
            //InitWords();

            //InitButtons();
        }

        private void InitButtons()
        {
            Random random = new Random();

            List<string> ENGwords = new List<string>();
            List<string> HEwords = new List<string>();

            int[] id = new int[4] { (random.Next(0, 50)), (random.Next(50, 100)), (random.Next(100, 150)), (random.Next(150, 199)) };

            for (int i = 0; i < id.Length; i++)
            {
                ENGwords.Add(words[id[i]].ENGword);
                HEwords.Add(words[id[i]].HEword);
            }

            Random rng = new Random();
            //random order
            int n = ENGwords.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = ENGwords[k];
                ENGwords[k] = ENGwords[n];
                ENGwords[n] = value;
            }
            n = HEwords.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = HEwords[k];
                HEwords[k] = HEwords[n];
                HEwords[n] = value;
            }

            btnENG1.Text = ENGwords[0];
            btnENG2.Text = ENGwords[1];
            btnENG3.Text = ENGwords[2];
            btnENG4.Text = ENGwords[3];
            btnHE1.Text = HEwords[0];
            btnHE2.Text = HEwords[1];
            btnHE3.Text = HEwords[2];
            btnHE4.Text = HEwords[3];
        }

        private void InitWords()
        {
            words = new List<ENG_HE_Words>();
            string filePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "ENGandHEwords.txt");
            string test = @"C:\Users\rozen\OneDrive\Рабочий стол\PoliglotPal\PolyglotPal_KimRozenberg\ENGandHEwords.txt";
            using (var reader = new StreamReader(test))
            {
                while (reader.EndOfStream == false)
                {
                    var line = reader.ReadLine().Split(' ');
                    words.Add(new ENG_HE_Words(line[0], line[1]));
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
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Exit from Level?");
            builder.SetMessage("If you exit this level you will lose all your XP\nStill exit?");
            builder.SetCancelable(true);
            builder.SetPositiveButton("yes", OkAction);
            builder.SetNegativeButton("cancel", CancelAction);
            d = builder.Create();
            d.Show();
        }

        private void CancelAction(object sender, DialogClickEventArgs e)
        {
            Toast.MakeText(this, "Task continues", ToastLength.Long).Show();
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
            Finish();
        }

        private void BtnNextLevel_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int id = random.Next(0, 2);

            int round = Intent.GetIntExtra("Round", -1);
            if(round >= 10)
            {
                Intent intent = new Intent(this, typeof(activity_LevelFinish));
                intent.PutExtra("XP", xp + 10);
                StartActivity(intent);
                Finish();
            }

            if (id == 0)
            {

                Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                intent.PutExtra("XP", xp + 10);
                intent.PutExtra("Round", Intent.GetIntExtra("Round", -1) + 1);
                StartActivity(intent);
                Finish();
            }
            else if (id == 1)
            {

                Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                intent.PutExtra("XP", xp + 10);
                intent.PutExtra("Round", Intent.GetIntExtra("Round", -1) + 1);
                StartActivity(intent);
                Finish();
            }
        }
    }
}
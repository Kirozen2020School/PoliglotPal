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

        //List<Tuple<string, string>> words;
        List<ENG_HE> words;
        int xp;

        Button lastClickedButtonEng = null;
        Button lastClickedButtonHeb = null;

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
            HashSet<int> selectedIndices = new HashSet<int>();
            int numberOfCouples = 4;

            //List<Tuple<string, string>> selectedCouples = new List<Tuple<string, string>>();
            List<ENG_HE> selectedCouples = new List<ENG_HE>();

            while (selectedCouples.Count < numberOfCouples)
            {
                int randomIndex = random.Next(words.Count);

                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                    selectedCouples.Add(words[randomIndex]);
                }
            }

            btnENG1.Text = selectedCouples[0].ENG;
            btnHE1.Text = selectedCouples[2].HE;
            btnENG2.Text = selectedCouples[1].ENG;
            btnHE2.Text = selectedCouples[0].HE;
            btnENG3.Text = selectedCouples[2].ENG;
            btnHE3.Text = selectedCouples[3].HE;
            btnENG4.Text = selectedCouples[3].ENG;
            btnHE4.Text = selectedCouples[1].HE;
        }

        private void InitWords()
        {
            //words = new List<Tuple<string, string>>();
            words = new List<ENG_HE>();
            string filePath = Path.Combine(System.Environment.CurrentDirectory, "ENGandHEwords.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(' ');
                        if (parts.Length == 2)
                        {
                            string englishWord = parts[0];
                            string hebrewTranslation = parts[1];
                            //words.Add(new Tuple<string, string>(englishWord, hebrewTranslation));
                            words.Add(new ENG_HE(hebrewTranslation, englishWord));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
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

            btnENG1.Click += BtnENG_Click;
            btnENG2.Click += BtnENG_Click;
            btnENG3.Click += BtnENG_Click;
            btnENG4.Click += BtnENG_Click;

            btnHE1.Click += BtnHE_Click;
            btnHE2.Click += BtnHE_Click;
            btnHE3.Click += BtnHE_Click;
            btnHE4.Click += BtnHE_Click;
        }

        private void BtnENG_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (lastClickedButtonHeb != null)
            {
                foreach (var item in words)
                {
                    if(item.ENG.Equals(clickedButton.Text) && item.HE.Equals(lastClickedButtonHeb.Text))
                    {
                        Toast.MakeText(this, "Translations match!", ToastLength.Short).Show();
                    }
                }
                
                lastClickedButtonHeb = null;
            }
            else
            {
                lastClickedButtonEng = clickedButton;
            }
        }

        private void BtnHE_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (lastClickedButtonEng != null)
            {
                foreach (var item in words)
                {
                    if (item.ENG.Equals(lastClickedButtonEng.Text) && item.HE.Equals(clickedButton.Text))
                    {
                        Toast.MakeText(this, "Translations match!", ToastLength.Short).Show();
                    }
                }
                
                lastClickedButtonEng = null;
            }
            else
            {
                lastClickedButtonHeb = clickedButton;
            }
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
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            StartActivity(intent);
            Finish();
        }

        private void BtnNextLevel_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int id = random.Next(0, 2);

            if(Intent.GetIntExtra("Round", -1) >= 10)
            {
                Intent intent = new Intent(this, typeof(activity_LevelFinish));
                intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                intent.PutExtra("XP", xp + 10);
                StartActivity(intent);
                Finish();
            }

            if (id == 0)
            {

                Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                intent.PutExtra("XP", xp + 10);
                intent.PutExtra("Round", Intent.GetIntExtra("Round", -1) + 1);
                StartActivity(intent);
                Finish();
            }
            else if (id == 1)
            {

                Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                intent.PutExtra("XP", xp + 10);
                intent.PutExtra("Round", Intent.GetIntExtra("Round", -1) + 1);
                StartActivity(intent);
                Finish();
            }
        }
    }
}
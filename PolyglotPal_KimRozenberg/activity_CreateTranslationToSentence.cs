using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_CreateTranslationToSentence : Activity
    {
        TextView tvSentence;
        Button btnCheck, btnClearAns;
        ImageButton btnExitLevel;
        EditText etAnswer;
        Android.App.AlertDialog d;

        List<ENG_HE> sentences;

        int xp;
        string correct_answer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_CreateTranslationToASentence);
            // Create your application here

            if (Intent.Extras != null)
            {
                xp = Intent.GetIntExtra("XP", 0);
            }

            InitViews();
            //InitLevel();
        }

        private void InitLevel()
        {
            sentences = new List<ENG_HE>();
            string filePath = Path.Combine(System.Environment.CurrentDirectory, "ENGtoHEsentence.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    for (int i = 0; i < lines.Length; i += 2)
                    {
                        string englishSentence = lines[i];
                        string hebrewTranslation = lines[i + 1];
                        sentences.Add(new ENG_HE(hebrewTranslation,englishSentence));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Random random = new Random();
                int id = random.Next(0, sentences.Count);
                this.correct_answer = sentences[id].HE;
                tvSentence.Text = "Translate \"" + sentences[id].ENG + "\" to hebrew: ";
            }
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

            etAnswer = FindViewById<EditText>(Resource.Id.etAnswer);
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
            Toast.MakeText(this, "Task continues" ,ToastLength.Long).Show();
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            StartActivity(intent);
            Finish();
        }

        private void BtnClearAns_Click(object sender, EventArgs e)
        {
            etAnswer.Text = string.Empty;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            if (this.correct_answer.Equals(etAnswer.Text))
            {
                Toast.MakeText(this, "You are correct!", ToastLength.Long).Show();

                if (Intent.Extras != null)
                {
                    if (Intent.GetIntExtra("Round", -1) >= 10)
                    {
                        Intent intent = new Intent(this, typeof(activity_LevelFinish));
                        intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                        intent.PutExtra("XP", this.xp + 10);
                        StartActivity(intent);
                        Finish();
                    }
                }

                Random random = new Random();
                int id = random.Next(0, 1);

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
            else
            {

                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Level info");
                builder.SetMessage("Your answer was wrong\nThe correct answer is:"+this.correct_answer);
                builder.SetCancelable(true);
                builder.SetPositiveButton("Ok", NullFunction);
                d = builder.Create();
                d.Show();

                Random random = new Random();
                int id = random.Next(0, 1);

                if (id == 0)
                {

                    Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                    intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                    intent.PutExtra("XP", xp);
                    intent.PutExtra("Round", Intent.GetIntExtra("Round", -1) + 1);
                    StartActivity(intent);
                    Finish();
                }
                else if (id == 1)
                {

                    Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                    intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                    intent.PutExtra("XP", xp);
                    intent.PutExtra("Round", Intent.GetIntExtra("Round", -1) + 1);
                    StartActivity(intent);
                    Finish();
                }
            }
        }

        private void NullFunction(object sender, DialogClickEventArgs e) { }
    }
}
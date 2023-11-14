using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_TaskWordToWord : Activity
    {
        Button btnENG1, btnENG2, btnENG3, btnENG4, btnHE1, btnHE2, btnHE3, btnHE4;
        Button btnNextLevel;
        ImageButton btnExitLevel;
        Android.App.AlertDialog d;
        ProgressBar progressBar;

        List<Button> buttons;
        List<ENG_HE> words;
        int xp;
        int addXP = 10;
        string mood;
        double errors;

        Button lastClickedButtonEng = null;
        Button lastClickedButtonHeb = null;

        string higthlight = "#27FF00";
        string goodColor = "#0FA70A";
        string badColor = "#629C60";
        string badColorText = "#575757";

        Timer time;

        bool isPlaying;
        ISharedPreferences sp;
        Intent music;


        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_TaskWordToWord);
            // Create your application here

            if(Intent.Extras != null)
            {
                xp = Intent.GetIntExtra("XP", 0);
                mood = Intent.GetStringExtra("Mood");
                errors = Intent.GetDoubleExtra("errors", 0);
            }

            InitViews();
            InitWords();
            InitButtons();

            if((Intent.GetIntExtra("Round", -1) == 1))
            {
                time = new Timer(TimeSpan.FromSeconds(1));
                time.Start();
            }
            else if (Intent.GetIntExtra("Round", -1) > 1)
            {
                long savedTicks = Intent.GetLongExtra("current_time", 0);
                TimeSpan savedTime = TimeSpan.FromTicks(savedTicks);
                
                time = new Timer(TimeSpan.FromSeconds(1));
                time.Start(savedTime);
            }
        }

        private void InitButtons()
        {
            Random random = new Random();
            int numberOfCouples = 4;

            List<ENG_HE> selectedCouples = new List<ENG_HE>();

            while (selectedCouples.Count < numberOfCouples)
            {
                int randomIndex = random.Next(words.Count);
                ENG_HE selectedCouple = words[randomIndex];

                // Check if the selected couple is not already in the list
                if (!selectedCouples.Contains(selectedCouple))
                {
                    selectedCouples.Add(selectedCouple);
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
            string name = "";
            switch (mood)
            {
                case "Daily":
                    name = "DailyActivity";
                    break;
                case "Family":
                    name = "Family";
                    break;
                case "Health":
                    name = "Health";
                    break;
                case "Hobbies":
                    name = "Hobbies";
                    break;
                case "Travel":
                    name = "Travel";
                    break;
                case "Business":
                    name = "Business";
                    break;
                case "Education":
                    name = "Education";
                    break;
                case "Food":
                    name = "Food";
                    break;
                case "Music":
                    name = "Music";
                    break;
                case "Animals":
                    name = "Animals";
                    break;
                case "Furniture":
                    name = "Furniture";
                    break;
                case "Emotions":
                    name = "Emotions";
                    break;
                case "Countries":
                    name = "Countries";
                    break;
                case "Tools":
                    name = "Tools";
                    break;
                case "Clothing":
                    name = "Clothing";
                    break;
            }
            words = new List<ENG_HE>();

            var tmp = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(activity_TaskWordToWord)).Assembly;

            System.IO.Stream s = tmp.GetManifestResourceStream($"PolyglotPal_KimRozenberg.{name}.txt");
            System.IO.StreamReader sr = new System.IO.StreamReader(s);
            string[] lines = sr.ReadToEnd().Split('\n');

            if(lines.Length > 0)
            {
                foreach(string line in lines)
                {
                    string[] parts = line.Split('-');
                    if (parts.Length == 2)
                    {
                        string englishWord = parts[0];
                        string hebrewTranslation = parts[1];
                        words.Add(new ENG_HE(hebrewTranslation, englishWord));
                    }
                }
            }
        }

        [Obsolete]
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

            buttons = new List<Button>();
            this.buttons.Add(btnENG1); this.buttons.Add(btnENG2); this.buttons.Add(btnENG3); this.buttons.Add(btnENG4);
            this.buttons.Add(btnHE1); this.buttons.Add(btnHE2); this.buttons.Add(btnHE3); this.buttons.Add(btnHE4);

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

            progressBar = FindViewById<ProgressBar>(Resource.Id.progress);
            int progress = ((Intent.GetIntExtra("Round", -1)-1)*100);
            progressBar.Progress = progress;
        }

        private void BtnENG_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            bool flag = true;
            if(lastClickedButtonEng != null)
            {
                lastClickedButtonEng.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                lastClickedButtonEng = null;
            }
            clickedButton.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(higthlight));

            if (lastClickedButtonHeb != null)
            {
                foreach (var item in words)
                {
                    if(item.ENG.Equals(clickedButton.Text) && item.HE.Equals(lastClickedButtonHeb.Text))
                    {
                        flag = false;
                        Toast.MakeText(this, "Translations match!", ToastLength.Short).Show();
                        foreach (Button button in buttons)
                        {
                            if (button.Equals(clickedButton))
                            {
                                button.Enabled = false;
                                ColorStateList colorStateList = ColorStateList.ValueOf(Color.ParseColor(badColor));
                                button.BackgroundTintList = colorStateList;
                                button.SetTextColor(Color.ParseColor(badColorText));
                            }
                            if (button.Equals(lastClickedButtonHeb))
                            {
                                button.Enabled = false;
                                ColorStateList colorStateList = ColorStateList.ValueOf(Color.ParseColor(badColor));
                                button.BackgroundTintList = colorStateList;
                                button.SetTextColor(Color.ParseColor(badColorText));
                            }
                        }
                        CheckIfEndLevel();
                        break;
                    }
                }
                if (flag)
                {
                    if(lastClickedButtonHeb != null)
                    {
                        lastClickedButtonHeb.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                    }
                    if(lastClickedButtonEng != null)
                    {
                        lastClickedButtonEng.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                    }
                    clickedButton.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                    this.errors++;
                }
                
                lastClickedButtonHeb = null;
                lastClickedButtonEng = null;
            }
            else
            {
                lastClickedButtonEng = clickedButton;
            }
        }

        private void BtnHE_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            bool flag = true;
            if(lastClickedButtonHeb != null)
            {
                lastClickedButtonHeb.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                lastClickedButtonHeb = null;
            }
            clickedButton.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(higthlight));

            if (lastClickedButtonEng != null)
            {
                foreach (var item in words)
                {
                    if (item.ENG.Equals(lastClickedButtonEng.Text) && item.HE.Equals(clickedButton.Text))
                    {
                        flag = false;
                        Toast.MakeText(this, "Translations match!", ToastLength.Short).Show();
                        foreach (Button button in buttons)
                        {
                            if (button.Equals(clickedButton))
                            {
                                button.Enabled = false;
                                ColorStateList colorStateList = ColorStateList.ValueOf(Color.ParseColor(badColor));
                                button.BackgroundTintList = colorStateList;
                                button.SetTextColor(Color.ParseColor(badColorText));
                            }
                            if (button.Equals(lastClickedButtonEng))
                            {
                                button.Enabled = false;
                                ColorStateList colorStateList = ColorStateList.ValueOf(Color.ParseColor(badColor));
                                button.BackgroundTintList = colorStateList;
                                button.SetTextColor(Color.ParseColor(badColorText));
                            }
                        }
                        CheckIfEndLevel();
                        break;
                    }
                }
                if (flag)
                {
                    addXP--;
                    if (lastClickedButtonHeb != null)
                    {
                        lastClickedButtonHeb.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                    }
                    if (lastClickedButtonEng != null)
                    {
                        lastClickedButtonEng.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                    }
                    clickedButton.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                    this.errors++;
                }
                lastClickedButtonEng = null;
                lastClickedButtonHeb = null;
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

        [Obsolete]
        private void BtnNextLevel_Click(object sender, EventArgs e)
        {
            Vibrator vibrator = (Vibrator)GetSystemService(VibratorService);
            if (vibrator.HasVibrator)
            {
                vibrator.Vibrate(500);
            }

            int round = Intent.GetIntExtra("Round", -1);
            if (round >= 7)//number of screens during one task
            {
                Intent intent = new Intent(this, typeof(activity_LevelFinish));
                intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                intent.PutExtra("XP", xp + addXP);
                intent.PutExtra("current_time", this.time.GetCurrentTime().Ticks);
                intent.PutExtra("errors", this.errors);
                StartActivity(intent);
                Finish();
            }
            else
            {
                Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                intent.PutExtra("XP", xp + addXP);
                intent.PutExtra("Round", round + 1);
                intent.PutExtra("Mood", mood);
                intent.PutExtra("current_time", this.time.GetCurrentTime().Ticks);
                intent.PutExtra("errors", this.errors);
                StartActivity(intent);
                Finish();
            }
        }
        private void CheckIfEndLevel()
        {
            bool flag = true;
            foreach (Button btn in buttons)
            {
                if (btn.Enabled == true)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                btnNextLevel.Enabled = true;
                ColorStateList colorStateList = ColorStateList.ValueOf(Color.ParseColor(goodColor));
                btnNextLevel.BackgroundTintList = colorStateList;
                btnNextLevel.SetTextColor(Color.ParseColor("#000000"));
            }
        }
    }
}
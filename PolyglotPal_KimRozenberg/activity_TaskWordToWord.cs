﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Content.PM;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_TaskWordToWord : Activity
    {
        Button btnENG1, btnENG2, btnENG3, btnENG4, btnENG5, btnOTHER1, btnOTHER2, btnOTHER3, btnOTHER4, btnOTHER5;
        Button btnNextLevel;
        ImageButton btnExitLevel;
        AlertDialog d;
        ProgressBar progressBar;

        List<Button> buttons;
        List<ENG_HE> words;
        int xp;
        int addXP = 10;
        string mood, language;
        double errors;

        Button lastClickedButtonEng = null;
        Button lastClickedButtonHeb = null;

        Timer time;
        
        [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override void OnCreate(Bundle savedInstanceState)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_TaskWordToWord);
            // Create your application here
            RequestedOrientation = ScreenOrientation.Portrait;
            if (Intent.Extras != null)
            {
                xp = Intent.GetIntExtra("XP", 0);
                mood = Intent.GetStringExtra("Mood");
                language = Intent.GetStringExtra("Language");
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
        //משנה את הטקסט של הכפתורים למילים שנבחרו
        private void InitButtons()
        {
            Random random = new Random();
            int numberOfCouples = 5;

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

            List<Button> btnENGList = new List<Button>
            {
                btnENG1, btnENG2, btnENG3, btnENG4, btnENG5
            };

            List<Button> btnOTHERList = new List<Button>
            {
                btnOTHER1, btnOTHER2, btnOTHER3, btnOTHER4, btnOTHER5
            };


            for (int i = 0; i < selectedCouples.Count; i++)
            {
                int random1 = random.Next(btnENGList.Count);
                btnENGList[random1].Text = selectedCouples[i].ENGLISH;
                int random2 = random.Next(btnOTHERList.Count);
                btnOTHERList[random2].Text = selectedCouples[i].OTHER;

                btnENGList.RemoveAt(random1);
                btnOTHERList.RemoveAt(random2);
            }

        }
        //מוריד ושומר את כל המילים הרלוונטיים למשימה
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

            if(language == "Russian") { name += "Ru"; }
            else if(language == "Ukrainian") { name += "Uk"; }
            else if (language == "Polish") { name += "Po"; }
            else if (language == "Germany") { name += "Ge"; }
            else if (language == "Yiddish") { name += "Yi"; }

            System.IO.Stream s = tmp.GetManifestResourceStream($"PolyglotPal_KimRozenberg.TextFiles.{name}.txt");
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
        //מאתחל את כל הפקדים שיש במסך
        [Obsolete]
        private void InitViews()
        {
            btnENG1 = FindViewById<Button>(Resource.Id.btnENG1);
            btnENG2 = FindViewById<Button>(Resource.Id.btnENG2);
            btnENG3 = FindViewById<Button>(Resource.Id.btnENG3);
            btnENG4 = FindViewById<Button>(Resource.Id.btnENG4);
            btnENG5 = FindViewById<Button>(Resource.Id.btnENG5);
            btnOTHER1 = FindViewById<Button>(Resource.Id.btnHE1);
            btnOTHER2 = FindViewById<Button>(Resource.Id.btnHE2);
            btnOTHER3 = FindViewById<Button>(Resource.Id.btnHE3);
            btnOTHER4 = FindViewById<Button>(Resource.Id.btnHE4);
            btnOTHER5 = FindViewById<Button>(Resource.Id.btnHE5);

            buttons = new List<Button>();
            this.buttons.Add(btnENG1); this.buttons.Add(btnENG2); this.buttons.Add(btnENG3); this.buttons.Add(btnENG4); this.buttons.Add(btnENG5);
            this.buttons.Add(btnOTHER1); this.buttons.Add(btnOTHER2); this.buttons.Add(btnOTHER3); this.buttons.Add(btnOTHER4); this.buttons.Add(btnOTHER5);

            btnNextLevel = FindViewById<Button>(Resource.Id.btnNextLevel);
            btnNextLevel.Click += BtnNextLevel_Click;

            btnExitLevel = FindViewById<ImageButton>(Resource.Id.btnExitFromPairsPage);
            btnExitLevel.Click += BtnExitLevel_Click;

            btnENG1.Click += BtnENG_Click;
            btnENG2.Click += BtnENG_Click;
            btnENG3.Click += BtnENG_Click;
            btnENG4.Click += BtnENG_Click;
            btnENG5.Click += BtnENG_Click;

            btnOTHER1.Click += BtnHE_Click;
            btnOTHER2.Click += BtnHE_Click;
            btnOTHER3.Click += BtnHE_Click;
            btnOTHER4.Click += BtnHE_Click;
            btnOTHER5.Click += BtnHE_Click;

            progressBar = FindViewById<ProgressBar>(Resource.Id.progress);
            int progress = ((Intent.GetIntExtra("Round", -1)-1)*100);
            progressBar.Progress = progress;
        }
        //כפתור המחזיק את המילה באנגלית
        private void BtnENG_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            bool flag = true;
            if(lastClickedButtonEng != null)
            {
                lastClickedButtonEng.SetBackgroundResource(Resource.Drawable.round_buttons);
                lastClickedButtonEng = null;
            }
            clickedButton.SetBackgroundResource(Resource.Drawable.active_round_buttons);

            if (lastClickedButtonHeb != null)
            {
                foreach (var item in words)
                {
                    if(item.ENGLISH.Equals(clickedButton.Text) && item.OTHER.Equals(lastClickedButtonHeb.Text))
                    {
                        flag = false;
                        foreach (Button button in buttons)
                        {
                            if (button.Equals(clickedButton))
                            {
                                button.Enabled = false;
                                button.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            }

                            lastClickedButtonHeb.Enabled = false;
                            lastClickedButtonHeb.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            if (button.Equals(lastClickedButtonHeb))
                            {
                                button.Enabled = false;
                                button.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            }
                        }
                        CheckIfEndLevel();
                        break;
                    }
                }
                if (flag)
                {
                    if (lastClickedButtonHeb != null)
                    {
                        lastClickedButtonHeb.SetBackgroundResource(Resource.Drawable.round_buttons);
                    }
                    if (lastClickedButtonEng != null)
                    {
                        lastClickedButtonEng.SetBackgroundResource(Resource.Drawable.round_buttons);
                    }
                    
                    clickedButton.SetBackgroundResource(Resource.Drawable.round_buttons);
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
        //כפתור המחזיק את המילה בשפה הנלמדת
        private void BtnHE_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            bool flag = true;
            if(lastClickedButtonHeb != null)
            {
                lastClickedButtonHeb.SetBackgroundResource(Resource.Drawable.round_buttons);
                lastClickedButtonHeb = null;
            }
            clickedButton.SetBackgroundResource(Resource.Drawable.active_round_buttons);

            if (lastClickedButtonEng != null)
            {
                foreach (var item in words)
                {
                    if (item.ENGLISH.Equals(lastClickedButtonEng.Text) && item.OTHER.Equals(clickedButton.Text))
                    {
                        flag = false;
                        foreach (Button button in buttons)
                        {
                            if (button.Equals(clickedButton))
                            {
                                button.Enabled = false;
                                button.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            }

                            lastClickedButtonEng.Enabled = false;
                            lastClickedButtonEng.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            if (button.Equals(lastClickedButtonEng))
                            {
                                button.Enabled = false;
                                button.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
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
                        lastClickedButtonHeb.SetBackgroundResource(Resource.Drawable.round_buttons);
                    }
                    if (lastClickedButtonEng != null)
                    {
                        lastClickedButtonEng.SetBackgroundResource(Resource.Drawable.round_buttons);
                    }
                    clickedButton.SetBackgroundResource(Resource.Drawable.round_buttons);
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
        //כפתור יציאה מהמשימה בלי שמירה של הנקודות
        private void BtnExitLevel_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Exit from Level?");
            builder.SetMessage("If you exit this level you will lose all your XP\nStill exit?");
            builder.SetCancelable(true);
            builder.SetPositiveButton("yes", OkAction);
            builder.SetNegativeButton("cancel", CancelAction);
            d = builder.Create();
            d.Show();
        }
        //ביטול יציאה מהשלב
        private void CancelAction(object sender, DialogClickEventArgs e)
        {
            Toast.MakeText(this, "Task continues", ToastLength.Long).Show();
        }
        //יציאה מהשלב
        private void OkAction(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            intent.PutExtra("First", true);
            StartActivity(intent);
            Finish();
        }
        //כפתור מעבר למשימה הבאה
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
                var random = new Random();
                int index = random.Next(2);
                if (index == 0)
                {
                    Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                    intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                    intent.PutExtra("XP", xp + addXP);
                    intent.PutExtra("Round", round + 1);
                    intent.PutExtra("Mood", mood);
                    intent.PutExtra("current_time", this.time.GetCurrentTime().Ticks);
                    intent.PutExtra("errors", this.errors);
                    intent.PutExtra("Language", language);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    Intent intent = new Intent(this, typeof(activity_TaskWordToWordSound));
                    intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                    intent.PutExtra("XP", xp + addXP);
                    intent.PutExtra("Round", round + 1);
                    intent.PutExtra("Mood", mood);
                    intent.PutExtra("current_time", this.time.GetCurrentTime().Ticks);
                    intent.PutExtra("errors", this.errors);
                    intent.PutExtra("Language", language);
                    StartActivity(intent);
                    Finish();
                }
            }
        }
        //בודק אם המשתמש התאים את כל המילים, בתנאי שכן הכפתור למעבר למשימה הבאה "ניפתח"
        private void CheckIfEndLevel()
        {
            bool flag = true;
            foreach (var btn in buttons)
            {
                if (btn.Enabled == true)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                btnNextLevel.Enabled = true;
                btnNextLevel.SetBackgroundResource(Resource.Drawable.active_round_buttons);
                btnNextLevel.SetTextColor(Color.ParseColor("#000000"));
            }
        }
    }
}
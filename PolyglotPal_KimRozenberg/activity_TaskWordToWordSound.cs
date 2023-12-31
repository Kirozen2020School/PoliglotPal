﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_TaskWordToWordSound")]
    public class activity_TaskWordToWordSound : Activity
    {
        private int currentImageIndex = 0;
        private int[] imageResources = { Resource.Drawable.sound1, Resource.Drawable.sound2, Resource.Drawable.sound3 };
        private Handler handler;

        private Button btnHE1, btnHE2, btnHE3, btnHE4, btnHE5;
        private ImageButton btnENG1, btnENG2, btnENG3, btnENG4, btnENG5;

        private Button btnNextLevel;
        private ImageButton btnExitLevel;
        private AlertDialog d;
        private ProgressBar progressBar;

        private List<Button> heButtons;
        private List<ImageButton> engButtons;
        private List<ENG_HE> words;
        private int xp;
        private int addXP = 10;
        private string mood, language;
        private double errors;

        private ImageButton lastClickedButtonEng = null;
        private Button lastClickedButtonHeb = null;

        private Timer time;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_TaskWordToWordSound);
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

            if ((Intent.GetIntExtra("Round", -1) == 1))
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
        private void InitViews()
        {
            btnENG1 = FindViewById<ImageButton>(Resource.Id.btnENG1Sound);
            btnENG2 = FindViewById<ImageButton>(Resource.Id.btnENG2Sound);
            btnENG3 = FindViewById<ImageButton>(Resource.Id.btnENG3Sound);
            btnENG4 = FindViewById<ImageButton>(Resource.Id.btnENG4Sound);
            btnENG5 = FindViewById<ImageButton>(Resource.Id.btnENG5Sound);
            btnHE1 = FindViewById<Button>(Resource.Id.btnHE1Sound);
            btnHE2 = FindViewById<Button>(Resource.Id.btnHE2Sound);
            btnHE3 = FindViewById<Button>(Resource.Id.btnHE3Sound);
            btnHE4 = FindViewById<Button>(Resource.Id.btnHE4Sound);
            btnHE5 = FindViewById<Button>(Resource.Id.btnHE5Sound);

            engButtons = new List<ImageButton>();
            heButtons = new List<Button>();
            this.engButtons.Add(btnENG1); this.engButtons.Add(btnENG2); this.engButtons.Add(btnENG3); this.engButtons.Add(btnENG4); this.engButtons.Add(btnENG5);
            this.heButtons.Add(btnHE1); this.heButtons.Add(btnHE2); this.heButtons.Add(btnHE3); this.heButtons.Add(btnHE4); this.heButtons.Add(btnHE5);

            btnNextLevel = FindViewById<Button>(Resource.Id.btnNextLevelSound);
            btnNextLevel.Click += BtnNextLevel_Click;

            btnExitLevel = FindViewById<ImageButton>(Resource.Id.btnExitFromSoundPage);
            btnExitLevel.Click += BtnExitLevel_Click;

            btnENG1.Click += BtnENG_Click;
            btnENG2.Click += BtnENG_Click;
            btnENG3.Click += BtnENG_Click;
            btnENG4.Click += BtnENG_Click;
            btnENG5.Click += BtnENG_Click;

            btnHE1.Click += BtnHE_Click;
            btnHE2.Click += BtnHE_Click;
            btnHE3.Click += BtnHE_Click;
            btnHE4.Click += BtnHE_Click;
            btnHE5.Click += BtnHE_Click;

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressSound);
            int progress = ((Intent.GetIntExtra("Round", -1) - 1) * 100);
            progressBar.Progress = progress;
        }

        private void BtnENG_Click(object sender, EventArgs e)
        {
            ImageButton clickedButton = (ImageButton)sender;
            TextToSpeak(clickedButton.Tag.ToString());
            StartImageChange(clickedButton);
            bool flag = true;
            if (lastClickedButtonEng != null)
            {
                lastClickedButtonEng.SetBackgroundResource(Resource.Drawable.round_buttons);
                lastClickedButtonEng = null;
            }
            clickedButton.SetBackgroundResource(Resource.Drawable.active_round_buttons);

            if (lastClickedButtonHeb != null)
            {
                foreach (var item in words)
                {
                    if (item.ENG.Equals(clickedButton.Tag.ToString()) && item.HE.Equals(lastClickedButtonHeb.Text))
                    {
                        flag = false;
                        foreach (var button in engButtons)
                        {
                            if (button.Equals(clickedButton))
                            {
                                button.Enabled = false;
                                button.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            }

                            lastClickedButtonHeb.Enabled = false;
                            lastClickedButtonHeb.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
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
        private void BtnHE_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            bool flag = true;
            if (lastClickedButtonHeb != null)
            {
                lastClickedButtonHeb.SetBackgroundResource(Resource.Drawable.round_buttons);
                lastClickedButtonHeb = null;
            }
            clickedButton.SetBackgroundResource(Resource.Drawable.active_round_buttons);

            if (lastClickedButtonEng != null)
            {
                foreach (var item in words)
                {
                    if (item.ENG.Equals(lastClickedButtonEng.Tag.ToString()) && item.HE.Equals(clickedButton.Text))
                    {
                        flag = false;
                        foreach (Button button in heButtons)
                        {
                            if (button.Equals(clickedButton))
                            {
                                button.Enabled = false;
                                button.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
                            }

                            lastClickedButtonEng.Enabled = false;
                            lastClickedButtonEng.SetBackgroundResource(Resource.Drawable.clicked_round_buttons);
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
        private void CancelAction(object sender, DialogClickEventArgs e)
        {
            Toast.MakeText(this, "Task continues", ToastLength.Short).Show();
        }
        private void OkAction(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            intent.PutExtra("First", true);
            StartActivity(intent);
            Finish();
        }

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
                if(index == 0)
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

            if (language == "Russian") { name += "Ru"; }
            else if (language == "Ukrainian") { name += "Uk"; }
            else if (language == "Polish") { name += "Po"; }
            else if (language == "Germany") { name += "Ge"; }
            else if (language == "Yiddish") { name += "Yi"; }

            System.IO.Stream s = tmp.GetManifestResourceStream($"PolyglotPal_KimRozenberg.{name}.txt");
            System.IO.StreamReader sr = new System.IO.StreamReader(s);
            string[] lines = sr.ReadToEnd().Split('\n');

            if (lines.Length > 0)
            {
                foreach (string line in lines)
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

            btnENG1.Tag = selectedCouples[4].ENG;
            btnHE1.Text = selectedCouples[2].HE;
            btnENG2.Tag = selectedCouples[1].ENG;
            btnHE2.Text = selectedCouples[0].HE;
            btnENG3.Tag = selectedCouples[2].ENG;
            btnHE3.Text = selectedCouples[3].HE;
            btnENG4.Tag = selectedCouples[3].ENG;
            btnHE4.Text = selectedCouples[1].HE;
            btnENG5.Tag = selectedCouples[0].ENG;
            btnHE5.Text = selectedCouples[4].HE;
        }

        private void StartImageChange(ImageButton temp)
        {
            handler = new Handler();
            handler.PostDelayed(new Action(() =>
            {
                ChangeImage(temp);
                StartImageChange(temp);
            }), 200);


            handler.PostDelayed(new Action(() =>
            {
                StopImageChangeSequence();
            }), 2000);
        }
        private void StopImageChangeSequence()
        {
            handler.RemoveCallbacksAndMessages(null);
        }
        private void ChangeImage(ImageButton temp)
        {
            temp.SetImageResource(imageResources[currentImageIndex]);

            currentImageIndex = (currentImageIndex + 1) % imageResources.Length;
        }
        private async void TextToSpeak(string text)
        {
            await TextToSpeech.SpeakAsync(text);
        }

        private void CheckIfEndLevel()
        {
            bool flag = true;
            foreach (var btn in heButtons)
            {
                if (btn.Enabled == true)
                {
                    flag = false;
                }
            }
            foreach(var btn in engButtons)
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
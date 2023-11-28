﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using static Android.Views.View;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_MainPage : AppCompatActivity, IOnClickListener, PopupMenu.IOnMenuItemClickListener
    {
        ImageButton btnGoToProfilePageFromTaskPage, btnTask;
        ImageButton btnDailyActivity, btnTravel, btnHealth, btnHobbies, btnFamily, btnBusiness, btnEducation,
            btnFood, btnMusic, btnAnimals, btnFurniture, btnEmotions, btnCountries, btnTools, btnClothing;
        List<Tuple<ImageButton, string>> buttons;
        ImageButton btnPopupMenu;
        TextView tvHiUsernameHomePage, tvTotalPointsHomePage;
        LinearLayout lyInfoMainPage, lyButtomMenuMainPage, lyBackgroundMainPage;

        string username;
        Account user;
        FirebaseManager firebase;
        ColorsClass colors = new ColorsClass();

        bool isPlaying;
        ISharedPreferences sp;
        Intent music;

        int xpAdded = -1;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_MainPage);
            // Create your application here

            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
                this.xpAdded = Intent.GetIntExtra("XP", -1);
            }

            firebase = new FirebaseManager();
            if(this.xpAdded != -1)
            {
                await firebase.UpdateXP(username, this.xpAdded);
            }
            user = await firebase.GetAccount(this.username);
            
            InitViews();
            UpdateViews();
            if(this.user != null)
            {
                UpdateColors();
                InitMusic();
            }
        }

        private void InitMusic()
        {
            this.music = new Intent(this, typeof(MusicService));
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            this.isPlaying = this.user.isPlaying;
            if (isPlaying)
            {
                StartService(music);
            }
        }
        private void UpdateViews()
        {
            UpdateColors();
            tvHiUsernameHomePage.Text = "Hi " + this.user.username;
            tvTotalPointsHomePage.Text = "Total points: " + this.user.totalxp;
        }
        private void UpdateColors()
        {
            switch (this.user.theme.ToString())
            {
                case "softBlue":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    foreach(var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));
                    }
                    break;
                case "softPink":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    foreach (var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));
                    }
                    break;
                case "blackRed":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    foreach (var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));
                    }
                    break;
                case "navy":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    foreach (var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));
                    }
                    break;
                default:

                    break;
            }
        }

        private void InitViews()
        {
            lyButtomMenuMainPage = FindViewById<LinearLayout>(Resource.Id.lyBottomLine);
            lyInfoMainPage = FindViewById<LinearLayout>(Resource.Id.lyInfoMainPage);
            lyBackgroundMainPage = FindViewById<LinearLayout>(Resource.Id.lyBackgroundMainPage);

            tvHiUsernameHomePage = FindViewById<TextView>(Resource.Id.tvHiUsernameHomePage);
            tvTotalPointsHomePage = FindViewById<TextView>(Resource.Id.tvTotalPointsHomePage);

            btnTask = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromTaskPage);
            btnPopupMenu = FindViewById<ImageButton>(Resource.Id.btnPopMenu);
            btnPopupMenu.SetOnClickListener(this);

            btnGoToProfilePageFromTaskPage = FindViewById<ImageButton>(Resource.Id.btnGoToProfilePageFromTaskPage);
            btnGoToProfilePageFromTaskPage.Click += BtnGoToProfilePageFromTaskPage_Click;

            buttons = new List<Tuple<ImageButton, string>>();
            btnDailyActivity = FindViewById<ImageButton>(Resource.Id.btnDailyActivity);
            buttons.Add(new Tuple<ImageButton, string>(btnDailyActivity, "Daily"));
            btnFamily = FindViewById<ImageButton>(Resource.Id.btnfamily);
            buttons.Add(new Tuple<ImageButton, string>(btnFamily, "Family"));
            btnHealth = FindViewById<ImageButton>(Resource.Id.btnHealth);
            buttons.Add(new Tuple<ImageButton, string>(btnHealth, "Health"));
            btnHobbies = FindViewById<ImageButton>(Resource.Id.btnHoobies);
            buttons.Add(new Tuple<ImageButton, string>(btnHobbies, "Hobbies"));
            btnTravel = FindViewById<ImageButton>(Resource.Id.btnTravel);
            buttons.Add(new Tuple<ImageButton, string>(btnTravel, "Travel"));
            btnBusiness = FindViewById<ImageButton>(Resource.Id.btnBusiness);
            buttons.Add(new Tuple<ImageButton, string>(btnBusiness, "Business"));
            btnEducation = FindViewById<ImageButton>(Resource.Id.btnEducation);
            buttons.Add(new Tuple<ImageButton, string>(btnEducation, "Education"));
            btnFood = FindViewById<ImageButton>(Resource.Id.btnFood);
            buttons.Add(new Tuple<ImageButton, string>(btnFood, "Food"));
            btnMusic = FindViewById<ImageButton>(Resource.Id.btnMusic);
            buttons.Add(new Tuple<ImageButton, string>(btnMusic, "Music"));
            btnAnimals = FindViewById<ImageButton>(Resource.Id.btnAnimals);
            buttons.Add(new Tuple<ImageButton, string>(btnAnimals, "Animals"));
            btnFurniture = FindViewById<ImageButton>(Resource.Id.btnFurniture);
            buttons.Add(new Tuple<ImageButton, string>(btnFurniture, "Furniture"));
            btnEmotions = FindViewById<ImageButton>(Resource.Id.btnEmotions);
            buttons.Add(new Tuple<ImageButton, string>(btnEmotions, "Emotions"));
            btnCountries = FindViewById<ImageButton>(Resource.Id.btnCountries);
            buttons.Add(new Tuple<ImageButton, string>(btnCountries, "Countries"));
            btnTools = FindViewById<ImageButton>(Resource.Id.btnTools);
            buttons.Add(new Tuple<ImageButton, string>(btnTools, "Tools"));
            btnClothing = FindViewById<ImageButton>(Resource.Id.btnClothing);
            buttons.Add(new Tuple<ImageButton, string>(btnClothing, "Clothing"));


            foreach (var button in buttons)
            {
                button.Item1.Click += BtnClick;
            }
        }

        private void BtnClick(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string mood = "";
            foreach(var tuple in buttons)
            {
                if (button.Equals(tuple.Item1))
                {
                    mood = tuple.Item2;
                }
            }

            Toast.MakeText(this, "Theme: "+mood, ToastLength.Short).Show();

            Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
            intent.PutExtra("Username", this.username);
            intent.PutExtra("XP", 0);
            intent.PutExtra("Round", 1);
            intent.PutExtra("Mood", mood);
            StartActivity(intent);
            StopService(this.music);
            Finish();
        }

        private void BtnGoToProfilePageFromTaskPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_ProfilePage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            //Finish();
        }

        public void OnClick(View v)
        {
            if(v.Id == btnPopupMenu.Id)
            {
                PopupMenu popup = new PopupMenu(this, v);
                MenuInflater inflater = popup.MenuInflater;
                inflater.Inflate(Resource.Menu.activity_menu, popup.Menu);

                popup.SetOnMenuItemClickListener(this);
                
                popup.Show();
            }
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_about)
            {
                Intent intent = new Intent(this, typeof(activity_InfoPage));
                intent.PutExtra("Username", this.username);
                StartActivity(intent);
                return true;
            }
            if (item.ItemId == Resource.Id.action_logout)
            {
                StopService(this.music);
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
                return true;
            }
            if (item.ItemId == Resource.Id.action_profile)
            {
                Intent intent = new Intent(this, typeof(activity_ProfilePage));
                intent.PutExtra("Username", this.username);
                StartActivity(intent);
                //Finish();
                return true;
            }
            if (item.ItemId == Resource.Id.action_settings)
            {
                Intent intent = new Intent(this, typeof(activity_Settings));
                intent.PutExtra("Username", this.username);
                StartActivity(intent);
                //Finish();
                return true;
            }

            return false;
        }
    }
}
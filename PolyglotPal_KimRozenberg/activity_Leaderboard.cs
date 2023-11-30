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
using static Android.Provider.CalendarContract;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_Leaderboard")]
    public class activity_Leaderboard : Activity
    {
        ImageButton btnGoToTaskPage, btnGoToProfilePage, btnLeaderboard;
        TextView tvUsername, tvTotalPonts;
        LinearLayout lyBackground, lyTopLine, lyBottomLine;

        string username, theme;
        Account user;
        FirebaseManager firebaseManager;
        List<Account> accounts;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Leaderboard);
            // Create your application here
            this.firebaseManager = new FirebaseManager();
            if(Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }
            InitViews();
            UpdateViews();
        }

        private async void UpdateViews()
        {
            try
            {
                this.user = await firebaseManager.GetAccount(this.username);
                this.accounts = await firebaseManager.GetAllUsers();
            }
            catch
            {
                Toast.MakeText(this, "Firebase Error", ToastLength.Short).Show();
            }

            if (this.user != null)
            {
                tvUsername.Text = this.user.username;
                tvTotalPonts.Text = "Total Points: " + this.user.totalxp;
                this.theme = this.user.theme;
                UpdateColors();
                if (this.accounts.Count > 0)
                {
                    /* Fill the leaderboard */
                }
            }
        }

        private void UpdateColors()
        {
            ColorsClass colors = new ColorsClass();
            switch (this.user.theme.ToString())
            {
                case "softBlue":
                    lyTopLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    lyBottomLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    lyBackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));

                    btnGoToProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    btnGoToTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    btnLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));

                    break;
                case "softPink":
                    lyTopLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    lyBottomLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    lyBackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));

                    btnGoToProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    btnGoToTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    btnLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    
                    break;
                case "blackRed":
                    lyTopLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    lyBottomLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    lyBackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));

                    btnGoToProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    btnGoToTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    btnLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    
                    break;
                case "navy":
                    lyTopLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    lyBottomLine.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    lyBackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));

                    btnGoToProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    btnGoToTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    btnLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    
                    break;
                default:
                    break;
            }
        }

        private void InitViews()
        {
            btnGoToProfilePage = FindViewById<ImageButton>(Resource.Id.btnGoToProfilePageFromLeaderBoardsPage);
            btnGoToProfilePage.Click += BtnGoToProfilePage_Click;
            btnGoToTaskPage = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromLeaderBoardsPage);
            btnGoToTaskPage.Click += BtnGoToTaskPage_Click;
            btnLeaderboard = FindViewById<ImageButton>(Resource.Id.btnGoToLeaderboardFromLeaderboardPage);

            tvUsername = FindViewById<TextView>(Resource.Id.tvUserNameLeaderBoardPage);
            tvTotalPonts = FindViewById<TextView>(Resource.Id.tvXPLeaderBoardPage);

            lyBackground = FindViewById<LinearLayout>(Resource.Id.lyLeaderBoardBackground);
            lyTopLine = FindViewById<LinearLayout>(Resource.Id.lyTopLineLeaderBoardPage);
            lyBottomLine = FindViewById<LinearLayout>(Resource.Id.lyBottomLineLeaderBoardPage);
        }

        private void BtnGoToProfilePage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_ProfilePage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }

        private void BtnGoToTaskPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }
    }
}
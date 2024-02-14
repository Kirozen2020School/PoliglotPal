using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.PM;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_Leaderboard")]
    public class activity_Leaderboard : Activity
    {
        ImageButton btnGoToTaskPage, btnGoToProfilePage, btnLeaderboard;
        TextView tvUsername, tvTotalPonts, tvCurrentPosition;
        LinearLayout lyBackground, lyTopLine, lyBottomLine;
        ListView lsLeaderboard;

        string username, theme;
        Account user;
        FirebaseManager firebaseManager;
        List<Account> accounts;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Leaderboard);
            // Create your application here
            RequestedOrientation = ScreenOrientation.Portrait;
            this.firebaseManager = new FirebaseManager();
            if(Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }
            InitViews();
            UpdateViews();
        }
        //מוסיך את כל המידע הרלונתי למסך
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
                tvUsername.Text = this.user.Username;
                tvTotalPonts.Text = "Total Points: " + this.user.TotalXP;
                this.theme = this.user.Theme;
                UpdateColors();
                if (this.accounts.Count > 0)
                {
                    this.accounts = SortByXP(this.accounts);

                    foreach (var account in this.accounts)
                    {
                        if (account.Username.Equals(this.username))
                        {
                            tvCurrentPosition.Text = "You'r current position: "+(this.accounts.IndexOf(account)+1);
                        }
                    }

                    var customAdaptor = new CustomAdapter(this, this.accounts, theme);
                    lsLeaderboard.Adapter = customAdaptor;


                }
            }
        }
        //משנה את צבעי המערכת לפי בחירת המשמש
        private void UpdateColors()
        {
            ColorsClass colors = new ColorsClass();
            switch (this.user.Theme.ToString())
            {
                case "softBlue":
                    lyTopLine.SetBackgroundColor(Color.ParseColor(colors.softBlue[2]));
                    lyBottomLine.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.softBlue[1]));

                    btnGoToProfilePage.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    btnGoToTaskPage.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    btnLeaderboard.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));

                    tvCurrentPosition.SetTextColor(Color.ParseColor("#000000"));

                    break;
                case "softPink":
                    lyTopLine.SetBackgroundColor(Color.ParseColor(colors.softPink[0]));
                    lyBottomLine.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.softPink[2]));

                    btnGoToProfilePage.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    btnGoToTaskPage.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    btnLeaderboard.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));

                    tvCurrentPosition.SetTextColor(Color.ParseColor("#000000"));

                    break;
                case "blackRed":
                    lyTopLine.SetBackgroundColor(Color.ParseColor(colors.blackRed[1]));
                    lyBottomLine.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.blackRed[0]));

                    btnGoToProfilePage.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    btnGoToTaskPage.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    btnLeaderboard.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));

                    tvCurrentPosition.SetTextColor(Color.ParseColor("#ffffff"));

                    break;
                case "navy":
                    lyTopLine.SetBackgroundColor(Color.ParseColor(colors.navy[1]));
                    lyBottomLine.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.navy[0]));

                    btnGoToProfilePage.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    btnGoToTaskPage.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    btnLeaderboard.SetBackgroundColor(Color.ParseColor(colors.navy[2]));

                    tvCurrentPosition.SetTextColor(Color.ParseColor("#ffffff"));


                    break;
                default:
                    tvCurrentPosition.SetTextColor(Color.ParseColor("#ffffff"));
                    break;
            }
        }
        //מאתחל את הפקדים בדף
        private void InitViews()
        {
            btnGoToProfilePage = FindViewById<ImageButton>(Resource.Id.btnGoToProfilePageFromLeaderBoardsPage);
            btnGoToProfilePage.Click += BtnGoToProfilePage_Click;
            btnGoToTaskPage = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromLeaderBoardsPage);
            btnGoToTaskPage.Click += BtnGoToTaskPage_Click;
            btnLeaderboard = FindViewById<ImageButton>(Resource.Id.btnGoToLeaderboardFromLeaderboardPage);

            tvUsername = FindViewById<TextView>(Resource.Id.tvUserNameLeaderBoardPage);
            tvTotalPonts = FindViewById<TextView>(Resource.Id.tvXPLeaderBoardPage);
            tvCurrentPosition = FindViewById<TextView>(Resource.Id.tvCurrentPositionLeaderboard);

            lyBackground = FindViewById<LinearLayout>(Resource.Id.lyLeaderBoardBackground);
            lyTopLine = FindViewById<LinearLayout>(Resource.Id.lyTopLineLeaderBoardPage);
            lyBottomLine = FindViewById<LinearLayout>(Resource.Id.lyBottomLineLeaderBoardPage);

            lsLeaderboard = FindViewById<ListView>(Resource.Id.lsLeaderboard);
        }
        //כפתור מעבר למסך הפרופיל
        private void BtnGoToProfilePage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_ProfilePage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }
        //כפתור מעבר למסך טבלת השיאים
        private void BtnGoToTaskPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }
        //מחזיר את רשימת המשתמשים לפי הסדר של הנקודות שיש למשתמש
        private List<Account> SortByXP(List<Account> accounts)
        {
            List<Account> sortedList = accounts.OrderByDescending(account => account.TotalXP).ToList();

            return sortedList;
        }
    }
}
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_Leaderboard")]
    public class activity_Leaderboard : Activity
    {
        ImageButton btnGoToTaskPage, btnGoToProfilePage, btnLeaderboard;
        TextView tvUsername, tvTotalPonts;
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
                    this.accounts = SortByXP(this.accounts);

                    var customAdaptor = new CustomAdapter(this, this.accounts);
                    lsLeaderboard.Adapter = customAdaptor;


                    //int index = 1;
                    //string background = "#000000";
                    //var containerLayout = new LinearLayout(this)
                    //{
                    //    LayoutParameters = new LinearLayout.LayoutParams(
                    //    ViewGroup.LayoutParams.MatchParent,
                    //    ViewGroup.LayoutParams.WrapContent
                    //    ),
                    //    Orientation = Orientation.Vertical
                    //};

                    //foreach (var account in this.accounts)
                    //{
                    //    Bitmap pic = ConvertByteArrayToBitmap(account.profilepic);
                    //    containerLayout.AddView(CreateCustomLinearLayout(account.username, account.totalxp, index, pic, background));
                    //    index++;
                    //}
                    
                }
            }
        }

        private LinearLayout CreateCustomLinearLayout(string username, int xp, int place, Bitmap pic, string background)
        {
            // Create the main LinearLayout
            var mainLinearLayout = new LinearLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent
            ),
                Orientation = Orientation.Horizontal
            };

            // Set background color using SetBackgroundColor
            mainLinearLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor(background));


            // Create the first TextView
            var textView1 = new TextView(this)
            {
                Text = place+"",
                LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent
                ),
                Gravity = GravityFlags.Center,
                TextSize = 20
            };

            // Create the ImageButton
            var imageButton = new ImageButton(this)
            {
                LayoutParameters = new ViewGroup.LayoutParams(50, 50)
            };

            // Set margin for the ImageButton
            //var layoutParams = (LinearLayout.LayoutParams)imageButton.LayoutParameters;
            //layoutParams.SetMargins(0, 0, 10, 0); // 10 pixels right margin
            //imageButton.LayoutParameters = layoutParams;

            // Create a Bitmap (replace this with your own Bitmap creation logic)
            //Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.Icon);

            // Set the Bitmap to the ImageButton
            imageButton.SetImageBitmap(pic);

            // Create the nested LinearLayout
            var nestedLinearLayout = new LinearLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent
                ),
                Orientation = Orientation.Vertical
            };

            // Create the second set of TextViews
            var textView2 = new TextView(this)
            {
                Text = username,
                Typeface = Typeface.DefaultBold,
                TextSize = 18
            };

            var textView3 = new TextView(this)
            {
                Text = "Total xp: "+xp,
                Typeface = Typeface.DefaultBold,
                TextSize = 18
            };

            // Add the views to the appropriate layouts
            mainLinearLayout.AddView(textView1);
            mainLinearLayout.AddView(imageButton);
            mainLinearLayout.AddView(nestedLinearLayout);
            nestedLinearLayout.AddView(textView2);
            nestedLinearLayout.AddView(textView3);

            return mainLinearLayout;
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

            lsLeaderboard = FindViewById<ListView>(Resource.Id.lsLeaderboard);
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

        private Bitmap ConvertByteArrayToBitmap(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return BitmapFactory.DecodeStream(stream);
            }
        }

        private List<Account> SortByXP(List<Account> accounts)
        {
            List<Account> sortedList = accounts.OrderByDescending(account => account.totalxp).ToList();

            return sortedList;
        }
    }
}
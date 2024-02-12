using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using Android.Content.PM;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_InfoPage : Activity
    {
        Button btnExitInfoPage;
        TextView tvInfo;
        LinearLayout lybackground, lyTop;

        FirebaseManager firebase;
        Account user;
        string username;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_InfoPage);
            // Create your application here
            RequestedOrientation = ScreenOrientation.Portrait;
            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }
            firebase = new FirebaseManager();
            InitViews();
            
        }
        //מוסיף את המידע על התוכנה
        private async void UpdateInfoText()
        {
            tvInfo.Text = "# About PolyglotPal - Your Language Learning Game\r\n\r\n" +
                "Welcome to PolyglotPal - where language learning meets gaming!\r\n\r\n" +
                "**Our Mission**: Transform language learning into an engaging adventure, making it enjoyable and educational for everyone.\r\n\r\n" +
                "**How It Works**: Play interactive mini-games to improve your vocabulary, listening, and speaking skills. Earn XP to track your progress.\r\n\r\n" +
                "**Get Started**: Join our global community of language learners. Download PolyglotPal and enjoy learning through play!\r\n\r\n" +
                "Thank you for choosing PolyglotPal for your language learning journey. Happy learning and gaming!";

            tvInfo.TextSize = 20;
            this.user = await firebase.GetAccount(this.username);
            UpdateColors();
        }
        //משנה את צבעי המערכת לפי בחירת המשמש
        private void UpdateColors()
        {
            ColorsClass colors = new ColorsClass();
            switch (this.user.theme.ToString())
            {
                case "softBlue":
                    lyTop.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));
                    break;

                case "softPink":
                    lyTop.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));
                    break;

                case "blackRed":
                    lyTop.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));
                    break;

                case "navy":
                    lyTop.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));
                    break;

                default:

                    break;
            }
        }
        //מאתחל את הפקדים בדף
        private void InitViews()
        {
            btnExitInfoPage = FindViewById<Button>(Resource.Id.btnExitFromInfoPage);
            btnExitInfoPage.Click += BtnExitInfoPage_Click;

            tvInfo = FindViewById<TextView>(Resource.Id.tvInfo);
            UpdateInfoText();

            lybackground = FindViewById<LinearLayout>(Resource.Id.lybackgroundInfoPage);
            lyTop = FindViewById<LinearLayout>(Resource.Id.lyTopLineInfoPage);

        }
        //כפתור יציאה ממסך המידע על התוכנה 
        private void BtnExitInfoPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            StartActivity(intent);
            Finish();
        }
    }
}
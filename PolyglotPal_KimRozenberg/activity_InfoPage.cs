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
            tvInfo.Text = "Welcome to PolyglotPal, your ultimate language companion!\r\n\r\n" +
                "PolyglotPal is a revolutionary program designed to enhance your vocabulary in multiple languages of your choice. " +
                "Whether you're a language enthusiast, a traveler, or someone looking to expand their linguistic skills, " +
                "PolyglotPal is here to guide you on your journey to fluency.\r\n\r\n" +
                "With PolyglotPal, you can seamlessly rephrase your vocabulary in various languages, " +
                "allowing you to communicate more effectively and confidently across different cultural contexts. Choose from a wide array of languages, " +
                "from Spanish to Mandarin, French to Arabic, and many more.\r\n\r\n" +
                "As you progress through different levels of proficiency, PolyglotPal rewards you with experience points (XP) to track your linguistic growth." +
                " Every level completed signifies a milestone in your language learning journey, unlocking new opportunities to master additional languages.\r\n\r\n" +
                "But the learning doesn't stop there! With the PolyglotPal Leaderboard feature, you can compare your progress with fellow users. " +
                "See where you stand among language learners worldwide, as users are ranked based on their accumulated XP. " +
                "Compete with friends, challenge yourself, and strive to reach the top of the leaderboard.\r\n\r\n" +
                "Whether you're aiming to become a polyglot or simply looking to improve your language skills, " +
                "PolyglotPal is your trusted companion every step of the way. " +
                "Start your linguistic adventure today and let PolyglotPal guide you towards multilingual mastery.";
            
            tvInfo.TextSize = 20;
            this.user = await firebase.GetAccount(this.username);
            UpdateColors();
        }
        //משנה את צבעי המערכת לפי בחירת המשמש
        private void UpdateColors()
        {
            ColorsClass colors = new ColorsClass();
            switch (this.user.Theme.ToString())
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
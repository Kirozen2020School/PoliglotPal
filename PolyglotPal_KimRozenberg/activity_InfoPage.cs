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

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_InfoPage")]
    public class activity_InfoPage : Activity
    {
        Button btnExitInfoPage;
        TextView tvInfo;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_InfoPage);
            // Create your application here

            InitViews();
            UpdateInfoText();
        }

        private void UpdateInfoText()
        {
            tvInfo.Text = "# About PolyglotPal - Your Language Learning Game\r\n\r\n" +
                "Welcome to PolyglotPal - where language learning meets gaming!\r\n\r\n" +
                "**Our Mission**: Transform language learning into an engaging adventure, making it enjoyable and educational for everyone.\r\n\r\n" +
                "**How It Works**: Play interactive mini-games to improve your vocabulary, listening, and speaking skills. Earn XP to track your progress.\r\n\r\n" +
                "**Meet Our Team**: We're a team of language enthusiasts and game developers dedicated to your language success.\r\n\r\n" +
                "**Get Started**: Join our global community of language learners. Download PolyglotPal and enjoy learning through play!\r\n\r\n" +
                "Thank you for choosing PolyglotPal for your language learning journey. Happy learning and gaming!";

            tvInfo.TextSize = 20;
            
        }

        private void InitViews()
        {
            btnExitInfoPage = FindViewById<Button>(Resource.Id.btnExitFromInfoPage);
            btnExitInfoPage.Click += BtnExitInfoPage_Click;

            tvInfo = FindViewById<TextView>(Resource.Id.tvInfo);
        }

        private void BtnExitInfoPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
            Finish();
        }
    }
}
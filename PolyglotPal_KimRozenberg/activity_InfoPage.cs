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
            tvInfo.Text = "";
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
        }
    }
}
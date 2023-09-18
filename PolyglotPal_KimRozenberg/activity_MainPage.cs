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
    [Activity(Label = "activity_MainPage")]
    public class activity_MainPage : Activity
    {
        ImageButton btnGoToProfilePageFromTaskPage;
        Button btnTask1, btnTask2, btnTask3, btnTask4, btnTask5, btnTask6, btnTask7, btnTask8, btnTask9;
        ImageButton btnInfo;
        TextView tvHiUsernameHomePage, tvTotalPointsHomePage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_MainPage);
            // Create your application here

            InitViews();
        }

        private void InitViews()
        {
            tvHiUsernameHomePage = FindViewById<TextView>(Resource.Id.tvHiUsernameHomePage);
            tvTotalPointsHomePage = FindViewById<TextView>(Resource.Id.tvTotalPointsHomePage);

            btnInfo = FindViewById<ImageButton>(Resource.Id.btnInfoMainPage);
            btnInfo.Click += BtnInfo_Click;
            btnGoToProfilePageFromTaskPage = FindViewById<ImageButton>(Resource.Id.btnGoToProfilePageFromTaskPage);
            btnGoToProfilePageFromTaskPage.Click += BtnGoToProfilePageFromTaskPage_Click;

            btnTask1 = FindViewById<Button>(Resource.Id.btnTask1);
            btnTask2 = FindViewById<Button>(Resource.Id.btnTask2);
            btnTask3 = FindViewById<Button>(Resource.Id.btnTask3);
            btnTask4 = FindViewById<Button>(Resource.Id.btnTask4);
            btnTask5 = FindViewById<Button>(Resource.Id.btnTask5);
            btnTask6 = FindViewById<Button>(Resource.Id.btnTask6);
            btnTask7 = FindViewById<Button>(Resource.Id.btnTask7);
            btnTask8 = FindViewById<Button>(Resource.Id.btnTask8);
            btnTask9 = FindViewById<Button>(Resource.Id.btnTask9);

            btnTask1.Click += BtnTask_Click;
            btnTask2.Click += BtnTask_Click;
            btnTask3.Click += BtnTask_Click;
            btnTask4.Click += BtnTask_Click;
            btnTask5.Click += BtnTask_Click;
            btnTask6.Click += BtnTask_Click;
            btnTask7.Click += BtnTask_Click;
            btnTask8.Click += BtnTask_Click;
            btnTask9.Click += BtnTask_Click;

        }

        private void BtnTask_Click(object sender, EventArgs e)
        {

        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            //pop up window for rulse and minimum manual for the user
        }

        private void BtnGoToProfilePageFromTaskPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_ProfilePage));
            StartActivity(intent);
        }
    }
}
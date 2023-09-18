using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText etUserName, etPassword;
        Button btnStart, btnCreateAccount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_Login);

            InitViews();
        }

        private void InitViews()
        {
            etUserName = FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnCreateAccount = FindViewById<Button>(Resource.Id.btnCreateAccount);

            btnStart.Click += BtnStart_Click;
            btnCreateAccount.Click += BtnCreateAccount_Click;
        }

        private void BtnCreateAccount_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_Register));
            StartActivity(intent);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            /*
             
            Check if there is an acoount in the firebase 

             */

            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
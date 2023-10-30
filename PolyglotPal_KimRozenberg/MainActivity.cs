using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText etUserName, etPassword;
        Button btnStart, btnCreateAccount;

        FirebaseManager firebase;
        List<Account> accounts;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_Login);

            InitViews();
        }

        async private void InitViews()
        {
            firebase = new FirebaseManager();
            try
            {
                accounts = await firebase.GetAllUsers();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Reading data from firebase error", ToastLength.Long).Show();
                
            }
            

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
            Finish();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            bool flag = false;
            //Check if there is an acoount in the firebase 
            if(accounts != null)
            {
                foreach (var account in accounts)
                {
                    if (etUserName.Text.Equals(account.username) && etPassword.Text.Equals(account.password))
                    {
                        flag = true;
                        Intent intent = new Intent(this, typeof(activity_MainPage));
                        intent.PutExtra("Username", etUserName.Text);
                        StartActivity(intent);
                        Finish();
                    }
                }
            }

            if (!flag)
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Login");
                builder.SetMessage("The Username or Password are incorect or not writen in our system\n" +
                    "Do you want to register or try again");
                builder.SetCancelable(true);
                builder.SetPositiveButton("Register", Register);
                builder.SetNegativeButton("Try again", TryAgain);
                builder.Show();
            }
        }

        private void TryAgain(object sender, DialogClickEventArgs e)
        {
            etPassword.Text = "";
            //etUserName.Text = "";
        }

        private void Register(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_Register));
            StartActivity(intent);
            Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
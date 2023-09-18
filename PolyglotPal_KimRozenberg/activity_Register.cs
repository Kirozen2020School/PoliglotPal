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
    [Activity(Label = "activity_Register")]
    public class activity_Register : Activity
    {
        EditText etFirstName, etLastName, etUserName, etPassword;
        Button btnCreatNewAccount;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Register);
            // Create your application here
            InitViews();
        }

        private void InitViews()
        {
            etFirstName = FindViewById<EditText>(Resource.Id.etFirstName);
            etLastName = FindViewById<EditText>(Resource.Id.etLastName);
            etUserName = FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            btnCreatNewAccount = FindViewById<Button>(Resource.Id.btnCreateNewAccount);

            btnCreatNewAccount.Click += BtnCreatNewAccount_Click;
        }

        private void BtnCreatNewAccount_Click(object sender, EventArgs e)
        {
            /*
             
            Creating new account and saving him in the firebase
             
             */

            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
        }
    }
}
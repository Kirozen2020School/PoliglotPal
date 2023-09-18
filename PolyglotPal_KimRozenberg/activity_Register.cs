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
        Button btnCreatNewAccount, btnCencle;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_RegisterPage);
            // Create your application here
            InitViews();
        }

        private void InitViews()
        {
            etFirstName = FindViewById<EditText>(Resource.Id.etFirstNameRegisterPage);
            etLastName = FindViewById<EditText>(Resource.Id.etLastNameRegisterPage);
            etUserName = FindViewById<EditText>(Resource.Id.etUserNameRegisterPage);
            etPassword = FindViewById<EditText>(Resource.Id.etPasswordRegisterPage);

            btnCreatNewAccount = FindViewById<Button>(Resource.Id.btnCreateNewAccountRegisterPage);
            btnCencle = FindViewById<Button>(Resource.Id.btnCencleRegisterPage);

            btnCreatNewAccount.Click += BtnCreatNewAccount_Click;
            btnCencle.Click += BtnCencle_Click;
        }

        private void BtnCencle_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
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
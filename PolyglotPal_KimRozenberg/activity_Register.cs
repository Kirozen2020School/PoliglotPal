using Android.App;
using Android.Content;
using Android.OS;
using Android.Graphics;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_Register")]
    public class activity_Register : Activity
    {
        EditText etFirstName, etLastName, etUserName, etPassword;
        Button btnCreatNewAccount, btnCencle;

        FirebaseManager firebase;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_RegisterPage);
            // Create your application here
            InitViews();
        }

        private void InitViews()
        {
            firebase = new FirebaseManager();

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
            Finish();
        }

        async private void BtnCreatNewAccount_Click(object sender, EventArgs e)
        {
            bool flag = true;
            List<Account> accounts = await firebase.GetAllUsers();

            if(accounts != null && accounts.Count > 0)
            {
                foreach(Account account in accounts)
                {
                    if (account.username.Equals(etUserName.Text))
                    {
                        Toast.MakeText(this, "The username is already in use, choose another one", ToastLength.Long).Show();
                        flag = false;
                    }
                }
            }
            if(flag)
            {
                string imageName = "ProfileIcon";
                int resourceId = Resources.GetIdentifier(imageName, "drawable", PackageName);
                Bitmap bitmap = BitmapFactory.DecodeResource(Resources, resourceId);

                Account user = new Account(etUserName.Text, etLastName.Text, etFirstName.Text, etPassword.Text, 0,0, DateTime.Now.ToString("d MMMM yyyy"), ConvertBitmapToByteArray(bitmap), "#13A90A");
                await firebase.AddAccount(user);


                Intent intent = new Intent(this, typeof(activity_MainPage));
                intent.PutExtra("Username", etUserName.Text);
                StartActivity(intent);
                Finish();
            }
        }

        private static byte[] ConvertBitmapToByteArray(Bitmap bm)
        {
            byte[] bytes;
            var stream = new MemoryStream();
            bm.Compress(Bitmap.CompressFormat.Png, 0, stream);
            bytes = stream.ToArray();
            return bytes;
        }
    }
}
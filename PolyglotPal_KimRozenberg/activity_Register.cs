using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using Android.Content.PM;
using Android.Graphics;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_Register : Activity
    {
        EditText etFirstName, etLastName, etUserName, etPassword;
        Button btnCreatNewAccount, btnCencle;
        Android.App.AlertDialog d;

        FirebaseManager firebase;
        List<Account> accounts;

        [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override async void OnCreate(Bundle savedInstanceState)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_RegisterPage);
            // Create your application here
            RequestedOrientation = ScreenOrientation.Portrait;
            InitViews();

            try
            {
                accounts = await firebase.GetAllUsers();
            }
            catch
            {
                Toast.MakeText(this, "Reading data from firebase error", ToastLength.Long);
            }
        }
        //מאתחל את הפקדים במסך
        [Obsolete]
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
        //כפתור מעבר למסך הכניסה
        private void BtnCencle_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
        //כפתור ליצירה של המשתמש ושמירה שלו ב Firebase
        [Obsolete]
        async private void BtnCreatNewAccount_Click(object sender, EventArgs e)
        {
            bool flag = true;

            if (IsInputValid(etUserName.Text) && IsInputValid(etFirstName.Text) && IsInputValid(etLastName.Text))
            {
                if (accounts != null && accounts.Count > 0)
                {
                    foreach (Account account in accounts)
                    {
                        if (account.Username.Equals(etUserName.Text))
                        {
                            Toast.MakeText(this, "The username is already in use, choose another one", ToastLength.Long).Show();
                            flag = false;
                        }
                    }
                }
                if (flag)
                {
                    Drawable drawable = Resources.GetDrawable(Resource.Drawable.blackprofile);
                    Bitmap bitmap = ((BitmapDrawable)drawable).Bitmap;
                    string date = DateTime.Now.ToString("d MMMM yyyy");
                    byte[] pic = ConvertBitmapToByteArray(bitmap);
                    
                    Account user = new Account(etUserName.Text,
                        etLastName.Text,
                        etFirstName.Text,
                        etPassword.Text,
                        0, 0, date,
                        pic, "", false, "Hebrow");

                    await firebase.AddAccount(user);


                    Intent intent = new Intent(this, typeof(activity_MainPage));
                    intent.PutExtra("Username", etUserName.Text);
                    StartActivity(intent);
                    Finish();
                }
            }
            else
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Account creation");
                builder.SetMessage("For creating account you need to fill every parametar using only letters (a -> z or A -> Z including \" \")");
                builder.SetCancelable(true);
                builder.SetPositiveButton("Ok", OkAction);
                d = builder.Create();
                d.Show();
            }
        }

        private void OkAction(object sender, DialogClickEventArgs e) { }
        //מעביר את התמונה של המשתמש ל byte[]
        private static byte[] ConvertBitmapToByteArray(Bitmap bm)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bm.Compress(Bitmap.CompressFormat.Png, 0, stream); // PNG format, quality 0 (max compression)
                return stream.ToArray();
            }
        }
        //בודק עם הערכים שהמשתמש הכניס נכונים
        private bool IsInputValid(string input)
        {
            input = input.ToLower();
            if(input.Length == 0)
            {
                return false;
            }
            foreach(char tav in input)
            {
                if((tav < 'a' || tav > 'z') && tav != ' ')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
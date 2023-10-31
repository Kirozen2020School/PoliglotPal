﻿using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_Settings")]
    public class activity_Settings : Activity
    {
        Button btnExitFromSettingPage, btnChangeTheme, btnChangeUsername, btnDeleteAccount;
        Switch swMusicBackground;

        Account user;
        string username;
        bool backMusic;

        private MediaPlayer player;
        private ISharedPreferences sp;

        FirebaseManager firebase;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Settings);

            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }

            // Create your application here
            InitViews();
        }

        private async void InitViews()
        {
            firebase = new FirebaseManager();
            user = await firebase.GetAccount(this.username);

            btnChangeTheme = FindViewById<Button>(Resource.Id.btnChangeTheme);
            btnExitFromSettingPage = FindViewById<Button>(Resource.Id.btnExitFromSettingsPage);
            btnChangeUsername = FindViewById<Button>(Resource.Id.btnChangeUsername);
            btnDeleteAccount = FindViewById<Button>(Resource.Id.btnDeleteAccountSettings);
            btnChangeUsername.Click += BtnChangeUsername_Click;
            btnExitFromSettingPage.Click += BtnExitFromSettingPage_Click;
            btnChangeTheme.Click += BtnChangeTheme_Click;
            btnDeleteAccount.Click += BtnDeleteAccount_Click;

            swMusicBackground = FindViewById<Switch>(Resource.Id.swMusic);
            swMusicBackground.CheckedChange += SwMusicBackground_CheckedChange;
        }

        private void BtnDeleteAccount_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Are you sure that you want to delete your account ?");
            builder.SetPositiveButton("Yes", async (sender, args) =>
            {
                await firebase.DeleteAccount(this.username);
                Toast.MakeText(this, "Account deleted", ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
                return;
            });
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                //null function
            });
            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        private void SwMusicBackground_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Switch sw = (Switch)sender;
            if (sw.Checked)
            {
                player = MediaPlayer.Create(this, Resource.Raw.background);
                player.Looping = true;
                player.SetVolume(100, 100);
                sp = this.GetSharedPreferences("details", FileCreationMode.Private);
                this.backMusic = true;
            }
            else
            {
                player.Stop();
                this.backMusic = false;
            }
        }

        private void BtnChangeTheme_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnExitFromSettingPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }

        private void BtnChangeUsername_Click(object sender, EventArgs e)
        {
            EditText userinput;

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Enter password");
            userinput = new EditText(this);
            userinput.InputType = Android.Text.InputTypes.NumberVariationPassword;
            builder.SetView(userinput);
            //builder.SetPositiveButton("Ok", ChangePassword(userinput.Text));
            builder.SetPositiveButton("OK", async (sender, args) =>
            {
                string inputText = userinput.Text;
                string pas = user.password;
                if (inputText.Equals(pas))
                {
                    EditText userinput1;

                    AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                    builder1.SetTitle("Enter new Username");
                    userinput1 = new EditText(this);
                    userinput1.InputType = Android.Text.InputTypes.NumberVariationPassword;
                    builder1.SetView(userinput1);
                    builder1.SetPositiveButton("OK", async (sender, args) =>
                    {
                        string inputText = userinput1.Text;
                        await firebase.UpdateUsername(user.username, inputText);
                        Toast.MakeText(this, $"Username change to {inputText}", ToastLength.Short).Show();

                        this.username = inputText;
                        this.user = await firebase.GetAccount(this.username);

                    });
                    builder1.SetNegativeButton("Cancel", (sender, args) =>
                    {
                        //null function
                    });

                    AlertDialog dialog1 = builder1.Create();
                    dialog1.Show();
                }

            });
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                //null function
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }
    }
}
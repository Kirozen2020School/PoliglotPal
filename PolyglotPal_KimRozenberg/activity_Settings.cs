using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Speech.Tts;
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
        Button btnExitFromSettingPage, btnChangeUsername, btnDeleteAccount;
        Switch swMusicBackground;
        Spinner spThemeSelector;
        LinearLayout ly, lyTopic;

        Account user;
        string username;
        bool backMusic;

        private MediaPlayer player;
        private ISharedPreferences sp;

        ColorsClass colors = new ColorsClass();

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

            btnExitFromSettingPage = FindViewById<Button>(Resource.Id.btnExitFromSettingsPage);
            btnChangeUsername = FindViewById<Button>(Resource.Id.btnChangeUsername);
            btnDeleteAccount = FindViewById<Button>(Resource.Id.btnDeleteAccountSettings);
            btnChangeUsername.Click += BtnChangeUsername_Click;
            btnExitFromSettingPage.Click += BtnExitFromSettingPage_Click;
            btnDeleteAccount.Click += BtnDeleteAccount_Click;

            swMusicBackground = FindViewById<Switch>(Resource.Id.swMusic);
            swMusicBackground.CheckedChange += SwMusicBackground_CheckedChange;

            spThemeSelector = FindViewById<Spinner>(Resource.Id.spThemeSelector);
            spThemeSelector.ItemSelected += SpThemeSelector_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ThemeSelector, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spThemeSelector.Adapter = adapter;

            ly = FindViewById<LinearLayout>(Resource.Id.lyBackgroundSettingsPage);
            lyTopic = FindViewById<LinearLayout>(Resource.Id.lySettingsTopic);
        }

        private async void SpThemeSelector_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var sp = (Spinner)sender;
            string theme = sp.GetItemAtPosition(e.Position).ToString();

            switch (theme)
            {
                case "Light Pink":
                    await firebase.UpdateTheme(this.user.username, "softPink");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));
                    break;
                case "Light Blue":
                    await firebase.UpdateTheme(this.user.username, "softBlue");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));
                    break;
                case "Red And Black":
                    await firebase.UpdateTheme(this.user.username, "blackRed");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));
                    break;
                case "Navy":
                    await firebase.UpdateTheme(this.user.username, "navy");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));
                    break;
                case "Dark":
                    await firebase.UpdateTheme(this.user.username, "");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor("#000"));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor("#000"));
                    break;
                default:
                    break;
            }
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
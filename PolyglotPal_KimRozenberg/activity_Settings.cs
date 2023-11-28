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
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Android.Telecom.Call;

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
        string theme;

        bool isPlaying;
        ISharedPreferences sp;
        Intent music;

        ColorsClass colors = new ColorsClass();

        FirebaseManager firebase;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Settings);

            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }


            InitViews();
            if(this.user != null)
            {
                this.theme = this.user.theme;
            }
            SetColor();
        }
        private void InitMusic()
        {
            this.music = new Intent(this, typeof(MusicService));
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            //isPlaying = Intent.GetBooleanExtra("music", false);
            this.isPlaying = this.user.isPlaying;
            swMusicBackground.Checked = isPlaying;
            //if (isPlaying)
            //{
            //    StartService(music);
            //}
        }

        [Obsolete]
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
            InitMusic();
            swMusicBackground.CheckedChange += SwMusicBackground_CheckedChange;
            

            spThemeSelector = FindViewById<Spinner>(Resource.Id.spThemeSelector);
            spThemeSelector.ItemSelected += SpThemeSelector_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ThemeSelector, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spThemeSelector.Adapter = adapter;

            ly = FindViewById<LinearLayout>(Resource.Id.lyBackgroundSettingsPage);
            lyTopic = FindViewById<LinearLayout>(Resource.Id.lySettingsTopic);
        }
        private async void UpdateColors()
        {
            string temp = "";
            if(this.theme != null)
            {
                temp = this.theme.ToUpper();
            }
            switch (temp)
            {
                case "LIGHT PINK":
                    await firebase.UpdateTheme(this.user.username, "softPink");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));
                    break;
                case "LIGHT BLUE":
                    await firebase.UpdateTheme(this.user.username, "softBlue");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));
                    break;
                case "BLACKRED":
                case "RED AND BLACK":
                    await firebase.UpdateTheme(this.user.username, "blackRed");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));
                    break;
                case "NAVY":
                    await firebase.UpdateTheme(this.user.username, "navy");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));
                    break;
                case "DARK":
                    await firebase.UpdateTheme(this.user.username, "");
                    lyTopic.SetBackgroundColor(Android.Graphics.Color.ParseColor("#000000"));
                    ly.SetBackgroundColor(Android.Graphics.Color.ParseColor("#000000"));
                    break;
                default:
                    break;
            }
        }

        [Obsolete]
        private async void SetColor()
        {
            ProgressDialog p = new ProgressDialog(this);
            p.SetTitle("Updating Data");
            p.SetMessage("Please wait...");
            p.SetCancelable(false);
            p.Show();
            UpdateColors();
            await Task.Delay(1000);
            p.Dismiss();
        }

        [Obsolete]
        private void SpThemeSelector_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var sp = (Spinner)sender;
            string temp = sp.GetItemAtPosition(e.Position).ToString();
            if(temp.Length > 0)
            {
                this.theme = temp;
            }
            else
            {
                if(this.user != null)
                {
                    this.theme = this.user.theme;
                }
            }
            SetColor();
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

        private async void SwMusicBackground_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Switch sw = (Switch)sender;
            isPlaying = sw.Checked;
            if (isPlaying)
            {
                StartService(music);
            }
            else
            {
                StopService(music);
            }
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutBoolean("IsPlaying", isPlaying);
            editor.Commit();
            await firebase.UpdateMusicValue(this.user.username, isPlaying);
        }

        private void BtnExitFromSettingPage_Click(object sender, EventArgs e)
        {
            //StopService(music);
            //Intent intent = new Intent(this, typeof(activity_MainPage));
            //intent.PutExtra("Username", this.username);
            //StartActivity(intent);
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode,
            permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode,
            permissions, grantResults);
        }
        protected override void OnDestroy()
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutBoolean("IsPlaying", false);
            editor.Commit();
            base.OnDestroy();
        }
    }
}
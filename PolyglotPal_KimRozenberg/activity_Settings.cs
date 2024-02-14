using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content.PM;

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

        bool isPlaying;
        ISharedPreferences sp;
        Intent music;

        ColorsClass colors = new ColorsClass();

        FirebaseManager firebase;

        [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override async void OnCreate(Bundle savedInstanceState)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Settings);
            RequestedOrientation = ScreenOrientation.Portrait;
            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }

            firebase = new FirebaseManager();
            this.user = await firebase.GetAccount(this.username);

            InitViews();
            UpdateColors();
        }
        //מתחיל את מוזיקת הרקע לפי ההגדרות של המשתמש
        private void InitMusic()
        {
            this.music = new Intent(this, typeof(MusicService));
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            this.isPlaying = this.user.IsPlaying;
            swMusicBackground.Checked = isPlaying;
        }
        //מאחל את כל הפקדים במסך
        [Obsolete]
        private void InitViews()
        {
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
            UpdateSpinnerSelection();

            ly = FindViewById<LinearLayout>(Resource.Id.lyBackgroundSettingsPage);
            lyTopic = FindViewById<LinearLayout>(Resource.Id.lySettingsTopic);
        }
        //מעדכן את הבחירה של צבעי המערכת לפי המידע על המשתמש
        private void UpdateSpinnerSelection()
        {
            switch (this.user.Theme.ToString().ToUpper())
            {
                case "SOFTPINK":
                case "LIGHT PINK":
                    spThemeSelector.SetSelection(1);
                    break;
                case "SOFTBLUE":
                case "LIGHT BLUE":
                    spThemeSelector.SetSelection(2);
                    break;
                case "BLACKRED":
                case "RED AND BLACK":
                    spThemeSelector.SetSelection(3);
                    break;
                case "NAVY":
                    spThemeSelector.SetSelection(4);
                    break;
                case "DARK":
                    spThemeSelector.SetSelection(0);
                    break;
                default:
                    spThemeSelector.SetSelection(0);
                    break;
            }
        }
        //שינוי צבעי מערכת לפי הבחירה של המשתמש
        private async void UpdateColors()
        {
            switch (this.user.Theme.ToString().ToUpper())
            {
                case "LIGHT PINK":
                    await firebase.UpdateTheme(this.user.Username, "softPink");
                    lyTopic.SetBackgroundColor(Color.ParseColor(colors.softPink[0]));
                    ly.SetBackgroundColor(Color.ParseColor(colors.softPink[2]));

                    swMusicBackground.SetTextColor(Color.ParseColor("#000000"));
                    btnChangeUsername.SetTextColor(Color.ParseColor("#000000"));
                    btnDeleteAccount.SetTextColor(Color.ParseColor("#000000"));

                    spThemeSelector.SetSelection(1);
                    break;
                case "LIGHT BLUE":
                    await firebase.UpdateTheme(this.user.Username, "softBlue");
                    lyTopic.SetBackgroundColor(Color.ParseColor(colors.softBlue[2]));
                    ly.SetBackgroundColor(Color.ParseColor(colors.softBlue[1]));

                    swMusicBackground.SetTextColor(Color.ParseColor("#ffffff"));
                    btnChangeUsername.SetTextColor(Color.ParseColor("#ffffff"));
                    btnDeleteAccount.SetTextColor(Color.ParseColor("#ffffff"));
                    break;
                case "BLACKRED":
                case "RED AND BLACK":
                    await firebase.UpdateTheme(this.user.Username, "blackRed");
                    lyTopic.SetBackgroundColor(Color.ParseColor(colors.blackRed[1]));
                    ly.SetBackgroundColor(Color.ParseColor(colors.blackRed[0]));

                    swMusicBackground.SetTextColor(Color.ParseColor("#ffffff"));
                    btnChangeUsername.SetTextColor(Color.ParseColor("#ffffff"));
                    btnDeleteAccount.SetTextColor(Color.ParseColor("#ffffff"));
                    break;
                case "NAVY":
                    await firebase.UpdateTheme(this.user.Username, "navy");
                    lyTopic.SetBackgroundColor(Color.ParseColor(colors.navy[1]));
                    ly.SetBackgroundColor(Color.ParseColor(colors.navy[0]));

                    swMusicBackground.SetTextColor(Color.ParseColor("#ffffff"));
                    btnChangeUsername.SetTextColor(Color.ParseColor("#ffffff"));
                    btnDeleteAccount.SetTextColor(Color.ParseColor("#ffffff"));
                    break;
                case "DARK":
                    await firebase.UpdateTheme(this.user.Username, "");
                    lyTopic.SetBackgroundColor(Color.ParseColor("#000000"));
                    ly.SetBackgroundColor(Color.ParseColor("#000000"));

                    swMusicBackground.SetTextColor(Color.ParseColor("#ffffff"));
                    btnChangeUsername.SetTextColor(Color.ParseColor("#ffffff"));
                    btnDeleteAccount.SetTextColor(Color.ParseColor("#ffffff"));
                    break;
                default:
                    break;
            }
        }

        [Obsolete]
        private void SpThemeSelector_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var sp = (Spinner)sender;
            string temp = sp.GetItemAtPosition(e.Position).ToString();
            if(temp.Length > 0)
            {
                this.user.Theme = temp;
                
            }
            
            UpdateColors();
        }
        //כפתור מחיקה של המשתמש
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
        //כפתור האחרי על הדלקת מוזיקת רקע
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
            await firebase.UpdateMusicValue(this.user.Username, isPlaying);
        }
        //כפתור יציאה ממסך ההגדרות למסך המשימות
        private void BtnExitFromSettingPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }
        //כפתור לשינוי שם המשתמש
        private void BtnChangeUsername_Click(object sender, EventArgs e)
        {
            EditText userinput;

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Enter password");
            userinput = new EditText(this);
            userinput.InputType = Android.Text.InputTypes.NumberVariationPassword;
            builder.SetView(userinput);
            builder.SetPositiveButton("OK", (sender, args) =>
            {
                string inputText = userinput.Text;
                string pas = user.Password;
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
                        await firebase.UpdateUsername(user.Username, inputText);
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

        protected override void OnDestroy()
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutBoolean("IsPlaying", false);
            editor.Commit();
            base.OnDestroy();
        }
    }
}
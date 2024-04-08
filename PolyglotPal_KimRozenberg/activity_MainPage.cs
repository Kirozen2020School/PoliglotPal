using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using Android.Content.PM;
using static Android.Views.View;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_MainPage : AppCompatActivity, IOnClickListener, PopupMenu.IOnMenuItemClickListener
    {
        ImageButton btnGoToProfilePageFromTaskPage, btnTask, btnGoToLeaderboard, btnSelectLanguage;
        ImageButton btnDailyActivity, btnTravel, btnHealth, btnHobbies, btnFamily, btnBusiness, btnEducation,
            btnFood, btnMusic, btnAnimals, btnFurniture, btnEmotions, btnCountries, btnTools, btnClothing;
        List<Tuple<ImageButton, string>> buttons;
        ImageButton btnPopupMenu;
        TextView tvHiUsernameHomePage, tvTotalPointsHomePage, tvFromEnglish;
        LinearLayout lyInfoMainPage, lyButtomMenuMainPage, lyBackgroundMainPage;

        string username;
        Account user;
        FirebaseManager firebase;
        ColorsClass colors = new ColorsClass();

        bool isPlaying;
        bool first;
        ISharedPreferences sp;
        Intent music;

        PopupWindow popupWindow;

        int xpAdded = -1;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_MainPage);
            // Create your application here
            RequestedOrientation = ScreenOrientation.Portrait;
            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
                this.xpAdded = Intent.GetIntExtra("XP", -1);
                try
                {
                    this.first = Intent.GetBooleanExtra("First", false);
                }
                catch
                {

                }
            }

            firebase = new FirebaseManager();
            if(this.xpAdded != -1)
            {
                await firebase.UpdateValue(username, FirebaseManager.Fields.Xp, xpAdded);
            }
            user = await firebase.GetAccount(this.username);
            
            InitViews();
            UpdateViews();
            if(this.user != null)
            {
                UpdateColors();
                InitMusic();
            }
        }
        //מאתחל את השרת של המוזיקה ומדליק אותה לפי הצורך
        private void InitMusic()
        {
            this.music = new Intent(this, typeof(MusicService));
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            this.isPlaying = this.user.IsPlaying;
            if (isPlaying)
            {
                if(this.first)
                {
                    StartService(music);
                }
            }
        }
        //משנה את הערכים של הטקסט בשביל להתאים למידע של המשתמש
        private void UpdateViews()
        {
            UpdateColors();
            tvHiUsernameHomePage.Text = "Hi " + this.user.Username;
            tvTotalPointsHomePage.Text = "Total points: " + this.user.TotalXP;
            switch (this.user.Lastname)
            {
                case "Ukrainian":
                    btnSelectLanguage.SetImageResource(Resource.Drawable.ukraine);
                    break;
                case "Russian":
                    btnSelectLanguage.SetImageResource(Resource.Drawable.russia);
                    break;
                case "Hebrew":
                    btnSelectLanguage.SetImageResource(Resource.Drawable.israel);
                    break;
                case "Polish":
                    btnSelectLanguage.SetImageResource(Resource.Drawable.poland);
                    break;
                case "Yiddish":
                    btnSelectLanguage.SetImageResource(Resource.Drawable.Yiddish);
                    break;
                case "Germany":
                    btnSelectLanguage.SetImageResource(Resource.Drawable.germany);
                    break;
            }
        }
        //משנה את צבעי התוכנה בהתאם להגדרות של המשתמש
        private void UpdateColors()
        {
            switch (this.user.Theme.ToString())
            {
                case "softBlue":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    btnGoToLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    foreach(var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));
                    }
                    tvFromEnglish.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    break;
                case "softPink":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    btnGoToLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    foreach (var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));
                    }
                    tvFromEnglish.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    break;
                case "blackRed":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    btnGoToLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    foreach (var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));
                    }
                    tvFromEnglish.SetTextColor(Android.Graphics.Color.ParseColor("#ffffff"));
                    break;
                case "navy":
                    lyInfoMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    lyButtomMenuMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    lyBackgroundMainPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));

                    btnTask.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    btnGoToProfilePageFromTaskPage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    btnGoToLeaderboard.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    foreach (var btn in this.buttons)
                    {
                        btn.Item1.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));
                    }
                    tvFromEnglish.SetTextColor(Android.Graphics.Color.ParseColor("#ffffff"));
                    break;
                default:

                    break;
            }
        }
        //פעולה האחראית על איתחול הפקדים
        private void InitViews()
        {
            lyButtomMenuMainPage = FindViewById<LinearLayout>(Resource.Id.lyBottomLine);
            lyInfoMainPage = FindViewById<LinearLayout>(Resource.Id.lyInfoMainPage);
            lyBackgroundMainPage = FindViewById<LinearLayout>(Resource.Id.lyBackgroundMainPage);

            tvHiUsernameHomePage = FindViewById<TextView>(Resource.Id.tvHiUsernameHomePage);
            tvTotalPointsHomePage = FindViewById<TextView>(Resource.Id.tvTotalPointsHomePage);
            tvFromEnglish = FindViewById<TextView>(Resource.Id.tvFromEnglish);

            btnTask = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromTaskPage);
            btnPopupMenu = FindViewById<ImageButton>(Resource.Id.btnPopMenu);
            btnPopupMenu.SetOnClickListener(this);

            btnGoToProfilePageFromTaskPage = FindViewById<ImageButton>(Resource.Id.btnGoToProfilePageFromTaskPage);
            btnGoToProfilePageFromTaskPage.Click += BtnGoToProfilePageFromTaskPage_Click;
            btnGoToLeaderboard = FindViewById<ImageButton>(Resource.Id.btnGoToLeaderBoardPageFromTaskPage);
            btnGoToLeaderboard.Click += BtnGoToLeaderboard_Click;
            btnSelectLanguage = FindViewById<ImageButton>(Resource.Id.btnSelecteLanguage);
            btnSelectLanguage.Click += BtnChangeLanguageLearning;

            buttons = new List<Tuple<ImageButton, string>>();
            btnDailyActivity = FindViewById<ImageButton>(Resource.Id.btnDailyActivity);
            buttons.Add(new Tuple<ImageButton, string>(btnDailyActivity, "Daily"));
            btnFamily = FindViewById<ImageButton>(Resource.Id.btnfamily);
            buttons.Add(new Tuple<ImageButton, string>(btnFamily, "Family"));
            btnHealth = FindViewById<ImageButton>(Resource.Id.btnHealth);
            buttons.Add(new Tuple<ImageButton, string>(btnHealth, "Health"));
            btnHobbies = FindViewById<ImageButton>(Resource.Id.btnHoobies);
            buttons.Add(new Tuple<ImageButton, string>(btnHobbies, "Hobbies"));
            btnTravel = FindViewById<ImageButton>(Resource.Id.btnTravel);
            buttons.Add(new Tuple<ImageButton, string>(btnTravel, "Travel"));
            btnBusiness = FindViewById<ImageButton>(Resource.Id.btnBusiness);
            buttons.Add(new Tuple<ImageButton, string>(btnBusiness, "Business"));
            btnEducation = FindViewById<ImageButton>(Resource.Id.btnEducation);
            buttons.Add(new Tuple<ImageButton, string>(btnEducation, "Education"));
            btnFood = FindViewById<ImageButton>(Resource.Id.btnFood);
            buttons.Add(new Tuple<ImageButton, string>(btnFood, "Food"));
            btnMusic = FindViewById<ImageButton>(Resource.Id.btnMusic);
            buttons.Add(new Tuple<ImageButton, string>(btnMusic, "Music"));
            btnAnimals = FindViewById<ImageButton>(Resource.Id.btnAnimals);
            buttons.Add(new Tuple<ImageButton, string>(btnAnimals, "Animals"));
            btnFurniture = FindViewById<ImageButton>(Resource.Id.btnFurniture);
            buttons.Add(new Tuple<ImageButton, string>(btnFurniture, "Furniture"));
            btnEmotions = FindViewById<ImageButton>(Resource.Id.btnEmotions);
            buttons.Add(new Tuple<ImageButton, string>(btnEmotions, "Emotions"));
            btnCountries = FindViewById<ImageButton>(Resource.Id.btnCountries);
            buttons.Add(new Tuple<ImageButton, string>(btnCountries, "Countries"));
            btnTools = FindViewById<ImageButton>(Resource.Id.btnTools);
            buttons.Add(new Tuple<ImageButton, string>(btnTools, "Tools"));
            btnClothing = FindViewById<ImageButton>(Resource.Id.btnClothing);
            buttons.Add(new Tuple<ImageButton, string>(btnClothing, "Clothing"));


            foreach (var button in buttons)
            {
                button.Item1.Click += BtnClick;
            }
        }
        //פותח חלון בחירה של שפות לימוד בשביל הגדרה של המשתמש
        private void BtnChangeLanguageLearning(object sender, EventArgs e)
        {
            View selectLanguage = LayoutInflater.Inflate(Resource.Layout.activity_SelectLanguage, null);

            this.popupWindow = new PopupWindow(
                selectLanguage,
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent,
                true
                );

            ImageButton btnSelectedHebrow = selectLanguage.FindViewById<ImageButton>(Resource.Id.btnSelectIsraelImage);
            TextView tvSelectedHewrow = selectLanguage.FindViewById<TextView>(Resource.Id.btnSelectIsraelText);
            btnSelectedHebrow.Click += SelectedHebrow;
            tvSelectedHewrow.Click += SelectedHebrow;

            ImageButton btnSelectedRussian = selectLanguage.FindViewById<ImageButton>(Resource.Id.btnSelectRussiaImage);
            TextView tvSelectedRussian = selectLanguage.FindViewById<TextView>(Resource.Id.btnSelectRussiaText);
            btnSelectedRussian.Click += SelectedRussian;
            tvSelectedRussian .Click += SelectedRussian;

            ImageButton btnSelectedUkranian = selectLanguage.FindViewById<ImageButton>(Resource.Id.btnSelectUkranianImage);
            TextView tvSelectedUkranian = selectLanguage.FindViewById<TextView>(Resource.Id.btnSelectUkranianText);
            btnSelectedUkranian.Click += SelectedUkranian;
            tvSelectedUkranian.Click += SelectedUkranian;

            ImageButton btnSelectedGermany = selectLanguage.FindViewById<ImageButton>(Resource.Id.btnSelectGermanyImage);
            TextView tvSelectedGermany = selectLanguage.FindViewById<TextView>(Resource.Id.btnSelectGermanyText);
            btnSelectedGermany.Click += SelectedGermany;
            tvSelectedGermany.Click += SelectedGermany;

            ImageButton btnSelectedPolish = selectLanguage.FindViewById<ImageButton>(Resource.Id.btnSelectPolishImage);
            TextView tvSelectedPolish = selectLanguage.FindViewById<TextView>(Resource.Id.btnSelectPolishText);
            btnSelectedPolish.Click += SelectedPolish;
            tvSelectedPolish.Click += SelectedPolish;

            ImageButton btnSelectedYiddish = selectLanguage.FindViewById<ImageButton>(Resource.Id.btnSelectYiddishImage);
            TextView tvSelectedYiddish = selectLanguage.FindViewById<TextView>(Resource.Id.btnSelectYiddishText);
            btnSelectedYiddish.Click += SelectedYiddish;
            tvSelectedYiddish.Click += SelectedYiddish;

            Button btnCancel = selectLanguage.FindViewById<Button>(Resource.Id.btnCancelSelectionLanguage);
            btnCancel.Click += BtnCancel_Click;

            popupWindow.ShowAtLocation(this.Window.DecorView.RootView, GravityFlags.Center,0,0);
        }
        //בחירת שפת לימוד יידיש
        private async void SelectedYiddish(object sender, EventArgs e)
        {
            this.user.Language = "Yiddish";
            await firebase.UpdateValue(username, FirebaseManager.Fields.Language, this.user.Language);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }

            btnSelectLanguage.SetImageResource(Resource.Drawable.Yiddish);
        }
        //בחירת שפת לימוד פולנית
        private async void SelectedPolish(object sender, EventArgs e)
        {
            this.user.Language = "Polish";
            await firebase.UpdateValue(username, FirebaseManager.Fields.Language, this.user.Language);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }

            btnSelectLanguage.SetImageResource(Resource.Drawable.poland);
        }
        //בחירת שפת לימוד גרמנית
        private async void SelectedGermany(object sender, EventArgs e)
        {
            this.user.Language = "Germany";
            await firebase.UpdateValue(username, FirebaseManager.Fields.Language, this.user.Language);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }

            btnSelectLanguage.SetImageResource(Resource.Drawable.germany);
        }
        //בחירת שפת לימוד אוקראינית
        private async void SelectedUkranian(object sender, EventArgs e)
        {
            this.user.Language = "Ukrainian";
            await firebase.UpdateValue(username, FirebaseManager.Fields.Language, this.user.Language);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }

            btnSelectLanguage.SetImageResource(Resource.Drawable.ukraine);
        }
        //בחירת שפת לימוד רוסית
        private async void SelectedRussian(object sender, EventArgs e)
        {
            this.user.Language = "Russian";
            await firebase.UpdateValue(username, FirebaseManager.Fields.Language, this.user.Language);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }

            btnSelectLanguage.SetImageResource(Resource.Drawable.russia);
        }
        //בחירת שפת לימוד עברית
        private async void SelectedHebrow(object sender, EventArgs e)
        {
            this.user.Language = "Hebrew";
            await firebase.UpdateValue(username, FirebaseManager.Fields.Language, this.user.Language);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }

            btnSelectLanguage.SetImageResource(Resource.Drawable.israel);
        }
        //כפתור יציאה ואי שמירה של שינויים מהמסך של בחירת שפת לימוד
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }
        }
        //כפתור המעביר את המשתמש למסך טבלת השיאים
        private void BtnGoToLeaderboard_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_Leaderboard));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }
        //כפתור המעבי את המשתמש למשימות בנושא שהוא בחר
        private void BtnClick(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string mood = "";
            foreach(var tuple in buttons)
            {
                if (button.Equals(tuple.Item1))
                {
                    mood = tuple.Item2;
                }
            }

            Toast.MakeText(this, "Theme: "+mood, ToastLength.Short).Show();

            Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
            intent.PutExtra("Username", this.username);
            intent.PutExtra("XP", 0);
            intent.PutExtra("Round", 1);
            intent.PutExtra("Mood", mood);
            intent.PutExtra("Language", this.user.Lastname);
            StartActivity(intent);
            StopService(this.music);
            Finish();
        }
        //כפתור המעביר את המשתמש למסך הפרופיל
        private void BtnGoToProfilePageFromTaskPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_ProfilePage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }
        
        public void OnClick(View v)
        {
            if(v.Id == btnPopupMenu.Id)
            {
                PopupMenu popup = new PopupMenu(this, v);
                MenuInflater inflater = popup.MenuInflater;
                inflater.Inflate(Resource.Menu.activity_menu, popup.Menu);

                popup.SetOnMenuItemClickListener(this);
                
                popup.Show();
            }
            
        }
        
        public bool OnMenuItemClick(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_about)
            {
                Intent intent = new Intent(this, typeof(activity_InfoPage));
                intent.PutExtra("Username", this.username);
                StartActivity(intent);
                return true;
            }
            if (item.ItemId == Resource.Id.action_logout)
            {
                StopService(this.music);
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
                return true;
            }
            if (item.ItemId == Resource.Id.action_profile)
            {
                Intent intent = new Intent(this, typeof(activity_ProfilePage));
                intent.PutExtra("Username", this.username);
                StartActivity(intent);
                Finish();
                return true;
            }
            if (item.ItemId == Resource.Id.action_settings)
            {
                Intent intent = new Intent(this, typeof(activity_Settings));
                intent.PutExtra("Username", this.username);
                StartActivity(intent);
                Finish();
                return true;
            }
            return false;
        }
    }
}
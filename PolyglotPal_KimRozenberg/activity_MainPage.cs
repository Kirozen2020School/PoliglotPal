using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using static Android.Views.View;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_MainPage : AppCompatActivity, IOnClickListener, PopupMenu.IOnMenuItemClickListener
    {
        ImageButton btnGoToProfilePageFromTaskPage;
        ImageButton btnDailyActivity, btnTravel, btnHealth, btnHobbies, btnFamily;
        List<Tuple<ImageButton, string>> buttons;
        ImageButton btnPopupMenu;
        TextView tvHiUsernameHomePage, tvTotalPointsHomePage;
        Android.App.AlertDialog d;

        string username;
        Account user;
        FirebaseManager firebase;

        int xpAdded = -1;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_MainPage);
            // Create your application here

            if (Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
                this.xpAdded = Intent.GetIntExtra("XP", -1);
            }

            firebase = new FirebaseManager();
            if(this.xpAdded != -1)
            {
                await firebase.UpdateXP(username, this.xpAdded);
            }
            user = await firebase.GetAccount(this.username);
            
            InitViews();
            UpdateViews();
        }

        private void UpdateViews()
        {
            tvHiUsernameHomePage.Text = "Hi " + this.user.username;
            tvTotalPointsHomePage.Text = "Total points: " + this.user.totalxp;
        }

        private void InitViews()
        {
            tvHiUsernameHomePage = FindViewById<TextView>(Resource.Id.tvHiUsernameHomePage);
            tvTotalPointsHomePage = FindViewById<TextView>(Resource.Id.tvTotalPointsHomePage);

            btnPopupMenu = FindViewById<ImageButton>(Resource.Id.btnPopMenu);
            btnPopupMenu.SetOnClickListener(this);

            btnGoToProfilePageFromTaskPage = FindViewById<ImageButton>(Resource.Id.btnGoToProfilePageFromTaskPage);
            btnGoToProfilePageFromTaskPage.Click += BtnGoToProfilePageFromTaskPage_Click;

            buttons = new List<Tuple<ImageButton, string>>();
            btnDailyActivity = FindViewById<ImageButton>(Resource.Id.btnDailyActivity);
            buttons.Add(new Tuple<ImageButton, string>(btnDailyActivity, "Daily"));
            btnFamily = FindViewById<ImageButton>(Resource.Id.btnfamily);
            buttons.Add(new Tuple<ImageButton, string>(btnFamily, "Family"));
            btnHealth = FindViewById<ImageButton>(Resource.Id.btnHealth);
            buttons.Add(new Tuple<ImageButton, string>(btnHealth, "Health"));
            btnHobbies = FindViewById<ImageButton>(Resource.Id.btnHoobies);
            buttons.Add(new Tuple<ImageButton, string>(btnHobbies, "Hoddies"));
            btnTravel = FindViewById<ImageButton>(Resource.Id.btnTravel);
            buttons.Add(new Tuple<ImageButton, string>(btnTravel, "Travel"));

            foreach(var button in buttons)
            {
                button.Item1.Click += BtnClick;
            }
        }

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
            Toast.MakeText(this, mood, ToastLength.Short).Show();
            Random random = new Random();
            int id = random.Next(0, 1);

            if (id == 0)
            {
                Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                intent.PutExtra("Username", this.username);
                intent.PutExtra("XP", 0);
                intent.PutExtra("Round", 1);
                intent.PutExtra("Mood", mood);
                StartActivity(intent);
                Finish();
            }
            else if (id == 1)
            {
                Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                intent.PutExtra("Username", this.username);
                intent.PutExtra("XP", 0);
                intent.PutExtra("Round", 1);
                intent.PutExtra("Mood", mood);
                StartActivity(intent);
                Finish();
            }
        }

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
            if (item.ItemId == Resource.Id.action_delete)
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Delete");
                builder.SetMessage("You want to delete your account?\nThere is mo way back after that");
                builder.SetCancelable(true);
                builder.SetPositiveButton("yes", OkAction);
                builder.SetNegativeButton("cancel", CancelAction);
                d = builder.Create();
                d.Show();
                return true;
            }

            return false;
        }
        private void CancelAction(object sender, DialogClickEventArgs e)
        {

        }

        private async void OkAction(object sender, DialogClickEventArgs e)
        {
            await firebase.DeleteAccount(this.username);
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
            return;
        }
    }
}
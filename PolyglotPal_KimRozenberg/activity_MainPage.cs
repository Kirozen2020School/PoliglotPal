using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using static Android.Views.View;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_MainPage : AppCompatActivity, IOnClickListener, PopupMenu.IOnMenuItemClickListener
    {
        ImageButton btnGoToProfilePageFromTaskPage;
        Button btnTask1, btnTask2, btnTask3, btnTask4, btnTask5, btnTask6, btnTask7, btnTask8, btnTask9;
        ImageButton btnPopupMenu;
        TextView tvHiUsernameHomePage, tvTotalPointsHomePage;

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

            btnTask1 = FindViewById<Button>(Resource.Id.btnTask1);
            btnTask2 = FindViewById<Button>(Resource.Id.btnTask2);
            btnTask3 = FindViewById<Button>(Resource.Id.btnTask3);
            btnTask4 = FindViewById<Button>(Resource.Id.btnTask4);
            btnTask5 = FindViewById<Button>(Resource.Id.btnTask5);
            btnTask6 = FindViewById<Button>(Resource.Id.btnTask6);
            btnTask7 = FindViewById<Button>(Resource.Id.btnTask7);
            btnTask8 = FindViewById<Button>(Resource.Id.btnTask8);
            btnTask9 = FindViewById<Button>(Resource.Id.btnTask9);

            btnTask1.Click += BtnTask_Click;
            btnTask2.Click += BtnTask_Click;
            btnTask3.Click += BtnTask_Click;
            btnTask4.Click += BtnTask_Click;
            btnTask5.Click += BtnTask_Click;
            btnTask6.Click += BtnTask_Click;
            btnTask7.Click += BtnTask_Click;
            btnTask8.Click += BtnTask_Click;
            btnTask9.Click += BtnTask_Click;
        }

        private void BtnTask_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int id = random.Next(0, 1);

            if (id == 0)
            {
                Intent intent = new Intent(this, typeof(activity_TaskWordToWord));
                intent.PutExtra("Username", this.username);
                intent.PutExtra("XP", 0);
                intent.PutExtra("Round", 1);
                StartActivity(intent);
                Finish();
            }
            else if (id == 1)
            {
                Intent intent = new Intent(this, typeof(activity_CreateTranslationToSentence));
                intent.PutExtra("Username", this.username);
                intent.PutExtra("XP", 0);
                intent.PutExtra("Round", 1);
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

            return false;
        }
    }
}
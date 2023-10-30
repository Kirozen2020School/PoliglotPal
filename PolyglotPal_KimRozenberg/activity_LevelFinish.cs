using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_LevelFinish : Activity
    {
        Button btnExitFromFinishLevelPage;
        TextView tvXP, tvTimer;
        int xp;
        Timer timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_LevelFinish);
            // Create your application here
            if (Intent.Extras != null)
            {
                this.xp = Intent.GetIntExtra("XP", -1);
            }
            
            long savedTicks = Intent.GetLongExtra("current_time", 0);
            TimeSpan savedTime = TimeSpan.FromTicks(savedTicks);

            timer = new Timer(TimeSpan.FromSeconds(1));
            timer.Start(savedTime);
            InitViews();
        }

        private void InitViews()
        {
            btnExitFromFinishLevelPage = FindViewById<Button>(Resource.Id.btnExitFromFinishLevelPage);
            btnExitFromFinishLevelPage.Click += BtnExitFromFinishLevelPage_Click;

            tvXP = FindViewById<TextView>(Resource.Id.tvTotalXPFinishLevelPage);
            tvXP.Text = $"Xp colected: {Intent.GetIntExtra("XP", -1)}";

            tvTimer = FindViewById<TextView>(Resource.Id.tvTimer);
            
            string time = this.timer.GetCurrentTimeString();

            tvTimer.Text = "Time: " + time;
        }

        private void BtnExitFromFinishLevelPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            intent.PutExtra("XP", this.xp);
            StartActivity(intent);
            Finish();
        }
    }
}
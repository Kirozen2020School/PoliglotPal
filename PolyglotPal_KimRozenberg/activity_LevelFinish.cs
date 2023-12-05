using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Widget;
using AndroidX.Activity.ContextAware;
using System;
using static Android.Media.MediaPlayer;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_LevelFinish : Activity
    {
        Button btnExitFromFinishLevelPage;
        TextView tvXP, tvTimer, tvAccuracy;
        VideoView videoBackground;
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
            InitVideo();
            //btnExitFromFinishLevelPage = FindViewById<Button>(Resource.Id.btnExitFromFinishLevelPage);
            //btnExitFromFinishLevelPage.Click += BtnExitFromFinishLevelPage_Click;

            //tvXP = FindViewById<TextView>(Resource.Id.tvTotalXPFinishLevelPage);
            //tvXP.Text = $"Xp:\n{Intent.GetIntExtra("XP", -1)}";

            //tvTimer = FindViewById<TextView>(Resource.Id.tvTimer);
            //tvTimer.Text = "Time:\n" + this.timer.GetCurrentTimeString();

            //tvAccuracy = FindViewById<TextView>(Resource.Id.tvAccoracy);
            //double prosent = (1-(Intent.GetDoubleExtra("errors", 0)/(7 * 4)*1.0)) * 100;
            //int temp = (int)Math.Round(prosent);
            //tvAccuracy.Text = $"Accuracy:\n{temp}%";

        }

        private void InitVideo()
        {
            videoBackground = FindViewById<VideoView>(Resource.Id.vvBackground);
            var path = ("android.resource://" + Application.PackageName + "/" + Resource.Raw.Firework);
            videoBackground.SetVideoPath(path);

            var onPreparedListener = new OnPreparedListener
            {
                OnPreparedAction = mp =>
                {
                    mp.Looping = true;
                    videoBackground.Start();
                }
            };

            videoBackground.SetOnPreparedListener(onPreparedListener);

        }

        private void BtnExitFromFinishLevelPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            intent.PutExtra("XP", this.xp);
            intent.PutExtra("First", true);
            StartActivity(intent);
            Finish();
        }

    }
    public class OnPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        public Action<MediaPlayer> OnPreparedAction { get; set; }

        public void OnPrepared(MediaPlayer mp)
        {
            OnPreparedAction?.Invoke(mp);
        }
    }
}
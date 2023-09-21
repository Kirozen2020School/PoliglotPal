using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_LevelFinish")]
    public class activity_LevelFinish : Activity
    {
        Button btnExitFromFinishLevelPage;
        int xp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_LevelFinish);
            // Create your application here
            if (Intent.Extras != null)
            {
                this.xp = Intent.GetIntExtra("XP", -1);
            }
            InitViews();
        }

        private void InitViews()
        {
            btnExitFromFinishLevelPage.Click += BtnExitFromFinishLevelPage_Click;
        }

        private void BtnExitFromFinishLevelPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("XP", this.xp);
            StartActivity(intent);
            Finish();
        }
    }
}
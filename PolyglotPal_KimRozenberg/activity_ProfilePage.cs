using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Graphics;
using System.IO;
using System.Linq;
using System.Text;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_ProfilePage")]
    public class activity_ProfilePage : Activity
    {
        ImageButton btnGotToTaskPageFromProfilePage;
        ImageView ivProfilePic;
        TextView tvUserName, tvFullUserName, tvJoiningDate;
        TextView tvTotalEX, tvTotalTaskDone;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_ProfilePage);
            // Create your application here
            InitViews();
        }

        private void InitViews()
        {
            btnGotToTaskPageFromProfilePage = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromProfilePage);

            ivProfilePic = FindViewById<ImageView>(Resource.Id.ivProfilePic);

            tvUserName = FindViewById<TextView>(Resource.Id.tvUserNameProfilePage);
            tvFullUserName = FindViewById<TextView>(Resource.Id.tvFullNameProfilePage);
            tvJoiningDate = FindViewById<TextView>(Resource.Id.tvDateJoiningProfilePage);
            tvTotalEX = FindViewById<TextView>(Resource.Id.tvTotalPointsProfilePage);
            tvTotalTaskDone = FindViewById<TextView>(Resource.Id.tvTotalTasksProfilePage);

            btnGotToTaskPageFromProfilePage.Click += BtnGotToTaskPageFromProfilePage_Click;
        }

        private void BtnGotToTaskPageFromProfilePage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            StartActivity(intent);
            Finish();
        }

        public byte[] ConvertBitmapToByteArray(Bitmap bm)
        {
            byte[] bytes;
            var stream = new MemoryStream();
            bm.Compress(Bitmap.CompressFormat.Png, 0, stream);
            bytes = stream.ToArray();
            return bytes;
        }
    }
}
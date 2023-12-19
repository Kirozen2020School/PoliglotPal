using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using Android.Graphics;
using System.IO;
using Android.Content.PM;
using Android.Provider;
using Android.Views;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_ProfilePage : Activity
    {
        ImageButton btnGotToTaskPageFromProfilePage, btnProfile, btnGoToLeaderboardPage;
        ImageView ivProfilePic, btnSettings;
        TextView tvUserName, tvFullUserName, tvJoiningDate;
        TextView tvTotalEX, tvTotalTaskDone;
        LinearLayout lyProfilePageBackgroundColor, lybackground, lyButtom;

        string username;
        Account user;
        FirebaseManager firebase;

        PopupWindow popupWindow;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_ProfilePage);
            // Create your application here
            RequestedOrientation = ScreenOrientation.Portrait;
            InitViews();
            firebase = new FirebaseManager();
            if(Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }
            UpdateViews();
        }
        private void UpdateColors()
        {
            ColorsClass colors = new ColorsClass();
            switch (this.user.theme.ToString())
            {
                case "softBlue":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Color.ParseColor(colors.softBlue[2]));
                    ivProfilePic.SetBackgroundColor(Color.ParseColor(colors.softBlue[2]));
                    btnSettings.SetBackgroundColor(Color.ParseColor(colors.softBlue[2]));
                    lyButtom.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    lybackground.SetBackgroundColor(Color.ParseColor(colors.softBlue[1]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    btnProfile.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    btnGoToLeaderboardPage.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    break;

                case "softPink":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Color.ParseColor(colors.softPink[0]));
                    btnSettings.SetBackgroundColor(Color.ParseColor(colors.softPink[0]));
                    ivProfilePic.SetBackgroundColor(Color.ParseColor(colors.softPink[0]));
                    lyButtom.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    lybackground.SetBackgroundColor(Color.ParseColor(colors.softPink[2]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    btnProfile.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    btnGoToLeaderboardPage.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));

                    /*----------------*/
                    tvUserName.SetTextColor(Color.ParseColor("#000000"));
                    tvFullUserName.SetTextColor(Color.ParseColor("#000000"));
                    tvJoiningDate.SetTextColor(Color.ParseColor("#000000"));
                    tvTotalEX.SetTextColor(Color.ParseColor("#000000"));
                    tvTotalTaskDone.SetTextColor(Color.ParseColor("#000000"));
                    break;

                case "blackRed":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Color.ParseColor(colors.blackRed[1]));
                    btnSettings.SetBackgroundColor(Color.ParseColor(colors.blackRed[1]));
                    ivProfilePic.SetBackgroundColor(Color.ParseColor(colors.blackRed[1]));
                    lyButtom.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    lybackground.SetBackgroundColor(Color.ParseColor(colors.blackRed[0]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    btnProfile.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    btnGoToLeaderboardPage.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    break;

                case "navy":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Color.ParseColor(colors.navy[1]));
                    btnSettings.SetBackgroundColor(Color.ParseColor(colors.navy[1]));
                    ivProfilePic.SetBackgroundColor(Color.ParseColor(colors.navy[1]));
                    lyButtom.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    lybackground.SetBackgroundColor(Color.ParseColor(colors.navy[0]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    btnProfile.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    btnGoToLeaderboardPage.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    break;

                default:

                    break;
            }
        }

        async private void UpdateViews()
        {
            try
            {
                this.user = await firebase.GetAccount(this.username);
            }
            catch (Exception)
            {
                Toast.MakeText(this, "User not found in firebase", ToastLength.Long).Show();
            }
            
            if(this.user != null)
            {
                tvUserName.Text = this.username;
                tvFullUserName.Text = this.user.firstname + " " + this.user.lastname;
                tvJoiningDate.Text = this.user.datejoining;
                tvTotalEX.Text = "Total points:\n" + this.user.totalxp;
                tvTotalTaskDone.Text = "Total tasks:\n" + this.user.totaltasks;

                Bitmap bitmap = ConvertByteArrayToBitmap(this.user.profilepic);
                ivProfilePic.SetImageBitmap(bitmap);

                UpdateColors();
            }
        }

        private void InitViews()
        {
            btnGotToTaskPageFromProfilePage = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromProfilePage);
            btnGotToTaskPageFromProfilePage.Click += BtnGotToTaskPageFromProfilePage_Click;

            btnSettings = FindViewById<ImageButton>(Resource.Id.btnSettingsFromProfilePage);
            btnSettings.Click += BtnSettings_Click;

            btnGoToLeaderboardPage = FindViewById<ImageButton>(Resource.Id.btnGoToLeaderBoardPageFromProfilePage);
            btnGoToLeaderboardPage.Click += BtnGoToLeaderboardPage_Click;

            ivProfilePic = FindViewById<ImageView>(Resource.Id.ivProfilePic);
            ivProfilePic.Click += IvProfilePic_Click;

            tvUserName = FindViewById<TextView>(Resource.Id.tvUserNameProfilePage);
            tvFullUserName = FindViewById<TextView>(Resource.Id.tvFullNameProfilePage);
            tvJoiningDate = FindViewById<TextView>(Resource.Id.tvDateJoiningProfilePage);
            tvTotalEX = FindViewById<TextView>(Resource.Id.tvTotalPointsProfilePage);
            tvTotalTaskDone = FindViewById<TextView>(Resource.Id.tvTotalTasksProfilePage);

            lyProfilePageBackgroundColor = FindViewById<LinearLayout>(Resource.Id.lyProfilePageBackgroundColor);

            btnProfile = FindViewById<ImageButton>(Resource.Id.btnProfileToProfile);
            lybackground = FindViewById<LinearLayout>(Resource.Id.lyBackgroundProfilePage);
            lyButtom = FindViewById<LinearLayout>(Resource.Id.lyButtomProfilePage);
        }

        private void BtnGoToLeaderboardPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_Leaderboard));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_Settings));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }

        
        private void IvProfilePic_Click(object sender, EventArgs e)
        {
            View changeProfilePic = LayoutInflater.Inflate(Resource.Layout.activity_ChangeProfilePage, null);

            this.popupWindow = new PopupWindow(
                changeProfilePic,
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent,
                true
                );

            ImageView ivProfilePicChangePage = changeProfilePic.FindViewById<ImageView>(Resource.Id.ivProfilePicChangeProfilePicPage);
            Button btnCameraSelect = changeProfilePic.FindViewById<Button>(Resource.Id.btnSelectCamera);
            Button btnGalarySelect = changeProfilePic.FindViewById<Button>(Resource.Id.btnSelectGalery);
            Button btnCancelSelect = changeProfilePic.FindViewById<Button>(Resource.Id.btnSelectCancel);

            btnCameraSelect.Click += BtnCameraSelect_Click;
            btnGalarySelect.Click += BtnGalarySelect_Click;
            btnCancelSelect.Click += BtnCancelSelect_Click;

            Bitmap pic = ConvertByteArrayToBitmap(this.user.profilepic);
            ivProfilePicChangePage.SetImageBitmap(pic);

            popupWindow.ShowAtLocation(this.Window.DecorView.RootView, GravityFlags.Center, 0, 0);

        }

        private void BtnCancelSelect_Click(object sender, EventArgs e)
        {
            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }
            Toast.MakeText(this, "You did not change your profile image", ToastLength.Long).Show();
        }

        private void BtnGalarySelect_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionPick);
            intent.SetType("image/*");
            StartActivityForResult(intent, 1);

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }
        }

        private void BtnCameraSelect_Click(object sender, EventArgs e)
        {
            if (CheckSelfPermission(Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
            {
                Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
                StartActivityForResult(intent, 0);
            }
            else
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, 1001);
            }

            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }
        }

        private void BtnGotToTaskPageFromProfilePage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_MainPage));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }

        public byte[] ConvertBitmapToByteArray(Bitmap bm)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bm.Compress(Bitmap.CompressFormat.Png, 0, stream); // PNG format, quality 0 (max compression)
                return stream.ToArray();
            }
        }

        public Bitmap ConvertByteArrayToBitmap(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return BitmapFactory.DecodeStream(stream);
            }
        }

        [Obsolete]
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && requestCode == 0)
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");

                ivProfilePic.SetImageBitmap(bitmap);
                this.user.profilepic = ConvertBitmapToByteArray(bitmap);
                await firebase.UpdateProfilePic(this.user.username, this.user.profilepic);
            }

            if (resultCode == Result.Ok && requestCode == 1)
            {
                Android.Net.Uri uri = data.Data;

                // Convert the Uri to a Bitmap
                Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);

                ivProfilePic.SetImageBitmap(bitmap);
                this.user.profilepic = ConvertBitmapToByteArray(bitmap);
                await firebase.UpdateProfilePic(this.user.username, this.user.profilepic);
            }
        }

    }
}
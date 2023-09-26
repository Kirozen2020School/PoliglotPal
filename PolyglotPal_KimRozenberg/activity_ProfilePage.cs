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
using Android.Graphics.Drawables;
using Android.Provider;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "activity_ProfilePage")]
    public class activity_ProfilePage : Activity
    {
        ImageButton btnGotToTaskPageFromProfilePage;
        ImageView ivProfilePic;
        TextView tvUserName, tvFullUserName, tvJoiningDate;
        TextView tvTotalEX, tvTotalTaskDone;
        LinearLayout lyProfilePageBackgroundColor;

        string username;
        Account user;
        FirebaseManager firebase;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_ProfilePage);
            // Create your application here
            InitViews();
            if(Intent.Extras != null)
            {
                this.username = Intent.GetStringExtra("Username");
            }
            UpdateViews();
        }

        async private void UpdateViews()
        {
            this.user = await firebase.GetAccount(this.username);

            tvUserName.Text = this.username;
            tvFullUserName.Text = this.user.firstname + " " + this.user.lastname;
            tvJoiningDate.Text = this.user.datejoining;
            tvTotalEX.Text = "Total points:\n"+this.user.totalxp;
            tvTotalTaskDone.Text = "Total tasks:\n"+this.user.totaltasks;

            Bitmap bitmap = BitmapFactory.DecodeByteArray(this.user.profilepic,0,this.user.profilepic.Length);
            ivProfilePic.SetImageBitmap(bitmap);

            lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(this.user.backgroundcolor));
        }

        private void InitViews()
        {
            btnGotToTaskPageFromProfilePage = FindViewById<ImageButton>(Resource.Id.btnGoToTaskPageFromProfilePage);
            btnGotToTaskPageFromProfilePage.Click += BtnGotToTaskPageFromProfilePage_Click;

            ivProfilePic = FindViewById<ImageView>(Resource.Id.ivProfilePic);
            ivProfilePic.Click += IvProfilePic_Click;

            tvUserName = FindViewById<TextView>(Resource.Id.tvUserNameProfilePage);
            tvFullUserName = FindViewById<TextView>(Resource.Id.tvFullNameProfilePage);
            tvJoiningDate = FindViewById<TextView>(Resource.Id.tvDateJoiningProfilePage);
            tvTotalEX = FindViewById<TextView>(Resource.Id.tvTotalPointsProfilePage);
            tvTotalTaskDone = FindViewById<TextView>(Resource.Id.tvTotalTasksProfilePage);

            lyProfilePageBackgroundColor = FindViewById<LinearLayout>(Resource.Id.lyProfilePageBackgroundColor);
            lyProfilePageBackgroundColor.Click += LyProfilePageBackgroundColor_Click;
        }

        private void LyProfilePageBackgroundColor_Click(object sender, EventArgs e)
        {
            EditText userinput;

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Enter HTML code of a color");
            userinput = new EditText(this);
            builder.SetView(userinput);
            builder.SetPositiveButton("OK", async (sender, args) =>
            {
                string inputText = userinput.Text;
                this.user.backgroundcolor = inputText;
                lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(inputText));
                await firebase.UpdateBackgroundColor(this.user.username, this.user.backgroundcolor);
            });
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "You did not change the bacground color", ToastLength.Long).Show();
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();
            
        }

        private void IvProfilePic_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("From here you want to get the new profile pic?");
            builder.SetPositiveButton("Camera", (sender, args) =>
            {
                Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
                StartActivityForResult(intent, 0);

            });
            builder.SetNeutralButton("Galary", (sender, args) =>
            {
                Intent intent = new Intent(Intent.ActionPick);
                intent.SetType("image/*");
                StartActivityForResult(intent, 1);

            });
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "You did not change your profile image", ToastLength.Long).Show();
            });
            AlertDialog dialog = builder.Create();
            dialog.Show();
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
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && requestCode == 0)
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");

                ivProfilePic.SetImageBitmap(bitmap);
                this.user.profilepic = ConvertBitmapToByteArray(((BitmapDrawable)ivProfilePic.Drawable).Bitmap);
                await firebase.UpdateProfilePic(this.user.username, this.user.profilepic);
            }

            if (resultCode == Result.Ok && requestCode == 1)
            {
                Android.Net.Uri uri = data.Data;

                // Convert the Uri to a Bitmap
                Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);

                ivProfilePic.SetImageBitmap(bitmap);
                this.user.profilepic = ConvertBitmapToByteArray(((BitmapDrawable)ivProfilePic.Drawable).Bitmap);
                await firebase.UpdateProfilePic(this.user.username, this.user.profilepic);
            }
        }

    }
}
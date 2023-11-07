using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using Android.Graphics;
using System.IO;
using Android.Provider;
using System.Text.RegularExpressions;

namespace PolyglotPal_KimRozenberg
{
    [Activity(Label = "PolyglotPal")]
    public class activity_ProfilePage : Activity
    {
        ImageButton btnGotToTaskPageFromProfilePage, btnProfile;
        ImageView ivProfilePic, btnSettings;
        TextView tvUserName, tvFullUserName, tvJoiningDate;
        TextView tvTotalEX, tvTotalTaskDone;
        LinearLayout lyProfilePageBackgroundColor, lybackground, lyButtom;

        string username;
        Account user;
        FirebaseManager firebase;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_ProfilePage);
            // Create your application here
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
                    lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    ivProfilePic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    btnSettings.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[2]));
                    lyButtom.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[1]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    btnProfile.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softBlue[3]));
                    break;

                case "softPink":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    btnSettings.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    ivProfilePic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[0]));
                    lyButtom.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[2]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));
                    btnProfile.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.softPink[1]));

                    /*----------------*/
                    tvUserName.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    tvFullUserName.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    tvJoiningDate.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    tvTotalEX.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    tvTotalTaskDone.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
                    break;

                case "blackRed":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    btnSettings.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    ivProfilePic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[1]));
                    lyButtom.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[0]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    btnProfile.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.blackRed[2]));
                    break;

                case "navy":
                    lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    btnSettings.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    ivProfilePic.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[1]));
                    lyButtom.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    lybackground.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[0]));

                    btnGotToTaskPageFromProfilePage.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
                    btnProfile.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors.navy[2]));
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

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(activity_Settings));
            intent.PutExtra("Username", this.username);
            StartActivity(intent);
            Finish();
        }

        /*
        private void LyProfilePageBackgroundColor_Click(object sender, EventArgs e)
        {
            EditText userinput;

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Enter HTML code of a color, format: #RRGGBB");
            userinput = new EditText(this);
            builder.SetView(userinput);
            builder.SetPositiveButton("OK", async (sender, args) =>
            {
                string inputText = userinput.Text;
                if (IsHtmlColor(inputText))
                {
                    if (!inputText.Contains("#"))
                    {
                        inputText = "#" + inputText;
                    }

                    this.user.backgroundcolor = inputText;
                    lyProfilePageBackgroundColor.SetBackgroundColor(Android.Graphics.Color.ParseColor(inputText));
                    ivProfilePic.SetBackgroundColor(Android.Graphics.Color.ParseColor(inputText));
                    await firebase.UpdateBackgroundColor(this.user.username, this.user.backgroundcolor);
                }
                else
                {
                    Toast.MakeText(this, "The format is incorect, enter in this format: #RRGGBB", ToastLength.Long).Show();
                }
                
            });
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "You did not change the background color", ToastLength.Long).Show();
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();
            
        }*/

        public static bool IsHtmlColor(string colorCode)
        {
            Regex hexColorRegex = new Regex("^#?[0-9a-fA-F]{6}$");
            return hexColorRegex.IsMatch(colorCode);
        }

        private void IvProfilePic_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("From where you want to get the new profile pic?");
            builder.SetPositiveButton("Camera", (sender, args) =>
            {
                if(CheckSelfPermission(Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
                {
                    Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
                    StartActivityForResult(intent, 0);
                }
                else
                {
                    RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, 1001);
                }

            });
            builder.SetNeutralButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "You did not change your profile image", ToastLength.Long).Show();
            });
            builder.SetNegativeButton("Galary", (sender, args) =>
            {
                Intent intent = new Intent(Intent.ActionPick);
                intent.SetType("image/*");
                StartActivityForResult(intent, 1);
            });
            AlertDialog dialog = builder.Create();
            dialog.Show();
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
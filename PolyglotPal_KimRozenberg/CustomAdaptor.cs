using Android.Accounts;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.IO;

namespace PolyglotPal_KimRozenberg
{
    public class CustomAdapter : BaseAdapter<Account>
    {
        private readonly Context context;
        private readonly List<Account> accounts;
        private LinearLayout lyBackground;
        private TextView textViewIndex, textViewUsername, textViewTotalXP;
        private readonly string theme;
        private PopupWindow popupWindow;
        private Account acc;

        public CustomAdapter(Context context, List<Account> accounts, string theme)
        {
            this.context = context;
            this.accounts = accounts;
            this.theme = theme;
        }

        public override int Count => accounts.Count;

        public override long GetItemId(int position) => position;

        public override Account this[int position] => accounts[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var account = accounts[position];
            var view = LayoutInflater.From(context).Inflate(Resource.Layout.customLayout, null);

            view.Tag = position;

            // Customize the view using the account data

            lyBackground = view.FindViewById<LinearLayout>(Resource.Id.lyBackgroundCustomLayout);
            textViewIndex = view.FindViewById<TextView>(Resource.Id.textViewIndex);
            textViewUsername = view.FindViewById<TextView>(Resource.Id.textViewUsername);
            textViewTotalXP = view.FindViewById<TextView>(Resource.Id.textViewTotalXP);
            var imageViewProfilePic = view.FindViewById<ImageView>(Resource.Id.imageViewProfilePic);

            textViewIndex.Text = (position+1).ToString();
            textViewUsername.Text = account.username;
            textViewTotalXP.Text = $"Total XP: {account.totalxp}";

            // Set the profile pic (replace this with your logic)
            Bitmap bitmap = ConvertByteArrayToBitmap(account.profilepic);
            this.acc = account;
            imageViewProfilePic.SetImageBitmap(bitmap);
            imageViewProfilePic.SetScaleType(ImageView.ScaleType.FitXy);

            lyBackground.Click += ViewProfile;
            //textViewIndex.Click += ViewProfile;
            //textViewUsername.Click += ViewProfile;
            //textViewTotalXP.Click += ViewProfile;
            //imageViewProfilePic.Click += ViewProfile;

            UpdateColors();

            return view;
        }

        private void ViewProfile(object sender, System.EventArgs e)
        {
            var clickedView = sender as View;

            var position = (int)clickedView.Tag;
            var clickedAccount = accounts[position];


            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            View viewProfile = inflater.Inflate(Resource.Layout.activity_ViewProfileLeaderboard, null);

            this.popupWindow = new PopupWindow(
                viewProfile,
                900,
                ViewGroup.LayoutParams.WrapContent,
                true
            );

            ImageView ivProfilePicViewProfile = viewProfile.FindViewById<ImageView>(Resource.Id.ivProfilePicViewProfile);
            TextView tvUsername = viewProfile.FindViewById<TextView>(Resource.Id.tvUsernameViewProfile);
            TextView tvFirstName = viewProfile.FindViewById<TextView>(Resource.Id.tvFirstNameViewProfile);
            TextView tvLastName = viewProfile.FindViewById<TextView>(Resource.Id.tvLastNameViewProfile);
            TextView tvXp = viewProfile.FindViewById<TextView>(Resource.Id.tvXpViewProfile);
            Button btnExit = viewProfile.FindViewById<Button>(Resource.Id.btnExitFromProfileView);

            tvUsername.Text = "Username: " + clickedAccount.username;
            tvFirstName.Text = "First name: " + clickedAccount.firstname;
            tvLastName.Text = "Last name: " + clickedAccount.lastname;
            tvXp.Text = "Xp: " + clickedAccount.totalxp;
            btnExit.Click += BtnCloseProfileView;

            Bitmap pic = ConvertByteArrayToBitmap(clickedAccount.profilepic);
            ivProfilePicViewProfile.SetImageBitmap(pic);

            // Use clickedView to anchor the PopupWindow
            popupWindow.ShowAsDropDown(clickedView,50,-(position*150));
        }

        private void BtnCloseProfileView(object sender, System.EventArgs e)
        {
            if (popupWindow != null && popupWindow.IsShowing)
            {
                popupWindow.Dismiss();
            }
        }

        private void UpdateColors()
        {
            ColorsClass colors = new ColorsClass();
            switch (this.theme)
            {
                case "softBlue":
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.softBlue[3]));
                    SetTextColor("#000000");
                    break;
                case "softPink":
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.softPink[1]));
                    SetTextColor("#000000");
                    break;
                case "blackRed":
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.blackRed[2]));
                    SetTextColor("#000000");
                    break;
                case "navy":
                    lyBackground.SetBackgroundColor(Color.ParseColor(colors.navy[2]));
                    SetTextColor("#000000");
                    break;
                default:
                    lyBackground.SetBackgroundColor(Color.ParseColor("#7A7A7A"));
                    SetTextColor("#ffffff");
                    break;
            }
        }

        private void SetTextColor(string color)
        {
            textViewIndex.SetTextColor(Color.ParseColor(color));
            textViewUsername.SetTextColor(Color.ParseColor(color));
            textViewTotalXP.SetTextColor(Color.ParseColor(color));
        }

        public Bitmap ConvertByteArrayToBitmap(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return BitmapFactory.DecodeStream(stream);
            }
        }
    }
}

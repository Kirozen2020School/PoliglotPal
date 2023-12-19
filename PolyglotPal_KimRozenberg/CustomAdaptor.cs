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
            imageViewProfilePic.SetImageBitmap(bitmap);
            imageViewProfilePic.SetScaleType(ImageView.ScaleType.FitXy);

            UpdateColors();

            return view;
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

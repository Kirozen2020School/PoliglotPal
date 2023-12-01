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

        public CustomAdapter(Context context, List<Account> accounts)
        {
            this.context = context;
            this.accounts = accounts;
        }

        public override int Count => accounts.Count;

        public override long GetItemId(int position) => position;

        public override Account this[int position] => accounts[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var account = accounts[position];
            var view = LayoutInflater.From(context).Inflate(Resource.Layout.customLayout, null);

            // Customize the view using the account data
            var textViewIndex = view.FindViewById<TextView>(Resource.Id.textViewIndex);
            var textViewUsername = view.FindViewById<TextView>(Resource.Id.textViewUsername);
            var textViewTotalXP = view.FindViewById<TextView>(Resource.Id.textViewTotalXP);
            var imageViewProfilePic = view.FindViewById<ImageView>(Resource.Id.imageViewProfilePic);

            textViewIndex.Text = (position+1).ToString();
            textViewUsername.Text = account.username;
            textViewTotalXP.Text = $"Total XP: {account.totalxp}";

            // Set the profile pic (replace this with your logic)
            Bitmap bitmap = ConvertByteArrayToBitmap(account.profilepic);
            imageViewProfilePic.SetImageBitmap(bitmap);
            imageViewProfilePic.SetScaleType(ImageView.ScaleType.FitXy);

            return view;
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

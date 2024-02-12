using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Media;

namespace PolyglotPal_KimRozenberg
{
    [Service]
    public class MusicService : Service
    {
        private ISharedPreferences sp;
        private MediaPlayer mp;

        public override IBinder OnBind(Intent intent) { return null; }

        public override void OnCreate()
        {
            base.OnCreate();
            mp = MediaPlayer.Create(this, Resource.Raw.background2);
            mp.Looping = true;
            mp.SetVolume(100, 100);
            sp = this.GetSharedPreferences("details",FileCreationMode.Private);
        }

        public override StartCommandResult OnStartCommand(Intent intent,
            [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            int position = sp.GetInt("position", 0);
            mp.SeekTo(position);
            mp.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnDestroy()
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutInt("position", mp.CurrentPosition);
            editor.Commit();
            mp.Stop();
            mp.Release();
            base.OnDestroy();
        }
    }
}
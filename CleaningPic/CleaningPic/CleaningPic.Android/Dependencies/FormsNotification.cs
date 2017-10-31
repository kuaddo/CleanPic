using CleaningPic.Utils;
using CleaningPic.Droid;
using Xamarin.Forms;
using Android.App;
using Android.Support.V7.App;
using System;
using Android.Content;

[assembly: Dependency(typeof(FormsNotification))]
namespace CleaningPic.Droid
{
    public class FormsNotification : IFormsNotification
    {
        public const string title = "title";
        public const string message = "message";
        private static DateTime unixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public void Notify(string title, string message)
        {
            var context = Android.App.Application.Context;

            var intent = new Intent(context, typeof(NotificationReceiver))
                .PutExtra(FormsNotification.title, title)
                .PutExtra(FormsNotification.message, message);
            var pending = PendingIntent.GetBroadcast(context, 9999, intent, PendingIntentFlags.UpdateCurrent);

            SetAlarm(DateTime.Now.AddSeconds(10), pending);
        }

        private void SetAlarm(DateTime dateTime, PendingIntent pending)
        {
            var context = Android.App.Application.Context;
            var manager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            // Set()はAPI19から正確では無くなったらしい。Ticksは100ns
            manager.SetExact(AlarmType.Rtc, (dateTime.Ticks - unixTime.Ticks) / 10_000, pending);
            // 上手く時間指定できてないけど、一旦保留
        }
    }

    [BroadcastReceiver]
    public class NotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var notification = new NotificationCompat.Builder(context)
                .SetContentTitle(intent.GetStringExtra(FormsNotification.title))
                .SetContentText(intent.GetStringExtra(FormsNotification.message))
                .SetSmallIcon(Resource.Drawable.ic_want)
                .SetAutoCancel(true)
                .Build();

            var manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Notify(9999, notification);
        }
    }
}
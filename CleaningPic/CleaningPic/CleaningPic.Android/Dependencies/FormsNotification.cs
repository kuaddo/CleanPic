using CleaningPic.Utils;
using CleaningPic.Droid;
using Xamarin.Forms;
using Android.App;
using Android.Support.V7.App;
using System;
using Android.Content;
using Android.OS;

[assembly: Dependency(typeof(FormsNotification))]
namespace CleaningPic.Droid
{
    public class FormsNotification : IFormsNotification
    {
        public const string title = "title";
        public const string message = "message";
        private static DateTime unixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public void Notify(string title, string message, DateTime time)
        {
            var context = Android.App.Application.Context;

            var intent = new Intent(context, typeof(NotificationReceiver))
                .PutExtra(FormsNotification.title, title)
                .PutExtra(FormsNotification.message, message);
            var pending = PendingIntent.GetBroadcast(context, 9999, intent, PendingIntentFlags.UpdateCurrent);

            SetAlarm(time, pending);
        }

        private void SetAlarm(DateTime dateTime, PendingIntent pending)
        {
            var context = Android.App.Application.Context;
            var manager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            var millis = (dateTime.Ticks - unixTime.Ticks) / 10_000;
            // APIごとにメソッドを変える。Ticksは100ns
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                manager.SetAlarmClock(new AlarmManager.AlarmClockInfo(millis, null), pending);
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                manager.SetExact(AlarmType.Rtc, millis, pending);
            else
                manager.Set(AlarmType.Rtc, millis, pending);
        }
    }

    [BroadcastReceiver]
    public class NotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // 通知をタップされた時に発行されるインテント
            var pending = TaskStackBuilder.Create(context)
                .AddNextIntent(new Intent(context, typeof(MainActivity)))
                .GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            var notification = new NotificationCompat.Builder(context)
                .SetContentTitle(intent.GetStringExtra(FormsNotification.title))
                .SetContentText(intent.GetStringExtra(FormsNotification.message))
                .SetSmallIcon(Resource.Drawable.ic_notification)
                .SetContentIntent(pending)
                .SetAutoCancel(true)
                .Build();

            var manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Notify(9999, notification);
        }
    }
}
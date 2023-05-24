using Android.App;
using Android.Content;
using System;

namespace ToDoList
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string title = intent.GetStringExtra("title");
            string message = intent.GetStringExtra("message");
            int notificationId = intent.GetIntExtra("notificationId", 0); 

            NotificationHelper notificationHelper = new NotificationHelper(context);
            var nb = notificationHelper.GetNotificationBuilder(title, message);
            notificationHelper.Notify(notificationId, nb.Build()); // Update the ID
        }
    }
}

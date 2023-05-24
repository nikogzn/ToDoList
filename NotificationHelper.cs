using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace ToDoList
{
    public class NotificationHelper
    {
        private static readonly string CHANNEL_ID = "reminder_notification_channel";

        private readonly Context mContext;
        private NotificationManager mManager;

        public NotificationHelper(Context context)
        {
            mContext = context;
            mManager = (NotificationManager)mContext.GetSystemService(Context.NotificationService);  // Initialize mManager here
            CreateNotificationChannel();
        }

        public void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var name = CHANNEL_ID;
                var descriptionText = "Channel for Reminder Notifications";
                var importance = NotificationImportance.Max;
                var channel = new NotificationChannel(CHANNEL_ID, name, importance)
                {
                    Description = descriptionText
                };

                var notificationManager = (NotificationManager)mContext.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        public NotificationCompat.Builder GetNotificationBuilder(string title, string message)
        {
            var notificationBuilder = new NotificationCompat.Builder(mContext, CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.notification)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(true);

            return notificationBuilder;
        }
        public void Notify(int id, Notification notification)
        {
            mManager.Notify(id, notification);
        }
    }
}
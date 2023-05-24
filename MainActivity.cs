using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity, TaskArrayAdapter.ITaskActions
    {
        private List<Task> tasks;
        private List<Task> filteredTasks;
        private Button _addTaskButton;
        private Button _filterByTagButton;
        private ListView _taskListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            if (!Android.Provider.Settings.CanDrawOverlays(this))
            {
                Intent intent = new Intent(Android.Provider.Settings.ActionManageOverlayPermission, Android.Net.Uri.Parse("package:" + PackageName));
                StartActivityForResult(intent, 0);
            }

            tasks = new List<Task>();
            filteredTasks = new List<Task>();

            _addTaskButton = FindViewById<Button>(Resource.Id.addTaskButton);
            _filterByTagButton = FindViewById<Button>(Resource.Id.filterByTagButton);
            _taskListView = FindViewById<ListView>(Resource.Id.taskListView);

            _addTaskButton.Click += (sender, e) =>
            {
                Task newTask = new Task("", "", DateTime.Now, 1, new List<string>());
                AddTaskDialog dialog = new AddTaskDialog(this, newTask);
                dialog.Show();
                dialog.DismissEvent += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(newTask.Title) || !string.IsNullOrEmpty(newTask.Description))
                    {
                        tasks.Add(newTask);
                        filteredTasks.Add(newTask);
                        UpdateTaskList();
                        // Set the notification for this task
                        SetNotification(newTask);
                    }
                };
            };

            _filterByTagButton.Click += delegate {
                // Inflate layout
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View dialogView = layoutInflater.Inflate(Resource.Layout.dialog_filter, null);

                Android.App.AlertDialog.Builder alertBuilder = new Android.App.AlertDialog.Builder(this);
                alertBuilder.SetView(dialogView);

                var userInput = (EditText)dialogView.FindViewById(Resource.Id.tagInput);
                alertBuilder.SetCancelable(false)
                    .SetPositiveButton("OK", delegate {
                        filterTasks(userInput.Text);
                        alertBuilder.Dispose();
                    })
                    .SetNegativeButton("Cancel", delegate {
                        alertBuilder.Dispose();
                    });

                Android.App.AlertDialog alertDialog = alertBuilder.Create();
                alertDialog.Show();
            };
        }


        private void UpdateTaskList()
        {
            TaskArrayAdapter adapter = new TaskArrayAdapter(this, filteredTasks, this);
            _taskListView.Adapter = adapter;
        }

        public void OnTaskUpdated(Task task)
        {
            // Save the task update to persistent storage if needed
        }

        public void OnTaskDeleted(Task task)
        {
            tasks.Remove(task);
            filteredTasks.Remove(task);
            UpdateTaskList();
        }


        void filterTasks(string tags)
        {
            // Check if input is empty or null, if so, set filteredTasks to tasks
            if (string.IsNullOrEmpty(tags))
            {
                filteredTasks = new List<Task>(tasks);
            }
            else
            {
                // Split the tags by comma and remove any leading or trailing white space and set to lowercase
                List<string> tagList = tags.Split(',').Select(t => t.Trim().ToLower()).ToList();

                // Filter the tasks based on the tags
                filteredTasks = tasks.Where(t => t.Tags.Intersect(tagList).Any()).ToList();
            }

            // Update the list view
            UpdateTaskList();
        }
        public void SetNotification(Task task)
        {
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);

            Intent intent = new Intent(this, typeof(AlarmReceiver));
            intent.PutExtra("title", task.Title);
            intent.PutExtra("message", task.Description);

            PendingIntent alarmIntent = PendingIntent.GetBroadcast(this, task.NotificationId, intent, PendingIntentFlags.UpdateCurrent);

            // Calculate the exact time to trigger the notification
            long triggerAtMillis = (task.DueDate.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMillis, alarmIntent);
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                alarmManager.SetExact(AlarmType.RtcWakeup, triggerAtMillis, alarmIntent);
            }
            else
            {
                alarmManager.Set(AlarmType.RtcWakeup, triggerAtMillis, alarmIntent);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList
{
    public class AddTaskDialog : Dialog
    {
        private EditText _taskTitleInput, _taskDescriptionInput;
        private DatePicker _taskDueDatePicker;
        private TimePicker _taskDueTimePicker;
        private Task _task;
        private Spinner _taskPriorityInput;
        private EditText _taskTagsInput;

        public new event EventHandler DismissEvent;

        public AddTaskDialog(Context context, Task task) : base(context)
        {
            _task = task;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.add_task_dialog);
            _taskTitleInput = FindViewById<EditText>(Resource.Id.taskTitleInput);
            _taskDescriptionInput = FindViewById<EditText>(Resource.Id.taskDescriptionInput);
            _taskDueDatePicker = FindViewById<DatePicker>(Resource.Id.taskDueDatePicker);
            _taskDueTimePicker = FindViewById<TimePicker>(Resource.Id.taskDueTimePicker);
            _taskPriorityInput = FindViewById<Spinner>(Resource.Id.taskPriorityInput);
            _taskTagsInput = FindViewById<EditText>(Resource.Id.taskTagsInput);

            _taskTagsInput.TextChanged += (sender, e) =>
            {
                var tags = _taskTagsInput.Text.Split(',').Select(tag => tag.Trim());
                _task.Tags = new List<string>(tags);
            };


            _taskTitleInput.Text = _task.Title;
            _taskDescriptionInput.Text = _task.Description;

            DateTime dueDate = _task.DueDate;
            _taskDueDatePicker.UpdateDate(dueDate.Year, dueDate.Month - 1, dueDate.Day);
            _taskDueTimePicker.Hour = dueDate.Hour;
            _taskDueTimePicker.Minute = dueDate.Minute;

            _taskDueDatePicker.DateChanged += (sender, e) =>
            {
                _task.DueDate = new DateTime(e.Year, e.MonthOfYear + 1, e.DayOfMonth, _task.DueDate.Hour, _task.DueDate.Minute, 0);
            };

            _taskDueTimePicker.TimeChanged += (sender, e) =>
            {
                _task.DueDate = new DateTime(_task.DueDate.Year, _task.DueDate.Month, _task.DueDate.Day, e.HourOfDay, e.Minute, 0);
            };

            _taskTitleInput.TextChanged += (sender, e) =>
            {
                _task.Title = _taskTitleInput.Text;
            };

            _taskDescriptionInput.TextChanged += (sender, e) =>
            {
                _task.Description = _taskDescriptionInput.Text;
            };

            _taskPriorityInput.ItemSelected += (sender, e) =>
            {
                _task.Priority = e.Position + 1;
            };

            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(Context, Resource.Array.priority_levels, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _taskPriorityInput.Adapter = adapter;

            Button addTaskDialogButton = FindViewById<Button>(Resource.Id.addTaskDialogButton);
            addTaskDialogButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(_taskTitleInput.Text) || string.IsNullOrWhiteSpace(_taskDescriptionInput.Text))
                {
                    Toast.MakeText(Context, "Please enter both title and description", ToastLength.Short).Show();
                }
                else if (_task.DueDate < DateTime.Now)
                {
                    Toast.MakeText(Context, "Due date cannot be in the past", ToastLength.Short).Show();
                }
                else if (_task.Priority < 1 || _task.Priority > 5)
                {
                    Toast.MakeText(Context, "Priority must be between 1 and 5", ToastLength.Short).Show();
                }
                else
                {
                    //Console.WriteLine($"New task added with tags: {string.Join(", ", _task.Tags)}");
                    DismissDialog();
                }
            };

            WindowManagerLayoutParams layoutParams = new WindowManagerLayoutParams();
            layoutParams.CopyFrom(Window.Attributes);
            layoutParams.Width = (int)(Context.Resources.DisplayMetrics.WidthPixels);
            layoutParams.Height = (int)(Context.Resources.DisplayMetrics.HeightPixels);
            Window.Attributes = layoutParams;
        }

        private void DismissDialog()
        {
            DismissEvent?.Invoke(this, EventArgs.Empty);
            Dismiss();
        }
    }
}


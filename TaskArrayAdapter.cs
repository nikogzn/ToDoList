using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace ToDoList
{
    public class TaskArrayAdapter : ArrayAdapter<Task>
    {
        private readonly ITaskActions _taskActions;

        public TaskArrayAdapter(Context context, List<Task> tasks, ITaskActions taskActions) : base(context, 0, tasks)
        {
            _taskActions = taskActions;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            if (convertView == null)
            {
                convertView = LayoutInflater.From(Context).Inflate(Resource.Layout.task_list_item, parent, false);
                holder = new ViewHolder
                {
                    TaskTitle = convertView.FindViewById<EditText>(Resource.Id.taskTitle),
                    TaskDescription = convertView.FindViewById<EditText>(Resource.Id.taskDescription),
                    DeleteButton = convertView.FindViewById<ImageButton>(Resource.Id.delete_button),
                    TaskDueDate = convertView.FindViewById<TextView>(Resource.Id.taskDueDate),
                    TaskPriority = convertView.FindViewById<TextView>(Resource.Id.taskPriority),
                    TaskTags = convertView.FindViewById<TextView>(Resource.Id.taskTags),
                    EventHandler = new TaskEventHandler()
                };
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            Task task = GetItem(position);

            holder.EventHandler.UpdateTask(task, _taskActions);

            holder.TaskTitle.AfterTextChanged -= holder.EventHandler.TaskTitle_AfterTextChanged;
            holder.TaskTitle.Text = task.Title;
            holder.TaskTitle.AfterTextChanged += holder.EventHandler.TaskTitle_AfterTextChanged;

            holder.TaskDescription.AfterTextChanged -= holder.EventHandler.TaskDescription_AfterTextChanged;
            holder.TaskDescription.Text = task.Description;
            holder.TaskDescription.AfterTextChanged += holder.EventHandler.TaskDescription_AfterTextChanged;

            holder.TaskDueDate.Text = task.DueDate.ToString("dd/MM/yy HH:mm");
            holder.TaskPriority.Text = new string('!', task.Priority);

            holder.TaskTags.Text = string.Join(", ", task.Tags);

            holder.DeleteButton.Click -= holder.EventHandler.DeleteButton_Click;
            holder.DeleteButton.Click += holder.EventHandler.DeleteButton_Click;

            return convertView;
        }


        public interface ITaskActions
        {
            void OnTaskUpdated(Task task);
            void OnTaskDeleted(Task task);
        }

        private class ViewHolder : Java.Lang.Object
        {
            public EditText TaskTitle { get; set; }
            public EditText TaskDescription { get; set; }
            public ImageButton DeleteButton { get; set; }
            public TextView TaskDueDate { get; set; }
            public TextView TaskPriority { get; set; }
            public TaskEventHandler EventHandler { get; set; }
            public TextView TaskTags { get; set; }
        }

        private class TaskEventHandler
        {
            private Task _task;
            private ITaskActions _taskActions;

            public void UpdateTask(Task task, ITaskActions taskActions)
            {
                _task = task;
                _taskActions = taskActions;
            }

            public void TaskTitle_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                _task.Title = (sender as EditText).Text;
                _taskActions.OnTaskUpdated(_task);
            }

            public void TaskDescription_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                _task.Description = (sender as EditText).Text;
                _taskActions.OnTaskUpdated(_task);
            }

            public void DeleteButton_Click(object sender, EventArgs e)
            {
                _taskActions.OnTaskDeleted(_task);
            }
        }
    }
}

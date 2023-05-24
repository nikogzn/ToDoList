using System;
using System.Collections.Generic;

namespace ToDoList
{
    public class Task
    {
        public string Title, Description;
        public DateTime DueDate;
        public int Priority;
        public List<string> Tags { get; set; }
        public int NotificationId { get; private set; }

        public Task(string title, string description, DateTime dueDate, int priority, List<string> tags)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Tags = tags;
            NotificationId = new Random().Next(int.MaxValue);
        }
    }
}

using System;

namespace ToDoList
{
    public class ToDoTask
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }

        public ToDoTask() { }

        public ToDoTask(string id, string title, string description, DateTime dueDate, int priority)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
        }
    }
}

using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Task = System.Threading.Tasks.Task;
using Android.Gms.Extensions;
using Java.Util;

namespace ToDoList
{
    public class TaskViewModel
    {
        private static readonly string TaskCollection = "tasks";
        private readonly FirebaseFirestore _db;
        private readonly string _userId;

        public TaskViewModel(string userId)
        {
            _db = AppDataHelper.GetFirestore();
            _userId = userId;
        }

        public async Task<List<ToDoTask>> GetTasksAsync()
        {
            Query query = _db.Collection(TaskCollection).WhereEqualTo("userId", _userId);
            QuerySnapshot snapshot = await query.Get().AsAsync<QuerySnapshot>();
            List<ToDoTask> tasks = new List<ToDoTask>();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                ToDoTask task = ConvertDocumentToTask(document);
                tasks.Add(task);
            }

            return tasks;
        }

        public async Task<ToDoTask> SaveTaskAsync(ToDoTask task)
        {
            DocumentReference docRef = _db.Collection(TaskCollection).Document();
            await docRef.Set((IDictionary<string, Java.Lang.Object>)ConvertTaskToDictionary(task));
            DocumentSnapshot docSnapshot = await docRef.Get().AsAsync<DocumentSnapshot>();
            return ConvertDocumentToTask(docSnapshot);
        }

        public async Task UpdateTaskAsync(ToDoTask task)
        {
            DocumentReference docRef = _db.Collection(TaskCollection).Document(task.Id);
            DocumentSnapshot snapshot = await docRef.Get().AsAsync<DocumentSnapshot>();
            if (snapshot.Exists())
            {
                await snapshot.Reference.Set((IDictionary<string, Java.Lang.Object>)ConvertTaskToDictionary(task));
            }
        }

        public async Task DeleteTaskAsync(ToDoTask task)
        {
            DocumentReference docRef = _db.Collection(TaskCollection).Document(task.Id);
            DocumentSnapshot snapshot = await docRef.Get().AsAsync<DocumentSnapshot>();
            if (snapshot.Exists())
            {
                await snapshot.Reference.Delete();
            }
        }

        private ToDoTask ConvertDocumentToTask(DocumentSnapshot document)
        {
            Date timestamp = document.GetDate("dueDate");
            return new ToDoTask
            {
                Id = document.Id,
                Title = document.GetString("title"),
                Description = document.GetString("description"),
                DueDate = DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Time).ToLocalTime().DateTime,
                Priority = Convert.ToInt32(document.Data["priority"])
            };
        }

        private Dictionary<string, Java.Lang.Object> ConvertTaskToDictionary(ToDoTask task)
        {
            return new Dictionary<string, Java.Lang.Object>
            {
                {"userId", _userId},
                {"title", task.Title},
                {"description", task.Description},
                {"dueDate", new Date(task.DueDate.ToUniversalTime().Ticks / 10000 - 62135596800000L)},
                {"priority", task.Priority}
            };
        }
    }
}

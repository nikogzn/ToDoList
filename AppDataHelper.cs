using Android.App;
using Android.Content;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

namespace ToDoList
{
    public static class AppDataHelper
    {
        static ISharedPreferences preferences = Application.Context.GetSharedPreferences("userinfo", FileCreationMode.Private);
        static ISharedPreferencesEditor editor;
        static FirebaseFirestore database;
        public static FirebaseFirestore GetFirestore()
        {
            if (database != null)
            {
                return database;
            }
            var app = FirebaseApp.InitializeApp(Application.Context);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("todolist-c9dff")
                    .SetApplicationId("todolist-c9dff")
                    .SetApiKey("AIzaSyD6TiIPnvMUX_4F2KLB2JWYusWTsd4VARk")
                    .SetDatabaseUrl("https://todolist-c9dff.firebaseio.com")
                    .SetStorageBucket("todolist-c9dff.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options, "ToDoList");
                //FirebaseApp.InitializeApp(context, options, "MarketList");
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }
        public static FirebaseAuth GetFirebaseAuthentication()
        {
            FirebaseAuth firebaseAuthentication;
            var app = FirebaseApp.InitializeApp(Application.Context);
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("todolist-c9dff")
                    .SetApplicationId("todolist-c9dff")
                    .SetApiKey("AIzaSyD6TiIPnvMUX_4F2KLB2JWYusWTsd4VARk")
                    .SetDatabaseUrl("https://todolist-c9dff.firebaseio.com")
                    .SetStorageBucket("todolist-c9dff.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(Application.Context, options);
                firebaseAuthentication = FirebaseAuth.Instance;
            }
            else
            {
                firebaseAuthentication = FirebaseAuth.Instance;
            }
            return firebaseAuthentication;
        }

        public static void SaveUserId(string userId)
        {
            editor = preferences.Edit();
            editor.PutString("userId", userId);
            editor.Apply();
        }

        public static string GetUserId()
        {
            string userId = "";
            userId = preferences.GetString("userId", "");
            return userId;
        }
    }
}
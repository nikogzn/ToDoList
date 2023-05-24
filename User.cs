using Firebase.Auth;
using Firebase;
using Firebase.Firestore;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using System.Collections.Generic;

namespace ToDoList
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        FirebaseAuth firebaseAuthentication;
        public List<Task> tasks;
        public const string COLLECTION_NAME = "Users";
        FirebaseFirestore database;

        public User()
        {
        }
        public User(string email)
        {
            this.Email = email;
            this.firebaseAuthentication = AppDataHelper.GetFirebaseAuthentication();
            this.database = AppDataHelper.GetFirestore();
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password; 
            this.firebaseAuthentication = AppDataHelper.GetFirebaseAuthentication();
            this.database = AppDataHelper.GetFirestore();
        }

        public async Task<bool> Login()
        {
            try
            {
                await this.firebaseAuthentication.SignInWithEmailAndPassword(this.Email, this.Password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> GetInfo()
        {
            try
            {
                object obj = await this.database.Collection(COLLECTION_NAME).WhereEqualTo("Email", this.Email).Get();
                QuerySnapshot query = (QuerySnapshot)obj;
                if (query.IsEmpty)
                {
                    return null;
                }
                DocumentSnapshot item = query.Documents[0];
                User user = new User(this.Email);
                if (item.Get("Email") != null)
                {
                    user.Email = item.Get("Email").ToString();
                }
                else
                {
                    user.Email = "";
                }
                if (item.Get("password") != null)
                {
                    user.Password = item.Get("password").ToString();
                }
                else
                {
                    user.Password = "";
                }
                return user;
            }
            catch
            {
                return null;
            }
            
        }
    }
}

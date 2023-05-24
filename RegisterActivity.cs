using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ToDoList
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private EditText _emailEditText, _passwordEditText;
        private Button _registerButton;
        private TextView _loginTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);

            _emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            _passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            _registerButton = FindViewById<Button>(Resource.Id.registerButton);
            _loginTextView = FindViewById<TextView>(Resource.Id.loginTextView);

            _registerButton.Click += RegisterButton_Click;
            _loginTextView.Click += LoginTextView_Click;
        }

        private void RegisterButton_Click(object sender, System.EventArgs e)
        {
            // Handle registration logic with Firebase Authentication
        }

        private void LoginTextView_Click(object sender, System.EventArgs e)
        {
            // Navigate to LoginActivity
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}

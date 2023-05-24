using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ToDoList
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText _emailEditText, _passwordEditText;
        private Button _loginButton;
        private TextView _registerTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            _emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            _passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            _loginButton = FindViewById<Button>(Resource.Id.loginButton);
            _registerTextView = FindViewById<TextView>(Resource.Id.registerTextView);

            _loginButton.Click += LoginButton_Click;
            _registerTextView.Click += RegisterTextView_Click;
        }

        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            //try
            //{
            //    User user = new User();
            //}
            //handle login logic here
        }

        private void RegisterTextView_Click(object sender, System.EventArgs e)
        {
            // Navigate to RegisterActivity
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
        }
    }
}

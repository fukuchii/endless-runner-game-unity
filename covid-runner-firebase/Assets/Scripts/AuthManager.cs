using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using System.Linq;


public class AuthManager : MonoBehaviour
{

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DbReference;

    //Login variables
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public Text warningLoginText;

    //Register variables
    [Header("Register")]
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField passwordRegisterVerifyField;
    public Text warningRegisterText;

    //Display
    [Header("Scoreboard")]
    //public InputField usernameField;
    public TMP_InputField UserText;
    public TMP_InputField EmailText;
    public TMP_InputField PasswordText;

    //Display
    [Header("Game Object")]
    public GameObject Control;
    public GameObject Edit;
    public GameObject success;
    public Text warningTextUpdate;
    public Transform scoreboardContent;

    private string message;
    private bool isUserUpdated;
    private bool isEmailUpdated;
    private bool isPasswordUpdate;

    public GameObject an;
    private ArrayList arlist = new ArrayList();
    public Dictionary<string, int> list = new Dictionary<string, int>();


    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Save()
    {
        warningTextUpdate.GetComponent<Text>().color = Color.black;
        warningTextUpdate.GetComponent<Text>().text = "wait...";
        StartCoroutine(LoadAllUsernames());
    }
    public IEnumerator SaveDataButton()
    {
        if (string.IsNullOrWhiteSpace(UserText.text) || string.IsNullOrWhiteSpace(EmailText.text) || string.IsNullOrWhiteSpace(PasswordText.text)) {
            warningTextUpdate.GetComponent<Text>().color = Color.red;
            warningTextUpdate.text = "Please fill up the required field!";
        }
        else if (arlist.Contains(UserText.text))
        {
            warningTextUpdate.GetComponent<Text>().color = Color.red;
            warningTextUpdate.text = "Username already existing!";
        }
        else if (!string.IsNullOrWhiteSpace(UserText.text) || !string.IsNullOrWhiteSpace(EmailText.text) || !string.IsNullOrWhiteSpace(PasswordText.text))
        {
            
            UpdateUsernameAuth(UserText.text);
            UpdateEmail(EmailText.text);
            UpdatePassword(PasswordText.text);
            yield return StartCoroutine(UpdateUsernameDatabase(UserText.text));
            yield return new WaitForSeconds(2f);
            Debug.Log("email: "+isEmailUpdated);
            Debug.Log("user: "+isUserUpdated);
            Debug.Log("email: "+isPasswordUpdate);

            if (isUserUpdated == true && isEmailUpdated == true && isPasswordUpdate == true) {
                warningTextUpdate.color = Color.green;
                warningTextUpdate.text = "Successfully edited";
                UserText.text = "";
                EmailText.text = "";
                PasswordText.text = "";
                yield return new WaitForSeconds(0.8f);
                Control.GetComponent<animator>().Navigate("Main Menu");
            }
            else
            {
                warningTextUpdate.GetComponent<Text>().color = Color.red;
                warningTextUpdate.text = "Error";
            }
        }


    }
    public void LoginButton()
    {
        SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.click);
        warningLoginText.GetComponent<Text>().color = Color.black;
        warningLoginText.GetComponent<Text>().text = "wait...";
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.click);
        warningRegisterText.GetComponent<Text>().color = Color.black;
        warningRegisterText.GetComponent<Text>().text = "wait...";
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(LoadAllUsernames(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));


    }
    private IEnumerator Login(string _email, string _password)
    {
        warningLoginText.text = "";
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them

            string message = "";
            try {
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
            }
            catch {
                message = "Invalid Login!";
            }

            warningLoginText.color = Color.red;
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result

            User = LoginTask.Result;
            warningLoginText.color = Color.green;
            warningLoginText.text = "Success!";
            StartCoroutine(LoadUserData());
            Control.GetComponent<animator>().FadeToLevel("Main Menu");
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        Debug.Log("runned register");
        
        if (_username == "")
        {
            warningRegisterText.GetComponent<Text>().color = Color.red;
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.GetComponent<Text>().color = Color.red;
            warningRegisterText.text = "Password Does Not Match!";
        }
        else if (arlist.Contains(_username)) 
        {
            warningRegisterText.GetComponent<Text>().color = Color.red;
            warningRegisterText.text = "Username already Taken!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx;
                string message = "";
                try
                {
                    firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;

                    }
                }
                catch
                {
                    if (passwordRegisterVerifyField.text.Length < 6)
                    {
                        message = "Insufficient password length!";
                    }
                    else { message = "Register Failed"; }

                }
                warningRegisterText.GetComponent<Text>().color = Color.red;
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };
                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        warningRegisterText.GetComponent<Text>().color = Color.red;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        yield return StartCoroutine(Login(_email, _password));
                        yield return StartCoroutine(UpdateUsernameDatabase(_username));
                        yield return StartCoroutine(InitiateScore());
                        auth.SignOut();
                        Debug.Log("Sucess Registration");
                        Control.GetComponent<animator>().Navigate("Register Complete");
                    }
                }

            }
        } 
    }
    public void SignOut() {
        auth.SignOut();
        an.GetComponent<animator>().Navigate("StartScreen");
        Debug.Log("signed out");
    }
    private void UpdateUsernameAuth(string _username)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            UserProfile profile = new UserProfile{DisplayName = _username};
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                isUserUpdated = true;
                Debug.Log("User profile updated successfully.");
            });
        }
    }
    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        FirebaseUser user = auth.CurrentUser;
        if (_username != null) 
        {
            var DBTask = DbReference.Child("users").Child(user.UserId).Child("username").SetValueAsync(_username);
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                warningTextUpdate.text = "Update failed!";
            }
        }
        else
        {
            warningTextUpdate.text = "Username empty!";
        }
    }
    
    public IEnumerator LoadUserData()
    {
        FirebaseUser user = auth.CurrentUser;
        //Get the currently logged in user data
        var DBTask = DbReference.Child("users").Child(user.UserId).Child("Highscore").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            Debug.Log("Null");
            
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            PlayerPrefs.SetString("username", user.DisplayName);
            PlayerPrefs.SetString("Email", user.Email);
            PlayerPrefs.SetString("Highscore", snapshot.Value.ToString());
        }
    }
    private IEnumerator InitiateScore() {
        FirebaseUser user = auth.CurrentUser;
        var DBTask = DbReference.Child("users").Child(user.UserId).Child("Highscore").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            
            var DBTask2 = DbReference.Child("users").Child(user.UserId).Child("Highscore").SetValueAsync(0);
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
            Debug.Log("updated score");

        }
        else
        {
            //Data has been retrieved
        }

    }

    public IEnumerator LoadScoreboardData()
      {
          //Get all the users data ordered by kills amount

          var DBTask = DbReference.Child("users").OrderByChild("Highscore").GetValueAsync();
          yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

          if (DBTask.Exception != null)
          {
              Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
          }
          else
          {
             DataSnapshot snapshot = DBTask.Result;

             foreach (DataSnapshot childSnapshot in snapshot.Children)
              {
                 string name = "";
                 int score=0;
                 foreach (DataSnapshot dataSnapshot in childSnapshot.Children)
                 {
                    if(dataSnapshot.Key == "username")
                     {
                         name = dataSnapshot.Value.ToString();
                     }
                     else
                     {
                         score = int.Parse(dataSnapshot.Value.ToString());
                     }
                 }
                 list.Add(name, score);
             }  
          }
      }

    private void UpdateEmail(string _email)
    {
        FirebaseUser user = auth.CurrentUser;
     
        if (user != null)
        {
            user.UpdateEmailAsync(_email).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("Email Update was cancelled.");
                    isEmailUpdated = false;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("EmailAsync encountered an error: " + task.Exception);
                    warningTextUpdate.text = "Email Error!";
                    isEmailUpdated = false;
                    return;
                }
                isEmailUpdated = true;
                Debug.Log("Email updated successfully.");
            });
        }
        else
        {
            warningTextUpdate.text = "Email Empty!";
            isEmailUpdated = false;
        }
    }
    private void UpdatePassword(string _password)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.UpdatePasswordAsync(_password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdatePasswordAsync was canceled.");
                    isPasswordUpdate = false;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdatePasswordAsync encountered an error: " + task.Exception);
                    warningTextUpdate.text = "Password error";
                    isPasswordUpdate = false;
                    return;
                }
                isPasswordUpdate = true;
                Debug.Log("Password updated successfully.");
            });
        }
        else
        {
            warningTextUpdate.text = "Password empty!";
            isPasswordUpdate = false;
        }
    }
    public IEnumerator UpdateScore(int hscore)
    {
        FirebaseUser user = auth.CurrentUser;
        var DBTask = DbReference.Child("users").Child(user.UserId).Child("Highscore").SetValueAsync(hscore);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else { 
            //inserted
        }
           
    }

    public void AnonymousLogin()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
    public IEnumerator LoadAllUsernames(string _email, string _password, string _username)
    {
        AnonymousLogin();
        var DBTask = DbReference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null) { Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}"); }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children) //for each userid
            {
                string str_name = "";
                foreach (DataSnapshot dataSnapshot in childSnapshot.Children) //for each child of userid
                {
                    if (dataSnapshot.Key == "username")
                    {
                        str_name = dataSnapshot.Value.ToString();
                    }
                }
                arlist.Add(str_name);
            }
        }
        Debug.Log("Loaded all user names");
        auth.SignOut();
        StartCoroutine(Register(_email, _password, _username));
    }
    public IEnumerator LoadAllUsernames()
    {
        var DBTask = DbReference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null) { Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}"); }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children) //for each userid
            {
                string str_name = "";
                foreach (DataSnapshot dataSnapshot in childSnapshot.Children) //for each child of userid
                {
                    if (dataSnapshot.Key == "username")
                    {
                        str_name = dataSnapshot.Value.ToString();
                    }
                }
                arlist.Add(str_name);
            }
            StartCoroutine(SaveDataButton());
        }
        Debug.Log("Loaded all user names");
    }
}

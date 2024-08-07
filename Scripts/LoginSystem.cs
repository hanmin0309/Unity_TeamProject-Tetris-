using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;


public class LoginSystem : MonoBehaviour
{

    public InputField email;
    public InputField password, signupEmail, signupPassword, signupPassword1, signupUserName;

    public GameObject loginPage, signUpPage, profilePage;
    public Text profileUserEmail, profileUserName;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public bool isSignIn = false;
    //public Text outputText;

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void OnChangedState(bool sign)
    {
        //outputText.text = sign ? "로그인" : "로그아웃 : ";
        // outputText.text += FirebaseAuthManager.Instance.UserId;
    }

    public void OpenLoginPage()
    {
        Debug.Log("실행");
        loginPage.SetActive(true);
        signUpPage.SetActive(false);
        profilePage.SetActive(false);
    }

    public void OpenSignUp()
    {
        Debug.Log("실행");
        loginPage.SetActive(false);
        signUpPage.SetActive(true);
        profilePage.SetActive(false);
    }

    public void OpenProfile()
    {
        Debug.Log("실행");
        loginPage.SetActive(false);
        profilePage.SetActive(true);
    }

    public void Back()
    {
        OpenLoginPage();
    }

    public void Create()
    {
        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupPassword1.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            return;
        }

        CreateUser(signupEmail.text, signupPassword.text, signupUserName.text);

    }


    public void LoginUser()
    {
        Login(email.text, password.text);
    }
    public void Login(string email, string password)
    {
        //FirebaseAuthManager.Instance.Login(email.text, password.text);
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            DataSaver.instance.userId = result.User.UserId;
            DataSaver.instance.LoadDataFn();

            OpenProfile();
        });

    }

    public void Logout()
    {
        auth.SignOut();
        profileUserEmail.text = "";
        profileUserName.text = "";
        OpenLoginPage();
    }

    void CreateUser(string email, string password, string userName)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Firebase.Auth.FirebaseUser newUser = result.User;

            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            if (result.User != null)
            {
                // 사용자 정보 저장
                DataSaver.instance.dts = new DataToSave
                {
                    userName = signupUserName.text,
                    userEmail = signupEmail.text,
                    bestScore = 0 // 초기 값 설정
                };
                DataSaver.instance.userId = newUser.UserId;
                DataSaver.instance.SaveDataFn();          
            }
            // 사용자 프로필 업데이트
            UpdateUserProfile(userName);

            Debug.Log("회원가입 및 데이터 저장이 완료되었습니다.");
        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                isSignIn = false;
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }


    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private void OnApplicationQuit()
    {
        Logout();
    }

    void UpdateUserProfile(string uerName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = uerName,
                PhotoUrl = new System.Uri("https://via.placeholder.com/150"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
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

                Debug.Log("User profile updated successfully.");

                //알림창
            });
        }
    }


    public bool isSigned = false;

    private void Update()
    {
        if (isSignIn)
        {
            if (!isSigned)
            {
                isSigned = true;
                profileUserName.text = "" + user.DisplayName;
                profileUserEmail.text = "" + user.Email;


                OpenProfile();
            }
        }

    }
}

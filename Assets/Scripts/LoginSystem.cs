using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System;
using System.Threading.Tasks;



public class LoginSystem : MonoBehaviour
{

    public InputField email;
    public InputField password, signupEmail, signupPassword, signupPassword1, signupUserName;

    public GameObject loginPage, signUpPage, profilePage, notification;
    public Text profileUserEmail, profileUserName, NotifiTitle, NotifiMessage, checkPasswordText;

    public Button loginButton, profileButton, startButton, optionButton;
    public Text profileButtonText;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference databaseRef;

    public bool isSignIn = false;
    string errorTitle = "����";
    string title = "�˸�";
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

    public void OpenLoginPage()
    {
        Debug.Log("�α��� ������");
        email.text = "";
        password.text = "";

        loginPage.SetActive(true);
        signUpPage.SetActive(false);
        profilePage.SetActive(false);
    }

    public void OpenSignUp()
    {
        Debug.Log("ȸ������");
        loginPage.SetActive(false);
        signUpPage.SetActive(true);
        profilePage.SetActive(false);

        signupPassword.onValueChanged.AddListener(delegate { CheckPassword(); });
        signupPassword1.onValueChanged.AddListener(delegate { CheckPassword(); });

    }
    public void OpenProfile()
    {
        Debug.Log("������");
        loginPage.SetActive(false);
        signUpPage.SetActive(false);
        profilePage.SetActive(true);
    }

    private void CheckPassword()
    {
        if ((signupPassword.text != signupPassword1.text) && signupPassword1 != null)
        {
            checkPasswordText.color = Color.red;
            checkPasswordText.text = "��ġ���� �ʽ��ϴ�.";
            return;
        }
        else if ((signupPassword.text == signupPassword1.text) && signupPassword1 != null)
        {
            checkPasswordText.color = Color.green;
            checkPasswordText.text = "��ġ�մϴ�.";
        }
        else if (signupPassword == null && signupPassword1 == null)
        {
            checkPasswordText.text = "";
        }
    }

    public void ShowNotification(string title, string message)
    {
        NotifiTitle.text = "" + title;
        NotifiMessage.text = "" + message;
        Debug.Log("�˸�â");
        notification.SetActive(true);
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }

    public void Back()
    {
        OpenLoginPage();
    }

    public void CloseSignInPage()
    {
        loginPage.SetActive(false);
    }

    public void CloseProfilePage()
    {
        profilePage.SetActive(false);
    }

    public void CloseNotification()
    {
        NotifiMessage.text = "";
        notification.SetActive(false);
    }

    public void OpenGame()
    {
        SceneManager.LoadScene("Tetris_Fighter");

    }

    public void Create()
    {
        if (string.IsNullOrEmpty(signupUserName.text))
        {
            ShowNotification(errorTitle, "�г����� �Է��ϼ���");
            return;
        }

        CreateUser(signupEmail.text, signupPassword.text, signupUserName.text);

    }
    void CreateUser(string email, string password, string userName)
    {
        if (signupPassword.text != signupPassword1.text)
        {
            return;
        }

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
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowNotification(errorTitle, GetErrorMessage(errorCode));

                    }
                }
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;

            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            DataSaver.instance.userId = result.User.UserId;
            DataSaver.instance.LoadDataFn(() =>
            {
                if (result.User != null)
                {
                    // ����� ���� ����
                    DataSaver.instance.dts = new DataToSave
                    {
                        userName = signupUserName.text,
                        userEmail = signupEmail.text,
                        enemyKill = 0,
                        bestScore = 0 // �ʱ� �� ����
                    };

                    DataSaver.instance.SaveDataFn();

                }
            });

            // ����� ������ ������Ʈ
            UpdateUserProfile(userName);

            ShowNotification(title, "ȸ�������� �Ϸ�Ǿ����ϴ�.");
            Logout();
            Debug.Log("ȸ������ �� ������ ������ �Ϸ�Ǿ����ϴ�.");

        });
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

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowNotification(errorTitle, GetErrorMessage(errorCode));

                    }
                }
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            DataSaver.instance.userId = result.User.UserId;
            DataSaver.instance.LoadDataFn();

            if (DataSaver.instance.dts == null)
            {
                DataSaver.instance.dts = new DataToSave
                {
                    userName = result.User.DisplayName,
                    userEmail = result.User.Email,
                    bestScore = 0 // �ʱ� �� ����
                };
                DataSaver.instance.userId = result.User.UserId;
                DataSaver.instance.SaveDataFn();
            }

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


    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
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

                loginButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(false);
                profileButton.GetComponent<Button>().interactable = false;

            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                isSignIn = true;
                Debug.Log("Signed in " + user.UserId);
                Debug.Log("Signed in " + user.Email);
                Debug.Log("Signed in " + user.DisplayName);

                loginButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(true);
                profileButton.GetComponent<Button>().interactable = true;

                profileUserName.text = "" + user.DisplayName;
                profileUserEmail.text = "" + user.Email;

                // profileButtonText.color = Color.white;

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
            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
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

            });
        }
    }



    private void Update()
    {

    }

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";

        switch (errorCode)
        {
            case AuthError.InvalidEmail:
                message = "�߸��� �̸��� ����";
                Debug.LogError("�߸��� �̸��� ����");
                break;
            case AuthError.MissingEmail:
                message = "�̸����� �Է��ϼ���";
                break;
            case AuthError.MissingPassword:
                message = "��й�ȣ�� �Է��ϼ���";
                break;
            case AuthError.WrongPassword:
                message = "��й�ȣ�� ��ġ�����ʽ��ϴ�.";
                Debug.LogError("��й�ȣ�� ��ġ�����ʽ��ϴ�.");
                break;
            case AuthError.UserNotFound:
                message = "����ڸ� ã�� �� ����";
                Debug.LogError("����ڸ� ã�� �� ����");
                break;
            case AuthError.EmailAlreadyInUse:
                message = "�� �̸����� �̹� ��� ���Դϴ�.";
                Debug.LogError("�� �̸����� �̹� ��� ���Դϴ�.");
                break;
            case AuthError.WeakPassword:
                message = "��й�ȣ�� �ʹ� ���մϴ�.\n(6���� �̻�)";
                Debug.LogError("��й�ȣ�� �ʹ� ���մϴ�");
                break;
            // Add more cases as needed
            default:
                message = "�߸��� ����";
                Debug.LogError($"Unrecognized error code: {errorCode}, AuthError: {errorCode}");
                break;
        }

        return message;
    }
}

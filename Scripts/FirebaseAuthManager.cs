using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System.Runtime.CompilerServices;
using System;

public class FirebaseAuthManager
{
    private static FirebaseAuthManager instance = null ;

    public static FirebaseAuthManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FirebaseAuthManager();   
            }
            return instance ;
        }
    }

    private FirebaseAuth auth;
    private FirebaseUser user;

    public string UserId => user.UserId;

    public Action<bool> LoginState;
     
    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            Logout();
        }
        auth.StateChanged += OnChanged;
    }
    private void OnChanged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser == user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("�α׾ƿ�");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("�α���");
                LoginState?.Invoke(true);
            }
        }
    }
    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("ȸ������ ����");
                SignError(task.Exception);
            return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.Log("ȸ������ �Ϸ�");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("�α��� ����");
                SignError(task.Exception);
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.Log("�α��� �Ϸ�");
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }

    public void SignError(AggregateException exception)
    {
        foreach (Exception innerException in exception.Flatten().InnerExceptions)
        {
            FirebaseException firebaseEx = innerException as FirebaseException;
            if (firebaseEx != null)
            {
                // You can now check the error code to see what went wrong
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.InvalidEmail:
                        Debug.LogError("�߸��� �̸��� ����");
                        break;
                    case AuthError.WrongPassword:
                        Debug.LogError("�߸��� ��й�ȣ");
                        break;
                    case AuthError.UserNotFound:
                        Debug.LogError("����ڸ� ã�� �� ����");
                        break;
                    case AuthError.EmailAlreadyInUse:
                        Debug.LogError("�� �̸����� �̹� ��� ���Դϴ�");
                        break;
                    case AuthError.WeakPassword:
                        Debug.LogError("��й�ȣ�� �ʹ� ���մϴ�");
                        break;
                    // Add more cases as needed
                    default:
                        Debug.LogError($"Unrecognized error code: {errorCode}, AuthError: {errorCode}");
                        break;
                }
            }
        }
        return;
    }
}


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
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("로그인");
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
                Debug.Log("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("회원가입 실패");
                SignError(task.Exception);
            return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.Log("회원가입 완료");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("로그인 실패");
                SignError(task.Exception);
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.Log("로그인 완료");
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
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
                        Debug.LogError("잘못된 이메일 형식");
                        break;
                    case AuthError.WrongPassword:
                        Debug.LogError("잘못된 비밀번호");
                        break;
                    case AuthError.UserNotFound:
                        Debug.LogError("사용자를 찾을 수 없음");
                        break;
                    case AuthError.EmailAlreadyInUse:
                        Debug.LogError("이 이메일은 이미 사용 중입니다");
                        break;
                    case AuthError.WeakPassword:
                        Debug.LogError("비밀번호가 너무 약합니다");
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


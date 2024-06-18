using System;
using UnityEngine;

public class LoginEventManager : MonoBehaviour
{
    public static event Func<bool> _connectDBCallback;
    public static event Func<bool> _disConnectDBCallback;
    public static event Func<bool> _loginCallback;
    public static event Action _signupCallback;

    public static bool OnEnable_ConnectDB_ActionInvoke()
    {
        bool? connected = _connectDBCallback?.Invoke();
        if (connected != null) return (bool)connected;
        else return false;
    }

    public static bool OnDisable_DisconnetDB_ActionInvoke()
    {
        bool? disconnected = _disConnectDBCallback?.Invoke();
        if (disconnected != null) return (bool)disconnected;
        else return false;
    }

    public static bool OnClick_Login_ActionInvoke()
    {
        bool? loginSuccess = _loginCallback?.Invoke();
        if (loginSuccess != null) return (bool)loginSuccess;
        else return false;
    }

    public static void OnClick_Signup_ActionInvoke()
    {
        _signupCallback?.Invoke();
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine;

public class LoginManager
{
    [Header("ConnectionInfo")]
    [SerializeField] string _ip;
    [SerializeField] string _dbName;
    [SerializeField] string _uid;
    [SerializeField] string _pwd;

    private static MySqlConnection _dbConnection;

    private static LoginManager _instance = null;

    public static LoginManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoginManager();
            }
            return _instance;
        }
    }

    public LoginManager()
    {
        _ip = DBConfig.Instance.IP;
        _dbName = DBConfig.Instance.DBname;
        _uid = DBConfig.Instance.Uid;
        _pwd = DBConfig.Instance.Pwd;

    }

    /*
    public void RegisterLoginEventCallback()
    {
        LoginEventManager._connectDBCallback += ConnectDB;
        LoginEventManager._disConnectDBCallback += DisconnectDB;
        LoginEventManager._loginCallback += Login_SendQuery;
        //LoginEventManager._signupCallback
    }

    public void UnRegisterLoginEventCallback()
    {
        LoginEventManager._connectDBCallback -= ConnectDB;
        LoginEventManager._disConnectDBCallback -= DisconnectDB;
        LoginEventManager._loginCallback -= Login_SendQuery;
        //LoginEventManager._signupCallback
    }
    */

    public bool LoginUI_OnEnable_ConnectDB()
    {
        _dbConnection = DBConnectionManager.Instance.OpenDBConnection();
        return (_dbConnection.State == ConnectionState.Open);
    }

    public bool LoginUI_OnDisable_DisconnectDB()
    {
        DBConnectionManager.Instance.CloseDBConnection();
        return (_dbConnection.State == ConnectionState.Closed);
    }

    public bool LoginUI_OnClick_Login_SendQuery(string id, string password)
    {
        /*
        COUNT(*) : 행의 수를 세는 집계 함수. *는 모든 열의 의미하며, 이 경우 조건에 맞는 모든 행의 수를 센다.
        FROM user_info : 데이터를 가져올 테이블을 지정하는 키워드. user_info가 우리가 조회하려는 테이블 이름.
        WHERE : 특정 조건을 지정하는 키워드.
        U_Name=@id : U_Name이 @id와 일치하는 행을 찾는 조건. @는 쿼리문에서 매개변수를 나타냄.
         */
        string query = "SELECT COUNT(*) FROM user_info WHERE U_Name=@id AND U_Password=@password";

        try
        {
            using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
            {
                // MySqlCommand.Parameters.AddWithValue("@id", id); 로 쿼리문의 매개변수에 사용자 입력값을 할당할 수 있다.
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);

                // MySqlCommand.ExecuteScalar()는 SQL쿼리를 실행하고 단일 값을 반환한다.
                // 여기서는 단일 값 COUNT(*)를 반환한다.
                // Convert.ToInt32로 반환값을 int형으로 바꾼다.
                int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                if (userCount > 0)
                {
                    Debug.Log("Login successful!");
                    return true;
                }
                else
                {
                    Debug.Log("Login failed.");
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
            return false;
        }
    }

    public void LoginUI_OnClick_Signup()
    {
        UIManager.Instance.ShowSignupUI();
    }
}

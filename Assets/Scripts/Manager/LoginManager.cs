using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    [Header("ConnectionInfo")]
    [SerializeField] string _ip;
    [SerializeField] string _dbName;
    [SerializeField] string _uid;
    [SerializeField] string _pwd;

    private static MySqlConnection _dbConnection;

    private int _user_id = -1;
    public int UserID { get { return _user_id; } }

    public static LoginManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 선택 사항: 씬 전환 시 유지
        }
    }

    public LoginManager()
    {
        _ip = DBConfig.Instance.IP;
        _dbName = DBConfig.Instance.DBname;
        _uid = DBConfig.Instance.Uid;
        _pwd = DBConfig.Instance.Pwd;

    }

    public bool LoginUI_OnEnable_ConnectDB()
    {
        _user_id = -1;
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
        string query = "SELECT U_id FROM user_info WHERE U_Name=@id AND U_Password=@password";

        try
        {
            using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
            {
                // MySqlCommand.Parameters.AddWithValue("@id", id); 로 쿼리문의 매개변수에 사용자 입력값을 할당할 수 있다.
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);

                // MySqlCommand.ExecuteScalar()는 SQL쿼리를 실행하고 단일 값을 반환한다.
                // 여기서는 단일 값 U_id를 반환한다.
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    _user_id = Convert.ToInt32(result);
                    Debug.Log($"Login successful! User ID: {_user_id}");
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

    public void LoginUI_OnClick_Login_Access()
    {
        UIManager.Instance.CloseSpecificUI(UIType.Login);
        UIManager.Instance.OpenSpecificUI(UIType.Lobby);
    }

    public void LoginUI_OnClick_Signup()
    {
        UIManager.Instance.CloseSpecificUI(UIType.Login);
        UIManager.Instance.OpenSpecificUI(UIType.Signup);
    }
}

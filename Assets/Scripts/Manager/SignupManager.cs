using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine;

public class SignupManager
{
    [Header("ConnectionInfo")]
    [SerializeField] string _ip;
    [SerializeField] string _dbName;
    [SerializeField] string _uid;
    [SerializeField] string _pwd;

    private static MySqlConnection _dbConnection;

    private static SignupManager _instance = null;

    public static SignupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SignupManager();
            }
            return _instance;
        }
    }

    public SignupManager()
    {
        _ip = DBConfig.Instance.IP;
        _dbName = DBConfig.Instance.DBname;
        _uid = DBConfig.Instance.Uid;
        _pwd = DBConfig.Instance.Pwd;
    }

    public bool SignupUI_OnEnable_ConnectDB()
    {
        _dbConnection = DBConnectionManager.Instance.OpenDBConnection();
        return (_dbConnection.State == ConnectionState.Open);
    }

    public bool SignupUI_OnDisable_DisconnectDB()
    {
        DBConnectionManager.Instance.CloseDBConnection();
        return (_dbConnection.State == ConnectionState.Closed);
    }

    public bool SignupUI_OnClick_Signup_SendQuery(string id, string password)
    {
        if(CheckNullOrWhiteSpace(id, password) == false) { Debug.Log("공백으로 계정 생성 불가."); return false; }

        string query = "SELECT COUNT(*) FROM user_info WHERE U_Name=@id";

        try
        {
            using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
            {
                cmd.Parameters.AddWithValue("@id", id);

                int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                if (userCount == 0)
                {
                    Debug.Log("DB에 중복되는 아이디가 없음. 회원가입 시작.");
                    return Signup_SendQuery(id, password);
                }
                else
                {
                    Debug.Log("DB에 이미 같은 아이디 있음.");
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

    private bool Signup_SendQuery(string id, string password)
    {
        int uid = GetMaxUid_SendQuery();

        //쿼리를 보내서 id와 password를 DB의 U_Name과 U_Password에 저장.
        string query = "INSERT INTO user_info (U_Name, U_Password, U_id) VALUES (@id, @password, @uid)";

        try
        {
            using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.ExecuteNonQuery();
                Debug.Log("Signup successful!");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
            return false;
        }
    }

    private int GetMaxUid_SendQuery()
    {
        string query = "SELECT IFNULL(MAX(U_id), 0) FROM user_info";
        int newUserId;

        using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
        {
            newUserId = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
        }
        return newUserId;
    }

    private bool CheckNullOrWhiteSpace(string id, string password)
    {
        if(string.IsNullOrWhiteSpace(id)) return false;
        if(string.IsNullOrWhiteSpace(password)) return false;
        return true;
    }

    public void SignupUI_OnClick_Exit()
    {
        UIManager.Instance.ShowLoginUI();
    }
}

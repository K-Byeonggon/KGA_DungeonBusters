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
            DontDestroyOnLoad(gameObject); // ���� ����: �� ��ȯ �� ����
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
        COUNT(*) : ���� ���� ���� ���� �Լ�. *�� ��� ���� �ǹ��ϸ�, �� ��� ���ǿ� �´� ��� ���� ���� ����.
        FROM user_info : �����͸� ������ ���̺��� �����ϴ� Ű����. user_info�� �츮�� ��ȸ�Ϸ��� ���̺� �̸�.
        WHERE : Ư�� ������ �����ϴ� Ű����.
        U_Name=@id : U_Name�� @id�� ��ġ�ϴ� ���� ã�� ����. @�� ���������� �Ű������� ��Ÿ��.
         */
        string query = "SELECT U_id FROM user_info WHERE U_Name=@id AND U_Password=@password";

        try
        {
            using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
            {
                // MySqlCommand.Parameters.AddWithValue("@id", id); �� �������� �Ű������� ����� �Է°��� �Ҵ��� �� �ִ�.
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);

                // MySqlCommand.ExecuteScalar()�� SQL������ �����ϰ� ���� ���� ��ȯ�Ѵ�.
                // ���⼭�� ���� �� U_id�� ��ȯ�Ѵ�.
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

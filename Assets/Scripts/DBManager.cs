using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
    [Header("ConnectionInfo")]
    [SerializeField] string _ip = "127.0.0.1";
    [SerializeField] string _dbName = "dungeon_busters";
    [SerializeField] string _uid = "root";
    [SerializeField] string _pwd = "1234";

    private static MySqlConnection _dbConnection;
    private bool _isConnectTestComplete;
    private string str_DBResult;

    private static DBManager _instance = null;

    public static DBManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new DBManager();
            }
            return _instance;
        }
    }

    public bool LoginUI_OnStart_ConnectDB()
    {
        string connectStr = $"Server={_ip};Database={_dbName};Uid={_uid};PWD={_pwd};";

        try
        {
            _dbConnection = new MySqlConnection(connectStr);
            _dbConnection.Open();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"e: {e.ToString()}");
            return false;
        }
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


    /*
    private void SendQuery(String query, string tableName)
    {
        if (query.Contains("SELECT"))
        {
            DataSet dataSet = OnSelectRequest(query, tableName);

            str_DBResult = DeformatResult(dataSet);
            
            Debug.Log(str_DBResult);
        }
    }

    private DataSet OnSelectRequest(string query, string tableName)
    {
        try
        {
            _dbConnection.Open();
            //MySqlCommand로 DB연결과 query 정보를 담고,
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = _dbConnection;
            sqlCmd.CommandText = query;

            //DataSet을 채워줄 MySqlDataAdapter를 MySqlCommand로 만들어준다.
            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlCmd);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);
            _dbConnection.Close();
            return dataSet;
        }
        catch (Exception e)
        {
            Debug.LogError($"{e}");
            return null;
        }
    }

    //DataSet을 string으로 바꿔준다.
    private string DeformatResult(DataSet dataSet)
    {
        string resultStr = string.Empty;

        foreach(DataTable table in dataSet.Tables)
        {
            foreach(DataRow row in table.Rows)
            {
                foreach(DataColumn column in table.Columns)
                {
                    resultStr += $"{column.ColumnName}: {row[column]}";
                }
                resultStr += "\n";
            }
        }
        return resultStr;
    }*/
}

using MySql.Data.MySqlClient;
using System;
using UnityEngine;

public class DBConnectionManager
{
    private static DBConnectionManager _instance;
    private MySqlConnection _dbConnection;

    public static DBConnectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DBConnectionManager();
            }
            return _instance;
        }
    }

    private DBConnectionManager() { }

    public MySqlConnection OpenDBConnection()
    {
        if (_dbConnection == null)
        {
            string ip = DBConfig.Instance.IP;
            string dbName = DBConfig.Instance.DBname;
            string uid = DBConfig.Instance.Uid;
            string pwd = DBConfig.Instance.Pwd;

            string connectStr = $"Server={ip};Database={dbName};Uid={uid};PWD={pwd};";
            _dbConnection = new MySqlConnection(connectStr);

            try
            {
                _dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error: {e.ToString()}");
            }
        }
        return _dbConnection;
    }

    public void CloseDBConnection()
    {
        if (_dbConnection != null)
        {
            try
            {
                _dbConnection.Close();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error: {e.ToString()}");
            }
            _dbConnection = null;
        }
    }
}
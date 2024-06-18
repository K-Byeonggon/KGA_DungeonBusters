public class DBConfig
{
    private static DBConfig _instance = null;

    public static DBConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DBConfig();
            }
            return _instance;
        }
    }

    private string _ip = "127.0.0.1";
    private string _dbName = "dungeon_busters";
    private string _uid = "root";
    private string _pwd = "1234";

    public string IP { get { return _ip; } }
    public string DBname { get { return _dbName; } }
    public string Uid { get { return _uid; } }
    public string Pwd { get { return _pwd; } }
}

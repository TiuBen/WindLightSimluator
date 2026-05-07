using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.Service
{
    // 这是一个单例类，全局只有一个实例
    public class DatabaseService
    {
        private static readonly DatabaseService _instance = new DatabaseService();
        public static DatabaseService Instance => _instance;

        private string _connectionString = "";
        private string _currentPath = "";

        // 1. 定义默认路径：当前用户的 "文档" 目录 + 数据库文件名
        // 结果类似于：C:\Users\HJW-AMD-PRP\Documents\MyAppData.db
        // 这里改成你想要的数据库文件名
        private readonly string _defaultDbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WeatherSimulator.db");


        // 私有构造函数，防止外部 new
        private DatabaseService()
        {
            LoadDefaultDatabase();
        }
        private void LoadDefaultDatabase()
        {
            try
            {
                if (!File.Exists(_defaultDbPath))
                    return;
                Connect(_defaultDbPath);

                Debug.WriteLine($"✅ 已加载默认数据库: {_defaultDbPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ 默认数据库加载失败: {ex.Message}");
            }
        }

        // 1. 连接数据库
        public bool Connect(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return false;

                _connectionString = $"Data Source={path}";
                _currentPath = path;

                Debug.WriteLine($"✅ 数据库已连接: {path}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ 连接失败: {ex.Message}");
                return false;
            }
        }

        public string CurrentPath => _currentPath;

        // 2. 通用查询方法 (返回数据表)
        // 这样你就不用每次都写 CreateCommand, ExecuteReader 了
        public DataTable Query(string sql)
        {
            Debug.WriteLine($"Query: {sql}");

            var table = new DataTable();

            try
            {
                using var connection = new SqliteConnection(_connectionString);

                connection.Open();

                using var command = connection.CreateCommand();

                command.CommandText = sql;

                using var reader = command.ExecuteReader();

                table.Load(reader);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return table;

        }

        // 3. 获取所有表名
        public List<string> GetTableNames()
        {
            var list = new List<string>();

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText =
                    "SELECT name FROM sqlite_master WHERE type='table';";

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    if (!name.StartsWith("sqlite_"))
                        list.Add(name);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetTableNames: {ex}");
            }

            return list;
        }

        public bool CreateCurrentTimeTable(string tableName)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var tx = connection.BeginTransaction();

                string createSql = $@"
                        CREATE TABLE ""{tableName}"" (
                            ""WindDirection"" TEXT,
                            ""WindSpeed""     TEXT,
                            ""Temperature""   TEXT,
                            ""QNH""           TEXT,
                            ""RVR""           TEXT,
                            ""VIS""           TEXT
                        );";

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = createSql;
                    cmd.Transaction = tx;
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = connection.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = $@"INSERT INTO ""{tableName}"" (""WindDirection"", ""WindSpeed"", ""Temperature"", ""QNH"", ""RVR"", ""VIS"")
                                            VALUES ('180', '2', '15', '1013', '2500', '5000');";

                    for (int i = 0; i < 120; i++)
                        cmd.ExecuteNonQuery();
                }

                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ CreateTable: {ex}");
                return false;
            }

        }

        public bool ReNameSelectedTable(string oldTableName, string newTableName)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = $@"ALTER TABLE ""{oldTableName}"" RENAME TO ""{newTableName}"";";

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Rename: {ex}");
                return false;
            }
        }

        public void SavePointsToSelectedTable(string tableName, string columnName, ObservableCollection<double> points)
        {
            Debug.WriteLine(points);

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var tx = connection.BeginTransaction();

                using var cmd = connection.CreateCommand();
                cmd.Transaction = tx;

                cmd.CommandText = $@"UPDATE ""{tableName}"" 
                                    SET ""{columnName}"" = @val 
                                    WHERE ROWID = @rowid";

                var pVal = cmd.Parameters.Add("@val", SqliteType.Text);
                var pId = cmd.Parameters.Add("@rowid", SqliteType.Integer);

                for (int i = 0; i < points.Count; i++)
                {
                    pVal.Value = points[i].ToString();
                    pId.Value = i + 1;
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Save: {ex}");
            }
        }

        public bool DeleteSelectedTable(string tableName)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(tableName))
            {
                Debug.WriteLine("❌ 删除失败: 表名不能为空");
                return false;
            }

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var tx = connection.BeginTransaction();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"DROP TABLE IF EXISTS \"{tableName}\";";
                cmd.Transaction = tx;

                cmd.ExecuteNonQuery();

                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Delete: {ex}");
                return false;
            }
        }

        public bool CopySelectTable(string selectedTable, string newTableName)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(selectedTable))
            {
                Debug.WriteLine("❌  CopySelectTable 失败: 表名不能为空");
                return false;
            }

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var tx = connection.BeginTransaction();

                using var cmd = connection.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText =
                    $"CREATE TABLE \"{newTableName}\" AS SELECT * FROM \"{selectedTable}\";";

                cmd.ExecuteNonQuery();

                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Copy: {ex}");
                return false;
            }
        }





    }
}


using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Windows.Documents;
using System.Xml.Linq;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.Service
{
    // 这是一个单例类，全局只有一个实例
    public class DatabaseService
    {
        private static readonly DatabaseService _instance = new DatabaseService();
        public static DatabaseService Instance => _instance;

        private SqliteConnection _connection;
        private string _currentPath = "";

        // 私有构造函数，防止外部 new
        private DatabaseService() { }

        // 1. 连接数据库
        public bool Connect(string path)
        {
            try
            {
                // 如果已经在连接同一个文件，直接返回
                if (_currentPath == path && _connection != null && _connection.State == ConnectionState.Open)
                {
                    return true;
                }

                // 关闭旧的连接
                if (_connection != null)
                {
                    _connection.Close();
                }

                // 确保目录存在
                string? directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 建立新连接
                _connection = new SqliteConnection($"Data Source={path}");
                _connection.Open();
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

        // 2. 通用查询方法 (返回数据表)
        // 这样你就不用每次都写 CreateCommand, ExecuteReader 了
        public DataTable Query(string sql)
        {
            Debug.WriteLine($"Query: {sql}");
            if (_connection == null || _connection.State != ConnectionState.Open) return new DataTable();

            var table = new DataTable();
            try
            {
                using (var command = new SqliteCommand(sql, _connection))
                using (var reader = command.ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"查询错误: {ex.Message}");
            }
            return table;
        }

        // 3. 获取所有表名
        public List<string> GetTableNames()
        {
            var tables = new List<string>();
            if (_connection == null || _connection.State != ConnectionState.Open) return tables;

            var command = _connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    if (!name.StartsWith("sqlite_"))
                    {
                        tables.Add(name);
                    }
                }
            }
            return tables;
        }

        public void CopySelectTable()
        {

        }

        public void CreateCurrentTimeTable()
        {
            // 1. 生成表名：例如 20260402210741
            string tableName = DateTime.Now.ToString("yyyyMMddHHmmss");

            // 2. 构建 SQL 语句
            // 注意：表名通常不需要加单引号，但为了安全起见（防止关键字冲突），可以用双引号或方括号括起来
            string createSql = $@"
                        CREATE TABLE ""{tableName}"" (
                            ""WindDirection"" TEXT,
                            ""WindSpeed""     TEXT,
                            ""Temperature""   TEXT,
                            ""QNH""           TEXT,
                            ""RVR""           TEXT,
                            ""VIS""           TEXT
                        );";
            // 2. 开启连接
            using (var connection = _connection) // 复用之前的连接
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                using (var transaction = connection.BeginTransaction()) // ✅ 开启事务
                {
                    try
                    {
                        // --- 第一步：建表 ---
                        using (var cmdCreate = connection.CreateCommand())
                        {
                            cmdCreate.CommandText = createSql;
                            cmdCreate.Transaction = transaction; // 将命令加入事务
                            cmdCreate.ExecuteNonQuery();
                        }

                        // --- 第二步：准备插入语句 ---
                        // 使用参数化查询防止 SQL 注入，且效率更高
                        string insertSql = $@"INSERT INTO ""{tableName}"" (""WindDirection"", ""WindSpeed"", ""Temperature"", ""QNH"", ""RVR"", ""VIS"")
                                            VALUES ('180', '2', '15', '1013', '2500', '5000');";

                        using (var cmdInsert = connection.CreateCommand())
                        {
                            cmdInsert.CommandText = insertSql;
                            cmdInsert.Transaction = transaction; // 将命令加入事务

                            for (int i = 0; i < 120; i++)
                            {
                                cmdInsert.ExecuteNonQuery();
                            }
                        }

                        // ✅ 提交事务：一次性写入磁盘
                        transaction.Commit();
                        Debug.WriteLine($"✅ 成功创建表 '{tableName}' 并插入 120 行数据！");
                    }
                    catch (Exception ex)
                    {
                        // ❌ 出错回滚：如果中间断了，表也不会创建成功，数据不会脏
                        transaction.Rollback();
                        Debug.WriteLine($"❌ 批量插入失败，已回滚: {ex.Message}");
                    }
                }


                Debug.WriteLine($"✅ 表 '{tableName}' 创建成功！");
            }
        }


        public void ReNameSelectedTable( string oldTableName,string newTableName)
        {
            

            // 2. 开启连接
            using (var connection = _connection) // 复用之前的连接
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                try
                {
                    var reNameCommand = _connection.CreateCommand();
                    reNameCommand.CommandText = $@"ALTER TABLE ""{oldTableName}"" RENAME TO ""{newTableName}"";";
                    reNameCommand.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                


                Debug.WriteLine($"改表 '{newTableName}' 成功！");
            }
        }

        public void SavePointsToSelectedTable(string tableName,string columnName, ObservableCollection<double> pointsValue)
        {   // 1. 开启事务（一次性写入，速度快）
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    string sql = $@"UPDATE ""{tableName}"" 
                                    SET ""{columnName}"" = @val 
                                    WHERE ROWID = @rowid"; // ROWID 是 SQLite 自带的隐藏主键，从 1 开始

                    using (var cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Transaction = transaction;

                        var pVal = cmd.Parameters.Add("@val", SqliteType.Text);
                        var pRowId = cmd.Parameters.Add("@rowid", SqliteType.Integer);

                        // 2. 循环 120 次
                        for (int i = 0; i < pointsValue.Count; i++)
                        {
                            pVal.Value = pointsValue[i].ToString();
                            pRowId.Value = i + 1; // 数据库 ROWID 从 1 开始，列表索引从 0 开始

                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    Debug.WriteLine($"✅ 更新 {columnName} 列完成");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine($"❌ 更新失败: {ex.Message}");
                }
            }
        }


        // 4. 获取当前路径 (方便外面知道连的是哪个文件)
        public string CurrentPath => _currentPath;
    }
}



//_currentDbPath = path;
//var vm = DataContext as EditableWeatherElementViewModel;
//var _tableNames = vm.Tables;
//try
//{
//    using (var connection = new SqliteConnection($"Data Source={path}"))
//    {

//        connection.Open();
//        var command = connection.CreateCommand();
//        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

//        // 2. 执行查询并读取结果
//        using (var reader = command.ExecuteReader())
//        {
//            _tableNames.Clear(); // 先清空旧数据

//            while (reader.Read())
//            {
//                // reader.GetString(0) 获取第一列的数据，也就是表名
//                string tableName = reader.GetString(0);
//                Debug.WriteLine(tableName);

//                // 过滤掉 SQLite 内部自动生成的表（如 sqlite_sequence）
//                if (!tableName.StartsWith("sqlite_"))
//                {
//                    _tableNames.Add(tableName);
//                }
//            }
//        }


//    }
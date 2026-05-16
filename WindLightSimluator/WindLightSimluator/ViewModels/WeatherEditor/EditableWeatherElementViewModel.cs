using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Xml.Linq;
using WindLightSimluator.Model;
using WindLightSimluator.Service;
using WindLightSimluator.utils;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.Views;
using WindLightSimluator.Views.Components;

namespace WindLightSimluator.ViewModels
{
    public class FieldConfig
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double BaseValue { get; set; }
        public int Step { get; set; } // 一个格子代表修改多少数据
        public double SubStep { get; set; }// 数据吸附的步长，用于精细控制
        public string Unit { get; set; }
    }

    public class EditableWeatherElementViewModel : ViewModelBase
    {
        #region 天气元素配置
        public List<string> FieldList { get; } = new()
        {
            "WindDirection",
            "WindSpeed",
            "Temperature",
            "QNH",
            "RVR",
            "VIS"

        };

        // ✅ 每个字段配置
        public Dictionary<string, FieldConfig> FieldConfigs { get; } = new()
        {
            ["WindDirection"] = new FieldConfig { Min = 0, Max = 360, BaseValue = 180, Step = 10, SubStep = 10, Unit = "°" },
            ["WindSpeed"] = new FieldConfig { Min = 0, Max = 20, BaseValue = 2, Step = 1, SubStep = 0.5, Unit = "m/s" },
            ["Temperature"] = new FieldConfig { Min = -20, Max = 50, BaseValue = 15, Step = 2, SubStep = 1, Unit = "℃" },
            ["QNH"] = new FieldConfig { Min = 980, Max = 1040, BaseValue = 1013, Step = 2, SubStep = 1, Unit = "hPa" },
            ["RVR"] = new FieldConfig { Min = 0, Max = 2500, BaseValue = 2000, Step = 100, SubStep = 25, Unit = "m" },
            ["VIS"] = new FieldConfig { Min = 0, Max = 15000, BaseValue = 5000, Step = 1000, SubStep = 500, Unit = "m" },
        };
        #endregion

        #region 字段数据
        private Dictionary<string, ObservableCollection<double>> RawData { get; } = new();
        private string _selectedField = "QNH";
        public string SelectedField
        {
            get => _selectedField;
            set {
                if (SetProperty(ref _selectedField, value))
                {
                    // ✅ 切换 Points 数据源

                    OnPropertyChanged(nameof(SelectedFieldConfig));
                    // ✅ 切换 Points 数据源
                    SelectedFieldPoints = RawData[value];
                }
            }
        }

        private ObservableCollection<double> _selectedFieldPoints;
        public ObservableCollection<double> SelectedFieldPoints
        {
            get => _selectedFieldPoints;
            set => SetProperty(ref _selectedFieldPoints, value);
        }


        public FieldConfig SelectedFieldConfig => FieldConfigs[SelectedField];
        #endregion

        #region 状态相关

        private bool _isModified;
        public bool IsModified
        {
            get => _isModified;
            set {
                if (SetProperty(ref _isModified, value))
                {
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        // 是否已连接数据库
        public bool IsDatabaseConnected =>
            !string.IsNullOrWhiteSpace(DatabaseFilePath)
            && File.Exists(DatabaseFilePath);

        // 是否已选中表
        public bool IsTableSelected =>
            IsDatabaseConnected
            && !string.IsNullOrWhiteSpace(SelectedTableName);

        // 新建
        public bool CanNew =>
            IsDatabaseConnected;

        // 复制 / 删除
        public bool CanCopyDelete =>
            IsTableSelected;

        // 保存
        public bool CanSave =>
            IsTableSelected
            && IsModified;

        // 重命名
        public bool CanRename
        {
            get {
                if (!IsTableSelected)
                    return false;

                if (string.IsNullOrWhiteSpace(NewTableName))
                    return false;

                // ✅ 新增：必须与原名称不同
                if (NewTableName == SelectedTableName)
                    return false;


                if (NewTableName.Length > 15)
                    return false;

                // 只允许字母数字下划线
                if (!Regex.IsMatch(NewTableName, @"^[a-zA-Z0-9_]+$"))
                    return false;

                // 禁止 sqlite_ 开头
                if (NewTableName.StartsWith("sqlite_", StringComparison.OrdinalIgnoreCase))
                    return false;

                // 已存在重名
                if (Tables.Contains(NewTableName)
                    && NewTableName != SelectedTableName)
                    return false;

                return true;
            }
        }

        #endregion


        #region 数据库相关

        //private readonly DatabaseService _db = DatabaseService.Instance;
        private readonly DatabaseService _db;
        private readonly AirportVM _airport;

        private string _databaseFilePath = string.Empty;
        public string DatabaseFilePath
        {
            get => _databaseFilePath;
            set {
                if (SetProperty(ref _databaseFilePath, value))
                {
                    OpenDatabase(value);

                    NotifyCanStates();
                }
            }
        }

        private ObservableCollection<string> _tables = new();
        public ObservableCollection<string> Tables
        {
            get => _tables;
            set => SetProperty(ref _tables, value);
        }

        private string _selectedTableName = string.Empty;
        public string SelectedTableName
        {
            get => _selectedTableName;
            set {
                if (SetProperty(ref _selectedTableName, value))
                {
                    // 默认同步到输入框
                    NewTableName = value;

                    // 加载数据
                    _ = LoadTableAsync(value);

                    NotifyCanStates();
                }
            }
        }

        private string _newTableName;
        public string NewTableName
        {
            get => _newTableName;
            set {
                if (SetProperty(ref _newTableName, value))
                {
                    OnPropertyChanged(nameof(CanRename));
                }
            }
        }

        #endregion


        #region 构造函数

        public EditableWeatherElementViewModel(DatabaseService db,AirportVM airport)
        {
            _db = db;

            _airport = airport;
            // 初始化数据
            foreach (var key in FieldList)
            {
                var config = FieldConfigs[key];

                RawData[key] =
                    new ObservableCollection<double>(
                        Enumerable.Repeat(config.BaseValue, 120));
            }
            // 被选中项目的
            SelectedFieldPoints = RawData[SelectedField];
        }

        #endregion


        #region 数据修改

        public void UpdatePointValue(int index, double newValue)
        {
            if (RawData.ContainsKey(SelectedField)
                && index >= 0
                && index < SelectedFieldPoints.Count)
            {
                SelectedFieldPoints[index] = newValue;

                IsModified = true;
            }
        }

        #endregion

        #region 数据库操作

        public void OpenDatabase(string path)
        {
            if (_db.Connect(path))
            {
                GetAllTableNames();
            }
            _airport.RefreshTables();

        }

        public void GetAllTableNames()
        {
            var tableNames = _db.GetTableNames();

            Tables.Clear();

            foreach (var name in tableNames)
            {
                Tables.Add(name);
            }

            NotifyCanStates();
        }

        private async Task LoadTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return;

            var data = await Task.Run(() => LoadDataFromTable(tableName));

            if (data == null)
            {
                Debug.WriteLine("data == null");
                return;
            }



            // 🔥 回到UI线程后再更新 ObservableCollection
            foreach (var key in FieldList)
            {
                RawData[key].Clear();

                foreach (var val in data[key])
                {
                    RawData[key].Add(val);
                }
            }
            // 切换回 UI 线程更新绑定
            SelectedFieldPoints = RawData[SelectedField];
            Debug.WriteLine(SelectedFieldPoints);

            IsModified = false;
        }

        public Dictionary<string, ObservableCollection<double>> LoadDataFromTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return null;

            var data =
                _db.Query($"SELECT * FROM \"{tableName}\"");

            // 检查是否有数据
            if (data == null || data.Rows.Count == 0)
                return null;

            var rawData =
                new Dictionary<string, ObservableCollection<double>>();

            foreach (var key in FieldList)
            {
                rawData[key] = new ObservableCollection<double>();
            }

            foreach (DataRow row in data.Rows)
            {
                foreach (string key in FieldList)
                {
                    if (double.TryParse(row[key].ToString(), out double val))
                    {
                        rawData[key].Add(val);
                    }
                    else
                    {
                        rawData[key].Add(FieldConfigs[key].BaseValue);
                    }
                }
            }

            return rawData;
        }

        public void CreateNewTable()
        {
            string newTableName =
                DateTime.Now.ToString("yyyyMMddHHmmss");

            if (_db.CreateCurrentTimeTable(newTableName))
            {
                GetAllTableNames();

                SelectedTableName = newTableName;
            }
        }

        public void CopyTable()
        {
            if (!CanCopyDelete)
                return;

            string newTableName =
                DateTime.Now.ToString("yyyyMMddHHmmss");

            if (_db.CopySelectTable(
                SelectedTableName,
                newTableName))
            {
                GetAllTableNames();

                SelectedTableName = newTableName;
            }
        }

        public void RenameTable()
        {
            if (!CanRename)
                return;

            if (_db.ReNameSelectedTable(
                SelectedTableName,
                NewTableName))
            {
                GetAllTableNames();

                SelectedTableName = NewTableName;
            }
        }

        public void SaveToDatabase()
        {
            if (!CanSave)
                return;

            _db.SavePointsToSelectedTable(
                SelectedTableName,
                SelectedField,
                SelectedFieldPoints);

            IsModified = false;
        }

        public void DeleteSelectedTable()
        {
            if (!CanCopyDelete)
                return;

            if (_db.DeleteSelectedTable(SelectedTableName))
            {
                GetAllTableNames();

                SelectedTableName = string.Empty;
            }
        }

        #endregion

        #region 通知刷新

        private void NotifyCanStates()
        {
            OnPropertyChanged(nameof(IsDatabaseConnected));
            OnPropertyChanged(nameof(IsTableSelected));

            OnPropertyChanged(nameof(CanNew));
            OnPropertyChanged(nameof(CanCopyDelete));
            OnPropertyChanged(nameof(CanRename));
            OnPropertyChanged(nameof(CanSave));

            _airport.RefreshTables();
        }

        #endregion



    }

}




//public void LoadDataFromTable(string tableName)
//{
//    if (string.IsNullOrEmpty(tableName)) return;
//    Debug.WriteLine("LoadDataFromTable");
//    // 查询数据
//    var data = _db.Query($"SELECT * FROM \"{tableName}\"");

//    // 初始化字典，给每个字段创建一个空的列表
//    // 假设 FieldList 是你的字段名列表，如 ["WindSpeed", "Temperature"...]
//    foreach (var key in FieldList)
//    {
//        // 先清空原有数据
//        if (!RawData.ContainsKey(key))
//        {
//            RawData[key] = new ObservableCollection<double>();
//        }
//        else
//        {
//            RawData[key].Clear();
//        }
//    }

//    // 遍历每一行
//    foreach (DataRow row in data.Rows)
//    {
//        foreach (string key in FieldList)
//        {
//            // ✅ 修正逻辑：先尝试解析，如果成功，把 val 加入列表
//            if (double.TryParse(row[key].ToString(), out double val))
//            {
//                RawData[key].Add(val);
//            }
//            else
//            {
//                RawData[key].Add(FieldConfigs[key].BaseValue); // 如果解析失败，存个默认值 0
//            }
//        }
//    }
//    // ✅ 更新 SelectedFieldPoints
//    if (RawData.ContainsKey(SelectedField))
//    {
//        SelectedFieldPoints = RawData[SelectedField];
//    }
//    OnPropertyChanged(nameof(SelectedFieldPoints));

//    Debug.WriteLine("LoadDataFromTable");
//}
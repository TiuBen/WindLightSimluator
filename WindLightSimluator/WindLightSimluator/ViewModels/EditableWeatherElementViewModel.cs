using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WindLightSimluator.Model;
using WindLightSimluator.ViewModels.Base;
using System.Windows;
using WindLightSimluator.Views.Components;
using System.Printing.IndexedProperties;
using WindLightSimluator.Views;
using System.IO;
using WindLightSimluator.utils;
using System.Windows.Markup;
using WindLightSimluator.Service;
using System.Diagnostics;
using System.Data;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

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

        private string _selectedField = "QNH";
        public string SelectedField
        {
            get => _selectedField;
            set {
                if (SetProperty(ref _selectedField, value))
                {
                    // ✅ 切换 Points 数据源
                    SelectedFieldPoints = RawData[value];

                    // ✅ 刷新依赖属性
                    OnPropertyChanged(nameof(SelectedFieldConfig));
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

        private Dictionary<string, ObservableCollection<double>> RawData { get; } = new();


        public EditableWeatherElementViewModel()
        {


            // 初始化 RawData（120分钟）
            foreach (var key in FieldList)
            {
                var config = FieldConfigs[key];

                RawData[key] = new ObservableCollection<double>( Enumerable.Repeat(config.BaseValue, 120));
            }

            // 被选中项目的
            SelectedFieldPoints = RawData[SelectedField];

        }
        private bool _isModified;
        public bool IsModified
        {
            get => _isModified;
            set => SetProperty(ref _isModified, value);
        }

        public void UpdatePointValue(int index, double newValue)
        {
            // 更新当前选中的字段数据
            if (RawData.ContainsKey(SelectedField) && index >= 0 && index < SelectedFieldPoints.Count)
            {
                SelectedFieldPoints[index] = newValue;
                
                // 可选：标记为已修改，用于保存提示
                IsModified = true;
            }
        }



        //
        public string DatabaseFilePath { get; set; }
        // 数据相关部分
        private readonly DatabaseService _db = DatabaseService.Instance;


        private ObservableCollection<string> _tables=new ObservableCollection<string>();
        public ObservableCollection<string> Tables
        {
            get => _tables;
            set => SetProperty(ref _tables, value);
        }

        private string _selectedTable;
        public string SelectedTable
        {
            get => _selectedTable;
            set {
                // 1. 先更新旧表名
                if (SetProperty(ref _selectedTable, value))
                {
                    // 2. 当旧表名改变时，自动把新表名重置为旧表名的值
                    // 这样用户一选中表，输入框里默认就是这个名字
                    NewTableName = value;
                }
            }
        }

        public string _newTableName;
        public string NewTableName
        {
            get => _newTableName;
            set {
                if (SetProperty(ref _newTableName, value))
                {
                    ValidateName();
                }
            }
        }

        private bool _canRename;
        public bool CanRename
        {
            get => _canRename;
            set => SetProperty(ref _canRename, value);
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(NewTableName))
            {
                CanRename = false;
                return;
            }

            if (NewTableName.Length > 15)
            {
                CanRename = false;
                return;
            }

            // ❗ 只允许字母 + 数字 + 下划线
            if (!Regex.IsMatch(NewTableName, @"^[a-zA-Z0-9]+$"))
            {
                CanRename = false;
                return;
            }

            CanRename = true;
        }


        // 2. 连接数据库的方法 (对应你之前的 GetAllTableNames)
        public void GetAllTableNames(string path)
        {
            if (_db.Connect(path))
            {
                // 连接成功后，自动刷新表名列表
                Tables.Clear();
                var tableNames = _db.GetTableNames();
                foreach (var name in tableNames)
                {
                    Tables.Add(name);
                }

            }
        }
        public void CreateNewTable(string path)
        {
            _db.CreateCurrentTimeTable();
            if (_db.Connect(path))
            {
                // 连接成功后，自动刷新表名列表
                Tables.Clear();
                var tableNames = _db.GetTableNames();
                foreach (var name in tableNames)
                {
                    Tables.Add(name);
                }

            }
        }



        // 3. 查询数据的方法 (对应你未来的 LoadData)
        public void LoadDataFromTable(string tableName)
        {
            Debug.WriteLine("LoadDataFromTable");

            // 1. 使用 List<double> 而不是数组，因为它可以动态添加数据
            var _rawData = new Dictionary<string, List<double>>();

            // 2. 初始化字典，给每个字段创建一个空的列表
            // 假设 FieldList 是你的字段名列表，如 ["WindSpeed", "Temperature"...]
            foreach (var key in FieldList)
            {
                _rawData[key] = new List<double>();
            }

            // 3. 查询数据
            var data = _db.Query($"SELECT * FROM \"{tableName}\"");

            // 4. 遍历每一行
            foreach (DataRow row in data.Rows)
            {
                foreach (string key in FieldList)
                {
                    // ✅ 修正逻辑：先尝试解析，如果成功，把 val 加入列表
                    if (double.TryParse(row[key].ToString(), out double val))
                    {
                        _rawData[key].Add(val);
                    }
                    else
                    {
                        _rawData[key].Add(0); // 如果解析失败，存个默认值 0
                    }
                }
                // 调试用：打印第一列的值
                Debug.WriteLine(row["WindSpeed"]);
            }

            // 5. (可选) 如果你最终必须要是 double[] 数组，可以在这里转换
            // var finalData = _rawData.ToDictionary(k => k.Key, v => v.Value.ToArray());



            Debug.WriteLine("LoadDataFromTable");

        }


        public void SaveToDatabase()
        {
            _db.SavePointsToSelectedTable(SelectedTable, SelectedField, SelectedFieldPoints);
        }


    }

}

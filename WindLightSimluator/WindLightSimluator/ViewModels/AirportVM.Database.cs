using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.Service;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
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

        
        public ObservableCollection<string> Tables { get; set; } = new ObservableCollection<string>();

        private string _selectedTableName;
        public string SelectedTableName
        {
            get => _selectedTableName;
            set {
                if (SetProperty(ref _selectedTableName, value))
                {
                    OnPropertyChanged(nameof(CanStart));
                    // 选择改变后自动预加载数据
                    PreloadData(value);

                }

            }
        }


        public bool CanStart => !string.IsNullOrEmpty(SelectedTableName);
        public void RefreshTables()
        {


            Tables.Clear();

            foreach (var table in _db.GetTableNames())
            {
                Tables.Add(table);
            }
        }

        // 预加载方法
        private async void PreloadData(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return;
            try
            {
                // 异步加载数据
                fakeData = await Task.Run(() => LoadDataFromTable(tableName));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"预加载失败: {ex.Message}");
            }
        }
        public Dictionary<string, ObservableCollection<double>> LoadDataFromTable(string tableName)
        {


            var data = _db.Query($"SELECT * FROM \"{tableName}\"");

            // 检查是否有数据
            if (data == null || data.Rows.Count == 0)
            {
                Debug.WriteLine("这题模拟数据加载失败!!!");

            }

            var rawData = new Dictionary<string, ObservableCollection<double>>();

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


    }
}

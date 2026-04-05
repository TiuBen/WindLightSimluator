using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using WindLightSimluator.ViewModels;
using WindLightSimluator.Views.Components;
using Microsoft.Data.Sqlite;


namespace WindLightSimluator.Views
{
    public partial class WeatherEditor : UserControl
    {
        private string _currentDbPath = "";

        public WeatherEditor()
        {
            InitializeComponent();
            this.Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
                var vm = DataContext as EditableWeatherElementViewModel;
                vm?.InitDatabase();
        }

        private void GridPointEditor_PointValueChanged(object sender, PointValueChangedEventArgs e)
        {
            var vm = DataContext as EditableWeatherElementViewModel;
            if (vm != null)
            {
                vm.UpdatePointValue(e.Index, e.NewValue);
            }
        }

        // 手动设置数据库的位置
        private  void SelectDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
           
            var dialog = new CommonOpenFileDialog {
                Title = "请选择数据库文件",
                IsFolderPicker = false,
                Multiselect = false
            };
            dialog.Filters.Add(new CommonFileDialogFilter("SQLite 数据库文件", "*.db;*.sqlite;*.sqlite3"));
            dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*.*"));
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string filePath = dialog.FileName;
                var vm = DataContext as EditableWeatherElementViewModel;
                vm.DatabaseFilePath = filePath;
            }
        }

       
        // ============================
        // 2️ 选择文件 → 加载
        // ============================
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 
            Debug.WriteLine(e);
            var vm = (EditableWeatherElementViewModel)DataContext;
            vm.LoadDataFromTable(vm.SelectedTable);
            Debug.WriteLine(vm.SelectedTable);
        }


        

        // ============================
        // 4️ 保存（覆盖）
        // ============================
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EditableWeatherElementViewModel;
            vm.SaveToDatabase();

        }

        private void NewTableButton_Click(object sender, RoutedEventArgs e)
        {
            // 尝试自动连接
            if (File.Exists(_defaultDbPath))
            {
                var vm = DataContext as EditableWeatherElementViewModel;
                vm?.CreateNewTable(_defaultDbPath);
            }
            else
            {
                Debug.WriteLine("default db not exit");
            }
        }
    }
}

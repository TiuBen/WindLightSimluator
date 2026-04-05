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
        }

        private void GridPointEditor_PointValueChanged(object sender, PointValueChangedEventArgs e)
        {
            var vm =(EditableWeatherElementViewModel) DataContext;
            vm.UpdatePointValue(e.Index, e.NewValue);
        }

        // 手动设置数据库的位置
        private void SelectDatabaseButton_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new CommonOpenFileDialog
            {
                Title = "请选择数据库文件",
                IsFolderPicker = false,
                Multiselect = false
            };
            dialog.Filters.Add(new CommonFileDialogFilter("SQLite 数据库文件", "*.db;*.sqlite;*.sqlite3"));
            dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*.*"));
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (dialog.FileName != null)
                {
                    var vm = DataContext as EditableWeatherElementViewModel;
                    if (vm != null)
                    {
                        vm.DatabaseFilePath = dialog.FileName;
                    }
                }
            }
        }


        // ============================
        // 保存编辑后的气象数据（覆盖）
        // ============================
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = (EditableWeatherElementViewModel)DataContext;
            vm.SaveToDatabase();

        }

        // ============================
        // 新建默认的气象数据
        // ============================
        private void NewTableButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (EditableWeatherElementViewModel)DataContext;
            vm.CreateNewTable();

        }

        private void ChangeNameButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (EditableWeatherElementViewModel)DataContext;
            vm.RenameTable();
        }
    }
}

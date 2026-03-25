using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WindLightSimluator.Model;

namespace WindLightSimluator.ViewModels
{
    internal class EditableWeatherElementViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EditableWeatherElement> Points { get; set; }
        private string _selectedField = "Temperature";
        public string SelectedField
        {
            get => _selectedField;
            set {
                _selectedField = value;
                OnPropertyChanged(nameof(SelectedField));
                OnPropertyChanged(nameof(Unit));
            }
        }

        public List<string> FieldList => new()
    {
        "WindDirection",
        "WindSpeed",
        "Temperature",
        "QNH",
        "QFE"
    };

        public string Unit => SelectedField switch
        {
            "WindDirection" => "°",
            "WindSpeed" => "m/s",
            "Temperature" => "℃",
            "QNH" => "hPa",
            "QFE" => "hPa",
            _ => ""
        };

        public EditableWeatherElementViewModel()
        {
            var start = DateTime.Now.Date;

            Points = new ObservableCollection<EditableWeatherElement>(
                Enumerable.Range(0, 24)
                    .Select(i => new EditableWeatherElement
                    {
                        Time = start.AddMinutes(i * 5),
                        Temperature = 20
                    })
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

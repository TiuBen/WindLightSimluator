using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindLightSimluator.Model;
using WindLightSimluator.utils;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public class RunwayVM : ViewModelBase
    {

        public int Id { get; set; }
    
        // 三段runway part vm 
        public RunwayPartVM startPart { get; set; } 
        public RunwayPartVM middlePart { get; set; } 
        public RunwayPartVM endPart { get; set; } 


        public RunwayPartVM selectedPart { get; set; } 

        public ICommand ToggleRunwayCommand { get; }

        public RunwayVM()
        {
        
            ToggleRunwayCommand = new RelayCommand<bool>(isOn =>
            {
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

                startPart.IsActive = !startPart.IsActive;
                endPart.IsActive = !endPart.IsActive;

                if ((bool)startPart.IsActive)
                {
                    selectedPart = startPart;

                }
                else
                {
                    selectedPart = endPart;

                }
                OnPropertyChanged(nameof(selectedPart));
            });
        }

        // 这里存放模拟数据源（假设每个跑道有自己的原始数据流）
        private readonly List<Wind> _dataSource;

      
    }
}

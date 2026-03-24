using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindLightSimluator.ViewModels.vm;
using System.Collections.ObjectModel;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.Service;
using System.Windows.Threading;
using WindLightSimluator.Model;
using System.Reflection;

namespace WindLightSimluator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<RunwayViewModel> Runways { get; set; }

        private readonly List<WTQR>? _fakeWindData;
        private readonly DispatcherTimer _timer;
        private DateTime _simulationTime;

        public DateTime SimulationTime
        {
            get => _simulationTime;
            private set => SetProperty(ref _simulationTime, value);
        }

        public MainWindowViewModel()
        {
            Runways = new ObservableCollection<RunwayViewModel>();
            RunwayViewModel _runway1 = new RunwayViewModel(1, 015, "01L", 195, "19R", FakeWQTProvider.Generate());
            RunwayViewModel _runway2 = new RunwayViewModel(2, 015, "01R", 195, "19L", FakeWQTProvider.Generate());

            Runways.Add(_runway1);
            Runways.Add(_runway2);

            SimulationTime = DateTime.Now;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            _timer.Tick += OnTick;
            _timer.Start();

        }
        private void OnTick(object? sender, EventArgs e)
        {
            SimulationTime = SimulationTime.AddSeconds(20);

            foreach (var runway in Runways)
            {
                runway.Update(SimulationTime);
            }
        }
    }



    public class RunwayViewModel : ViewModelBase
    {

        public short Id { get; set; }
        public short SmallRunwayDir { get; set; }
        public string SmallDirName { get; set; }
        public short LargeRunwayDir { get; set; }
        public string LargeDirName { get; set; }
        public RunwayPartViewModel RunwayStart { get; }
        public RunwayPartViewModel RunwayMiddle { get; }
        public RunwayPartViewModel RunwayEnd { get; }

        private readonly List<WTQR> _fakeWindData;


        public RunwayViewModel(short id, short smalllRwyDir, string smallRwyName, short largeRwyDir, string largeRwyName, List<WTQR> fakeWindData)
        {

            Id = id;
            SmallRunwayDir = smalllRwyDir;
            SmallDirName = smallRwyName;
            LargeRunwayDir = largeRwyDir;
            LargeDirName = largeRwyName;

            // 初始值
            _fakeWindData = fakeWindData;
            var firstWQTR = _fakeWindData.First();

            RunwayStart = new RunwayPartViewModel(this, RunwayPartEnum.Start);
            RunwayStart.HeadCrossWind = new HeadCrossWindViewModel(this);
            RunwayStart.Wind.RangeArcIndex = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            RunwayStart.Wind.DirectedArcIndex = 2;
            RunwayStart.Wind.IsActive = true;
            HeadCrossWindViewModel _testHeadCrossWind = new HeadCrossWindViewModel();
            RunwayStart.HeadCrossWind = _testHeadCrossWind;
            RunwayStart.RvrVis = new RvrVisViewModel(RunwayStart);


            RunwayMiddle = new RunwayPartViewModel(this, RunwayPartEnum.Middle);
            RunwayMiddle.HeadCrossWind = new HeadCrossWindViewModel(this);
            RunwayMiddle.Wind.RangeArcIndex = new HashSet<int>() { 0, 1, 2, 3, 10, 11, 12, 13, 14, 15 };
            RunwayMiddle.Wind.DirectedArcIndex = 2;
            RunwayMiddle.Wind.IsActive = false;
            HeadCrossWindViewModel _testHeadCrossWind2 = new HeadCrossWindViewModel();
            _testHeadCrossWind2.Avg2HeadWindSpeed = 2;
            RunwayMiddle.HeadCrossWind = _testHeadCrossWind2;


            RunwayEnd = new RunwayPartViewModel(this, RunwayPartEnum.End);
            RunwayEnd.HeadCrossWind = new HeadCrossWindViewModel(this);
            RunwayEnd.Wind.RangeArcIndex = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 15 };
            RunwayEnd.Wind.DirectedArcIndex = 2;
            RunwayEnd.Wind.IsActive = false;
            HeadCrossWindViewModel _testHeadCrossWind3 = new HeadCrossWindViewModel();
            _testHeadCrossWind3.Max2WindSpeed = 4;
            RunwayEnd.HeadCrossWind = _testHeadCrossWind3;


            RunwayStart.IsActive = true;
            RunwayMiddle.IsActive = null;
            RunwayEnd.IsActive = false;
            Id = id;
            RunwayDir = runwayDir;




            //// 中间列固定 Normal
            //RunwayMiddle.State = RunwayColumnState.Normal;
            ////Columns[1].IsSelectable = false;
            //RunwayStart.State = RunwayColumnState.Selected;
            //RunwayEnd.State = RunwayColumnState.Disabled;
        }


        public void ChangeRunwayDirection(RunwayPartViewModel selected)
        {
            if (selected == RunwayStart)
            {
                RunwayStart.IsActive = true;
                RunwayEnd.IsActive = false;
            }
            if (selected == RunwayEnd)
            {
                RunwayEnd.IsActive = true;
                RunwayStart.IsActive = false;
            }

            /// 这里还要更新 Head/Cross Wind 的计算
        }

        public void Update(DateTime simulationTime)
        {
            var sample = GetNearest(simulationTime);

            /// tianchong filll
        }

        private WTQR GetNearest(DateTime time)
        {
        //在 Tick × 跑道 × 段数 多了以后会慢。
        //正确工程做法是：
        //FakeData 已排序
        //维护一个 currentIndex
        //时间只往前 → index 只递增
            return _fakeWindData
                .OrderBy(x => Math.Abs((x.Time - time).TotalSeconds))
                .First();
        }

    }


    public class RunwayPartViewModel : ViewModelBase
    {
        public RunwayPartEnum RunwayPart { get; set; }

        public RunwayStatusViewModel Status { get; set; }
        public WindPanelViewModel Wind { get; set; }
        public HeadCrossWindViewModel HeadCrossWind { get; set; }
        public RvrVisViewModel RvrVis { get; set; }
        public WeatherConditionViewModel Weather { get; set; }

        //public ICommand SelectCommand { get; }

        //private RunwayColumnState _state;
        //public RunwayColumnState State
        //{
        //    get { return _state; }
        //    set { _state = value; 
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(IsActive));
        //        Status.IsActive = IsActive; // ⭐ 关键
        //    }

        //}

        //public bool IsActive => State == RunwayColumnState.Selected;



        //private readonly RunwayViewModel _parent;
        //public bool IsSelectable { get; }

        //public string RunwayNumber
        //{
        //    get => Status.RunwayNumber;
        //    set => Status.RunwayNumber = value;
        //}

        private bool? _isActive;
        public bool? IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private RunwayViewModel _runway;
        public RunwayViewModel Runway
        {
            get { return _runway; }
            set
            {
                _runway = value;
            }
        }


        public RunwayPartViewModel(RunwayViewModel runway, RunwayPartEnum part)
        {
            _runway = runway;
            RunwayPart = part;


            Status = new RunwayStatusViewModel(this, "rets");
            Wind = new WindPanelViewModel(this);
            HeadCrossWind = new HeadCrossWindViewModel(runway);
            RvrVis = new RvrVisViewModel(this);
            Weather = new WeatherConditionViewModel(this);


            //IsSelectable = part != RunwayPart.Middle;
            //State = RunwayColumnState.Normal;

            //SelectCommand = new RelayCommand(
            //    () => _parent.OnColumnSelected(this),
            //    () => IsSelectable);
        }
        public static void ChangeRunwayHeading()
        {

        }

    }


























}

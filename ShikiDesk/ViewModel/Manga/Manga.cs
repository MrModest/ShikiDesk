using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShikiDesk.ViewModel
{
    public class MangaVM : ViewModel
    {
        public MangaVM(ShikiApiLib.ShikiApi user)
        {
            User = user;

            Tabs = new MangaTabVM[5];

            for (int i = 0; i < 5; i++)
            {
                Tabs[i] = new MangaTabVM();
                Tabs[i].UserStatusRus = UserStatusesRus[i];
            }
            onUpdateTabs();

            UpdateRate = new RelayCommand(onUpdateRate);
            UpdateTabs = new RelayCommand(onUpdateTabs);
            AddTitle = new RelayCommand(onAddTitle);
            GetInfo = new RelayCommand(onGetInfo);

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.IsEnabled = true;
            timer.Tick += TimerUpdate;
        }

        public static string[] UserStatusesRus { get; } = new string[] { "Запланировано", "Читаю", "Прочитано", "Отложено", "Брошено" };

        MangaTabVM[] _tabs;
        public MangaTabVM[] Tabs
        {
            get { return _tabs; }
            set { Set(ref _tabs, value); }
        }

        MangaViewVM _selectedView;
        public MangaViewVM SelectedView
        {
            get { return _selectedView; }
            set
            {
                if (value != null)
                {
                    var clone = JsonConvert.DeserializeObject<MangaViewVM>(JsonConvert.SerializeObject(value));
                    Set(ref _selectedView, clone);
                }
            }
        }

        public ICommand UpdateRate { get; }

        void onUpdateRate()
        {
            var rate = DBMethods.GetMRateFromDB(SelectedView.UserRateId);
            var rate_upd = User.UpdateMangaRate(rate, (ShikiApiLib.UserStatus)Enum.Parse(typeof(ShikiApiLib.UserStatus), SelectedView.UserStatus), SelectedView.Score, SelectedView.CompletedVolumes, SelectedView.CompletedChapters);
            if (rate.UserStatus        != rate_upd.UserStatus ||
                rate.Score             != rate_upd.Score ||
                rate.CompletedVolumes  != rate_upd.CompletedVolumes ||
                rate.CompletedChapters != rate_upd.CompletedChapters)
            {
                DBMethods.UpdateSelectedMRateInDB(rate_upd);
                onUpdateTabs();
            }
        }

        public ICommand UpdateTabs { get; }

        void onUpdateTabs() //need async (?)
        {
            List<ShikiApiLib.MangaRate> MRates = DBMethods.GetMRatesFromDB();
            for (int i = 0; i < 5; i++)
            {
                Tabs[i].List.Clear();
                foreach (var rate in MRates.Where(x => x.UserStatus == ((ShikiApiLib.UserStatus)i).ToString()).ToList())
                {
                    Tabs[i].List.Add(new MangaViewVM(rate));
                }
            }
        }

        public ICommand AddTitle { get; }

        void onAddTitle()
        {
            DialogService.ShowDialog(new MangaSearchVM(User));
        }

        public ICommand GetInfo { get; }

        void onGetInfo()
        {
            var titleFullInfo = User.GetMangaFullInfo(SelectedView.TitleId);
            DialogService.ShowDialog(new MangaFullViewVM(titleFullInfo));
        }

        void TimerUpdate(object sender, EventArgs e)
        {
            onUpdateTabs();
        }
    }

    public class MangaViewVM : TitleViewVM
    {
        string _poster;
        public string Poster
        {
            get
            {
                var relPuth = System.Environment.CurrentDirectory + @"\TitlePosters\Manga\";

                if (System.IO.File.Exists(relPuth + TitleId + ".png"))
                {
                    return relPuth + TitleId + ".png";
                }
                else
                {
                    Console.WriteLine("Not Found!");
                    return "https://shikimori.org" + _poster;
                }
            }
            set { Set(ref _poster, value); }
        }

        int _completed_ch;
        public int CompletedChapters
        {
            get { return _completed_ch; }
            set { Set(ref _completed_ch, value); RaisePropertyChanged(nameof(ProgressChapters)); }
        }

        int _total_ch;
        public int TotalChapters
        {
            get { return _total_ch; }
            set { Set(ref _total_ch, value); RaisePropertyChanged(nameof(ProgressChapters)); }
        }

        public string ProgressChapters { get { return CompletedChapters + " | " + TotalChapters; } }

        int _completed_vol;
        public int CompletedVolumes
        {
            get { return _completed_vol; }
            set { Set(ref _completed_vol, value); RaisePropertyChanged(nameof(ProgressVolumes)); }
        }

        int _total_vol;
        public int TotalVolumes
        {
            get { return _total_vol; }
            set { Set(ref _total_vol, value); RaisePropertyChanged(nameof(ProgressVolumes)); }
        }

        public string ProgressVolumes { get { return CompletedVolumes + "  | " + TotalVolumes; } }

        public MangaViewVM() : base() { }

        public MangaViewVM(ShikiApiLib.MangaRate rate) : base(rate)
        {
            Poster = rate.Poster.original;
            CompletedVolumes = rate.CompletedVolumes;
            CompletedChapters = rate.CompletedChapters;
            TotalVolumes = rate.TotalVolumes;
            TotalChapters = rate.TotalChapters;
        }
    }

    public class MangaTabVM : TitleTabVM
    {
        public ObservableCollection<MangaViewVM> List { get; } = new ObservableCollection<MangaViewVM>();
    }
}

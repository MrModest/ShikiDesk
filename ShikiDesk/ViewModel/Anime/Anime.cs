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
    public class AnimeVM : ViewModel
    {
        public AnimeVM(ShikiApiLib.ShikiApi user)
        {
            User = user;

            Tabs = new AnimeTabVM[5];
            
            for (int i = 0; i < 5; i++)
            {
                Tabs[i] = new AnimeTabVM();
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

        public static string[] UserStatusesRus { get; } = new string[] { "Запланировано", "Смотрю", "Просмотрено", "Отложено", "Брошено" };

        AnimeTabVM[] _tabs;
        public AnimeTabVM[] Tabs
        {
            get { return _tabs; }
            set { Set(ref _tabs, value); }
        }

        AnimeViewVM _selectedView;
        public AnimeViewVM SelectedView
        {
            get { return _selectedView; }
            set
            {
                if (value != null)
                {
                    var clone = JsonConvert.DeserializeObject<AnimeViewVM>(JsonConvert.SerializeObject(value));
                    Set(ref _selectedView, clone);
                }
            }
        }

        public ICommand UpdateRate { get; }

        void onUpdateRate()
        {
            var rate = DBMethods.GetARateFromDB(SelectedView.UserRateId);
            var rate_upd = User.UpdateAnimeRate(rate, (ShikiApiLib.UserStatus)Enum.Parse(typeof(ShikiApiLib.UserStatus), SelectedView.UserStatus), SelectedView.Score, SelectedView.CompletedEpisodes);
            if (rate.UserStatus        != rate_upd.UserStatus || 
                rate.Score             != rate_upd.Score || 
                rate.CompletedEpisodes != rate_upd.CompletedEpisodes)
            {
                DBMethods.UpdateSelectedARateInDB(rate_upd);
                onUpdateTabs();
            }
        }

        public ICommand UpdateTabs { get; }

        void onUpdateTabs() //need async (?)
        {
            List<ShikiApiLib.AnimeRate> ARates = DBMethods.GetARatesFromDB();
            for (int i = 0; i < 5; i++)
            {
                Tabs[i].List.Clear();
                foreach (var rate in ARates.Where(x => x.UserStatus == ((ShikiApiLib.UserStatus)i).ToString()).ToList())
                {
                    Tabs[i].List.Add(new AnimeViewVM(rate));
                }
            }
            Console.WriteLine("Updated!");
        }

        public ICommand AddTitle { get; }

        void onAddTitle()
        {
            DialogService.ShowDialog(new AnimeSearchVM(User));
        }

        public ICommand GetInfo { get; }

        void onGetInfo()
        {
            var titleFullInfo = User.GetAnimeFullInfo(SelectedView.TitleId);
            DialogService.ShowDialog(new AnimeFullViewVM(titleFullInfo));
        }

        void TimerUpdate(object sender, EventArgs e)
        {
            onUpdateTabs();
        }
    }

    public class AnimeViewVM : TitleViewVM
    {
        string _poster;
        public string Poster
        {
            get
            {
                var relPuth = System.Environment.CurrentDirectory + @"\TitlePosters\Anime\";

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

        int _completed_ep;
        public int CompletedEpisodes
        {
            get { return _completed_ep; }
            set { Set(ref _completed_ep, value); RaisePropertyChanged(nameof(ProgressEpisodes)); }
        }

        int _total_ep;
        public int TotalEpisodes
        {
            get { return _total_ep; }
            set { Set(ref _total_ep, value); RaisePropertyChanged(nameof(ProgressEpisodes)); }
        }

        public string ProgressEpisodes { get { return CompletedEpisodes + " | " + TotalEpisodes; } }

        public AnimeViewVM() : base() { }

        public AnimeViewVM(ShikiApiLib.AnimeRate rate) : base(rate)
        {
            Poster = rate.Poster.original;
            CompletedEpisodes = rate.CompletedEpisodes;
            TotalEpisodes = Math.Max(rate.AiredEpisodes, rate.TotalEpisodes);
        }
    }

    public class AnimeTabVM : TitleTabVM
    {
        public ObservableCollection<AnimeViewVM> List { get; } = new ObservableCollection<AnimeViewVM>();
    }
}

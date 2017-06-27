using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ShikiDesk.ViewModel;
using LinqToDB;

namespace ShikiDesk
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GenerateDB();

            var mainVM = new MainVM();
            AutorizationWindow authorizationWindow = null;
            if (!mainVM.HasCredentials()) // нет авторизации?
            {
                var authorizationVM = new AutorizationVM();
                authorizationWindow = new AutorizationWindow() { DataContext = authorizationVM };
                authorizationWindow.Show();
                await authorizationVM.RunAuthorization();
                
                mainVM.SetCredentials(authorizationVM);

                var splashWindow = new SplashWindow() { DataContext = mainVM };
                splashWindow.Show();
                authorizationWindow?.Close(); // знак вопроса нужен, а то вдруг null   
                await mainVM.DownloadData();  
                                              
                var mainWindow = new MainWindow() { DataContext = mainVM };
                mainWindow.Show();

                splashWindow.Close();
                await mainVM.DownloadPosters();
            }
            else
            {
                var mainWindow = new MainWindow() { DataContext = mainVM };
                mainWindow.Show();
            }

            DialogService.Default.Register<AnimeSearchVM, AddNewAnimeWindow>();
            DialogService.Default.Register<MangaSearchVM, AddNewMangaWindow>();
            DialogService.Default.Register<AnimeFullViewVM, AnimeDetailWindow>();
            DialogService.Default.Register<MangaFullViewVM, MangaDetailWindow>();

            System.Windows.Threading.DispatcherTimer updateTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
            updateTimer.Interval = TimeSpan.FromMinutes(5);
            updateTimer.IsEnabled = true;
            updateTimer.Tag = mainVM;
            updateTimer.Tick += UpdateDB;

            System.Windows.Threading.DispatcherTimer scanTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
            scanTimer.Interval = TimeSpan.FromSeconds(2);
            scanTimer.IsEnabled = true;
            scanTimer.Tag = mainVM;
            scanTimer.Tick += ScanProc;
        }

        private async void UpdateDB(object sender, EventArgs e)
        {
            var vm = (sender as System.Windows.Threading.DispatcherTimer).Tag as MainVM;
            await vm.DownloadData();
        }

        private void ScanProc(object sender, EventArgs e)
        {
            var vm = (sender as System.Windows.Threading.DispatcherTimer).Tag as MainVM;
            ScanProcesses(vm);
        }

        void GenerateDB()
        {
            try
            {
                using (var db = new FiltersDB("Filters"))
                {
                    db.TGenres.Take(1).ToList();
                    db.TStudios.Take(1).ToList();
                    db.TPublishers.Take(1).ToList();
                }
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                System.IO.File.Delete("Filters.sqlite3");
                using (var db = new FiltersDB("Filters"))
                {
                    db.CreateTable<TStudio>();
                    db.CreateTable<TGenre>();
                    db.CreateTable<TPublisher>();
                }
            }

            try
            {
                using (var db = new UserRatesDB("UserRates"))
                {
                    db.TAnimeRates.Take(1).ToList();
                    db.TMangaRates.Take(1).ToList();
                }
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                System.IO.File.Delete("UserRates.sqlite3");
                using (var db = new UserRatesDB("UserRates"))
                {
                    db.CreateTable<TAnimeRate>();
                    db.CreateTable<TMangaRate>();
                }
            }
        }

        private int seeTick = 0;
        WatchingAlarmWindow watchingAlarm;

        async void ScanProcesses(MainVM vm)
        {
            var processes = System.Diagnostics.Process.GetProcesses();
            string titleName = processes.FirstOrDefault(x => x.ProcessName == "mpc-hc64")?.MainWindowTitle;

            if (String.IsNullOrWhiteSpace(titleName)) { return; }

            Dictionary<string, string> title = new Dictionary<string, string>();
            System.Text.RegularExpressions.Regex reg;

            reg = new System.Text.RegularExpressions.Regex(@"- ?(\d+)(\(\d*\))?");
            var match = reg.Match(titleName);
            int ep = 0;
            if (match.Index != 0 && match.Length != 0)
            {
                int.TryParse(titleName.Substring(match.Index, match.Length).Replace("- ", ""), out ep);
                titleName = reg.Replace(titleName, "");    
            }
            title.Add("episode", ep.ToString());

            reg = new System.Text.RegularExpressions.Regex(@"TV(\d*)");
            match = reg.Match(titleName);
            string season = "";
            if (match.Index != 0 && match.Length != 0)
            {
                season = titleName.Substring(match.Index, match.Length).Replace("TV", "");
                titleName = reg.Replace(titleName, "");
            }
            if (String.IsNullOrWhiteSpace(season))
            {
                season = "1";
            }
            title.Add("season", season);

            reg = new System.Text.RegularExpressions.Regex(@"\[.*?\]|\(.*?\)");
            string name = reg.Replace(titleName, "");
            name = name.Replace("серия", "");
            if (name.Contains('.'))
            {
                name = name.Substring(0, name.LastIndexOf('.'));
            }
            name = name.Trim(' ');
            title.Add("name", name);

            var searchText = title["name"];

            if (int.Parse(season) > 1)
            {
                searchText += " " + title["season"];
            }

            var result = DBMethods.GetARatesByName(searchText);

            if (result.Count > 0 && ep > result[0].CompletedEpisodes)
            {
                if (seeTick == 1)
                {
                    watchingAlarm = new WatchingAlarmWindow(result[0].Name);
                    
                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        watchingAlarm.Show();
                        await Task.Delay(2000);
                        watchingAlarm.Close();
                    });

                    Console.WriteLine("Showed!");
                }
                if (seeTick == 20)
                {
                    var resp = vm.User.UpdateAnimeRate(result[0], episodes: ep);
                    seeTick = 0;
                    await vm.DownloadData();
                    vm.Anime.UpdateTabs.Execute(null);
                    Console.WriteLine("Edit: " + resp.CompletedEpisodes);
                }
                else
                {
                    seeTick++;
                    Console.WriteLine("Find: " + result[0].Name + " | " + result[0].UserRateId + " | " + seeTick);
                }
            }
            else
            {
                seeTick = 0;
            }

            //Console.WriteLine("{0} | {1} | {2}", title["name"], title["episode"], title["season"]);
        }
    }
}

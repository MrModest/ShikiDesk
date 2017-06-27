using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShikiDesk.ViewModel
{
    public class MainVM : ViewModel
    {
        public MainVM()
        {
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.nickname) &&
                !String.IsNullOrWhiteSpace(Properties.Settings.Default.access_token) &&
                Properties.Settings.Default.curren_user_id != -1)
            {
                User = new ShikiApiLib.ShikiApi(Properties.Settings.Default.nickname,
                                        Properties.Settings.Default.access_token,
                                        Properties.Settings.Default.curren_user_id);
            }

            Mode = new ModeVM();
            Anime = new AnimeVM(User);
            Manga = new MangaVM(User);
            ListHeight = 240;
        }

        public static int[] Scores { get; } = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };

        ProgressBarVM _progressBar;
        public ProgressBarVM ProgressBar
        {
            get { return _progressBar; }
            set { Set(ref _progressBar, value); }
        }

        ModeVM _mode;
        public ModeVM Mode
        {
            get { return _mode; }
            set { Set(ref _mode, value); }
        }

        AnimeVM _anime;
        public AnimeVM Anime
        {
            get { return _anime; }
            set { Set(ref _anime, value); }
        }

        MangaVM _manga;
        public MangaVM Manga
        {
            get { return _manga; }
            set { Set(ref _manga, value); }
        }

        double _listHeight;
        public double ListHeight
        {
            get { return _listHeight; }
            set { Set(ref _listHeight, value); }
        }

        public bool HasCredentials()
        {
            return (User != null);
        }

        public void SetCredentials(AutorizationVM autorizationVM)
        {
            User = autorizationVM.User;
            Anime.User = Manga.User = User;
        }

        public async Task DownloadPosters()
        {
            await Task.Run(() =>
            {
                var relPathAPosters = "TitlePosters/Anime/";
                var relPathMPosters = "TitlePosters/Manga/";

                if (!System.IO.Directory.Exists(relPathAPosters)) { System.IO.Directory.CreateDirectory(relPathAPosters); }
                if (!System.IO.Directory.Exists(relPathMPosters)) { System.IO.Directory.CreateDirectory(relPathMPosters); }

                using (var webClient = new System.Net.WebClient())
                {
                    foreach (var rate in User.AnimeRates)
                    {
                        var poster = rate.Poster.original;
                        try
                        {
                            webClient.DownloadFile(poster, relPathAPosters + rate.Poster + ".png");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("WebClien (AnimePosters): " + ex.Message);
                        }

                    }
                    foreach (var rate in User.MangaRates)
                    {
                        var poster = rate.Poster.original;
                        try
                        {
                            webClient.DownloadFile(poster, relPathMPosters + rate.TitleId + ".png");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("WebClien (MangaPosters): " + ex.Message);
                        }
                    }
                }
            });
        }

        public async Task DownloadData()
        {
            ProgressBar = new ProgressBarVM();

            ProgressBar.Set(0, "Идёт первоначальная настройка. Пожалуйста подождите немного.");

            if (Properties.Settings.Default.firstStart)
            {
                await Task.Run(() =>
                {
                    User.GetUserInfo();

                    DBMethods.FillGenresDB(ShikiApiLib.ShikiApiStatic.GetGenres());
                    DBMethods.FillStudiosDB(ShikiApiLib.ShikiApiStatic.GetStudios());
                    DBMethods.FillPublishersDB(ShikiApiLib.ShikiApiStatic.GetPublishers());
                });
            }

            ProgressBar.Set(30, "Идёт загрузка аниме листа. Продолжительность ожидания зависит от количества аниме в вашем списке.");

            await Task.Run(() => User.GetAnimeRates());

            ProgressBar.Set(50, "Идёт загрузка манга листа. Продолжительность ожидания зависит от количества манги в вашем списке.");

            await Task.Run(() => User.GetMangaRates());

            ProgressBar.Set(70, "Кэширование данных на диск. Ещё пара секунд..");

            await Task.Run(() =>
            {
                DBMethods.FillDataDB(User.AnimeRates);
                DBMethods.FillDataDB(User.MangaRates);
            });

            ProgressBar.Set(100, "Загрузка завершена. Запуск приложения.");
        }
    }
}

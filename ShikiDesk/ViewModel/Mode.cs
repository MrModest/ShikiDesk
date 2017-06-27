using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShikiDesk.ViewModel
{
    public class ModeVM : ViewModel
    {
        public ModeVM()
        {
            SwitchToAnimeMode = new RelayCommand(onSwitchToAnimeMode);
            SwitchToMangaMode = new RelayCommand(onSwitchToMangaMode);

            this.onSwitchToAnimeMode();
        }

        bool _animeIsEnable;
        public bool AnimeIsEnable
        {
            get { return _animeIsEnable; }
            set { Set(ref _animeIsEnable, value); }
        }

        bool _mangaIsEnable;
        public bool MangaIsEnable
        {
            get { return _mangaIsEnable; }
            set { Set(ref _mangaIsEnable, value); }
        }

        string _title;
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        bool _animeVisibility;
        public bool AnimeVisibility
        {
            get { return _animeVisibility; }
            set { Set(ref _animeVisibility, value); }
        }

        bool _mangaVisibility;
        public bool MangaVisibility
        {
            get { return _mangaVisibility; }
            set { Set(ref _mangaVisibility, value); }
        }

        public ICommand SwitchToAnimeMode { get; }
        public ICommand SwitchToMangaMode { get; }

        void onSwitchToAnimeMode()
        {
            AnimeIsEnable = false;
            MangaIsEnable = true;

            Title = "Shikimori | Anime";

            AnimeVisibility = true;
            MangaVisibility = false;
        }

        void onSwitchToMangaMode()
        {
            AnimeIsEnable = true;
            MangaIsEnable = false;

            Title = "Shikimori | Manga";

            AnimeVisibility = false;
            MangaVisibility = true;
        }
    }
}

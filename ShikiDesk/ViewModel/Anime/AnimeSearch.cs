using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiDesk.ViewModel
{
    public class AnimeSearchVM : TitleSearchVM
    {
        Dictionary<ShikiApiLib.AKind, NotifyBool> _kind;
        public Dictionary<ShikiApiLib.AKind, NotifyBool> Kind
        {
            get { return _kind; }
            set { Set(ref _kind, value); }
        }

        Dictionary<ShikiApiLib.Duration, NotifyBool> _duration;
        public Dictionary<ShikiApiLib.Duration, NotifyBool> Duration
        {
            get { return _duration; }
            set { Set(ref _duration, value); }
        }

        Dictionary<int, NotifyBool> _studio;
        public Dictionary<int, NotifyBool> Studio
        {
            get { return _studio; }
            set { Set(ref _studio, value); }
        }

        string[] _studioName;
        public string[] StudioName
        {
            get { return _studioName; }
            set { Set(ref _studioName, value); }
        }

        List<AnimeFullViewVM> _searchResult;
        public List<AnimeFullViewVM> SearchResult
        {
            get { return _searchResult; }
            set { Set(ref _searchResult, value); }
        }

        override protected void onSearch()
        {
            var search = new ShikiApiLib.AnimeSearch();
            search.Censored = Censored;
            if (Limit > 1) { search.Limit = Limit; }
            search.Page = Page;
            search.TitleScore = TitleScore;
            search.SearchText = SearchText;
            search.Genre = ParceDict(Genre);
            search.Order = SetOrder(Order);
            search.MyList = ParceDict(MyList);
            search.Rating = ParceDict(Rating);
            search.Season = ParceDict(Season);
            search.TitleStatus = ParceDict(TitleStatus);
            search.Kind = ParceDict(Kind);
            search.Duration = ParceDict(Duration);
            search.Studio = ParceDict(Studio);
            SearchResult = search.GetSearch(User).Select(x => new AnimeFullViewVM(x, User)).ToList();
        }

        public AnimeSearchVM() : base()
        {
            var genres = DBMethods.GetGenresFromDB().Where(genre => genre.kind.ToLower() == "anime").ToList();
            foreach (var genre in genres)
            {
                Genre.Add(genre.id, new NotifyBool(null));
            }

            Studio = new Dictionary<int, NotifyBool>();
            var studios = DBMethods.GetStudiosFromDB();
            StudioName = new string[studios.Max(x => x.id) + 1];

            foreach (var studio in studios)
            {
                Studio.Add(studio.id, new NotifyBool(null));
                StudioName[studio.id] = studio.name;
            }

            Kind = InitEnumDict<ShikiApiLib.AKind>();
            Duration = InitEnumDict<ShikiApiLib.Duration>();
        }

        public AnimeSearchVM(ShikiApiLib.ShikiApi user) : base(user)
        {
            var genres = DBMethods.GetGenresFromDB().Where(genre => genre.kind.ToLower() == "anime").ToList();
            foreach (var genre in genres)
            {
                Genre.Add(genre.id, new NotifyBool(null));
            }

            Studio = new Dictionary<int, NotifyBool>();
            var studios = DBMethods.GetStudiosFromDB();
            StudioName = new string[studios.Max(x => x.id) + 1];

            foreach (var studio in studios)
            {
                Studio.Add(studio.id, new NotifyBool(null));
                StudioName[studio.id] = studio.name;
            }

            Kind = InitEnumDict<ShikiApiLib.AKind>();
            Duration = InitEnumDict<ShikiApiLib.Duration>();
        }
    }
}

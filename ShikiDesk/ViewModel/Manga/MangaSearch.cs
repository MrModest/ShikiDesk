using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiDesk.ViewModel
{
    public class MangaSearchVM : TitleSearchVM
    {
        Dictionary<ShikiApiLib.MKind, NotifyBool> _kind;
        public Dictionary<ShikiApiLib.MKind, NotifyBool> Kind
        {
            get { return _kind; }
            set { Set(ref _kind, value); }
        }

        Dictionary<int, NotifyBool> _publisher;
        public Dictionary<int, NotifyBool> Publisher
        {
            get { return _publisher; }
            set { Set(ref _publisher, value); }
        }

        string[] _publisherName;
        public string[] PublisherName
        {
            get { return _publisherName; }
            set { Set(ref _publisherName, value); }
        }

        List<MangaFullViewVM> _searchResult;
        public List<MangaFullViewVM> SearchResult
        {
            get { return _searchResult; }
            set { Set(ref _searchResult, value); }
        }

        override protected void onSearch()
        {
            var search = new ShikiApiLib.MangaSearch();
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
            search.Publisher = ParceDict(Publisher);
            SearchResult = search.GetSearch(User).Select(x => new MangaFullViewVM(x) { User = User }).ToList();
        }

        public MangaSearchVM() : base()
        {
            var genres = DBMethods.GetGenresFromDB().Where(genre => genre.kind.ToLower() == "manga").ToList();
            foreach (var genre in genres)
            {
                Genre.Add(genre.id, new NotifyBool(null));
            }

            Publisher = new Dictionary<int, NotifyBool>();
            var publishers = DBMethods.GetPublishersFromDB();
            //var studios = ShikiApiLib.ShikiApiStatic.GetStudios();
            PublisherName = new string[publishers.Max(x => x.id) + 1];

            foreach (var publisher in publishers)
            {
                Publisher.Add(publisher.id, new NotifyBool(null));
                PublisherName[publisher.id] = publisher.name;
            }

            Kind = InitEnumDict<ShikiApiLib.MKind>();
        }

        public MangaSearchVM(ShikiApiLib.ShikiApi user) : base(user)
        {
            var genres = DBMethods.GetGenresFromDB().Where(genre => genre.kind.ToLower() == "manga").ToList();
            foreach (var genre in genres)
            {
                Genre.Add(genre.id, new NotifyBool(null));
            }

            Publisher = new Dictionary<int, NotifyBool>();
            var publishers = DBMethods.GetPublishersFromDB();
            //var studios = ShikiApiLib.ShikiApiStatic.GetStudios();
            PublisherName = new string[publishers.Max(x => x.id) + 1];

            foreach (var publisher in publishers)
            {
                Publisher.Add(publisher.id, new NotifyBool(null));
                PublisherName[publisher.id] = publisher.name;
            }

            Kind = InitEnumDict<ShikiApiLib.MKind>();
        }
    }
}

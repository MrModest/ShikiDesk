using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShikiDesk.ViewModel
{
    public class AnimeFullViewVM : TitleFullViewVM
    {
        string _total_ep;
        public string TotalEpisodes
        {
            get { return _total_ep; }
            set { Set(ref _total_ep, value); }
        }

        string _duration;
        public string Duration
        {
            get { return _duration; }
            set { Set(ref _duration, value); }
        }

        string _rating;
        public string Rating
        {
            get { return _rating; }
            set { Set(ref _rating, value); }
        }

        AddAnimeInfoVM _addInfo;
        public AddAnimeInfoVM AddInfo
        {
            get { return _addInfo; }
            set { Set(ref _addInfo, value); }
        }

        public AnimeFullViewVM() : base() { }

        public AnimeFullViewVM(ShikiApiLib.AnimeFullInfo title) : base(title)
        {
            Rating = title.Rating.ToUpper().Replace('_', ' ');

            if (title.AiredEpisodes != 0)
            {
                TotalEpisodes = title.AiredEpisodes + " / ";
            }
            else
            {
                TotalEpisodes = "";
            }
            if (title.TotalEpisodes != 0)
            {
                TotalEpisodes += title.TotalEpisodes;
            }
            else
            {
                TotalEpisodes += "???";
            }

            Duration = title.Duration + " мин.";

            NoInList = (DBMethods.GetARateFromDB(title.TitleId, "TitleId") == null);

            AddInfo = new AddAnimeInfoVM(User, TitleId);
        }

        public AnimeFullViewVM(ShikiApiLib.AnimeFullInfo title, ShikiApiLib.ShikiApi user) : this(title)
        {
            User = user;
            AddInfo.User = user;
        }

        public AnimeFullViewVM(ShikiApiLib.AnimeShortInfo title) : base(title)
        {
            if (title.AiredEpisodes != 0)
            {
                TotalEpisodes = title.AiredEpisodes + " / ";
            }
            else
            {
                TotalEpisodes = "";
            }
            if (title.TotalEpisodes != 0)
            {
                TotalEpisodes += title.TotalEpisodes;
            }
            else
            {
                TotalEpisodes += "???";
            }

            NoInList = (DBMethods.GetARateFromDB(title.TitleId, "TitleId") == null);

            AddInfo = new AddAnimeInfoVM(User, TitleId);
        }

        public AnimeFullViewVM(ShikiApiLib.AnimeShortInfo title, ShikiApiLib.ShikiApi user) : this(title)
        {
            User = user;
            AddInfo.User = user;
        }

        protected override void onAddNewTitle(int titleId)
        {
            AddInfo.Visibility = !AddInfo.Visibility;
        }

        protected override void onGetTitleInfo(int titleId)
        {
            var titleFullInfo = User.GetAnimeFullInfo(titleId);
            DialogService.ShowDialog(new AnimeFullViewVM(titleFullInfo) { User = User });
        }
    }
}

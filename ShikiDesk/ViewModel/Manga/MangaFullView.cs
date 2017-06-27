using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShikiDesk.ViewModel
{
    public class MangaFullViewVM : TitleFullViewVM
    {
        string _total_ch;
        public string TotalChapters
        {
            get { return _total_ch; }
            set { Set(ref _total_ch, value); }
        }

        string _total_vol;
        public string TotalVolumes
        {
            get { return _total_vol; }
            set { Set(ref _total_vol, value); }
        }

        AddMangaInfoVM _addInfo;
        public AddMangaInfoVM AddInfo
        {
            get { return _addInfo; }
            set { Set(ref _addInfo, value); }
        }

        public MangaFullViewVM() : base() { }

        public MangaFullViewVM(ShikiApiLib.MangaFullInfo title) : base(title)
        {
            if (title.TotalChapters > 0 && title.TitleStatus != ShikiApiLib.TitleStatus.anons.ToString())
            {
                TotalChapters = title.TotalChapters.ToString();
            }
            else
            {
                TotalChapters = "???";
            }

            if (title.TotalVolumes > 0 && title.TitleStatus != ShikiApiLib.TitleStatus.anons.ToString())
            {
                TotalVolumes = title.TotalVolumes.ToString();
            }
            else
            {
                TotalVolumes = "???";
            }

            NoInList = (DBMethods.GetMRateFromDB(title.TitleId, "TitleId") == null);

            AddInfo = new AddMangaInfoVM(User, TitleId);
        }

        public MangaFullViewVM(ShikiApiLib.MangaFullInfo title, ShikiApiLib.ShikiApi user) : this(title)
        {
            User = user;
            AddInfo.User = user;
        }

        public MangaFullViewVM(ShikiApiLib.MangaShortInfo title) : base(title)
        {
            if (title.TotalChapters > 0 && title.TitleStatus != ShikiApiLib.TitleStatus.anons.ToString())
            {
                TotalChapters = title.TotalChapters.ToString();
            }
            else
            {
                TotalChapters = "???";
            }

            if (title.TotalVolumes > 0 && title.TitleStatus != ShikiApiLib.TitleStatus.anons.ToString())
            {
                TotalVolumes = title.TotalVolumes.ToString();
            }
            else
            {
                TotalVolumes = "???";
            }

            NoInList = (DBMethods.GetMRateFromDB(title.TitleId, "TitleId") == null);

            AddInfo = new AddMangaInfoVM(User, TitleId);
        }

        protected override void onAddNewTitle(int titleId)
        {
            AddInfo.Visibility = !AddInfo.Visibility;
        }

        protected override void onGetTitleInfo(int titleId)
        {
            var titleFullInfo = User.GetMangaFullInfo(titleId);
            DialogService.ShowDialog(new MangaFullViewVM(titleFullInfo) { User = User });
        }
    }
}

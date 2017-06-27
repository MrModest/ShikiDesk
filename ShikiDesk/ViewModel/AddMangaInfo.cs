using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiDesk.ViewModel
{
    public class AddMangaInfoVM : AddTitleInfoVM
    {
        int _volumes;
        public int Volumes
        {
            get { return _volumes; }
            set { Set(ref _volumes, value); }
        }

        int _chapters;
        public int Chapters
        {
            get { return _chapters; }
            set { Set(ref _chapters, value); }
        }

        public static string[] UserStatusesRus { get; } = MangaVM.UserStatusesRus;

        protected override void onAddTitle()
        {
            try
            {
                var rate = User.CreateMangaRate(TitleId, Status, Score, Volumes, Chapters);
                if (rate != null) { DBMethods.AddMRateToDB(rate); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddTitle: " + ex.Message);
            }
        }

        public AddMangaInfoVM() : base()
        {
            Volumes = 0;
            Chapters = 0;
        }

        public AddMangaInfoVM(ShikiApiLib.ShikiApi user, int titleId) : base(user, titleId)
        {
            Volumes = 0;
            Chapters = 0;
        }
    }
}

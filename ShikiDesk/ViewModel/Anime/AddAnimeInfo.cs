using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiDesk.ViewModel
{
    public class AddAnimeInfoVM : AddTitleInfoVM
    {
        int _episodes;
        public int Episodes
        {
            get { return _episodes; }
            set { Set(ref _episodes, value); }
        }

        public static string[] UserStatusesRus { get; } = AnimeVM.UserStatusesRus;

        protected override void onAddTitle()
        {
            try
            {
                var rate = User.CreateAnimeRate(TitleId, Status, Score, Episodes);
                if (rate != null) { DBMethods.AddARateToDB(rate); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddTitle: " + ex.Message);
            }
        }

        public AddAnimeInfoVM() : base()
        {
            Episodes = 0;
        }

        public AddAnimeInfoVM(ShikiApiLib.ShikiApi user, int titleId) : base(user, titleId)
        {
            Episodes = 0;
        }
    }
}

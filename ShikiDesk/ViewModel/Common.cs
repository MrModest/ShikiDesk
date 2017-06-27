using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ShikiDesk.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;

        private IDialogService _dialogService;

        public IDialogService DialogService
        {
            get { return _dialogService ?? ShikiDesk.DialogService.Default; }
            set { _dialogService = value; }
        }

        ShikiApiLib.ShikiApi _user;
        public ShikiApiLib.ShikiApi User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("предикат не должен быть равным нулю");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("предикат не должен быть равным нулю");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class ProgressBarVM : ViewModel
    {
        int _percent = 0;
        public int Percent
        {
            get { return _percent; } // валидация при записи
            set { if (Set(ref _percent, (value > 100) ? 100 : (value < 0) ? 0 : value)) RaisePropertyChanged(nameof(Log)); }
        }

        string _log = "Загрузка...";
        public string Log
        {
            get { return "[" + _percent + "%] " + _log; }
        }

        public void Set(int percent, string log)
        {
            Percent = percent;
            _log = log;
            RaisePropertyChanged(nameof(Log));
        }
    }

    public abstract class TitleViewVM : ViewModel
    {
        int _user_rate_id;
        public int UserRateId
        {
            get { return _user_rate_id; }
            set { Set(ref _user_rate_id, value); }
        }

        int _title_id;
        public int TitleId
        {
            get { return _title_id; }
            set { Set(ref _title_id, value); }
        }

        string _name;
        public string MainName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        string _secondName;
        public string SecondName
        {
            get { return _secondName; }
            set { Set(ref _secondName, value); }
        }

        string _user_status;
        public string UserStatus
        {
            get { return _user_status; }
            set { Set(ref _user_status, value); }
        }

        string _title_status;
        public string TitleStatus
        {
            get { return _title_status; }
            set { Set(ref _title_status, value); }
        }

        int _score;
        public int Score
        {
            get { return _score; }
            set { Set(ref _score, value); }
        }

        string _kind;
        public string Kind
        {
            get { return _kind; }
            set { Set(ref _kind, value); }
        }

        public TitleViewVM() { }

        public TitleViewVM(ShikiApiLib.TitleRate rate)
        {
            UserRateId = rate.UserRateId;
            TitleId = rate.TitleId;
            if (Properties.Settings.Default.titleLanguage == "EN")
            {
                MainName = rate.Name; //Настроить!
                SecondName = rate.RusName;
            }
            else
            {
                MainName = rate.RusName; //Настроить!
                SecondName = rate.Name;
            }
            UserStatus = rate.UserStatus;
            TitleStatus = rate.TitleStatus;
            Score = rate.Score;
            Kind = rate.Kind;
        }
    }

    public abstract class TitleTabVM : ViewModel
    {
        string _userStatusRus;
        public string UserStatusRus
        {
            get { return _userStatusRus; }
            set { Set(ref _userStatusRus, value); }
        }
    }

    public abstract class TitleFullViewVM : ViewModel
    {
        int _title_id;
        public int TitleId
        {
            get { return _title_id; }
            set { Set(ref _title_id, value); }
        }

        string _poster;
        public string Poster
        {
            get { return _poster; }
            set { Set(ref _poster, value); }
        }

        string _url;
        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }

        int _myAnimeListId;
        public int MyAnimeListId
        {
            get { return _myAnimeListId; }
            set { Set(ref _myAnimeListId, value); }
        }

        bool _noInList;
        public bool NoInList
        {
            get { return _noInList; }
            set { Set(ref _noInList, value); }
        }

        string _name;
        public string MainName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        string _secondName;
        public string SecondName
        {
            get { return _secondName; }
            set { Set(ref _secondName, value); }
        }

        string _title_status;
        public string TitleStatus
        {
            get { return _title_status; }
            set { Set(ref _title_status, value); }
        }

        double _titleScore;
        public double TitleScore
        {
            get { return _titleScore; }
            set { Set(ref _titleScore, value); }
        }

        string _kind;
        public string Kind
        {
            get { return _kind; }
            set { Set(ref _kind, value); }
        }

        string _genres;
        public string Genres
        {
            get { return _genres; }
            set { Set(ref _genres, value); }
        }

        string _jpNames;
        public string JpNames
        {
            get { return _jpNames; }
            set { Set(ref _jpNames, value); }
        }

        string _enNames;
        public string EnNames
        {
            get { return _enNames; }
            set { Set(ref _enNames, value); }
        }

        string _otherNames;
        public string OtherNames
        {
            get { return _otherNames; }
            set { Set(ref _otherNames, value); }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        int[] _scoreStat;
        public int[] ScoreStat
        {
            get { return _scoreStat; }
            set { Set(ref _scoreStat, value); }
        }

        double[] _scoreStatProc;
        public double[] ScoreStatProc
        {
            get { return _scoreStatProc; }
            set { Set(ref _scoreStatProc, value); }
        }

        int[] _statusStat;
        public int[] StatusStat
        {
            get { return _statusStat; }
            set { Set(ref _statusStat, value); }
        }

        int[] _statusStatProc;
        public int[] StatusStatProc
        {
            get { return _statusStatProc; }
            set { Set(ref _statusStatProc, value); }
        }

        public ICommand AddNewTitle { get; }

        protected abstract void onAddNewTitle(int titleId);

        public ICommand GetTitleInfo { get; }

        protected abstract void onGetTitleInfo(int titleId);

        public ICommand GoToShiki { get; }

        protected void onGoToShiki()
        {
            System.Diagnostics.Process.Start(Url);
        }

        public ICommand GoToMAL { get; }

        protected void onGoToMAL()
        {
            System.Diagnostics.Process.Start("https://myanimelist.net/anime/" + MyAnimeListId);
        }

        public TitleFullViewVM()
        {
            ScoreStat = new int[11]; // 0 - not use! another is score (1 - 10)
            ScoreStatProc = new double[11];
            for (int i = 0; i < 11; i++) { ScoreStat[i] = 0; ScoreStatProc[i] = 0; }
            StatusStat = new int[5]; // 0 - planned, 1 - watching, etc.
            StatusStatProc = new int[5];
            for (int i = 0; i < 5; i++) { StatusStat[i] = 0; StatusStatProc[i] = 0; }

            GoToShiki = new RelayCommand(onGoToShiki);
            GoToMAL = new RelayCommand(onGoToMAL);
        }

        public TitleFullViewVM(ShikiApiLib.TitleFullInfo title) : this()
        {
            TitleId = title.TitleId;
            Poster = title.Poster.original;
            Url = title.Url;
            MyAnimeListId = title.MyAnimeListId;

            if (Properties.Settings.Default.titleLanguage == "EN")
            {
                MainName = title.Name; //Настроить!
                SecondName = title.RusName;
            }
            else
            {
                MainName = title.RusName; //Настроить!
                SecondName = title.Name;
            }
            TitleStatus = title.TitleStatus.Substring(0, 1).ToUpper() + title.TitleStatus.Remove(0, 1);
            TitleScore = title.TitleScore;
            Kind = title.Kind.ToUpper();
            Genres = ListToStr(title.Genres.Select(genre => genre.russian).ToList());
            JpNames = ListToStr(title.JpNames);
            EnNames = ListToStr(title.EnNames);
            OtherNames = ListToStr(title.Synonyms);
            Regex reg = new Regex(@"\[.*?\]|\(.*?\)");
            if (title.Description != null)
            {
                Description = reg.Replace(title.Description.Replace("[[", "").Replace("]]", "").Replace("\t", ""), "").Replace("  ", " ").Replace(" ,", ",");
            }
            

            int maxCountScore = 0;

            for (int i = 1; i < 11; i++)
            {
                if (title.GetScoresStats(i) > maxCountScore)
                {
                    maxCountScore = title.GetScoresStats(i);
                }
            }

            for (int i = 1; i < 11; i++)
            {
                ScoreStat[i] = title.GetScoresStats(i);
                ScoreStatProc[i] = (double)ScoreStat[i] * 250 / maxCountScore;
            }
        }

        public TitleFullViewVM(ShikiApiLib.TitleFullInfo title, ShikiApiLib.ShikiApi user) : this(title)
        {
            User = user;
        }

        public TitleFullViewVM(ShikiApiLib.TitleShortInfo title)
        {
            TitleId = title.TitleId;
            Poster = title.Poster.original;
            Url = title.Url;

            if (Properties.Settings.Default.titleLanguage == "EN")
            {
                MainName = title.Name; //Настроить!
                SecondName = title.RusName;
            }
            else
            {
                MainName = title.RusName; //Настроить!
                SecondName = title.Name;
            }
            TitleStatus = title.TitleStatus.Substring(0, 1).ToUpper() + title.TitleStatus.Remove(0, 1);
            Kind = title.Kind.ToUpper();

            GetTitleInfo = new RelayCommand<int>(onGetTitleInfo);
            AddNewTitle = new RelayCommand<int>(onAddNewTitle);
        }


        public TitleFullViewVM(ShikiApiLib.TitleShortInfo title, ShikiApiLib.ShikiApi user) : this(title)
        {
            User = user;
        }

        protected string ListToStr(List<string> list)
        {
            string str = "";
            foreach (var item in list)
            {
                str += item + ", ";
            }
            if (list.Count > 0) { str = str.Remove(str.Length - 2); }
            return str;
        }
    }

    public class NotifyBool : ViewModel
    {
        bool? _value;
        public bool? Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        public NotifyBool() { }

        public NotifyBool(bool? value)
        {
            Value = value;
        }
    }

    public abstract class TitleSearchVM : ViewModel
    {
        bool _censored;
        public bool Censored
        {
            get { return _censored; }
            set { Set(ref _censored, value); }
        }

        int _limit;
        public int Limit
        {
            get { return _limit; }
            set { Set(ref _limit, value); }
        }

        int _page;
        public int Page
        {
            get { return _page; }
            set { Set(ref _page, value); }
        }

        string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { Set(ref _searchText, value); }
        }

        Dictionary<int, NotifyBool> _genre;
        public Dictionary<int, NotifyBool> Genre
        {
            get { return _genre; }
            set { Set(ref _genre, value); }
        }

        string[] _genreName;
        public string[] GenreName
        {
            get { return _genreName; }
            set { Set(ref _genreName, value); }
        }

        Dictionary<ShikiApiLib.Order, NotifyBool> _order;
        public Dictionary<ShikiApiLib.Order, NotifyBool> Order
        {
            get { return _order; }
            set { Set(ref _order, value); }
        }

        Dictionary<ShikiApiLib.UserStatus, NotifyBool> _myList;
        public Dictionary<ShikiApiLib.UserStatus, NotifyBool> MyList
        {
            get { return _myList; }
            set { Set(ref _myList, value); }
        }

        Dictionary<ShikiApiLib.Rating, NotifyBool> _rating;
        public Dictionary<ShikiApiLib.Rating, NotifyBool> Rating
        {
            get { return _rating; }
            set { Set(ref _rating, value); }
        }

        Dictionary<string, NotifyBool> _season;
        public Dictionary<string, NotifyBool> Season
        {
            get { return _season; }
            set { Set(ref _season, value); }
        }

        int _titleScore;
        public int TitleScore
        {
            get { return _titleScore; }
            set { Set(ref _titleScore, value); }
        }

        Dictionary<ShikiApiLib.TitleStatus, NotifyBool> _titleStatus;
        public Dictionary<ShikiApiLib.TitleStatus, NotifyBool> TitleStatus
        {
            get { return _titleStatus; }
            set { Set(ref _titleStatus, value); }
        }

        public ICommand Search { get; }

        protected abstract void onSearch();

        public TitleSearchVM()
        {
            Censored = false;
            TitleScore = 0;
            Genre = new Dictionary<int, NotifyBool>();
            Season = new Dictionary<string, NotifyBool>();
            Order = InitEnumDict<ShikiApiLib.Order>();
            MyList = InitEnumDict<ShikiApiLib.UserStatus>();
            Rating = InitEnumDict<ShikiApiLib.Rating>();
            TitleStatus = InitEnumDict<ShikiApiLib.TitleStatus>();

            Search = new RelayCommand(onSearch);
        }

        public TitleSearchVM(ShikiApiLib.ShikiApi user) : this()
        {
            User = user;
        }

        protected Dictionary<T, bool> ParceDict<T>(Dictionary<T, NotifyBool> dict)
        {
            var parcedDict = new Dictionary<T, bool>();
            foreach (var item in dict)
            {
                if (item.Value.Value != null)
                {
                    parcedDict[item.Key] = (bool)item.Value.Value;
                }
            }
            return parcedDict;
        }

        protected Dictionary<T, NotifyBool> InitEnumDict<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Ключём словаря должен быть перечислимый тип!");
            }

            var dict = new Dictionary<T, NotifyBool>();

            foreach (var key in Enum.GetValues(typeof(T)))
            {
                if (typeof(T) == typeof(ShikiApiLib.Order))
                {
                    dict.Add((T)key, new NotifyBool(false));
                }
                else
                {
                    dict.Add((T)key, new NotifyBool(null));
                }
            }

            return dict;
        }

        protected ShikiApiLib.Order SetOrder(Dictionary<ShikiApiLib.Order, NotifyBool> order)
        {
            foreach (var item in order)
            {
                if (item.Value.Value == true)
                {
                    return item.Key;
                }
            }
            return ShikiApiLib.Order.ranked;
        }
    }

    public abstract class AddTitleInfoVM : ViewModel
    {
        int _titleId;
        public int TitleId
        {
            get { return _titleId; }
            set { Set(ref _titleId, value); }
        }

        ShikiApiLib.UserStatus _status;
        public ShikiApiLib.UserStatus Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        int _score;
        public int Score
        {
            get { return _score; }
            set { Set(ref _score, value); }
        }

        bool _visibility;
        public bool Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        public static int[] Scores { get; } = MainVM.Scores;

        public ICommand AddTitle { get; }

        protected abstract void onAddTitle();

        public AddTitleInfoVM()
        {
            AddTitle = new RelayCommand(onAddTitle);

            Status = ShikiApiLib.UserStatus.planned;
            Score = 0;
            Visibility = false;
        }

        public AddTitleInfoVM(ShikiApiLib.ShikiApi user, int titleId) : this()
        {
            User = user;
            TitleId = titleId;
        }
    }
}
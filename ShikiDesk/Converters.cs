using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ShikiDesk
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (String.IsNullOrWhiteSpace((string)value))
                {
                    //throw new Exception();
                }
                return new System.Windows.Media.Imaging.BitmapImage(new Uri((string)value, UriKind.Absolute));
            }
            catch
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri("Resources/Images/missing_original.jpg", UriKind.Relative));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class StatusStringToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string) value)
            {
                case "planned":
                    return 0;
                case "watching":
                    return 1;
                case "completed":
                    return 2;
                case "on_hold":
                    return 3;
                case "dropped":
                    return 4;
                default:
                    return -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return ShikiApiLib.UserRateStatus.Planned;
                case 1:
                    return ShikiApiLib.UserRateStatus.Watching;
                case 2:
                    return ShikiApiLib.UserRateStatus.Completed;
                case 3:
                    return ShikiApiLib.UserRateStatus.OnHold;
                case 4:
                    return ShikiApiLib.UserRateStatus.Dropped;
                default:
                    return null;
            }
        }
    }

    public class UserStatusToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ShikiApiLib.UserStatus)value)
            {
                case ShikiApiLib.UserStatus.planned:
                    return 0;
                case ShikiApiLib.UserStatus.watching:
                    return 1;
                case ShikiApiLib.UserStatus.completed:
                    return 2;
                case ShikiApiLib.UserStatus.on_hold:
                    return 3;
                case ShikiApiLib.UserStatus.dropped:
                    return 4;
                default:
                    return -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return ShikiApiLib.UserStatus.planned;
                case 1:
                    return ShikiApiLib.UserStatus.watching;
                case 2:
                    return ShikiApiLib.UserStatus.completed;
                case 3:
                    return ShikiApiLib.UserStatus.on_hold;
                case 4:
                    return ShikiApiLib.UserStatus.dropped;
                default:
                    return null;
            }
        }
    }

    public class ScoreToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 10 - (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return 10;
                case 1:
                    return 9;
                case 2:
                    return 8;
                case 3:
                    return 7;
                case 4:
                    return 6;
                case 5:
                    return 5;
                case 6:
                    return 4;
                case 7:
                    return 3;
                case 8:
                    return 2;
                case 9:
                    return 1;
                default:
                    return 0;
            }
        }
    }

    public class ToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).ToUpper().Replace('_', ' ');
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // ((string)value).ToLower().Replace(' ', '_');
        }
    }

    public class TotalEpisodesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var title = value as ShikiApiLib.AnimeFullInfo;
            int total = Math.Max(title.TotalEpisodes, title.AiredEpisodes);
            return (total != 0) ? total.ToString() : "?";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class GenreIdToRussianConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DBMethods.GetGenreFromDB((int)value).russian;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class StudioIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DBMethods.GetStudioFromDB((int)value).name;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class PublisherIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DBMethods.GetPublisherFromDB((int)value).name;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

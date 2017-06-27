using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;

namespace ShikiDesk
{
    public static class DBMethods
    {
        public static void FillDataDB<T>(List<T> rates)
        {
            using(var db = new UserRatesDB("UserRates"))
            {
                if (typeof(T) == typeof(ShikiApiLib.AnimeRate))
                {
                    db.TAnimeRates.Delete();
                    db.BeginTransaction();
                    foreach (var rate in (rates as List<ShikiApiLib.AnimeRate>))
                    {
                        db.TAnimeRates
                            .Value(p => p.user_rate_id,       () => rate.UserRateId)
                            .Value(p => p.title_id,           () => rate.TitleId)
                            .Value(p => p.original_name,      () => rate.Name)
                            .Value(p => p.rus_name,           () => rate.RusName)
                            .Value(p => p.url,                () => rate.Url)
                            .Value(p => p.kind,               () => rate.Kind)
                            .Value(p => p.title_status,       () => rate.TitleStatus)
                            .Value(p => p.aired_on,           () => rate.AiredOn)
                            .Value(p => p.released_on,        () => rate.ReleasedOn)
                            .Value(p => p.total_episodes,     () => rate.TotalEpisodes)
                            .Value(p => p.aired_episodes,     () => rate.AiredEpisodes)
                            .Value(p => p.completed_episodes, () => rate.CompletedEpisodes)
                            .Value(p => p.user_status,        () => rate.UserStatus)
                            .Value(p => p.score,              () => rate.Score)
                            .Insert();
                    }
                    db.CommitTransaction();
                    return;
                }

                if (typeof(T) == typeof(ShikiApiLib.MangaRate))
                {
                    db.TMangaRates.Delete();
                    db.BeginTransaction();
                    foreach (var rate in rates as List<ShikiApiLib.MangaRate>)
                    {
                        db.TMangaRates
                            .Value(p => p.user_rate_id, () => rate.UserRateId)
                            .Value(p => p.title_id, () => rate.TitleId)
                            .Value(p => p.original_name, () => rate.Name)
                            .Value(p => p.rus_name, () => rate.RusName)
                            .Value(p => p.url, () => rate.Url)
                            .Value(p => p.kind, () => rate.Kind)
                            .Value(p => p.title_status, () => rate.TitleStatus)
                            .Value(p => p.aired_on, () => rate.AiredOn)
                            .Value(p => p.released_on, () => rate.ReleasedOn)
                            .Value(p => p.total_volumes, () => rate.TotalVolumes)
                            .Value(p => p.total_chapters, () => rate.TotalChapters)
                            .Value(p => p.completed_volumes, () => rate.CompletedVolumes)
                            .Value(p => p.completed_chapters, () => rate.CompletedChapters)
                            .Value(p => p.user_status, () => rate.UserStatus)
                            .Value(p => p.score, () => rate.Score)
                            .Insert();
                    }
                    db.CommitTransaction();
                    return;
                }

                throw new ArgumentException("Тип аргумента может быть только ShikiApiLib.AnimeRate или ShikiApiLib.MangaRate");
            }
        }

        public static void AddARateToDB(ShikiApiLib.AnimeRate rate)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                if (db.TAnimeRates.Count(x => x.title_id == rate.TitleId) > 0)
                {
                    return;
                }

                db.TAnimeRates
                    .Value(p => p.user_rate_id, () => rate.UserRateId)
                    .Value(p => p.title_id, () => rate.TitleId)
                    .Value(p => p.original_name, () => rate.Name)
                    .Value(p => p.rus_name, () => rate.RusName)
                    .Value(p => p.url, () => rate.Url)
                    .Value(p => p.kind, () => rate.Kind)
                    .Value(p => p.title_status, () => rate.TitleStatus)
                    .Value(p => p.aired_on, () => rate.AiredOn)
                    .Value(p => p.released_on, () => rate.ReleasedOn)
                    .Value(p => p.total_episodes, () => rate.TotalEpisodes)
                    .Value(p => p.aired_episodes, () => rate.AiredEpisodes)
                    .Value(p => p.completed_episodes, () => rate.CompletedEpisodes)
                    .Value(p => p.user_status, () => rate.UserStatus)
                    .Value(p => p.score, () => rate.Score)
                    .Insert();
            }
        }

        public static void AddMRateToDB(ShikiApiLib.MangaRate rate)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                if (db.TAnimeRates.Count(x => x.title_id == rate.TitleId) > 0)
                {
                    return;
                }

                db.TMangaRates
                    .Value(p => p.user_rate_id, () => rate.UserRateId)
                    .Value(p => p.title_id, () => rate.TitleId)
                    .Value(p => p.original_name, () => rate.Name)
                    .Value(p => p.rus_name, () => rate.RusName)
                    .Value(p => p.url, () => rate.Url)
                    .Value(p => p.kind, () => rate.Kind)
                    .Value(p => p.title_status, () => rate.TitleStatus)
                    .Value(p => p.aired_on, () => rate.AiredOn)
                    .Value(p => p.released_on, () => rate.ReleasedOn)
                    .Value(p => p.total_volumes, () => rate.TotalVolumes)
                    .Value(p => p.total_chapters, () => rate.TotalChapters)
                    .Value(p => p.completed_volumes, () => rate.CompletedVolumes)
                    .Value(p => p.completed_chapters, () => rate.CompletedChapters)
                    .Value(p => p.user_status, () => rate.UserStatus)
                    .Value(p => p.score, () => rate.Score)
                    .Insert();
            }
        }

        public static List<ShikiApiLib.AnimeRate> GetARatesFromDB()
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var response = new List<ShikiApiLib.AnimeRate>();
                var table = db.TAnimeRates.Select(x => x);
                foreach (var rate in db.TAnimeRates.Select(x => x).ToList())
                {
                    //Console.WriteLine(rate.original_name + ": " + rate.total_episodes);
                    response.Add(new ShikiApiLib.AnimeRate()
                    {
                        UserRateId = rate.user_rate_id,
                        TitleId = rate.title_id,
                        Name = rate.original_name,
                        RusName = rate.rus_name,
                        Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.anime),
                        Url = rate.url,
                        Kind = rate.kind,
                        TitleStatus = rate.title_status,
                        AiredOn = rate.aired_on,
                        ReleasedOn = rate.released_on,
                        TotalEpisodes = rate.total_episodes,
                        AiredEpisodes = rate.aired_episodes,
                        CompletedEpisodes = rate.completed_episodes,
                        UserStatus = rate.user_status,
                        Score = rate.score
                    });
                }
                return response;
            }
        }

        public static List<ShikiApiLib.AnimeRate> GetARatesFromDB(ShikiApiLib.UserStatus userStatus)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var response = new List<ShikiApiLib.AnimeRate>();
                foreach (var rate in db.TAnimeRates.Where(x => x.user_rate_id == (int)userStatus).Select(x => x).ToList())
                {
                    response.Add(new ShikiApiLib.AnimeRate()
                    {
                        UserRateId = rate.user_rate_id,
                        TitleId = rate.title_id,
                        Name = rate.original_name,
                        RusName = rate.rus_name,
                        Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.anime),
                        Url = rate.url,
                        Kind = rate.kind,
                        TitleStatus = rate.title_status,
                        AiredOn = rate.aired_on,
                        ReleasedOn = rate.released_on,
                        TotalEpisodes = rate.total_episodes,
                        AiredEpisodes = rate.aired_episodes,
                        CompletedEpisodes = rate.completed_episodes,
                        UserStatus = rate.user_status,
                        Score = rate.score
                    });
                }
                return response;
            }
        }

        public static ShikiApiLib.AnimeRate GetARateFromDB(int id, string searchFor = "UserRateId")
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var rate = db.TAnimeRates.FirstOrDefault(r => (searchFor == "UserRateId") ? r.user_rate_id == id : r.title_id == id);
                if (rate == null) { return null; }
                return new ShikiApiLib.AnimeRate()
                {
                    UserRateId = rate.user_rate_id,
                    TitleId = rate.title_id,
                    Name = rate.original_name,
                    RusName = rate.rus_name,
                    Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.anime),
                    Url = rate.url,
                    Kind = rate.kind,
                    TitleStatus = rate.title_status,
                    AiredOn = rate.aired_on,
                    ReleasedOn = rate.released_on,
                    TotalEpisodes = rate.total_episodes,
                    AiredEpisodes = rate.aired_episodes,
                    CompletedEpisodes = rate.completed_episodes,
                    UserStatus = rate.user_status,
                    Score = rate.score
                };
            }
        }

        public static List<ShikiApiLib.AnimeRate> GetARatesByName(string name)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var search = db.TAnimeRates.Where(r => r.original_name == name || r.rus_name == name).ToList();
                var rates = new List<ShikiApiLib.AnimeRate>();

                foreach (var rate in search)
                {
                    rates.Add(new ShikiApiLib.AnimeRate()
                    {
                        UserRateId = rate.user_rate_id,
                        TitleId = rate.title_id,
                        Name = rate.original_name,
                        RusName = rate.rus_name,
                        Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.anime),
                        Url = rate.url,
                        Kind = rate.kind,
                        TitleStatus = rate.title_status,
                        AiredOn = rate.aired_on,
                        ReleasedOn = rate.released_on,
                        TotalEpisodes = rate.total_episodes,
                        AiredEpisodes = rate.aired_episodes,
                        CompletedEpisodes = rate.completed_episodes,
                        UserStatus = rate.user_status,
                        Score = rate.score
                    });
                }

                return rates;
            }
        }

        public static List<ShikiApiLib.MangaRate> GetMRatesFromDB()
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var response = new List<ShikiApiLib.MangaRate>();
                foreach (var rate in db.TMangaRates.Select(x => x).ToList())
                {
                    response.Add(new ShikiApiLib.MangaRate()
                    {
                        UserRateId = rate.user_rate_id,
                        TitleId = rate.title_id,
                        Name = rate.original_name,
                        RusName = rate.rus_name,
                        Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.manga),
                        Url = rate.url,
                        Kind = rate.kind,
                        TitleStatus = rate.title_status,
                        AiredOn = rate.aired_on,
                        ReleasedOn = rate.released_on,
                        TotalVolumes = rate.total_volumes,
                        TotalChapters = rate.total_chapters,
                        CompletedVolumes = rate.completed_volumes,
                        CompletedChapters = rate.completed_chapters,
                        UserStatus = rate.user_status,
                        Score = rate.score
                    });
                }
                return response;
            }
        }

        public static ShikiApiLib.MangaRate GetMRateFromDB(int id, string searchFor = "UserRateId")
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var rate = db.TMangaRates.FirstOrDefault(r => (searchFor == "UserRateId") ? r.user_rate_id == id : r.title_id == id);
                if (rate == null) { return null; }
                return new ShikiApiLib.MangaRate()
                {
                    UserRateId = rate.user_rate_id,
                    TitleId = rate.title_id,
                    Name = rate.original_name,
                    RusName = rate.rus_name,
                    Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.manga),
                    Url = rate.url,
                    Kind = rate.kind,
                    TitleStatus = rate.title_status,
                    AiredOn = rate.aired_on,
                    ReleasedOn = rate.released_on,
                    TotalVolumes = rate.total_volumes,
                    TotalChapters = rate.total_chapters,
                    CompletedVolumes = rate.completed_volumes,
                    CompletedChapters = rate.completed_chapters,
                    UserStatus = rate.user_status,
                    Score = rate.score
                };
            }
        }

        public static List<ShikiApiLib.MangaRate> GetMRatesFromDB(ShikiApiLib.UserStatus userStatus)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                var response = new List<ShikiApiLib.MangaRate>();
                foreach (var rate in db.TMangaRates.Where(x => x.user_rate_id == (int)userStatus).Select(x => x).ToList())
                {
                    response.Add(new ShikiApiLib.MangaRate()
                    {
                        UserRateId = rate.user_rate_id,
                        TitleId = rate.title_id,
                        Name = rate.original_name,
                        RusName = rate.rus_name,
                        Poster = new ShikiApiLib.TitleImage(rate.title_id, ShikiApiLib.TitleType.manga),
                        Url = rate.url,
                        Kind = rate.kind,
                        TitleStatus = rate.title_status,
                        AiredOn = rate.aired_on,
                        ReleasedOn = rate.released_on,
                        TotalVolumes = rate.total_volumes,
                        TotalChapters = rate.total_chapters,
                        CompletedVolumes = rate.completed_volumes,
                        CompletedChapters = rate.completed_chapters,
                        UserStatus = rate.user_status,
                        Score = rate.score
                    });
                }
                return response;
            }
        }

        public static void UpdateSelectedARateInDB(ShikiApiLib.AnimeRate rate)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                db.TAnimeRates
                    .Where(x => x.user_rate_id == rate.UserRateId)
                    .Set(x => x.title_id, rate.TitleId)
                    .Set(x => x.original_name, rate.Name)
                    .Set(x => x.rus_name, rate.RusName)
                    .Set(x => x.url, rate.Url)
                    .Set(x => x.kind, rate.Kind)
                    .Set(x => x.title_status, rate.TitleStatus)
                    .Set(x => x.aired_on, rate.AiredOn)
                    .Set(x => x.released_on, rate.ReleasedOn)
                    .Set(x => x.total_episodes, rate.TotalEpisodes)
                    .Set(x => x.aired_episodes, rate.AiredEpisodes)
                    .Set(x => x.completed_episodes, rate.CompletedEpisodes)
                    .Set(x => x.user_status, rate.UserStatus)
                    .Set(x => x.score, rate.Score)
                    .Update();
            }
        }

        public static void UpdateSelectedMRateInDB(ShikiApiLib.MangaRate rate)
        {
            using (var db = new UserRatesDB("UserRates"))
            {
                db.TMangaRates
                    .Where(x => x.user_rate_id == rate.UserRateId)
                    .Set(x => x.title_id, rate.TitleId)
                    .Set(x => x.original_name, rate.Name)
                    .Set(x => x.rus_name, rate.RusName)
                    .Set(x => x.url, rate.Url)
                    .Set(x => x.kind, rate.Kind)
                    .Set(x => x.title_status, rate.TitleStatus)
                    .Set(x => x.aired_on, rate.AiredOn)
                    .Set(x => x.released_on, rate.ReleasedOn)
                    .Set(x => x.total_volumes, rate.TotalVolumes)
                    .Set(x => x.total_chapters, rate.TotalChapters)
                    .Set(x => x.completed_volumes, rate.CompletedVolumes)
                    .Set(x => x.completed_chapters, rate.CompletedChapters)
                    .Set(x => x.user_status, rate.UserStatus)
                    .Set(x => x.score, rate.Score)
                    .Update();
            }
        }

        public static void FillGenresDB(List<ShikiApiLib.Genre> genres)
        {
            using (var db = new FiltersDB("Filters"))
            {
                db.TGenres.Delete();

                db.BeginTransaction();

                foreach (var genre in genres)
                {
                    db.TGenres
                        .Value(p => p.id, () => genre.id)
                        .Value(p => p.name, () => genre.name)
                        .Value(p => p.russian, () => genre.russian)
                        .Value(p => p.kind, () => genre.kind)
                        .Insert();
                }

                db.CommitTransaction();
            }
        }

        public static void FillStudiosDB(List<ShikiApiLib.Studio> studios)
        {
            using (var db = new FiltersDB("Filters"))
            {
                db.TStudios.Delete();

                db.BeginTransaction();

                foreach (var genre in studios)
                {
                    db.TStudios
                        .Value(p => p.id, () => genre.id)
                        .Value(p => p.name, () => genre.name)
                        .Value(p => p.filtered_name, () => genre.filtered_name)
                        .Value(p => p.real, () => genre.real)
                        .Value(p => p.image, () => genre.image )
                        .Insert();
                }

                db.CommitTransaction();
            }
        }

        public static void FillPublishersDB(List<ShikiApiLib.Publisher> publishers)
        {
            using (var db = new FiltersDB("Filters"))
            {
                db.TPublishers.Delete();

                db.BeginTransaction();

                foreach (var genre in publishers)
                {
                    db.TPublishers
                        .Value(p => p.id, () => genre.id)
                        .Value(p => p.name, () => genre.name)
                        .Insert();
                }

                db.CommitTransaction();
            }
        }

        public static List<ShikiApiLib.Genre> GetGenresFromDB()
        {
            using (var db = new FiltersDB("Filters"))
            {
                var response = new List<ShikiApiLib.Genre>();

                foreach (var genre in db.TGenres.Select(x => x).ToList())        
                {
                    response.Add(new ShikiApiLib.Genre()
                    {
                        id = genre.id,
                        name = genre.name,
                        russian = genre.russian,
                        kind = genre.kind
                    });
                }

                return response;
            }
        }

        public static ShikiApiLib.Genre GetGenreFromDB(int id)
        {
            using (var db = new FiltersDB("Filters"))
            {
                var response = new List<ShikiApiLib.Genre>();

                var genre = db.TGenres.FirstOrDefault(x => x.id == id);

                return new ShikiApiLib.Genre()
                {
                    id = genre.id,
                    name = genre.name,
                    russian = genre.russian,
                    kind = genre.kind
                };
            }
        }

        public static List<ShikiApiLib.Studio> GetStudiosFromDB()
        {
            using (var db = new FiltersDB("Filters"))
            {
                var response = new List<ShikiApiLib.Studio>();
                foreach (var studio in db.TStudios.Select(x => x).ToList())
                {
                    response.Add(new ShikiApiLib.Studio()
                    {
                        id = studio.id,
                        name = studio.name,
                        filtered_name = studio.filtered_name,
                        real = studio.real,
                        image = studio.image
                    });
                }

                return response;
            }
        }

        public static ShikiApiLib.Studio GetStudioFromDB(int id)
        {
            using (var db = new FiltersDB("Filters"))
            {
                var response = new List<ShikiApiLib.Studio>();
                var studio = db.TStudios.FirstOrDefault(x => x.id == id);

                return new ShikiApiLib.Studio()
                {
                    id = studio.id,
                    name = studio.name,
                    filtered_name = studio.filtered_name,
                    real = studio.real,
                    image = studio.image
                };
            }
        }

        public static List<ShikiApiLib.Publisher> GetPublishersFromDB()
        {
            using (var db = new FiltersDB("Filters"))
            {
                var response = new List<ShikiApiLib.Publisher>();

                foreach (var publisher in db.TPublishers.Select(x => x).ToList())
                {
                    response.Add(new ShikiApiLib.Publisher()
                    {
                        id = publisher.id,
                        name = publisher.name,
                    });
                }

                return response;
            }
        }

        public static ShikiApiLib.Publisher GetPublisherFromDB(int id)
        {
            using (var db = new FiltersDB("Filters"))
            {
                var response = new List<ShikiApiLib.Publisher>();

                var publisher = db.TPublishers.FirstOrDefault(x => x.id == id);

                return new ShikiApiLib.Publisher()
                {
                    id = publisher.id,
                    name = publisher.name,
                };
            }
        }
    }
}

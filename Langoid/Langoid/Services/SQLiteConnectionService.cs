using Langoid.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using SQLiteException = Android.Database.Sqlite.SQLiteException;

namespace Langoid.Services
{
    public class SqLiteConnectionService
    {
        private SQLiteConnection connection;
        private string CreateDatabase()
        {
            try
            {
                connection = new SQLiteConnection(Constans.DataBaseName);
                {
                    connection.CreateTable<Score>();
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public List<Score> GetEnglishPronunciationsScores()
        {
            return connection.Table<Score>().Where(x => x.Language == Enums.Language.English && x.Mode == "Pronunciations").ToList();
        }

        public List<Score> GetEnglishVocabularyScores()
        {
            return connection.Table<Score>().Where(x => x.Language == Enums.Language.English && x.Mode == "Vocabulary").ToList();
        }

        public List<Score> GetGermanPronunciationsScores()
        {
            return connection.Table<Score>().Where(x => x.Language == Enums.Language.German && x.Mode == "Pronunciations").ToList();
        }

        public List<Score> GetGermanVocabularyScores()
        {
            return connection.Table<Score>().Where(x => x.Language == Enums.Language.German && x.Mode == "Vocabulary").ToList();
        }

        public bool CheckScoreIsHighScore(Score score)
        {
            var scores = connection.Table<Score>().Where(x => x.Language == score.Language && x.Mode == score.Mode).OrderByDescending(x => x.Value).ToList();
            var count = scores.Count;

            if (scores.Count < 3)
                return true;
            else if (score.Value < scores[count - 1].Value)
                return false;
            else
                return true;
        }

        public void InsertHighScore(Score score)
        {
            var scores = connection.Table<Score>().Where(x => x.Language == score.Language && x.Mode == score.Mode).OrderByDescending(x => x.Value).ToList();

            foreach (var item in scores)
                connection.Delete(item);

            scores.Add(score);
            scores.OrderByDescending(x => x.Value);

            if (scores.Count > 3)
            {
                scores.RemoveAt(scores.Count - 1);
            }

            foreach (var item in scores)
            {
                connection.Insert(item);
            }

        }
    }
}
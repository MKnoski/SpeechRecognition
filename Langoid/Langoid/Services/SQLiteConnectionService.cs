using Android.Content.Res;
using SQLite;
using SQLiteException = Android.Database.Sqlite.SQLiteException;

namespace Langoid.Services
{
    public class SqLiteConnectionService
    {
        private string CreateDatabase(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(Constans.DataBaseName);
                {
                    // connection.CreateTableAsync<Model>();
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
    }
}
using Langoid.Enums;
using SQLite;

namespace Langoid.Models
{
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nick { get; set; }
        public int Value { get; set; }
        public Language Language { get; set; }
        public Mode Mode { get; set; }
    }
}
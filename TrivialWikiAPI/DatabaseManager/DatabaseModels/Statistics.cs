using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }

        public User ArchNemesis { get; set; }
        public Topic BestTopic { get; set; }
        public int GamesPlayed { get; set; }
        public User MostPlayedOpponent { get; set; }
        public Topic MostPlayedTopic { get; set; }
        public int Wins { get; set; }
        public int WinsInARow { get; set; }
    }
}

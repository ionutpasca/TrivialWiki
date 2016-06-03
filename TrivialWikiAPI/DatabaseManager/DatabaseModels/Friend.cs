using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}
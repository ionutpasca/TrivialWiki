using System.Data.Entity;

namespace DatabaseManager.DatabaseModels
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PrivateChat> PrivateChats { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
        public DbSet<RelatedTopic> RelatedTopics { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TriviaMessage> TriviaMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProposedTopic> ProposedTopics { get; set; }
    }
}
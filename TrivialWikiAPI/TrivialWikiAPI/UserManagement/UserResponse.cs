namespace TrivialWikiAPI.UserManagement
{
    public sealed class UserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
    }
}
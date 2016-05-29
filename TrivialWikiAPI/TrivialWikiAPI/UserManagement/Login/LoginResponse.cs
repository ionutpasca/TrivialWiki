namespace TrivialWikiAPI.UserManagement.Login
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SecurityToken { get; set; }
        public string Role { get; set; }
        public int Rank { get; set; }
        public string Avatar { get; set; }
    }
}
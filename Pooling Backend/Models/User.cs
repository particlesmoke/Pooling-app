namespace Pooling_Backend.Models
{

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }

    public class UserProfile
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class UserProfileInput : UserProfile
    {
        public string PlaintextPassword { get; set; }
    }
}

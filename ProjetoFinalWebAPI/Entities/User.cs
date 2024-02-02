using Services;

namespace Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Admin { get; set; } 
        public string Password { get; set; }

        public User(string name, string email, bool admin, string password, int id = 0)
        {
            Id = (id == 0) ? IdGenerator.UserId() : id;
            Name = name;
            Email = email;
            Admin = admin;
            Password = password;
        }
    }
}

namespace FinalProj.Entities
{
    public class User : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public bool Admin { get; private set; }
        public User(string name, string email, bool admin)
        {
            Id = IdGenerator.ItemId();
            Name = name;
            Email = email;
            Admin = admin;
        }
    }
}

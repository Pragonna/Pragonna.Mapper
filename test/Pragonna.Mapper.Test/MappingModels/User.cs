public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid Guid { get; set; }

    public User User_User
    {
        get
        {
            return new User()
            {
                Id = 2
            };
        }  
    } 
}
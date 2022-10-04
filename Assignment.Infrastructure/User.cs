namespace Assignment.Infrastructure;

public class User
{
    [Required]
    public int Id { get; set; }

    
    [MaxLength(100)]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public ICollection<WorkItem> Items { get; set; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
        Items = new HashSet<WorkItem>();
    }
}

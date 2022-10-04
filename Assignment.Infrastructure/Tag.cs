namespace Assignment.Infrastructure;

public class Tag
{
    public int Id { get; set; }
    [MaxLength(100)]public string Name { get; set; }
    public ICollection<WorkItem> WorkItems { get; set; }

    public Tag(string name)
    {
        Name = name;
        WorkItems = new HashSet<WorkItem>();
    }
}

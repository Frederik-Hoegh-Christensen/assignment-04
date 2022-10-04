using System.ComponentModel.DataAnnotations;

namespace Assignment.Infrastructure;

public class WorkItem
{
    private DateTime _statusUpdated;
    private State _currentState;
    public int Id { get; set; }

    [MaxLength(100)] public string Title { get; set; }

    public int? AssignedToId { get; set; }

    public User? AssignedTo { get; set; }

    [Required] public State State { get => _currentState; set { UpdateStatus(); _currentState = value;  } }
    

    public ICollection<Tag> Tags { get; set; }

    public DateTime Created { get; init; }

    
    public DateTime StateUpdated { get => _statusUpdated; set => _statusUpdated = value; }
    

    [MaxLength(100)] public string? Description { get; set; }

    public WorkItem(string title)
    {
        Title = title;
        Tags = new HashSet<Tag>();
        State = State.New;
        Created = DateTime.UtcNow;
        
          
        
    }
    private void UpdateStatus()
    {
        _statusUpdated = DateTime.UtcNow;

    }
    
}

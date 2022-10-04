namespace Assignment.Infrastructure;

public class KanbanContext : DbContext
{
    public virtual DbSet<WorkItem> Items => Set<WorkItem>();
    public virtual DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();

    

    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
        Database.EnsureCreated();   
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkItem>()
                    .Property(i => i.Title)
                    .HasMaxLength(100);

        modelBuilder.Entity<WorkItem>()
                    .Property(e => e.State)
                    .HasConversion(new EnumToStringConverter<State>(new ConverterMappingHints(size: 50)));

        modelBuilder.Entity<User>()
                    .Property(i => i.Name)
                    .HasMaxLength(100);

        modelBuilder.Entity<User>()
                    .Property(i => i.Email)
                    .HasMaxLength(100);

        modelBuilder.Entity<User>()
                    .HasIndex(i => i.Email)
                    .IsUnique();

        modelBuilder.Entity<Tag>()
                    .Property(i => i.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<Tag>()
                    .HasIndex(i => i.Name)
                    .IsUnique();
    }
}

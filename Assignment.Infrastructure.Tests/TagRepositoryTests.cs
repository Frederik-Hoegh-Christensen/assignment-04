
using Microsoft.EntityFrameworkCore;
using Assignment.Core;
using Microsoft.Data.Sqlite;

namespace Assignment.Infrastructure.Tests;

public class TagRepositoryTests : IDisposable
{
    private readonly TagRepository _tagRepository;
    private readonly KanbanContext _kanbanContext;
    private readonly SqliteConnection connection;
   

    public TagRepositoryTests ()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Tags.Add(new Tag("Do one stuff"));
        context.SaveChanges();

        _kanbanContext = context;
        _tagRepository = new TagRepository(_kanbanContext);
    }

    [Fact] 
    public void Create_tag_returns_created_and_id()
    {
        var (response, id) = _tagRepository.Create(new TagCreateDTO("dothis"));
        response.Should().Be(Response.Created);
        id.Should().Be(2);
    }
    
    [Fact] 
    public void Create_tag_that_exists_returns_conflict_and_id()
    {
        var (response, id) = _tagRepository.Create(new TagCreateDTO("Do one stuff"));
        response.Should().Be(Response.Conflict);
        id.Should().Be(1);
    }

    [Fact] 
    public void find_tag_that_exists()
    {
        var tag = _tagRepository.Find(1);
        tag.Name.Should().Be("Do one stuff");
    }

    [Fact]
    public void Delete_exsisting_tag_returns_deleted()
    {
        var respone = _tagRepository.Delete(1, default);
        respone.Should().Be(Response.Deleted);
    }

    [Fact]
    public void Read_should_return_1_tagDTO()
    {
        var tagDTOS = _tagRepository.Read();
        tagDTOS.Count.Should().Be(1);    
    }

    



    void IDisposable.Dispose()
    {
        connection.Dispose();   
    }
}

using Assignment.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace Assignment.Infrastructure.Tests;

public class WorkItemRepositoryTests : IDisposable
{
    private readonly WorkItemRepository _repository;
    private readonly KanbanContext _kanbanContext;
    private readonly SqliteConnection connection;


    public WorkItemRepositoryTests()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Items.Add(new WorkItem("atitle"));
        context.SaveChanges();
        var item = context.Items.First((a => a.Id == 1));
        context.SaveChanges();
        item.Tags.Add(new Tag("atag"));
        context.SaveChanges();
        
        

        _kanbanContext = context;
        _repository = new WorkItemRepository(_kanbanContext);
    }

    [Fact]
    public void Create_new_item_should_return_created_and_id ()
    {
        var item = new WorkItemCreateDTO("hej", null, "Hejhej", new List<string> { "hej"});
        var (response, id) =  _repository.Create(item);

        response.Should().Be(Response.Created);
        id.Should().Be(2);

    }
    
    [Fact]
    public void Delete_exsisting_item_should_return_deleted ()
    {
        var response = _repository.Delete(1);
        response.Should().Be(Response.Deleted);

    }

    [Fact]
    public void Find_an_item_should_return_the_title()
    {
        var tag = _repository.Find(1);
        tag.Title.Should().Be("atitle");

    }

    [Fact]
    public void Read_should_return_the_right_title_of_element_at_index_0()
    {
        var list = _repository.Read();
        list.ElementAt(0).Title.Should().Be("atitle");

    }

    [Fact]
    public void Read_by_state_should_return_1_element()
    {
        var list = _repository.ReadByState(State.New);
        list.Count.Should().Be(1);
    }

    [Fact]
    public void Read_by_tag_should_return_count_1()
    {
        var list = _repository.ReadByTag("atag");
        list.Count.Should().Be(1);
    }

    public void Read_by_user_id_should_return_count_1()
    {

    }

    
    public void Dispose()
    {
        connection.Dispose();
    }
}

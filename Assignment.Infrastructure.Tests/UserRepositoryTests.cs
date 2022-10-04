using Assignment.Core;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Assignment.Infrastructure.Tests;

public class UserRepositoryTests
{
    private readonly UserRepository _userRepository;
    private readonly KanbanContext _kanbanContext;
    private readonly SqliteConnection connection;

    public UserRepositoryTests()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Users.Add(new User("thor", "thor@gmail.com"));
        context.SaveChanges();

        _kanbanContext = context;
        _userRepository = new UserRepository(_kanbanContext);


    }

    [Fact]
    public void Create_new_user_returns_created_and_id_2()
    {
        var (response, id) = _userRepository.Create(new UserCreateDTO("frederik", "frederik@gmail.com"));
        response.Should().Be(Response.Created);
        id.Should().Be(2);
    }


    [Fact]
    public void Create_exisiting_user_should_return_conflict_and_id()
    {
        var (response, id) = _userRepository.Create(new UserCreateDTO("thor", "thor@gmail.com"));
        response.Should().Be(Response.Conflict);
        id.Should().Be(1);
    }

    //[Fact]B
    //public void Create_exisiting_user_should_return_conflict_and_id()
    //{
    //    var (response, id) = _userRepository.Create(new UserCreateDTO("thor", "thor@gmail.com"));
    //    response.Should().Be(Response.Conflict);
    //    id.Should().Be(1);
    //}






}

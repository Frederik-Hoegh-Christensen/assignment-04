namespace Assignment.Infrastructure;

public class UserRepository : IUserRepository
{
    private KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        
        var entity = _context.Users
            .Where(e => e.Email == user.Email)
            .FirstOrDefault();

        if (entity == null)
        {
            var newUser = new User(user.Name, user.Email);
            _context.Add(newUser);   
            _context.SaveChanges();
            return (Response.Created, newUser.Id);
        }
        return (Response.Conflict, entity.Id);

    }

    public Response Delete(int userId, bool force = false)
    {
        var entity = _context.Users
                     .Where(u => u.Id == userId)
                     .FirstOrDefault();


        if (entity is not null && force == true)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return Response.Deleted;
        }

        if (entity != null)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return Response.Deleted;
        }

        else return Response.NotFound;
    }

    public UserDTO Find(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<UserDTO> Read()
    {
        throw new NotImplementedException();
    }

    public Response Update(UserUpdateDTO user)
    {
        throw new NotImplementedException();
    }
}

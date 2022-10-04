using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Assignment.Infrastructure;

public class TagRepository : ITagRepository
{  
    private readonly KanbanContext _kanbanContext;  
    public TagRepository(KanbanContext context )
    {
        _kanbanContext = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var entity = new Tag(tag.Name);
        var exists = from t in _kanbanContext.Tags
                     where tag.Name == t.Name
                     select new TagDTO(t.Id, t.Name);
       if (exists.Any())
        {
            return (Response.Conflict, exists.First().Id);
        }
        _kanbanContext.Tags.Add(entity);
        _kanbanContext.SaveChanges();
        
        return (Response.Created, entity.Id);

    }

    public Response Delete(int tagId, bool force = false)
    {
        var entity = _kanbanContext.Tags
                     .Where(t => t.Id == tagId)
                     .FirstOrDefault();
                     
        
        if (entity.WorkItems.Any() && force == true)
        {
            _kanbanContext.Remove(entity);
            _kanbanContext.SaveChanges();
            return Response.Deleted;
        }

        if (entity != null)
        {
            _kanbanContext.Remove(entity);
            _kanbanContext.SaveChanges();
            return Response.Deleted;
        }

        else return Response.NotFound;
        

    }

    public TagDTO Find(int tagId)
    {
        var exists = from t in _kanbanContext.Tags
                     where t.Id == tagId
                     select new TagDTO(t.Id, t.Name);
        var exstistingTag = exists.FirstOrDefault();

        if (exists.Any())
        {
            return new TagDTO(exstistingTag.Id, exstistingTag.Name);
        }
        else
        {
            return null;
        }
                     
    }

    public IReadOnlyCollection<TagDTO> Read()
    {
        var list = new List<TagDTO>();
        foreach (var item in _kanbanContext.Tags)
        {
            list.Add(Find(item.Id));
        }
        return list;    
    }

    public Response Update(TagUpdateDTO tag)
    {
        throw new NotImplementedException();
    }
}

using Assignment.Core;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _kanbanContext;
    public WorkItemRepository(KanbanContext context)
    {
        _kanbanContext = context;
    }


    public (Response Response, int ItemId) Create(WorkItemCreateDTO item)
    {
        var entity = new WorkItem(item.Title);
        var exsits = from i in _kanbanContext.Items
                      where i.Title == item.Title
                      select i;
        if (exsits.Any())
        {
            return (Response.Conflict, exsits.FirstOrDefault().Id);
            
        }
        _kanbanContext.Add(entity);
        _kanbanContext.SaveChanges();
        return (Response.Created, entity.Id);




    }

    public Response Delete(int itemId)
    {
        var entity = _kanbanContext.Items
            .Where(e => e.Id == itemId)
            .FirstOrDefault();

        if (entity != null)
        {
            _kanbanContext.Remove(entity);
            _kanbanContext.SaveChanges();
            return Response.Deleted;
        }
        return Response.NotFound;   
    }

    public WorkItemDetailsDTO Find(int itemId)
    {
        var entity = _kanbanContext.Items
            .Where(item => item.Id == itemId)
            .FirstOrDefault();
        if (entity != null)
        {

            return new WorkItemDetailsDTO(entity.Id, entity.Title, null, entity.Created, null, null, entity.State, entity.StateUpdated);

        }
        return null;

    }

    public IReadOnlyCollection<WorkItemDTO> Read()
    {
        var list = new List<WorkItemDTO>();
        foreach (var item in _kanbanContext.Items)
        {
            var temp = new WorkItemDTO(item.Id, item.Title, null, null, item.State);
            list.Add(temp);
        }
        return list;
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByState(State state)
    {
        var list = _kanbanContext.Items
            .Where(e => e.State == state);

        var ActualList = new List<WorkItemDTO>();

        foreach (var item in list)
        {
            var temp = new WorkItemDTO(item.Id, item.Title, null, null, item.State);
            ActualList.Add(temp);
        }
        return ActualList;
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag)
    {
        var list = _kanbanContext.Items.Where(e => e.Tags.Any(a => a.Name == tag));


        var ActualList = new List<WorkItemDTO>();

        foreach (var item in list)
        {
            var temp = new WorkItemDTO(item.Id, item.Title, null, null, item.State);
            ActualList.Add(temp);
        }
        return ActualList;
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId)
    {
        var list = _kanbanContext.Items.Where(e => e.AssignedToId == userId);


        var ActualList = new List<WorkItemDTO>();

        foreach (var item in list)
        {
            var temp = new WorkItemDTO(item.Id, item.Title, null, null, item.State);
            ActualList.Add(temp);
        }
        return ActualList;
    }

    public IReadOnlyCollection<WorkItemDTO> ReadRemoved()
    {
        throw new NotImplementedException();
    }

    public Response Update(WorkItemUpdateDTO item)
    {
        throw new NotImplementedException();
    }
}

using MWS.Domain.Exceptions;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

public class Resource
{
    public const int DEFAULT_STATE_NUMBER = 1;

    private Resource() { }
    
    private Resource(Guid id, string title, State state)
    {
        Id = id;
        Title = title;
        State = state;
    }
    
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public State State { get; private set; }

    public static Resource Create(Guid id, string title, State state)
    {
        if(id == Guid.Empty)
            throw new DomainException("Guid is empty");

        if(string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title can not be empty or writespace");

        return new Resource(id, title, state);
    }

    public void Update(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");

        if (Title != title)
            Title = title;
    }

    public void Archive()
    {
        if (State == State.Archived)
            return;

        State = State.Archived;
    }

    public void Restore()
    {
        if (State == State.Working)
            return;

        State = State.Working;
    }
}

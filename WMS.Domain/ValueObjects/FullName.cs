using WMS.Domain.ValueObjects.Abstracts;

namespace WMS.Domain.ValueObjects;
public class FullName : ValueObject
{
    public string LastName { get; private set; }

    public string FirstName { get; private set; }

    public string? Patronymic { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

namespace WMS.Domain.ValueObjects.Abstracts;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(default(int), (hashCode, value) =>
            HashCode.Combine(hashCode, value.GetHashCode()));

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a == null || b == null)
            return false;

        if (a == null && b == null)
            return true;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) =>
        !(a == b);
}

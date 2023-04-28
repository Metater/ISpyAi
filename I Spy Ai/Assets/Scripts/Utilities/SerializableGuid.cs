using System;

[Serializable]
public struct SerializableGuid : IComparable, IComparable<SerializableGuid>, IEquatable<SerializableGuid>
{
    public string value;

    private SerializableGuid(string value)
    {
        this.value = value;
    }

    public static implicit operator SerializableGuid(Guid guid)
    {
        return new SerializableGuid(guid.ToString());
    }

    public static implicit operator Guid(SerializableGuid serializableGuid)
    {
        return new Guid(serializableGuid.value);
    }

    public int CompareTo(object value)
    {
        if (value == null)
            return 1;
        if (value is not SerializableGuid)
            throw new ArgumentException("Must be SerializableGuid");
        SerializableGuid guid = (SerializableGuid)value;
        return guid.value == this.value ? 0 : 1;
    }

    public int CompareTo(SerializableGuid other)
    {
        return other.value == this.value ? 0 : 1;
    }

    public bool Equals(SerializableGuid other)
    {
        return this.value == other.value;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return (this.value != null ? this.value.GetHashCode() : 0);
    }

    public override string ToString()
    {
        return (this.value != null ? new Guid(this.value).ToString() : string.Empty);
    }

    public static bool operator ==(SerializableGuid left, SerializableGuid right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SerializableGuid left, SerializableGuid right)
    {
        return !(left == right);
    }

    public static bool operator <(SerializableGuid left, SerializableGuid right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(SerializableGuid left, SerializableGuid right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(SerializableGuid left, SerializableGuid right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(SerializableGuid left, SerializableGuid right)
    {
        return left.CompareTo(right) >= 0;
    }
}
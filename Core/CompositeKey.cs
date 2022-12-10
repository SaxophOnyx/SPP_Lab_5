namespace Core
{
    public class CompositeKey<A, B>
    {
        public A AValue { get;}

        public B BValue { get; }


        public CompositeKey(A aValue, B bValue)
        {
            AValue = aValue;
            BValue = bValue;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            CompositeKey<A, B> key = obj as CompositeKey<A, B>;
            if (key == null)
                return false;

            return AValue.Equals(key.AValue) && BValue.Equals(key.BValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AValue, BValue);
        }
    }
}

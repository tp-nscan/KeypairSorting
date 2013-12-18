namespace MathUtils
{
    public static class Types
    {
        public static T Cast<T>(this object value)
        {
            return (T) value;
        }
    }
}

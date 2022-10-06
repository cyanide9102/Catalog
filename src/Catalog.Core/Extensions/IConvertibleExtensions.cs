namespace Catalog.Core.Extensions
{
    public static class IConvertibleExtensions
    {
        public static T? To<T>(this IConvertible obj)
        {
            try
            {
                Type t = typeof(T);
                Type? u = Nullable.GetUnderlyingType(t);
                if (u != null)
                {
                    return (obj == null) ? default : (T)Convert.ChangeType(obj, u);
                }
                else
                {
                    return (T)Convert.ChangeType(obj, t);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}

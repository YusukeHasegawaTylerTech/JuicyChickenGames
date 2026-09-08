using System.Linq;

namespace JuicyChickenGames
{
    // Reflection-based copies/lookups. Convenient for editor tooling or rarely-called
    // code paths, but each call walks the type's members - avoid these in per-frame code.
    public static class ReflectionExtensions
    {
        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }
            }
        }

        public static void CopyFieldsTo<T, TU>(this T source, TU dest)
        {
            var sourceProps = typeof(T).GetFields().ToList();
            var destProps = typeof(TU).GetFields().ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    p.SetValue(dest, sourceProp.GetValue(source));
                }
            }
        }

        public static TOut GetPropValue<T, TOut>(this T source, string propName)
        {
            var sourceProp = typeof(T).GetProperties().Where(x => x.CanRead).First(x => x.Name.Equals(propName));

            var value = (TOut)sourceProp.GetValue(source);

            return value;
        }
    }
}

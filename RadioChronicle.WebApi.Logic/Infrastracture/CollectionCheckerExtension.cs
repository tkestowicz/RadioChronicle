using System.Collections.Generic;
using System.Linq;

namespace RadioChronicle.WebApi.Logic.Infrastracture
{
    public static class CollectionCheckerExtension
    {
        internal static bool HasExpectedNumberOfElements<TType>(this IEnumerable<TType> collection, int expectedNumberOfElements)
        {
            return collection != null && collection.Count() == expectedNumberOfElements;
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Project.Scripts.EventSystem.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) =>
            self.Select((item, index) => (item, index));
        
        public static string ToCamelCase<T>([NotNull]this T self) =>
            string.IsNullOrEmpty(self.ToString()) || self.ToString().Length < 2
                ? self.ToString().ToLowerInvariant()
                : JsonConvert.SerializeObject(self, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FoxTunes
{
    public static class CommonImageTypes
    {
        public const string FrontCover = "FrontCover";
        public const string BackCover = "BackCover";

        public static IDictionary<string, string> Lookup = GetLookup();

        private static IDictionary<string, string> GetLookup()
        {
            var lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var field in typeof(CommonImageTypes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var name = field.GetValue(null) as string;
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                lookup.Add(name, name);
            }
            return lookup;
        }
    }
}

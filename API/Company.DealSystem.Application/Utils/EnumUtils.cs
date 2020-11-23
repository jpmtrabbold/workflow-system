using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Domain.ExtensionMethods;

namespace Company.DealSystem.Application.Utils
{
    public static class EnumUtils
    {
        static Dictionary<string, List<LookupRequest>> EnumLookupsCache = new Dictionary<string, List<LookupRequest>>();
        public static List<LookupRequest> GetEnumAsLookups<E>() where E : Enum
        {
            var type = typeof(E);
            var name = type.FullName;
            if (EnumLookupsCache.ContainsKey(name))
                return EnumLookupsCache[name];

            var lookups = Enum.GetValues(type)
                .Cast<E>()
                .Select(v => new LookupRequest { Id = Convert.ToInt32(v), Name = v.GetDescription(), Active = true })
                .ToList();

            EnumLookupsCache.Add(name, lookups);
            return lookups;
        }

    }
}

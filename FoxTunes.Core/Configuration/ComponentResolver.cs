﻿using FoxTunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FoxTunes
{
    public class ComponentResolver : IComponentResolver
    {
        static ComponentResolver()
        {
            Slots = new Dictionary<string, string>();
        }

        public static IDictionary<string, string> Slots { get; private set; }

        public string Get(string slot)
        {
            if (string.IsNullOrEmpty(slot))
            {
                return ComponentSlots.None;
            }
            var id = default(string);
            if (Slots.TryGetValue(slot, out id))
            {
                return id;
            }
#if NET40
            throw new NotImplementedException();
#else
            id = ConfigurationManager.AppSettings.Get(slot);
#endif
            if (string.IsNullOrEmpty(id))
            {
                return ComponentSlots.None;
            }
            return id;
        }

        public static readonly IComponentResolver Instance = new ComponentResolver();
    }
}

using System;
using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public sealed class Registry
    {
        private readonly Dictionary<Type, Registration> registrationCache = new Dictionary<Type, Registration>();

        public void Build(IEnumerable<Registration> registrations)
        {
            foreach (var registration in registrations)
            {
                if (registration.interfaceTypes != null && registration.interfaceTypes.Count > 0)
                {
                    for (var i = 0; i < registration.interfaceTypes.Count; i++)
                    {
                        var interfaceType = registration.interfaceTypes[i];
                        registrationCache[interfaceType] = registration;
                    }
                }
                else
                {
                    registrationCache[registration.implementationType] = registration;
                }
            }
        }

        public bool TryGet(Type type, out Registration registration)
        {
            return registrationCache.TryGetValue(type, out registration);
        }

    }
}
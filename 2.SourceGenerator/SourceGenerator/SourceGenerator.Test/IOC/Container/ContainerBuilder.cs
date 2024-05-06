using System.Collections.Generic;

namespace QSFramework.Runtime.IOC
{
    public interface IContainerBuilder
    {
        T Register<T>(T registrationBuilder) where T : RegistrationBuilder;
        IObjectResolver Build();
    }


    public class ContainerBuilder : IContainerBuilder
    {
        private readonly List<RegistrationBuilder> registrationBuilders = new List<RegistrationBuilder>();

        public T Register<T>(T registrationBuilder) where T : RegistrationBuilder
        {
            registrationBuilders.Add(registrationBuilder);
            return registrationBuilder;
        }

        public IObjectResolver Build()
        {
            var registry = BuildRegistry();
            var container = new Container(registry);
            return container;
        }

        private Registry BuildRegistry()
        {
            var registry = new Registry();
            var registrations = new List<Registration>(registrationBuilders.Count);
            for (int i = 0; i < registrationBuilders.Count; i++)
            {
                registrations.Add(registrationBuilders[i].Build());
            }
            registry.Build(registrations);
            return registry;
        }
    }
}


namespace QSFramework.Runtime.IOC
{
    public static class ContainerBuilderExtension
    {
        public static RegistrationBuilder Register<T>(this IContainerBuilder builder, ELifetime lifetime) 
            => builder.Register(new RegistrationBuilder(typeof(T), lifetime));

        public static RegistrationBuilder Register<TInterface, TImplement>(this IContainerBuilder builder, ELifetime lifetime)
            => builder.Register<TImplement>(lifetime).As<TInterface>();
    }
}